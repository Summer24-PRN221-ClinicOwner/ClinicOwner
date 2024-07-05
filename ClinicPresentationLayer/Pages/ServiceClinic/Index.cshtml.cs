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
    public class IndexModel : PageModel
    {
        private readonly IServiceService _serviceService;

        public IndexModel(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public IList<Service> Service { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Service = await _serviceService.GetAllAsync();
        }
    }
}
