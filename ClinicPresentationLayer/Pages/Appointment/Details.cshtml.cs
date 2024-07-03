using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using ClinicRepositories;
using ClinicServices.Interfaces;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class DetailsModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public DetailsModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public string GetTimeFromSlot(int slot)
        {
            return slot switch
            {
                1 => "7:00",
                2 => "8:00",
                3 => "9:00",
                4 => "10:00",
                5 => "11:00",
                6 => "13:00",
                7 => "14:00",
                8 => "15:00",
                9 => "16:00",
                10 => "17:00",
                _ => "Invalid Slot"
            };
        }

        public BusinessObjects.Entities.Appointment Appointment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _appointmentService.GetAppointmentsByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }
            else
            {
                Appointment = appointment;
            }
            return Page();
        }
    }
}
