using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Entities;
using ClinicRepositories;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class CreateModel : PageModel
    {
        private readonly ClinicRepositories.ClinicContext _context;

        public CreateModel(ClinicRepositories.ClinicContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var rooms = _context.Rooms.ToList();
            rooms.Insert(0, new Room { Id = 0, RoomNumber = "Please select a room" }); 
            ViewData["RoomId"] = new SelectList(rooms, "Id", "RoomNumber");
        
            var dentists = _context.Dentists.ToList();
            dentists.Insert(0, new Dentist { Id = 0, Name = "Please select a dentist" }); 
            ViewData["DentistId"] = new SelectList(dentists, "Id", "Name");

            var services = _context.Services.ToList();
            services.Insert(0, new Service { Id = 0, Name = "Please select a service" }); 
            ViewData["ServiceId"] = new SelectList(services, "Id", "Name");

            var startSlots = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "" },
                new SelectListItem { Value = "1", Text = "9h" },
                new SelectListItem { Value = "2", Text = "10h" },
                new SelectListItem { Value = "3", Text = "11h" },
                new SelectListItem { Value = "4", Text = "13h" },
                new SelectListItem { Value = "5", Text = "14h" },
                new SelectListItem { Value = "6", Text = "15h" },
                new SelectListItem { Value = "7", Text = "16h" },
                new SelectListItem { Value = "8", Text = "17h" }
            };
            ViewData["StartSlot"] = new SelectList(startSlots, "Value", "Text");
            return Page();
        }

        [BindProperty]
        public BusinessObjects.Entities.Appointment Appointment { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Appointments.Add(Appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
