using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        public Task<bool> AddAppointmentAsync(Appointment appointment);
        public Task<List<Appointment>> GetByDate(DateTime date);
    }
}
