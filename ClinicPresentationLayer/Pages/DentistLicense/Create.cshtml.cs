using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.DentistLicense
{
    [CustomAuthorize(UserRoles.Dentist)]
    public class CreateModel : PageModel
    {
        private readonly ILicenseService _licenseService;

        public CreateModel(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }
        [BindProperty]
        public int DentistId { get; set; } = default;
        public IActionResult OnGet(int id)
        {
            DentistId = id;
            return Page();
        }

        [BindProperty]
        public License License { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var list = await _licenseService.GetAllAsync();
            if (list.FirstOrDefault(item => item.LicenseNumber == License.LicenseNumber) != null)
            {
                TempData["ErrorMessage"] = "Seem like License Number has been already added into system! Please check and try again!";
                return Page();
            }
            License.DentistId = DentistId;
            var temp = await _licenseService.GetAllAsync();
            await _licenseService.AddAsync(License);
            return Redirect("./Index?id=" + DentistId);
        }
    }
}
