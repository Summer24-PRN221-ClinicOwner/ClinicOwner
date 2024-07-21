using BusinessObjects;
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
                if(check.Role == UserRoles.Patient)
                {
                    return RedirectToPage("/MainPage");
                }
                if(check.Role == UserRoles.Staff)
                {
                    return RedirectToPage("/Staffs/Main");
                }
                if(check.Role == UserRoles.Dentist)
                {
                    return RedirectToPage("/Appointment/List");
                }
                if(check.Role == UserRoles.ClinicOwner)
                {
                    return RedirectToPage("/ClinicReport");
                }
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
