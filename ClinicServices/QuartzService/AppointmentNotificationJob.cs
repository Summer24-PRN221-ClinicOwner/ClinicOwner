using BusinessObjects.Entities;
using BusinessObjects;
using ClinicServices.EmailService;
using ClinicServices.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ClinicServices.QuartzService
{
    [DisallowConcurrentExecution]
    public class AppointmentNotificationJob : IJob
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IEmailSender _emailSender;
        private readonly IJobExecutionLogService _jobExecutionLogService;
        private readonly ILogger<AppointmentNotificationJob> _logger;

        public AppointmentNotificationJob(IAppointmentService appointmentService, IEmailSender emailSender, ILogger<AppointmentNotificationJob> logger, IJobExecutionLogService jobExecutionLogService)
        {
            _appointmentService = appointmentService;
            _emailSender = emailSender;
            _logger = logger;
            _jobExecutionLogService = jobExecutionLogService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsBeforeDaysAsync(3);
                if (appointments != null)
                {
                    foreach (var appointment in appointments)
                    {
                        // Send email notification to the patient
                        await SendEmailToPatientAsync(appointment);
                    }
                    var executionTime = DateTime.UtcNow.AddHours(7);
                    await _jobExecutionLogService.LogExecutionTimeAsync("AppointmentNotificationJob", executionTime);

                    _logger.LogInformation("Appointment notifications sent successfully.");
                }
                else
                {
                    _logger.LogInformation("No Appointments to send notification.");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending appointment notifications.");
            }
        }

        private async Task SendEmailToPatientAsync(Appointment appointment)
        {
            var patient = appointment.Patient;
            var dentist = appointment.Dentist;
            var room = appointment.Room;
            var service = appointment.Service;
            var startSlot = SlotDefiner.NewSlot(appointment.StartSlot);

            var subject = "Your Appointment Details";
            var content = $"Dear {patient.Name},<br/><br/>" +
                          $"Here are the details of your upcoming appointment:<br/>" +
                          $"<b>Appointment Date:</b> {appointment.AppointDate:dd/MM/yyyy}<br/>" +
                          $"<b>Service name:</b> {service.Name}<br/>" +
                          $"<b>Dentist name:</b> {dentist.Name}<br/>" +
                          $"<b>Start Slot:</b> {startSlot.DisplayTime}<br/>" +
                          $"<b>Room ID:</b> {room.RoomNumber}<br/>" +
                          $"Thank you,<br/>";

            EmailAddress emailAddress = new() { Email = patient.Email, DisplayName = patient.Name };
            EmailService.Message message = new([emailAddress], subject, content);

            await _emailSender.SendEmailAsync(message);
        }
    }
}