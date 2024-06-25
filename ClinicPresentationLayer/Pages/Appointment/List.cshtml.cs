using BusinessObjects;
using ClinicServices;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class ListModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        public ListModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;

        }
        [BindProperty(SupportsGet = true)]
        public int PageWeek { get; set; } = 0;
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; } = 5;
        public AppointmentDentistSchedule AppointmentSchedule { get; set; }
        
        public async Task<IActionResult> OnGet()
        {
            try
        {
                Console.WriteLine(PageWeek);
                AppointmentSchedule = await _appointmentService.GetAppoinmentSchedule(PageWeek, Id);
                Console.WriteLine(AppointmentSchedule.Monday);
                return Page();
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., log, display error)
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
        public string[,] Schedule { get; set; } = new string[10, 7];

    }
}
