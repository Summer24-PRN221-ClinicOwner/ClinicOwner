using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicPresentationLayer.Pages.Dentists
{
    [CustomAuthorize(UserRoles.ClinicOwner)]
    public class IndexModel : PageModel
    {
        private readonly IDentistService _dentistService;
        private readonly IClinicService _clinicService;

        public IndexModel(IDentistService dentistService, IClinicService clinicService)
        {
            _dentistService = dentistService;
            _clinicService = clinicService;
        }
        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [BindProperty]
        public Dentist Dentist { get; set; } = default!;
        [BindProperty]
        public IList<Dentist> Dentists { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Dentists = await _dentistService.GetAllAsync();
            var clinicList = await _clinicService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinicList, "Id", "Name");

        }
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            try
            {
                Dentists = await _dentistService.GetAllAsync();
                User newUser = new() { Username = Username, Role = UserRoles.Dentist, Password = Password };

                var result = await _dentistService.AddAsync(Dentist, newUser);
                if (result != null)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

        }
    }
}
