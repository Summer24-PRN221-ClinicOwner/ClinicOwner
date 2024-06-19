using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepository _iClinicReposity;

        public ClinicService(IClinicRepository clinicRepository)
        {
            _iClinicReposity = clinicRepository;
        }

        public async Task<Clinic> AddAsync(Clinic entity)
        {
            return await _iClinicReposity.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _iClinicReposity.DeleteAsync(id);
        }

        public async Task<IEnumerable<Clinic>> GetAllAsync()
        {
            return await _iClinicReposity.GetAllAsync();
        }

        public async Task<Clinic> GetByIdAsync(int id)
        {
            return await _iClinicReposity.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Clinic entity)
        {
            await _iClinicReposity.UpdateAsync(entity);
        }
    }
}
