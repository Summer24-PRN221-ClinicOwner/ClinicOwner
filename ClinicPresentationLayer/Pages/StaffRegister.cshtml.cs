using BusinessObjects.Entities;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClinicPresentationLayer.Pages
{
    public class StaffRegisterModel : PageModel
    {
        private readonly IPatientService _patientService;
        public StaffRegisterModel(IPatientService patientService)
        {
            _patientService = patientService;
        }
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public DateOnly DateOfBirth { get; set; }

        [BindProperty]
        public string Gender { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string Address { get; set; }

        public void OnGet()
        {
            // Optional: Initialize properties or perform logic when the page is loaded
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User newUser = new() { Username = Username, Role = 2, Password = Password };
            Patient newPatient = new() { Email = Email, Name = Name, 
                                        Address = Address, DateOfBirth = DateOfBirth,
                                        Gender = Gender, Phone = Phone
                                        };
            var check = await _patientService.StaffAddAsync(newPatient, newUser);
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

