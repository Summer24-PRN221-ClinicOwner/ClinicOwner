using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        public Task<bool> AddAppointmentAsync(Appointment appointment);
        public Task<List<Appointment>> GetByDate(DateTime date, int dentistId);
        public Task<List<Appointment>> GetByPatientId(int id);
        public Task<Appointment> GetAppointmentsByIdAsync(int  appointId);
        public Task<List<Appointment>> GetAppointmentsBeforeDaysAsync(int days);
        public void SaveChanges();
        public void Dispose();
        Task<int> GetAppointmentCountAsync();
        Task<int> GetTodayAppointmentCountAsync();
        Task<decimal> GetTodayTotalEarningsAsync();
        Task<int> GetTomorrowAppointmentAsync();
    }
}
