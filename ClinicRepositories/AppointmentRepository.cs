using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository() : base()
        {
        }

        public async Task<bool> AddAppointmentAsync(Appointment appointment)
        {
            try
            {
                await _context.Appointments.AddAsync(appointment);
                await _context.SaveChangesAsync();
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
                                .Where(ap => ap.PatientId == id)
                                .ToListAsync();
            return result;
        }
        public async Task<Appointment> GetAppointmentsByIdAsync(int id)
        {
            return await _context.Appointments
                                 .Include(a => a.Patient)
                                 .Include(a => a.Dentist)
                                 .Include(a => a.Room)
                                 .Include(a => a.Service)
                                 .FirstOrDefaultAsync(ap => ap.Id == id);
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
            _context.SaveChanges();
        }
    }
}
