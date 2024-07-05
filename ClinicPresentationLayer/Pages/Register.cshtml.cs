using BusinessObjects.Entities;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicPresentationLayer.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IPatientService _patientService;
        public RegisterModel(IPatientService patientService)
        {
            _patientService = patientService;
        }
        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [Required]
        public string Email { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User newUser = new() { Username = Username, Role = 2, Password = Password };
            var check = await _patientService.AddAsync(new Patient { Email = Email, Name = Name }, newUser);
            if (check != null)
            {
                return RedirectToPage("/Login");
            }
            else
            {
                return Page();
            }
        }
    }
}
