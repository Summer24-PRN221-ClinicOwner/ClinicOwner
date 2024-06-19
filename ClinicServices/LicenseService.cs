using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class LicenseService : ILicenseService
    {
        private readonly ILicenseRepository _repository;

        public LicenseService(ILicenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<License> AddAsync(License entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<License>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<License> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(License entity)
        {
            await _repository.UpdateAsync(entity);
        }
    }
}
