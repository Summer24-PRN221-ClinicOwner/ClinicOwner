using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicPresentationLayer.Extension;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.Appointment
{
    [CustomAuthorize(UserRoles.Patient)]
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IDentistAvailabilityService _dentistAvailService;

        [BindProperty]
        public Service Service { get; set; } = default!;
        [BindProperty]
        public List<Service> Services { get; set; } = default!;

        [BindProperty]
        public BusinessObjects.Entities.Appointment Appointment { get; set; } = default!;

        public CreateModel(IAppointmentService appointmentService, IServiceService serviceService, IDentistAvailabilityService dentistService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _dentistAvailService = dentistService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            Services = await _serviceService.GetAllAvailAsync();
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
                // Handle scenario where service with given id was not found
                return NotFound();
            }
            else
            {
                if(Service.Status == (int)ServiceStatus.Unavailable)
                {
                    return RedirectToPage("/MainPage");
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            Services = await _serviceService.GetAllAvailAsync();
            if (currentAcc == null)
            {
                return RedirectToPage("/Login");
            }

            Appointment.PatientId = currentAcc.Id;

            // Ensure Service is properly loaded from database
            Service = await _serviceService.GetByIdAsync(Appointment.ServiceId);
            if (Service == null)
            {
                ModelState.AddModelError("", "Selected service not found.");
                return Page();
            }

            try
            {
                var availRoom = _appointmentService.GetRoomAvailable(Appointment.AppointDate, Service.Duration, Appointment.StartSlot);
                Appointment.RoomId = availRoom.Id;
                Appointment.Status = (int)AppointmentStatus.Waiting;
                Appointment.CreateDate = Appointment.ModifyDate = DateTime.UtcNow.AddHours(7);
                Appointment.EndSlot = Appointment.StartSlot + Service.Duration - 1;

                var result = await _appointmentService.AddAsync(Appointment);

                if (result != null)
                {
                    return RedirectToPage("/PatientHistory");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                OnGetAvailableSlotsPartial(Appointment.AppointDate, Service.Duration);
                OnGetAvailableDentistsPartial(Appointment.AppointDate, Appointment.StartSlot, Service.Duration, Service.Id);
                //Appointment = new BusinessObjects.Entities.Appointment();
                return RedirectToPage("/Appointment/Create", new { id = Service.Id });
            }
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
