using BusinessObjects.Entities;

namespace ClinicServices.Interfaces
{
    public interface IPatientService
    {
        Task<List<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(int id);
        Task<Patient> AddAsync(Patient entity, User user);
        Task<Patient> StaffAddAsync(Patient entity, User user);
        Task UpdateAsync(Patient entity);
        Task DeleteAsync(int id);
        Task<Patient> FindPatientAsync(string searchTerm);
    }
}
