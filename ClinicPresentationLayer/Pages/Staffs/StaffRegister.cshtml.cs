using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicPresentationLayer.Extension;
using ClinicServices;
using ClinicServices.Interfaces;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages
{
    [CustomAuthorize(UserRoles.Staff)]
    public class StaffRegisterModel : PageModel
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IDentistAvailabilityService _dentistAvailService;
        private readonly IPaymentService _paymentService;
        public StaffRegisterModel(IPatientService patientService, IAppointmentService appointmentService, IServiceService serviceService, IDentistAvailabilityService dentistAvailabilityService, IPaymentService paymentService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _dentistAvailService = dentistAvailabilityService;
            _serviceService = serviceService;
            _paymentService = paymentService;
        }
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public DateOnly DateOfBirth { get; set; }

        [BindProperty]
        public string Gender { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Address { get; set; }

        public string SearchTerm { get; set; }
        public Patient FoundPatient { get; set; }
        [BindProperty]
        public BusinessObjects.Entities.Appointment Appointment { get; set; } = default!;
        [BindProperty]
        public Service Service { get; set; } = default!;
        [BindProperty]
        public List<Service> Services { get; set; } = default!;
        public async void OnGet()
        {
            Services =await _serviceService.GetAllAvailAsync();
            Service = Services.First();
        }
        public async Task<IActionResult> OnPostSearchAsync(string searchTerm)
        {
            Services = await _serviceService.GetAllAvailAsync();
            Service = Services.First();
            if (string.IsNullOrEmpty(searchTerm))
            {
                TempData["ErrorMessage"] = "Search term is required.";
                return Page();
            }

            FoundPatient = await _patientService.FindPatientAsync(searchTerm);
            TempData["PatientId"] = FoundPatient.Id;
            if (FoundPatient == null)
            {
                TempData["ErrorMessage"] = "No patient found with the provided details.";
            }

            return Page();
        }
        public async Task<IActionResult> OnPostRegister()
        {
            Services = await _serviceService.GetAllAvailAsync();
            Service = Services.First();
            try
            {
                User newUser = new() { Username = Username, Role = UserRoles.Patient, Password = Password };
                Patient newPatient = new()
                {
                    Email = Email,
                    Name = Name,
                    Address = Address,
                    DateOfBirth = DateOfBirth,
                    Gender = Gender,
                    Phone = Phone
                };
                var check = await _patientService.StaffAddAsync(newPatient, newUser);
                if (check != null)
                {
                    FoundPatient = check;
                    return Page();
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while creating the patient account.";
                    return Page();
                }
            }catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }
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
                    PaymentStatus = "Checkout",
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

