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

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IDentistAvailabilityService _dentistAvailService;

        public CreateModel(IAppointmentService appointmentService, IServiceService serviceService, IDentistAvailabilityService dentistService)
        {
            _appointmentService = appointmentService;
            _serviceService = serviceService;
            _dentistAvailService = dentistService;
        }
        public bool IsServiceIdDisabled { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            Service services = await _serviceService.GetByIdAsync(id);
            List<Service> services1 = new List<Service>();
            services1.Add(services);
            //List<Dentist> dentists =  await _dentistAvailService.GetAvailableDentist(Appointment.AppointDate, services.Duration);

            //dentists.Insert(0, new Dentist { Id = 0, Name = "Please select a dentist" }); 
            //ViewData["DentistId"] = new SelectList(dentists, "Id", "Name");
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
        public async Task<IActionResult> OnPostAsync()
        {
            Service services = await _serviceService.GetByIdAsync(Appointment.ServiceId);
            User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            Appointment.PatientId = currentAcc.Id;
            var availRoom = await _appointmentService.GetRoomAvailable(Appointment.AppointDate, services.Duration);
            Appointment.RoomId= availRoom.Id;
            Appointment.Status = (int)AppointmentStatus.Waiting;
            Appointment.CreateDate = Appointment.ModifyDate = DateTime.UtcNow.AddHours(7);
            var result = await _appointmentService.AddAsync(Appointment);
            if(result != null)
            {
                return RedirectToPage("./Index");
            }
            return Page();
            
        }
    }
}
