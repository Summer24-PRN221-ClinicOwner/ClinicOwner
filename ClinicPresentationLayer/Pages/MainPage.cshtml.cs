using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages
{
    public class MainPageModel : PageModel
    {
        private readonly IServiceService _service;

        [CustomAuthorize(UserRoles.ClinicOwner, UserRoles.Patient, UserRoles.Dentist)]
        public MainPageModel(IServiceService service)
        {
            _service = service;
        }

        public List<Service> Services { get; set; } = default!;
        public async Task<IActionResult> OnGet()
        {
            Services = await _service.GetAllAvailAsync();
            return Page();
        }
    }
}
