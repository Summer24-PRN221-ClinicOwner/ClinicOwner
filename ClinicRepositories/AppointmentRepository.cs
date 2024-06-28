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

        public Task<List<Appointment>> GetByPatientId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
