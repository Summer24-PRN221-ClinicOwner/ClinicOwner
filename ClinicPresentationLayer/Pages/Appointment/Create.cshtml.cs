﻿using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Extension;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class CreateModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IServiceService _serviceService;
        private readonly IDentistAvailabilityService _dentistAvailService;

        [BindProperty]
        public Service Service { get; set; } = default!;

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

            return Page();
        }

        public async Task<IActionResult> OnPostSubmitAsync()
        {
            User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
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
                var availRoom = _appointmentService.GetRoomAvailable(Appointment.AppointDate, Service.Duration);
                Appointment.RoomId = availRoom.Id;
                Appointment.Status = (int)AppointmentStatus.Waiting;
                Appointment.CreateDate = Appointment.ModifyDate = DateTime.UtcNow.AddHours(7);
                Appointment.EndSlot = Appointment.StartSlot + Service.Duration - 1;

                var result = await _appointmentService.AddAsync(Appointment);

                if (result != null)
                {
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {
                // Log the exception for debugging and monitoring
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "An error occurred while processing your request.");
                // Optionally, return a specific error page or handle the error
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
            List<Dentist> availableDentists = await _dentistAvailService.GetAvailableDentist(appointmentDate, startSlot, serviceDuration, serviceId);
            return Partial("_DentistPartial", availableDentists.ToList());
        }
    }
}
