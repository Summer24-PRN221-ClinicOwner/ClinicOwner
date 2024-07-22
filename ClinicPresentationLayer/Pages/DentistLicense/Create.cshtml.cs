using BusinessObjects.Entities;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.DentistLicense
{
    public class CreateModel : PageModel
    {
        private readonly ILicenseService _licenseService;

        public CreateModel(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }
        [BindProperty]
        public int DentistId { get; set; }
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
            License.DentistId = DentistId;
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _licenseService.AddAsync(License);
            return RedirectToPage("./Index");
        }
    }
}
