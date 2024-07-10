using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.ProfileUser
{
    [CustomAuthorize(UserRoles.ClinicOwner)]
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;

        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public IList<User> User { get; set; } = default!;

        public async Task OnGetAsync()
        {
            User = await _userService.GetAllAsync();
        }
    }
}
