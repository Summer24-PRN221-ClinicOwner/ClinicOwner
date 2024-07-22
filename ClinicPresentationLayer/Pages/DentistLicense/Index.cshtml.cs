using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.DentistLicense
{
    [CustomAuthorize(UserRoles.Dentist)]
    public class IndexModel : PageModel
    {
        private readonly IDentistService _dentistService;

        public IndexModel(IDentistService dentistService)
        {
            _dentistService = dentistService;
        }

        public IList<License> License { get; set; } = default!;
        public int DentistId { get; set; } = default!;

        public async Task OnGetAsync(int id)
        {
            DentistId = id;
            License = _dentistService.GetDentistById(id).Licenses.ToList();
        }
    }
}
