using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using ClinicRepositories;
using ClinicServices.Interfaces;
using System.Transactions;
using ClinicPresentationLayer.Extension;

namespace ClinicPresentationLayer.Pages.ProfileUser
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IPatientService _patientService;
        private readonly IDentistService _dentistService;
        private readonly IClinicOwnerService _clinicalOwnerService;

        public EditModel(IUserService userService, IPatientService patientService, IDentistService dentistService, IClinicOwnerService clinicOwnerService)
        {
            _userService = userService;
            _patientService = patientService;
            _dentistService = dentistService;
            _clinicalOwnerService = clinicOwnerService;
        }

        [BindProperty]
        public User User { get; set; } = default!;
        [BindProperty]
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
                return RedirectToPage("/Privacy");
                //if (currentAcc.Role == 1)
                //{
                //    if (user.Role != 2)
                //    {
                //        return RedirectToPage("/Privacy");
                //    }
                //}
            }
            if (user == null)
            {
                return NotFound();
            }
            User = user;
            if (currentAcc.Role == 2)
            {
                if (patient == null)
                {
                    return NotFound();
                }
                Patient = patient;
            }
            else if (currentAcc.Role == 1) 
            {
                if (dentist == null)
                {
                    return NotFound();
                }
                Dentist = dentist;
            }
            else if (currentAcc.Role == 0)
            {
                if (owner == null)
                {
                    return NotFound();
                }
                ClinicOwner = owner;
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await _userService.UpdateAsync(User);
            User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            if (currentAcc.Role == 2)
            {
                await _patientService.UpdateAsync(Patient);
            } else if (currentAcc.Role == 1)
            {
                await _dentistService.UpdateAsync(Dentist);
            }else
            {
                await _clinicalOwnerService.UpdateAsync(ClinicOwner);
            }
            return RedirectToPage("/ProfileUser/Details", new { id = User.Id });
        }
    }
}
