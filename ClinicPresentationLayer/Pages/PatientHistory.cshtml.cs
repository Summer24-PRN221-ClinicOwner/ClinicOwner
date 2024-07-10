using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicPresentationLayer.Extension;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        [BindProperty(SupportsGet = true)]
        public string StatusFilter { get; set; }

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

            var allAppointments = await _appointmentService.GetAppoinmentHistoryAsync(patientId);

            if (!string.IsNullOrEmpty(StatusFilter))
            {
                Appointments = allAppointments
                    .Where(a => a.Status.ToString().Equals(StatusFilter, System.StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            else
            {
                Appointments = allAppointments.ToList();
            }

            return Page();
        }
    }
}
