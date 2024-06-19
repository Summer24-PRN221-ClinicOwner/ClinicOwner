using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class DentistAvailabilityService : IDentistAvailabilityService
    {
        private readonly IDentistAvailabilityRepository _iDentistAvailabilityRepository;

        public DentistAvailabilityService(IDentistAvailabilityRepository iDentistAvailabilityRepository)
        {
            _iDentistAvailabilityRepository = iDentistAvailabilityRepository;
        }

        public async Task<DentistAvailability> AddAsync(DentistAvailability entity)
        {
            return await _iDentistAvailabilityRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _iDentistAvailabilityRepository.DeleteAsync(id);
        }

        public async Task<List<DentistAvailability>> GetAllAsync()
        {
            return await _iDentistAvailabilityRepository.GetAllAsync();
        }

        public async Task<DentistAvailability> GetByIdAsync(int id)
        {
            return await _iDentistAvailabilityRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(DentistAvailability entity)
        {
            await _iDentistAvailabilityRepository.UpdateAsync(entity);
        }
    }
}
