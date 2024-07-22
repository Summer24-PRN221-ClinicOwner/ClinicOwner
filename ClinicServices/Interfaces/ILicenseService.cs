using BusinessObjects.Entities;

namespace ClinicServices.Interfaces
{
    public interface ILicenseService
    {
        Task<List<License>> GetAllAsync();
        Task<License> GetByIdAsync(int id);
        Task<License> AddAsync(License entity);
        Task UpdateAsync(License entity);
        Task DeleteAsync(int id);
        public void UpdateLicense(License license);
    }
}