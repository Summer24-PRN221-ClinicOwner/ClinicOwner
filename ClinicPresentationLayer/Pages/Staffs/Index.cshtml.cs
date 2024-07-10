using BusinessObjects;
using BusinessObjects.Entities;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicPresentationLayer.Pages.Staffs
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;


        public IndexModel(IUserService userService)
        {
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
        public User User { get; set; } = default!;
        public IList<User> Staffs { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Staffs = await _userService.GetAllStaffAsync();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                Staffs = await _userService.GetAllStaffAsync();
                bool isUsernameExisted =  await _userService.IsUsernameExisted(Username);
                if (isUsernameExisted)
                {
                    TempData["ErrorMessage"] = "Username already existed";
                    return Page();
                }

                Staffs = await _userService.GetAllAsync();
                User newUser = new() { Username = Username, Role = UserRoles.Staff, Password = Password };

                var result = await _userService.AddAsync(newUser);
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
