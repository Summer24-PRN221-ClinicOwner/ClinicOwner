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
using System.ComponentModel.DataAnnotations;
using ClinicServices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace ClinicPresentationLayer.Pages.Dentists
{
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
        public BusinessObjects.Entities.Dentist Dentist { get; set; } = default!;
        public IList<BusinessObjects.Entities.Dentist> Dentists { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Dentists = await _dentistService.GetAllAsync();
            var clinicList = await _clinicService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinicList, "Id", "Name");

        }
        public async Task<IActionResult> OnPostAsync()
        {
            //ModelState.Clear();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User newUser = new() { Username = Username, Role = 1, Password = Password };

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
    }
}
