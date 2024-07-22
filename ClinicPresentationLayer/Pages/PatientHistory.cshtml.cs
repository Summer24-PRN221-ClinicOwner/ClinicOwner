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

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalPages { get; set; }
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
                TotalPages = (int)Math.Ceiling(allAppointments.Count / (double)PageSize);
                Appointments = allAppointments
                    .Where(a => a.Status.ToString().Equals(StatusFilter, System.StringComparison.OrdinalIgnoreCase))
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
            else
            {
                TotalPages = (int)Math.Ceiling(allAppointments.Count / (double)PageSize);
                Appointments = allAppointments
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }

            return Page();
        }
    }
}
