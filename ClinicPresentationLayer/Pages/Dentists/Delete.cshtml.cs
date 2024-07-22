using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClinicServices.Interfaces;
using BusinessObjects.Entities;

namespace ClinicPresentationLayer.Pages.Dentists
{
    public class DeleteModel : PageModel
    {
        private readonly IDentistService _dentistService;

        public DeleteModel(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        [BindProperty]
        public Dentist Dentist { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Dentist = await _dentistService.GetByIdAsync(id.Value);

            if (Dentist == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dentist = await _dentistService.GetByIdAsync(id.Value);
            if (dentist == null)
            {
                TempData["ErrorMessage"] = "Dentist not found.";
                return RedirectToPage("./Index");
            }

            try
            {
                await _dentistService.DeleteAsync(id.Value);
                TempData["SuccessMessage"] = "Dentist deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error when deleting the dentist: " + ex.Message;
            }

            return RedirectToPage("./Index");
        }
    }
}
