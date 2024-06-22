using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Entities;
using ClinicRepositories;
using BusinessObjects;
using ClinicServices.Interfaces;
using ClinicPresentationLayer.Extension;
using System.Globalization;
using ClinicServices.EmailService;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IDentistAvailabilityService _dentistAvailService;

        public Service Service;
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
            Service = await _serviceService.GetByIdAsync(id);
            services1.Add(Service);
            //ViewData["StartSlot"] = new SelectList(SlotDefiner.slots, "Key", "DisplayTime");
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
            Service = await _serviceService.GetByIdAsync(Appointment.ServiceId);
            List<Dentist> dentists = await _dentistAvailService.GetAvailableDentist(Appointment.AppointDate, Appointment.StartSlot, Service.Duration, Appointment.ServiceId);
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
            Service = await _serviceService.GetByIdAsync(Appointment.ServiceId);

            // Retrieve data from TempData or ViewData set in OnPostSubmitBasicAsync
            Appointment.AppointDate = (DateTime)TempData["AppointmentDate"];
            Appointment.StartSlot = (int)TempData["StartSlot"];
            Appointment.ServiceId = (int)TempData["ServiceId"];

            Appointment.PatientId = currentAcc.Id;
            var availRoom = await _appointmentService.GetRoomAvailable(Appointment.AppointDate, Service.Duration);
            Appointment.RoomId = availRoom.Id;
            Appointment.Status = (int)AppointmentStatus.Waiting;
            Appointment.CreateDate = Appointment.ModifyDate = DateTime.UtcNow.AddHours(7);
            Appointment.EndSlot = Appointment.StartSlot + Service.Duration - 1;
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
        public async Task<IActionResult> OnGetAvailableSlotsPartial(DateTime appointmentDate, int serviceDuration)
        {
            List<Slot> availableSlots = await _appointmentService.GetAvailableSlotAsync(appointmentDate, serviceDuration);;
            return Partial("_SlotPartial", availableSlots.Where(item=> item.IsAvailable == true).ToList());
        }
    }
}
