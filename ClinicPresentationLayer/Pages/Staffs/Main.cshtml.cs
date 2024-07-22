using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicPresentationLayer.Extension;
using ClinicServices;
using ClinicServices.Interfaces;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Transactions;

namespace ClinicPresentationLayer.Pages.Staffs
{
    [CustomAuthorize(UserRoles.Staff)]
    public class MainModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IDentistAvailabilityService _dentistAvailService;
        private readonly IPaymentService _paymentService;
        public MainModel(IPatientService patientService, IAppointmentService appointmentService, IServiceService serviceService, IDentistAvailabilityService dentistAvailabilityService, IPaymentService paymentService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _dentistAvailService = dentistAvailabilityService;
            _serviceService = serviceService;
            _paymentService = paymentService;
        }
        public List<BusinessObjects.Entities.Appointment> PatientAppointments { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        public Patient FoundPatient { get; set; }
        public decimal? RefundAmount { get; set; }
        public string ErrorMessage { get; set; }
        public string CheckMessage { get; set; }
        [BindProperty]
        public BusinessObjects.Entities.Appointment Appointment { get; set; } = default!;
        [BindProperty]
        public Service Service { get; set; } = default!;
        [BindProperty]
        public List<Service> Services { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            Services = await _serviceService.GetAllAvailAsync();
            Service = Services.First();
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                FoundPatient = await _patientService.FindPatientAsync(SearchTerm);
                if (FoundPatient != null)
                {
                    PatientAppointments = await _appointmentService.GetAppoinmentHistoryAsync(FoundPatient.Id);
                }
            }
            ErrorMessage = TempData["ErrorMessage"] as string;
            CheckMessage = TempData["SuccessMessage"] as string;
            return Page();
        }
        public async Task<IActionResult> OnPostSearchAsync()
        {
            Services = await _serviceService.GetAllAvailAsync();
            Service = Services.First();
            if (string.IsNullOrEmpty(SearchTerm))
            {
                TempData["ErrorMessage"] = "Search term is required.";
                return Page();
            }

            FoundPatient = await _patientService.FindPatientAsync(SearchTerm);
            if(FoundPatient != null)
            {
                PatientAppointments = await _appointmentService.GetAppoinmentHistoryAsync(FoundPatient.Id);
            }
            TempData["PatientId"] = FoundPatient.Id;
            if (FoundPatient == null)
            {
                TempData["ErrorMessage"] = "No patient found with the provided details.";
            }

            return Page();
        }
        public async Task<IActionResult> OnPostCheckRefundAsync(int appointmentId)
        {
            ErrorMessage = TempData["ErrorMessage"] as string;
            CheckMessage = TempData["SuccessMessage"] as string;
            var appointment = await _appointmentService.GetAppointmentsByIdAsync(appointmentId);
            if (appointment == null)
            {
                TempData["ErrorMessage"] = "No appointment found with the provided ID.";
                return RedirectToPage(new { SearchTerm });
            }
            if(appointment.Payment == null)
            {
                TempData["ErrorMessage"] = "No payment found with the appointment.";
                return RedirectToPage(new { SearchTerm });
            }

            if (appointment.Payment.PaymentStatus != PaymentStatus.CHECKOUT)
            {
                TempData["ErrorMessage"] = "The appointment is not eligible for a refund.";
                return RedirectToPage(new { SearchTerm });
            }

            var cancellationTimeSpan = DateTime.UtcNow.AddHours(7) - appointment.CreateDate;
            RefundAmount = cancellationTimeSpan.TotalDays < 1
                ? appointment.Payment.Amount // Full refund
                : appointment.Payment.Amount * 0.5M; // 50% refund
            TempData["SuccessMessage"] = $"Refund valid for {RefundAmount}";
            return RedirectToPage(new { SearchTerm });
        }

        public async Task<IActionResult> OnPostRefundAsync(int appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentsByIdAsync(appointmentId);
            if (appointment == null)
            {
                TempData["ErrorMessage"] = "No appointment found with the provided ID.";
                return RedirectToPage(new { SearchTerm });
            }

            if (appointment.Payment.PaymentStatus != PaymentStatus.CHECKOUT)
            {
                TempData["ErrorMessage"] = "The appointment is not eligible for a refund.";
                return RedirectToPage(new { SearchTerm });
            }

            var cancellationTimeSpan = DateTime.UtcNow.AddHours(7) - appointment.CreateDate;
            var refundAmount = cancellationTimeSpan.TotalDays < 1
                ? appointment.Payment.Amount // Full refund
                : appointment.Payment.Amount * 0.5M; // 50% refund

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _appointmentService.UpdateAppointmentStatus(appointmentId, (int)AppointmentStatus.Canceled, null);
                    TempData["SuccessMessage"] = $"Refund of {refundAmount} processed successfully.";
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Failed to process refund: {ex.Message}";
                }
            }

            //PatientAppointments = await _appointmentService.GetAppoinmentHistoryAsync(FoundPatient.Id);
            return RedirectToPage(new { SearchTerm });
        }


        public async Task<IActionResult> OnPostSubmitAsync()
        {
            var currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            Services = await _serviceService.GetAllAsync();
            if (currentAcc == null)
            {
                return RedirectToPage("/Login");
            }
            try
            {
                Appointment.PatientId = TempData["PatientId"] as int? ?? throw new Exception("Patient id not found");

                Service = await _serviceService.GetByIdAsync(Appointment.ServiceId);
                if (Service == null)
                {
                    TempData["ErrorMessage"] = "Service not found";
                    return Page();
                }

                var availRoom = _appointmentService.GetRoomAvailable(Appointment.AppointDate, Service.Duration, Appointment.StartSlot);
                Appointment.RoomId = availRoom.Id;
                Appointment.Status = (int)AppointmentStatus.Waiting;
                Appointment.CreateDate = Appointment.ModifyDate = DateTime.UtcNow.AddHours(7);
                Appointment.EndSlot = Appointment.StartSlot + Service.Duration - 1;
                var payment = new Payment
                {
                    Amount = Service.Cost.Value, // Assuming vnp_Amount is in the smallest currency unit (like cents)
                    PaymentStatus = PaymentStatus.CHECKOUT,
                    PaymentDate = DateTime.UtcNow.AddHours(7),
                    TransactionId = Guid.NewGuid().ToString()
                };
                await _appointmentService.AddAsync(Appointment, payment);
                return Page();
            }
            catch(Exception ex) 
            {
                TempData["ErrorMessage"] = $"error when create appointment: {ex.Message}";
                return Page();
            }
            
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

