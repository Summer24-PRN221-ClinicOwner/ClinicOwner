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

namespace ClinicPresentationLayer.Pages.Room
{
    public class EditModel : PageModel
    {
        private readonly IRoomService _roomService;
        private readonly IClinicService _clinicService;

        public EditModel(IRoomService roomService, IClinicService clinicService)
        {
            _roomService = roomService;
            _clinicService = clinicService; 
        }

        [BindProperty]
        public BusinessObjects.Entities.Room Room { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Room = await _roomService.GetByIdAsync(id.Value);

            if (Room == null)
            {
                return NotFound();
            }
            var clinicList = await _clinicService.GetAllAsync();
            ViewData["ClinicId"] = new SelectList(clinicList, "Id", "Name", Room.ClinicId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _roomService.UpdateAsync(Room);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_roomService.GetByIdAsync(Room.Id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
