using BusinessObjects;
using BusinessObjects.Entities;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ClinicPresentationLayer.Pages
{
    public class ReportModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IReportService _reportService;

        [BindProperty]
        public Report Report { get; set; } = new Report();

        [BindProperty]
        public BusinessObjects.Entities.Appointment Appointment { get; set; } = default!; // Initialize as needed

        public string DefaultTemplate { get; set; } = "";

        public ReportModel(IAppointmentService appointmentService, IReportService reportService)
        {
            _appointmentService = appointmentService;
            _reportService = reportService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Appointment = await _appointmentService.GetByIdAsync(id);
            if (Appointment == null)
            {
                return NotFound();
            }
            var content = $"<b>Patient Name:</b> {Appointment.Patient.Name},<br/><br/>" +
                          $"<b>Appointment Date:</b> {Appointment.AppointDate:dd/MM/yyyy}<br/>" +
                          $"<b>Service name:</b> {Appointment.Service.Name}<br/>" +
                          $"<b>Room ID:</b> {Appointment.Room.RoomNumber}<br/>" +
                          "<b>Diagnoses:</b> <ul>" +
                              "<li>Diagnosis 1</li>" +
                              "<li>Diagnosis 2</li>" +
                              "<li>Diagnosis 3</li>" +
                          "</ul><br/>" +
                          "<b>Treatments:</b> <ul>" +
                              "<li>Treatment 1</li>" +
                              "<li>Treatment 2</li>" +
                              "<li>Treatment 3</li>" +
                          "</ul><br/>" +
                        $"<b>Dentist name:</b> {Appointment.Dentist.Name}<br/>";
            DefaultTemplate = content;
            Report.Data = DefaultTemplate;
            Report.AppointmentId = id;
            return Page();
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
