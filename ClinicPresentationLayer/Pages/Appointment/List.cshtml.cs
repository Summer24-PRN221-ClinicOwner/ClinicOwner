using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Extension;
using ClinicServices;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class ListModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDentistService _dentistService;
        public ListModel(IAppointmentService appointmentService, IDentistService dentistService)
        {
            _appointmentService = appointmentService;
            _dentistService = dentistService;
        }
        [BindProperty(SupportsGet = true)]
        public int PageWeek { get; set; } = 0;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; } = default;
        [BindProperty(SupportsGet = true)]
        public Dentist DentistName { get; set; }
        public AppointmentDentistSchedule AppointmentSchedule { get; set; }
        
        public async Task<IActionResult> OnGet()
        {
            User currentAcc = HttpContext.Session.GetObject<User>("UserAccount");
            if (currentAcc.Role != 1)
            {
                return RedirectToPage("/Privacy");
            }
            try
        {
                Id = currentAcc.Id;
                Console.WriteLine(PageWeek);
                AppointmentSchedule = await _appointmentService.GetAppoinmentSchedule(PageWeek, Id);
                DentistName = await _dentistService.GetByIdAsync(Id);
                //Console.WriteLine(AppointmentSchedule.Monday.);
                return Page();
        }
        catch (Exception ex)
        {
            return Page();  
        }
        }

        public IActionResult OnGetNext()
        {
            PageWeek++;
            return RedirectToPage(new { PageWeek });
        }

        public IActionResult OnGetPrevious()
        {
            if (PageWeek > 0)
            {
                PageWeek--;
            }
            return RedirectToPage(new { PageWeek });
        }
        public IActionResult OnGetCurrent()
        {
            PageWeek = 0;
            return RedirectToPage(new { PageWeek });
        }
        public string[,] Schedule { get; set; } = new string[10, 7];

    }
}
