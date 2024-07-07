using BusinessObjects;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages
{
    public class ClinicReportModel : PageModel
    {
        private readonly IClinicOwnerService _clinicOwnerService;

        public ClinicReportModel(IClinicOwnerService clinicOwnerService)
        {
            _clinicOwnerService = clinicOwnerService;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ClinicReportDataObject> ReportData { get; set; } // Assuming the full namespace for ClinicReportDataObject

        public void OnGet()
        {
            StartDate = new DateTime(2024, 1, 1);
            EndDate = new DateTime(2025, 1, 1);
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                ReportData = _clinicOwnerService.MakeClinicReport(new DateTime(2024, 7, 1), new DateTime(2025, 7, 2));
                ReportData.Add(_clinicOwnerService.MakeClinicReportTotal(new DateTime(2024, 7, 1), new DateTime(2025, 7, 2)));
            }

            return Page();
        }
    }
}