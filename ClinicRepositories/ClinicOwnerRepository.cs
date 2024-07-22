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


            var services = _context.Services.Include(item => item.Appointments).ToList();

            while (startDate.Date <= endDate.Date)
            {
                // Pre-load appointments with dentist and service information (eager loading)
                var dailyAppointments = _context.Appointments.Include(item => item.Service).Include(item => item.Dentist).Where(item => item.AppointDate.Date == startDate.Date && (item.Status == 5 || item.Status == 4 || item.Status == 6)).ToList();

                // Check if there are any appointments for this date
                if (dailyAppointments.Count != 0)
                {
                    var dentistAppointmentGroups = _context.Dentists.Include(item => item.Appointments).Select(item => new { Key = item, Value = item.Appointments.Where(item => item.AppointDate.Date == startDate.Date && (item.Status == 3 || item.Status == 5 || item.Status == 4 || item.Status == 6)).Count() });
                    var serviceAppointmentGroups = _context.Services.Include(item => item.Appointments).Select(item => new { Key = item, Value = item.Appointments.Where(item => item.AppointDate.Date == startDate.Date && (item.Status == 3 || item.Status == 5 || item.Status == 4 || item.Status == 6)).Count() });


                    // **Dentist Metrics**

                    var maxDentistAppointmentCount = dentistAppointmentGroups.OrderByDescending(item => item.Value).FirstOrDefault()?.Value ?? 0;
                    var minDentistAppointmentCount = dentistAppointmentGroups.OrderBy(group => group.Value).FirstOrDefault()?.Value ?? 0;

                    var dentistsWithMostAppointments = dentistAppointmentGroups
                      .Where(group => group.Value == maxDentistAppointmentCount)
                      .Select(group => group.Key)
                      .ToList();
                    var dentistsWithLeastAppointments = dentistAppointmentGroups
                      .Where(group => group.Value == minDentistAppointmentCount)
                      .Select(group => group.Key)
                      .ToList();


                    // **Service Metrics**

                    var maxServiceAppointmentCount = serviceAppointmentGroups.OrderByDescending(item => item.Value).FirstOrDefault()?.Value ?? 0;
                    var minServiceAppointmentCount = serviceAppointmentGroups.OrderBy(item => item.Value).FirstOrDefault()?.Value ?? 0;

                    var servicesWithMostAppointments = serviceAppointmentGroups
                      .Where(group => group.Value == maxServiceAppointmentCount)
                      .Select(group => group.Key)
                      .ToList();
                    var servicesWithLeastAppointments = serviceAppointmentGroups
                      .Where(group => group.Value == minServiceAppointmentCount)
                      .Select(group => group.Key)
                      .ToList();


                    reports.Add(new()
                    {
                        Date = startDate,
                        TotalAppointment = dailyAppointments.Count,
                        ReportDentistAppointment = dentistAppointmentGroups.ToDictionary(g => g.Key.Id, g => g.Value),
                        ReportServicesAppointment = serviceAppointmentGroups.ToDictionary(g => g.Key.Id, g => g.Value),

                        MostAppointmentDentist = dentistsWithMostAppointments,
                        LeastAppointmentDentist = dentistsWithLeastAppointments,
                        LeastAppointmentAmountOfDentist = minDentistAppointmentCount,
                        MostAppointmentAmountOfDentist = maxDentistAppointmentCount
          ,
                        MostPopularService = servicesWithMostAppointments,
                        LeastPopularService = servicesWithLeastAppointments,
                        LeastAppointmentAmountOfService = minServiceAppointmentCount,
                        MostAppointmentAmountOfService = maxServiceAppointmentCount,

                        TotalRevenue = CalculateTotalRevenue(dailyAppointments),
                        RevenuePerAppointment = CalculateRevenuePerAppointment(dailyAppointments),
                        RevenuePerCustomer = CalculateRevenuePerCustomer(dailyAppointments)
                    });
                }

                startDate = startDate.AddDays(1);
            }
            return reports;
        }

        private decimal CalculateTotalRevenue(List<Appointment> dailyAppointments)
        {

            decimal totalRevenue = dailyAppointments.Sum(appointment => appointment.Status == (int)AppointmentStatus.LateCanceled ? appointment.Service.Cost / 2 ?? 0 : appointment.Service.Cost ?? 0);
            return totalRevenue;
        }

        private decimal CalculateRevenuePerAppointment(List<Appointment> dailyAppointments)
        {
            if (dailyAppointments.Count == 0)
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
        public ClinicReportDataObject GetClinicReportTotal(DateTime startDate, DateTime endDate)
        {
            var dailyAppointments = _context.Appointments.Include(item => item.Service).Include(item => item.Dentist).Where(item => item.AppointDate.Date >= startDate.Date && item.AppointDate.Date <= endDate.Date && (item.Status == 5 || item.Status == 4 || item.Status == 6)).ToList();

            // Check if there are any appointments for this date
            var dentistAppointmentGroups = _context.Dentists.Include(item => item.Appointments).Select(item => new { Key = item, Value = item.Appointments.Where(item => item.AppointDate.Date >= startDate.Date && item.AppointDate.Date <= endDate.Date && (item.Status == 3 || item.Status == 5 || item.Status == 4 || item.Status == 6)).Count() });
            var serviceAppointmentGroups = _context.Services.Include(item => item.Appointments).Select(item => new { Key = item, Value = item.Appointments.Where(item => item.AppointDate.Date >= startDate.Date && item.AppointDate.Date <= endDate.Date && (item.Status == 3 || item.Status == 5 || item.Status == 4 || item.Status == 6)).Count() });


            // **Dentist Metrics**

            var maxDentistAppointmentCount = dentistAppointmentGroups.OrderByDescending(item => item.Value).FirstOrDefault()?.Value ?? 0;
            var minDentistAppointmentCount = dentistAppointmentGroups.OrderBy(item => item.Value).FirstOrDefault()?.Value ?? 0;

            var dentistsWithMostAppointments = dentistAppointmentGroups
              .Where(group => group.Value == maxDentistAppointmentCount)
              .Select(group => group.Key)
              .ToList();
            var dentistsWithLeastAppointments = dentistAppointmentGroups
              .Where(group => group.Value == minDentistAppointmentCount)
              .Select(group => group.Key)
              .ToList();


            // **Dentist Metrics**

            var maxServiceAppointmentCount = serviceAppointmentGroups.OrderByDescending(item => item.Value).FirstOrDefault()?.Value ?? 0;
            var minServiceAppointmentCount = serviceAppointmentGroups.OrderBy(item => item.Value).FirstOrDefault()?.Value ?? 0;

            var servicesWithMostAppointments = serviceAppointmentGroups
              .Where(group => group.Value == maxServiceAppointmentCount)
              .Select(group => group.Key)
              .ToList();
            var servicesWithLeastAppointments = serviceAppointmentGroups
              .Where(group => group.Value == minServiceAppointmentCount)
              .Select(group => group.Key)
              .ToList();


            return new()
            {
                Date = startDate,
                TotalAppointment = dailyAppointments.Count,
                ReportDentistAppointment = dentistAppointmentGroups.ToDictionary(g => g.Key.Id, g => g.Value),
                ReportServicesAppointment = serviceAppointmentGroups.ToDictionary(g => g.Key.Id, g => g.Value),

                MostAppointmentDentist = dentistsWithMostAppointments,
                LeastAppointmentDentist = dentistsWithLeastAppointments,
                LeastAppointmentAmountOfDentist = minDentistAppointmentCount,
                MostAppointmentAmountOfDentist = maxDentistAppointmentCount
      ,
                MostPopularService = servicesWithMostAppointments,
                LeastPopularService = servicesWithLeastAppointments,
                LeastAppointmentAmountOfService = minServiceAppointmentCount,
                MostAppointmentAmountOfService = maxServiceAppointmentCount,

                TotalRevenue = CalculateTotalRevenue(dailyAppointments),
                RevenuePerAppointment = CalculateRevenuePerAppointment(dailyAppointments),
                RevenuePerCustomer = CalculateRevenuePerCustomer(dailyAppointments)
            };
        }
        public bool InformationIsUnique(string phone, string email)
        {
            return !_context.Dentists.Any(item => item.Phone == phone || item.Email == email);
        }
    }
}