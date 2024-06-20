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

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class CreateModel : PageModel
    {
        private readonly ClinicRepositories.ClinicContext _context;

        public CreateModel(ClinicRepositories.ClinicContext context)
        {
            _context = context;
        }
        public bool IsServiceIdDisabled { get; set; }

        public IActionResult OnGet(int? id)
        {
            var rooms = _context.Rooms.ToList();
            rooms.Insert(0, new Room { Id = 0, RoomNumber = "Please select a room" }); 
            ViewData["RoomId"] = new SelectList(rooms, "Id", "RoomNumber");
        
            var dentists = _context.Dentists.ToList();
            dentists.Insert(0, new Dentist { Id = 0, Name = "Please select a dentist" }); 
            ViewData["DentistId"] = new SelectList(dentists, "Id", "Name");

            var services = _context.Services.ToList();
            services.Insert(0, new Service { Id = 0, Name = "Please select a service" }); 
            ViewData["ServiceId"] = new SelectList(services, "Id", "Name", id);

            ViewData["StartSlot"] = new SelectList(SlotDefiner.slots, "Key", "DisplayTime");

            IsServiceIdDisabled = id.HasValue;
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
