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
    public class DetailsModel : PageModel
    {
        private readonly IServiceService _serviceService
;

        public DetailsModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public Service Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceService.GetByIdAsync(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            else
            {
                Service = service;
            }
            return Page();
        }
    }
}
