using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicPresentationLayer.Extension;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages
{
    [CustomAuthorize(UserRoles.Patient)]
    public class PatientHistoryModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;

        public PatientHistoryModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        [BindProperty]
        public List<BusinessObjects.Entities.Appointment> Appointments { get; set; } = new List<BusinessObjects.Entities.Appointment>();

        public async Task<IActionResult> OnGetAsync()
        {
            User currentUser = HttpContext.Session.GetObject<User>("UserAccount");
            if (currentUser == null)
            {
                return RedirectToPage("/Login");
            }
            else if (currentUser.Role != 2)
            {
                return RedirectToPage("/MainPage");
            }
            var patientId = currentUser.Id;

            Appointments = await _appointmentService.GetAppoinmentHistoryAsync(patientId);
            return Page();
        }
    }
}
