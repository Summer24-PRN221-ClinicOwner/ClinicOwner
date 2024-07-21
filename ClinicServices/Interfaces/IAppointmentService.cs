using BusinessObjects;
using BusinessObjects.Entities;

namespace ClinicServices.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment> GetByIdAsync(int id);
        Task<Appointment> AddAsync(Appointment entity, Payment payment);
        Task UpdateAsync(Appointment entity);
        Task DeleteAsync(int id);
        Task<List<Slot>> GetAvailableSlotAsync(DateTime date, int slotRequired);
        Room GetRoomAvailable(DateTime date, int slotRequired, int startSlot);
        Task<AppointmentDentistSchedule> GetAppoinmentSchedule(int pageWeek, int dentistId);
        Task<List <Appointment>> GetAppoinmentHistoryAsync(int patientId);
        Task<List<Appointment>> GetAppointmentsBeforeDaysAsync(int days);
        Task<bool> UpdateAppointmentStatus(int appointId, int Status, DateTime? newDate);
        public Task<Appointment> GetAppointmentsByIdAsync(int id);
<<<<<<< HEAD
        Task<bool> IsValidStatusTransition(int currentStatus, int newStatus, DateTime createDate, string paymenStatus, DateTime? newDate);
        Task<List<Appointment>> GetAppointmentsOfTodayAsync();
=======
        Task<int> GetAppointmentCountAsync();
        Task<int> GetTodayAppointmentCountAsync();
        Task<decimal> GetTodayTotalEarningsAsync();
        Task<int> GetTomorrowAppointmentAsync();
>>>>>>> dc6c496b2fd0a1041c97bbe82eca870bb61f8921
    }
}