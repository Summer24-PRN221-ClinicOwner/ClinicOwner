using BusinessObjects;
using ClinicPresentationLayer.Authorization;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages
{
    [CustomAuthorize(UserRoles.ClinicOwner)]
    public class ClinicReportModel : PageModel
    {
        private readonly IClinicOwnerService _clinicOwnerService;
        private readonly IAppointmentService _appointmentService;

        public ClinicReportModel(IClinicOwnerService clinicOwnerService, IAppointmentService appointmentService)
        {
            _clinicOwnerService = clinicOwnerService;
            _appointmentService = appointmentService;
        }
        [BindProperty]
        public DateTime StartDate { get; set; }
        [BindProperty]
        public DateTime EndDate { get; set; }
        public List<ClinicReportDataObject> ReportData { get; set; } // Assuming the full namespace for ClinicReportDataObject
        [BindProperty]
        public int TotalAppointment { get; set; }
        [BindProperty]
        public int TotalTodayAppoinemt { get; set; }
        [BindProperty]
        public decimal TotalEarnToday { get; set; }

        public async Task OnGet()
        {
            StartDate = new DateTime(2024, 1, 1);
            EndDate = new DateTime(2025, 1, 1);
            TotalAppointment = await _appointmentService.GetAppointmentCountAsync();
            TotalTodayAppoinemt = await _appointmentService.GetTodayAppointmentCountAsync();
            TotalEarnToday = await _appointmentService.GetTodayTotalEarningsAsync();
            Console.WriteLine(TotalEarnToday);
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                ReportData = _clinicOwnerService.MakeClinicReport(new DateTime(2024, 7, 1), new DateTime(2025, 7, 2));
                ReportData.Add(_clinicOwnerService.MakeClinicReportTotal(new DateTime(2024, 7, 1), new DateTime(2025, 7, 2)));
            }

            TotalAppointment = await _appointmentService.GetAppointmentCountAsync();
            TotalTodayAppoinemt = await _appointmentService.GetTodayAppointmentCountAsync();
            TotalEarnToday = await _appointmentService.GetTodayTotalEarningsAsync();

            return Page();
        }
    }
}