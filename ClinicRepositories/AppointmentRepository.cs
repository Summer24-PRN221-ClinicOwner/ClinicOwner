using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private ClinicContext localContext;
        public AppointmentRepository() : base()
        {

        }

        public async Task<bool> AddAppointmentAsync(Appointment appointment)
        {
            localContext = new ClinicContext();
            try
            {
                await localContext.Appointments.AddAsync(appointment);

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Appointment>> GetByDate(DateTime date, int dentistId)
        {
            var a = await _context.Appointments
                 .Where(c => c.AppointDate.Date == date.Date && c.DentistId == dentistId)
                 .Include(a => a.Room)
                 .Include(a => a.Patient)
                 .Include(a => a.Service)
                 .Include(a => a.Dentist)
                 .ToListAsync();
            Console.WriteLine(a);
            return a;
        }

        public async Task<List<Appointment>> GetByPatientId(int id)
        {
            var result = await _context.Appointments
                                .Include(ap => ap.Room)
                                .Include(ap => ap.Dentist)
                                .Include(ap => ap.Service)
                                .Include(ap => ap.Patient)
                                .Include(ap => ap.Report)
                                .Where(ap => ap.PatientId == id)
                                .ToListAsync();
            return result;
        }
        public async Task<Appointment> GetAppointmentsByIdAsync(int id)
        {
            var result = await _context.Appointments
                                 .Include(a => a.Patient)
                                 .Include(a => a.Dentist)
                                 .Include(a => a.Room)
                                 .Include(a => a.Service)
                                 .Include(a=> a.Report)
                                 .FirstOrDefaultAsync(ap => ap.Id == id);
            if(result == null)
            {
                throw new Exception("No appointment found");
            }
            return result;
        }

        public async Task<List<Appointment>> GetAppointmentsBeforeDaysAsync(int days)
        {
            var targetDate = DateTime.Today.AddDays(days);

            return await _context.Appointments
                .Include(ap => ap.Patient)
                .Include(ap => ap.Dentist)
                .Include(ap => ap.Room)
                .Include(ap => ap.Service)
                .Where(ap => ap.AppointDate.Date == targetDate.Date)
                .ToListAsync();
        }
        public void SaveChanges()
        {
            localContext.SaveChanges();
        }
        public void Dispose()
        {
            localContext.Dispose();
        }
    }
}
