using BusinessObjects;
using BusinessObjects.Entities;

namespace ClinicServices.Interfaces
{
    public interface IAppointmentService
    {
        Task<List<Appointment>> GetAllAsync();
        Task<Appointment> GetByIdAsync(int id);
        Task<Appointment> AddAsync(Appointment entity);
        Task UpdateAsync(Appointment entity);
        Task DeleteAsync(int id);
        Task<List<Slot>> GetAvailableSlotAsync(DateTime date, int slotRequired);
        Task<Room> GetRoomAvailable(DateTime date, int slotRequired);
    }
}