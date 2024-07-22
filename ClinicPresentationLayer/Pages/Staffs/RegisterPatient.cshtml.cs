using BusinessObjects.Entities;
using BusinessObjects;
using ClinicPresentationLayer.Authorization;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.Staffs
{
    [CustomAuthorize(UserRoles.Staff)]
    public class RegisterPatientModel : PageModel
    {
        private readonly IPatientService _patientService;

        public RegisterPatientModel(IPatientService patientService)
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

        public async Task<IActionResult> OnPostRegister()
        {
            try
            {
                User newUser = new() { Username = Username, Role = UserRoles.Patient, Password = Password };
                Patient newPatient = new()
                {
                    Email = Email,
                    Name = Name,
                    Address = Address,
                    DateOfBirth = DateOfBirth,
                    Gender = Gender,
                    Phone = Phone
                };
                var check = await _patientService.StaffAddAsync(newPatient, newUser);
                if (check != null)
                {
                    TempData["SuccessMessage"] = "Patient account created successfully.";
                    return RedirectToPage("/Staffs/Main");
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while creating the patient account.";
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
