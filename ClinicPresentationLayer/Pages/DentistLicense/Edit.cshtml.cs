using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.DentistLicense
{
    [CustomAuthorize(UserRoles.Dentist)]
    public class EditModel : PageModel
    {
        private readonly IDentistService _dentistService;
        private readonly ILicenseService _licenseService;

        public EditModel(IDentistService dentistService, ILicenseService licenseService)
        {
            _dentistService = dentistService;
            _licenseService = licenseService;
        }

        [BindProperty]
        public License License { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var license = await _licenseService.GetByIdAsync(id ?? throw new Exception("Invalid Id"));
            if (license == null)
            {
                return NotFound();
            }
            License = license;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            _licenseService.UpdateLicense(License);
            return RedirectToPage("./Index");
        }

    }
}
