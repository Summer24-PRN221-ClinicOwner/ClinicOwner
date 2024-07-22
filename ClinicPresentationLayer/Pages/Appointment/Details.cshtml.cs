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
        [BindProperty]
        public Report Report { get; set; } = new Report();
        [BindProperty]
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
                if (appointment.Report == null && CurrentUser.Role == UserRoles.Dentist)
                {
                    DefaultTemplate = content;
                    Report.Data = DefaultTemplate;
                    Report.AppointmentId = Appointment.Id;
                }
                else if (appointment.Report == null && CurrentUser.Role == UserRoles.Patient)
                {
                    Report.Data = "<p>(Not reported yet)</p>";
                    Report.AppointmentId = appointment.Id;
                }
                else if (appointment.Report != null)
                {
                    {
                        Report.Data = appointment.Report.Data;
                        Report.AppointmentId = appointment.Id;
                    }

                }
                return Page();
            }
        }
            public async Task<IActionResult> OnPost()
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }
                Report.AppointmentId = Appointment.Id;
                Report.GeneratedDate = DateTime.UtcNow.AddHours(7);
                var result = await _reportService.AddAsync(Report);
                if (result == null)
                {
                    ModelState.AddModelError("create_report_error", "Error when create report");
                    return Page();
                }
                else
                {
                    try
                    {
                        await _appointmentService.UpdateAppointmentStatus(Appointment.Id, (int)AppointmentStatus.Reported, null);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("update_appointment_status_error", "Error when update status for appointment after create report");
                        return Page();
                    }
                }

                return RedirectToPage("/Appointment/List");
            }
        }
    }

