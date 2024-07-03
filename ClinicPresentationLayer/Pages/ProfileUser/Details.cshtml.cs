using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using ClinicRepositories;
using ClinicServices.Interfaces;
using ClinicPresentationLayer.Extension;

namespace ClinicPresentationLayer.Pages.ProfileUser
{
    public class DetailsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;
        private readonly IDentistService _dentistService;
        private readonly IClinicOwnerService _clinicalOwnerService;
        public DetailsModel(IUserService userService, IPatientService patientService, IDentistService dentistService, IClinicOwnerService clinicOwnerService)
        {
            _userService = userService;
            _patientService = patientService;
            _dentistService = dentistService;
            _clinicalOwnerService = clinicOwnerService;
        }

        public User User { get; set; } = default!;
        public Patient Patient { get; set; } = default!;
        [BindProperty]
        public ClinicOwner ClinicOwner { get; set; } = default!;
        [BindProperty]
        public Dentist Dentist { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
			User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            var user = await _userService.GetByIdAsync(id.Value);
            var patient = await _patientService.GetByIdAsync(id.Value);
            var dentist = await _dentistService.GetByIdAsync(id.Value);
            var owner = await _clinicalOwnerService.GetByIdAsync(id.Value);
            if (currentAcc == null)
			{
				return RedirectToPage("/Login");
			}
			else if (currentAcc.Id != id && currentAcc.Role == 2)
			{
				return RedirectToPage("/Privacy");
			}
            else if (currentAcc.Id != id)
            {
                if (currentAcc.Role == 1)
                {
                    if (user.Role != 2)
                    {
                        return RedirectToPage("/Privacy");
                    }
                }
            }
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
                Patient = patient;
                Dentist = dentist;
                ClinicOwner = owner;
            }
            return Page();
        }
    }
}
