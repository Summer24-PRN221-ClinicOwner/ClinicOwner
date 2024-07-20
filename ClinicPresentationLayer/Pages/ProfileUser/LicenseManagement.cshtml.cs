using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicPresentationLayer.Extension;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.ProfileUser
{
    [CustomAuthorize(UserRoles.Dentist)]
    public class LicenseManagement : PageModel
    {
        private readonly ILicenseService _licenseService; // Replace with your DbContext

        public LicenseManagement(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        public License License { get; set; }

        public async Task<IActionResult> OnGet(string licenseNumber, DateTime issueDate, DateTime expiredDate, string licenseType, int action)
        {
            var dentistId = int.Parse(HttpContext.Session.GetObject<User>("UserAccount").ToString() ?? throw new Exception("Invalid"));
            License = new License { DentistId = dentistId, LicenseNumber = licenseNumber, IssueDate = issueDate, ExpireDate = expiredDate, LicenceType = licenseType };
            //if (action == 1)
            //{
            //    await OnPost(License);
            //}
            //else if (action == 2)
            //{
            //    await OnDelete(License);
            //}
            return Page();
        }

        public async Task<IActionResult> OnPost(License license)
        {
            await _licenseService.GetByIdAsync(license.Id);
            if (license == null)
            {
                await _licenseService.AddAsync(license);
            }
            else
            {
                await _licenseService.UpdateAsync(license);
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnDelete(License license)
        {
            await _licenseService.DeleteAsync(license.Id);
            return RedirectToPage();

        }
    }
}
