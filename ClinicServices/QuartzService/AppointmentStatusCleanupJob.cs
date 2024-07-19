using BusinessObjects;
using BusinessObjects.Entities;
using ClinicServices.Interfaces;
using Microsoft.Extensions.Logging;
using Quartz;

namespace ClinicServices.QuartzService
{
    [DisallowConcurrentExecution]
    public class AppointmentStatusCleanupJob : IJob
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IJobExecutionLogService _jobExecutionLogService;
        private readonly ILogger<AppointmentStatusCleanupJob> _logger;

        public AppointmentStatusCleanupJob(IAppointmentService appointmentService, ILogger<AppointmentStatusCleanupJob> logger, IJobExecutionLogService jobExecutionLogService)
        {
            _appointmentService = appointmentService;
            _logger = logger;
            _jobExecutionLogService = jobExecutionLogService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsOfTodayAsync();
                if (appointments != null)
                {
                    foreach (var appointment in appointments)
                    {
                        // Update appointment status to Absent if the status is still Waiting
                        //if (appointment.Status == (int)AppointmentStatus.Waiting)
                        //{
                        //    appointment.Status = (int)AppointmentStatus.Absent;
                        //    await _appointmentService.UpdateAppointmentStatus(appointment.Id, (int)AppointmentStatus.Absent, null);
                        //}
                    }
                    var executionTime = DateTime.UtcNow.AddHours(7);
                    await _jobExecutionLogService.LogExecutionTimeAsync("AppointmentStatusCleanupJob", executionTime);
                    _logger.LogInformation("Appointment status cleanup completed successfully.");
                }
                else
                {
                    _logger.LogInformation("No appointments found for cleanup.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cleaning up appointment statuses.");
            }
        }
    }
}
