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
           return  await _context.Appointments.Where(ap => ap.AppointDate.Date == date.Date).ToListAsync();
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
    }
}
