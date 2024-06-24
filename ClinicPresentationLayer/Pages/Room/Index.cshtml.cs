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
using ClinicServices;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicPresentationLayer.Pages.Room
{
    public class IndexModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IClinicService _clinicService;
        public IndexModel(IRoomService roomService, IClinicService clinicService)
        {
            _roomService = roomService;
            _clinicService = clinicService;
        }

        [BindProperty]
        public BusinessObjects.Entities.Room Room { get; set; } = default!;
        public IList<BusinessObjects.Entities.Room> Rooms { get; set; }= default!;

        public async Task OnGetAsync()
        {
            Rooms = await _roomService.GetAllAsync();
            var clinicList = await _clinicService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinicList, "Id", "Name");
            
        }
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
