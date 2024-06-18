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
        ViewData["ClinicId"] = new SelectList(_context.Clinics, "ClinicId", "ClinicId");
        ViewData["DentistId"] = new SelectList(_context.Dentists, "DentistId", "DentistId");
        ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId");
        ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId");
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
