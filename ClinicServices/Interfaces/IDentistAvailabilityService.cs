using BusinessObjects.Entities;

namespace ClinicServices.Interfaces
{
    public interface IDentistAvailabilityService
    {
        Task<List<DentistAvailability>> GetAllAsync();
        Task<DentistAvailability> GetByIdAsync(int id);
        Task<DentistAvailability> AddAsync(DentistAvailability entity);
        Task UpdateAsync(DentistAvailability entity);
        Task DeleteAsync(int id);
        Task<List<Dentist>> GetAvailableDentist(DateTime date, int slotRequired, int serviceId);
    }
}