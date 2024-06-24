using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObjects.Entities;
using ClinicRepositories;
using ClinicServices.Interfaces;

namespace ClinicPresentationLayer.Pages.Room
{
    public class CreateModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IClinicService _clinicService;

        public CreateModel(IRoomService roomService, IClinicService clinicService)
        {
            _roomService = roomService;
            _clinicService = clinicService;
        }

        public async Task<IActionResult> OnGet()
        {
            var clinicList = await _clinicService.GetAllAsync();
        ViewData["ClinicId"] = new SelectList(clinicList, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public BusinessObjects.Entities.Room Room { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

           bool result = await _roomService.AddAsync(Room);
            if (result)
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
