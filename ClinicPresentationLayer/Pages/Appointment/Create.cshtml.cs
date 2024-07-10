using BusinessObjects;
using BusinessObjects.Entities;
//using ClinicPresentationLayer.Athorization;
using ClinicPresentationLayer.Authorization;
using ClinicPresentationLayer.Extension;
using ClinicPresentationLayer.Extension.Libraries;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicPresentationLayer.Pages.Appointment
{
    [CustomAuthorize(UserRoles.ClinicOwner, UserRoles.Patient, UserRoles.Dentist)]
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IDentistAvailabilityService _dentistAvailService;
        private readonly IConfiguration _configuration;
        private readonly IPaymentService _paymentService;

        [BindProperty]
        public Service Service { get; set; } = default!;
        [BindProperty]
        public List<Service> Services { get; set; } = default!;
        [BindProperty]
        public BusinessObjects.Entities.Appointment Appointment { get; set; } = default!;

        // Additional property for payment URL
        [BindProperty]
        public string VnpayUrl { get; set; } = string.Empty;

        public CreateModel(IAppointmentService appointmentService, IServiceService serviceService, IDentistAvailabilityService dentistService, IConfiguration configuration, IPaymentService paymentService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _dentistAvailService = dentistService;
            _configuration = configuration;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            Services = await _serviceService.GetAllAsync();
            User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            if (currentAcc == null)
            {
                return RedirectToPage("/Login");
            }
            else if (currentAcc.Role != 2)
            {
                return RedirectToPage("/MainPage");
            }
            Service = await _serviceService.GetByIdAsync(id);
            if (Service == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            var currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            Services = await _serviceService.GetAllAsync();
            if (currentAcc == null)
            {
                return RedirectToPage("/Login");
            }

            Appointment.PatientId = currentAcc.Id;

            Service = await _serviceService.GetByIdAsync(Appointment.ServiceId);
            if (Service == null)
            {
                ModelState.AddModelError("", "Selected service not found.");
                return Page();
            }

            var availRoom = _appointmentService.GetRoomAvailable(Appointment.AppointDate, Service.Duration, Appointment.StartSlot);
            Appointment.RoomId = availRoom.Id;
            Appointment.Status = (int)AppointmentStatus.Waiting;
            Appointment.CreateDate = Appointment.ModifyDate = DateTime.UtcNow.AddHours(7);
            Appointment.EndSlot = Appointment.StartSlot + Service.Duration - 1;

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((int)(Service.Cost * 100)).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", HttpContext.Connection.RemoteIpAddress.ToString());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"Payment for appointment #{Appointment.Id + 1}");
            vnpay.AddRequestData("vnp_OrderType", "other");

            // Construct return URL
            string returnUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/PaymentReturn";
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);

            vnpay.AddRequestData("vnp_TxnRef", Appointment.Id.ToString());
            vnpay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmmss"));

            string paymentUrl = vnpay.CreateRequestUrl("https://sandbox.vnpayment.vn/paymentv2/vpcpay.html", _configuration["Vnpay:HashSecret"]);
            VnpayUrl = paymentUrl;

            // Save payment information to your data store
            await _paymentService.AddAsync(new Payment
            {
                AppointmentId = Appointment.Id,
                Amount = (decimal)Service.Cost,
                PaymentStatus = "Paid", // Set initial status
                PaymentDate = DateTime.UtcNow,
                // Add other relevant payment details
            });

            return Page();
        }

        public async Task<IActionResult> OnGetAvailableSlotsPartial(DateTime appointmentDate, int serviceDuration)
        {
            List<Slot> availableSlots = await _appointmentService.GetAvailableSlotAsync(appointmentDate, serviceDuration);
            availableSlots = availableSlots.Where(item => item.IsAvailable).ToList();
            availableSlots = SlotDefiner.DurationDiplayTimeOnSlot(availableSlots, serviceDuration);
            return Partial("_SlotPartial", availableSlots);
        }

        public async Task<IActionResult> OnGetAvailableDentistsPartial(DateTime appointmentDate, int startSlot, int serviceDuration, int serviceId)
        {
            List<BusinessObjects.Entities.Dentist> availableDentists = await _dentistAvailService.GetAvailableDentist(appointmentDate, startSlot, serviceDuration, serviceId);
            return Partial("_DentistPartial", availableDentists.ToList());
        }
    }
}
