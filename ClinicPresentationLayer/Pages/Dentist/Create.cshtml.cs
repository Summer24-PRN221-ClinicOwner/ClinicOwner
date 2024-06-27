using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Entities;
using ClinicRepositories;

namespace ClinicPresentationLayer.Pages.Dentist
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
        ViewData["ClinicId"] = new SelectList(_context.Clinics, "Id", "Id");
        ViewData["Id"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public BusinessObjects.Entities.Dentist Dentist { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Dentists.Add(Dentist);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
