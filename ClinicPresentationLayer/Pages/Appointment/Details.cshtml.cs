using BusinessObjects;
using BusinessObjects.Entities;
using ClinicPresentationLayer.Authorization;
using ClinicPresentationLayer.Extension;
using ClinicServices;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.Appointment
{
    [CustomAuthorize(UserRoles.Dentist, UserRoles.Patient)]
    public class DetailsModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IReportService _reportService;

        public DetailsModel(IAppointmentService appointmentService, IReportService reportService)
        {
            _appointmentService = appointmentService;
            _reportService = reportService;
        }

        public string GetTimeFromSlot(int slot)
        {
            return slot switch
            {
                1 => "7:00",
                2 => "8:00",
                3 => "9:00",
                4 => "10:00",
                5 => "11:00",
                6 => "13:00",
                7 => "14:00",
                8 => "15:00",
                9 => "16:00",
                10 => "17:00",
                _ => "Invalid Slot"
            };
        }

        public User CurrentUser { get; set; }
        [BindProperty]
        public BusinessObjects.Entities.Appointment Appointment { get; set; } = default!;
        public string DefaultTemplate { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CurrentUser = HttpContext.Session.GetObject<User>("UserAccount");
            if (CurrentUser == null)
            {
                return RedirectToPage("/Login");
            }
            var appointment = await _appointmentService.GetAppointmentsByIdAsync(id.Value);
            if (appointment == null)
            {
                return NotFound();
            }
            else
            {
                Appointment = appointment;
                if (CurrentUser.Id != Appointment.PatientId && CurrentUser.Id != Appointment.DentistId)
                {
                    return RedirectToPage("/Unauthorized");
                }

                if (Appointment.Report == null && CurrentUser.Role == UserRoles.Dentist)
                {
                    var content =
                          "<b>Diagnoses:</b> <ul>" +
                              "<li>Diagnosis 1</li>" +
                              "<li>Diagnosis 2</li>" +
                              "<li>Diagnosis 3</li>" +
                          "</ul><br/>" +
                          "<b>Treatments:</b> <ul>" +
                              "<li>Treatment 1</li>" +
                              "<li>Treatment 2</li>" +
                              "<li>Treatment 3</li>" +
                          "</ul><br/>";
                    DefaultTemplate = content;
                    Appointment.Report = new Report
                    {
                        Data = DefaultTemplate,
                        AppointmentId = Appointment.Id
                    };
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnPost(int id)
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            try
            {
                CurrentUser = HttpContext.Session.GetObject<User>("UserAccount");
                var appointment = await _appointmentService.GetAppointmentsByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound();
                }
                bool statusUpdated = false;
                // Update the appointment status if it's not already 'Reported'
                if (appointment.Status != (int)AppointmentStatus.Reported)
                {
                    statusUpdated = await _appointmentService.UpdateAppointmentStatus(appointment.Id, (int)AppointmentStatus.Reported, null);
                }

                if (statusUpdated)
                {
                    TempData["SuccessMessage"] = "Updated appointment status successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update appointment status.";
                    return Page();
                }

                // Create or update the report
                if (appointment.Report == null)
                {
                    appointment.Report = new Report();
                }

                appointment.Report.Name = Appointment.Report.Name;
                appointment.Report.Data = Appointment.Report.Data;
                appointment.Report.GeneratedDate = DateTime.UtcNow.AddHours(7);
                appointment.Report.AppointmentId = appointment.Id;

                var reportResult = await _reportService.AddOrUpdateAsync(appointment.Report);
                if (reportResult == null)
                {
                    TempData["ErrorMessage"] = "Error when creating or updating the report";
                    return Page();
                }

                TempData["SuccessMessage"] = "Create/Update report successfully";
                return RedirectToPage("/Appointment/List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while processing your request {ex.Message}";
                return Page();
            }
        }
    }
}
