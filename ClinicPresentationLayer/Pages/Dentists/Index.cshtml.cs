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
        private readonly IUserService _userService;


        public IndexModel(IDentistService dentistService, IClinicService clinicService, IUserService userService)
        {
            _dentistService = dentistService;
            _clinicService = clinicService;
            _userService = userService;
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
            User newUser = new() { Username = Username, Role = UserRoles.Dentist, Password = Password };
            try
            {
                Dentists = await _dentistService.GetAllAsync();

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
                var listDentist = await _dentistService.GetAllAsync();
                if (listDentist.FirstOrDefault(item => item.Id == newUser.Id) != null)
                {
                    await _dentistService.DeleteAsync(newUser.Id);
                }
                var listUser = await _userService.GetAllAsync();
                if (listUser.FirstOrDefault(item => item.Id == newUser.Id) != null)
                {
                    await _userService.DeleteAsync(newUser.Id);
                }
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }

        }
    }
}
