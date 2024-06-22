using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Extension;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IDentistAvailabilityService _dentistAvailService;

        private static Service service;
        public CreateModel(IAppointmentService appointmentService, IServiceService serviceService, IDentistAvailabilityService dentistService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _dentistAvailService = dentistService;
        }
        public bool IsServiceIdDisabled { get; set; }
        public bool ShowDentistForm { get; set; } = false;
        public async Task<IActionResult> OnGet(int id)
        {
            User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            if (currentAcc == null)
            {
                return RedirectToPage("/Login");
            }
            else if (currentAcc.Role != 2)
            {
                return RedirectToPage("/MainPage");
            }
            service = await _serviceService.GetByIdAsync(id);
            List<Service> services1 = new List<Service>();
            services1.Add(service);

            ViewData["ServiceId"] = new SelectList(services1, "Id", "Name");
            ViewData["StartSlot"] = new SelectList(SlotDefiner.slots, "Key", "DisplayTime");
            if (id != null)
            {
                IsServiceIdDisabled = true;
            }
            return Page();
        }

        [BindProperty]
        public BusinessObjects.Entities.Appointment Appointment { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostSubmitBasicAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            List<Dentist> dentists = await _dentistAvailService.GetAvailableDentist(Appointment.AppointDate, Appointment.StartSlot, service.Duration, Appointment.ServiceId);
            dentists.Insert(0, new Dentist { Id = 0, Name = "Please select a dentist" });

            // Set ViewData for DentistId dropdown in Form 2
            ViewData["DentistId"] = new SelectList(dentists, "Id", "Name");
            // Proceed to form2 by redirecting with TempData or ViewData
            TempData["AppointmentDate"] = Appointment.AppointDate;
            TempData["StartSlot"] = Appointment.StartSlot;
            TempData["ServiceId"] = Appointment.ServiceId;
            ShowDentistForm = true;
            return Page();
        }

        public async Task<IActionResult> OnPostSubmitCompleteAsync()
        {
            User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");

            // Retrieve data from TempData or ViewData set in OnPostSubmitBasicAsync
            Appointment.AppointDate = (DateTime)TempData["AppointmentDate"];
            Appointment.StartSlot = (int)TempData["StartSlot"];
            Appointment.ServiceId = (int)TempData["ServiceId"];

            Appointment.PatientId = currentAcc.Id;
            var availRoom = _appointmentService.GetRoomAvailable(Appointment.AppointDate, service.Duration);
            Appointment.RoomId = availRoom.Id;
            Appointment.Status = (int)AppointmentStatus.Waiting;
            Appointment.CreateDate = Appointment.ModifyDate = DateTime.UtcNow.AddHours(7);
            Appointment.EndSlot = Appointment.StartSlot + service.Duration - 1;
            try
            {
                var result = await _appointmentService.AddAsync(Appointment);

                if (result != null)
                {
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {

            }


            return Page();
        }
    }
}
