using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicPresentationLayer.Extension;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.DentistServices
{
    [CustomAuthorize(UserRoles.ClinicOwner)]
    public class IndexModel : PageModel
    {
        public readonly IServiceService _serviceService;
        public readonly IDentistService _dentistService;
        [BindProperty]
        public string SearchTermModel { get; set; }
        [BindProperty]
        public Dentist Dentist { get; set; }
        public IndexModel(IDentistService dentistService, IServiceService serviceService)
        {
            _dentistService = dentistService;
            _serviceService = serviceService;
        }
        [BindProperty]
        public IList<ServiceOfDentist> Service { get; set; } = default!;
        [BindProperty]
        public List<int> SelectedServiceIds { get; set; } = new List<int>();

        public async Task OnGetAsync(int id, string? SearchTerm)
        {
            var user = HttpContext.Session.GetObject<User>("UserAccount");
            SearchTermModel = SearchTerm ?? "";
            var service = await _serviceService.GetAllAsync();
            Dentist = _dentistService.GetDentistById(id);
            var dentistService = _dentistService.GetDentistById(id).Services.ToList();
            if (Service == null)
                Service = service.Select(item => new ServiceOfDentist { Status = dentistService.Any(ser => ser.Id == item.Id), ServiceInfor = item }).ToList();
            foreach (var item in dentistService)
            {
                SelectedServiceIds.Add(item.Id);
            }
        }

        public async Task OnPostAsync()
        {
            var selectedServiceIds = Request.Form["selectedServices"].Select(int.Parse).ToList();
            Dentist.Services.Clear();
            foreach (var service in selectedServiceIds)
            {
                var item = await _serviceService.GetByIdAsync(service);
                if (service != null) Dentist.Services.Add(item);
            }
            _dentistService.UpdateDentistServices(Dentist);
            //Redirect ve profile
        }
    }
    public class ServiceOfDentist
    {
        public bool Status { get; set; } = default;
        public Service ServiceInfor { get; set; }
    }
}
