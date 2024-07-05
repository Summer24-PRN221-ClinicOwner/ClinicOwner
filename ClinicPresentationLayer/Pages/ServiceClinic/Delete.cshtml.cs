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

namespace ClinicPresentationLayer.Pages.ServiceClinic
{
    public class DeleteModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public DeleteModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        [BindProperty]
        public Service Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = _serviceService.GetByIdAsync(id);

            if (service == null)
            {
                return NotFound();
            }
            else
            {
                Service = await service;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = _serviceService.GetByIdAsync(id);
            if (service != null)
            {
                _serviceService.DeleteAsync(id);
            }

            return RedirectToPage("./Index");
        }
    }
}
