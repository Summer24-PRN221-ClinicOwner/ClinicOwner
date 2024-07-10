using BusinessObjects.Entities;
using ClinicPresentationLayer.Extension;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicPresentationLayer.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        public LoginModel(IUserService userService)
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
        public bool RememberMe { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User check = await _userService.LoginAsync(Username, Password);
            if (check != null)
            {
                HttpContext.Session.SetObject("UserAccount", check);
                return RedirectToPage("/MainPage");
            }
            else
            {
               TempData["ErrorMessage"] = "Invalid username or password.";
                return Page();
            }
        }
    }
}
