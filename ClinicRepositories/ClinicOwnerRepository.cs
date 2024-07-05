using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories
{
    public class ClinicOwnerRepository : GenericRepository<ClinicOwner>, IClinicOwnerRepository
    {
        public ClinicOwnerRepository() : base()
        {
        }

        public List<ClinicReportDataObject> GetClinicReport(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                var tmp = startDate;
                startDate = endDate;
                endDate = tmp;
            }

            List<ClinicReportDataObject> reports = new List<ClinicReportDataObject>();

            while (startDate.Date <= endDate.Date)
            {
                // Pre-load appointments with dentist and service information (eager loading)
                var dailyAppointments = _context.Appointments.Include(item => item.Service).Include(item => item.Dentist).ToList();

                // Check if there are any appointments for this date
                if (dailyAppointments.Any())
                {
                    var report = new ClinicReportDataObject();
                    report.Date = startDate;

                    // **Dentist Metrics**
                    var dentistAppointmentGroups = dailyAppointments.GroupBy(a => a.Dentist);
                    report.TotalAppointmentOfDentist = dentistAppointmentGroups.Count();
                    report.ReportDentistAppointment = dentistAppointmentGroups.ToDictionary(g => g.Key.Id, g => g.Count());

                    // Find maximum appointment count for dentists on this day
                    var maxDentistAppointmentCount = dentistAppointmentGroups.Max(group => group.Count());

                    // Dentists with most appointments
                    var dentistsWithMostAppointments = dentistAppointmentGroups
                      .Where(group => group.Count() == maxDentistAppointmentCount)
                      .Select(group => group.Key)
                      .ToList();
                    report.MostAppointmentDentist = dentistsWithMostAppointments;

                    // Find minimum appointment count for dentists on this day
                    var minDentistAppointmentCount = dentistAppointmentGroups.Min(group => group.Count());

                    // Dentists with least appointments
                    var dentistsWithLeastAppointments = dentistAppointmentGroups
                      .Where(group => group.Count() == minDentistAppointmentCount)
                      .Select(group => group.Key)
                      .ToList();
                    report.LeastAppointmentDentist = dentistsWithLeastAppointments;
                    report.LeastAppointmentAmountOfDentist = minDentistAppointmentCount;

                    // **Service Metrics** (similar logic as dentists)
                    var serviceAppointmentGroups = dailyAppointments.GroupBy(a => a.Service);
                    report.TotalAppointmentOfService = serviceAppointmentGroups.Count();
                    report.ReportServicesAppointment = serviceAppointmentGroups.ToDictionary(g => g.Key.Id, g => g.Count());

                    // Find maximum appointment count for services on this day
                    var maxServiceAppointmentCount = serviceAppointmentGroups.Max(group => group.Count());
                    // Services with most appointments
                    var servicesWithMostAppointments = serviceAppointmentGroups
                      .Where(group => group.Count() == maxServiceAppointmentCount)
                      .Select(group => group.Key)
                      .ToList();
                    report.MostPopularService = servicesWithMostAppointments;

                    // Find minimum appointment count for services on this day
                    var minServiceAppointmentCount = serviceAppointmentGroups.Min(group => group.Count());

                    // Services with least appointments
                    var servicesWithLeastAppointments = serviceAppointmentGroups
                      .Where(group => group.Count() == minServiceAppointmentCount)
                      .Select(group => group.Key)
                      .ToList();
                    report.LeastPopularService = servicesWithLeastAppointments;
                    report.LeastAppointmentAmountOfService = minServiceAppointmentCount; // Update to store minimum count

                    // Additional calculations (assuming you have logic to calculate these)
                    report.TotalRevenue = CalculateTotalRevenue(dailyAppointments);
                    report.RevenuePerAppointment = CalculateRevenuePerAppointment(dailyAppointments);
                    report.RevenuePerCustomer = CalculateRevenuePerCustomer(dailyAppointments);

                    reports.Add(report);
                }

                startDate = startDate.AddDays(1);
            }

            return reports;
        }
        private decimal CalculateTotalRevenue(List<Appointment> dailyAppointments)
        {
            decimal totalRevenue = dailyAppointments.Sum(appointment => appointment.Service.Cost ?? 0);
            return totalRevenue;
        }

        private decimal CalculateRevenuePerAppointment(List<Appointment> dailyAppointments)
        {
            if (!dailyAppointments.Any())
            {
                return 0; // Handle no appointments scenario
            }

            decimal totalRevenue = CalculateTotalRevenue(dailyAppointments);
            int appointmentCount = dailyAppointments.Count;
            decimal revenuePerAppointment = totalRevenue / appointmentCount;
            return revenuePerAppointment;
        }
        private decimal CalculateRevenuePerCustomer(List<Appointment> dailyAppointments)
        {
            if (!dailyAppointments.Any())
            {
                return 0; // Handle no appointments scenario
            }

            decimal totalRevenue = CalculateTotalRevenue(dailyAppointments);
            // Assuming appointments have a Customer property
            int customerCount = dailyAppointments.Select(a => a.Patient).Distinct().Count();
            decimal revenuePerCustomer = totalRevenue / customerCount;
            return revenuePerCustomer;
        }
    }
}