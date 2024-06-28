using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Entities;
using ClinicRepositories;
using ClinicServices.Interfaces;

namespace ClinicPresentationLayer.Pages.ServiceClinic
{
    public class CreateModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public CreateModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _serviceService.AddAsync(Service);

            return RedirectToPage("./Index");
        }
    }
}
