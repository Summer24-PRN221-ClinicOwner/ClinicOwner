using BusinessObjects;
using ClinicPresentationLayer.Authorization;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClinicPresentationLayer.Pages.Room
{
    [CustomAuthorize(UserRoles.ClinicOwner)]
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
        public IList<BusinessObjects.Entities.Room> Rooms { get; set; } = default!;

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
