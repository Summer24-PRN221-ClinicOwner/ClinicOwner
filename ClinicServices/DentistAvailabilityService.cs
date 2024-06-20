using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class DentistAvailabilityService : IDentistAvailabilityService
    {
        private readonly IDentistAvailabilityRepository _dentistAvailabilityRepository;
        private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;
        private readonly IDentistService _dentistService;

        public DentistAvailabilityService(IDentistAvailabilityRepository iDentistAvailabilityRepository, IDentistService dentistService)
        {
            _dentistAvailabilityRepository = iDentistAvailabilityRepository;
            _dentistService = dentistService;
        }

        public async Task<DentistAvailability> AddAsync(DentistAvailability entity)
        {
            return await _dentistAvailabilityRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _dentistAvailabilityRepository.DeleteAsync(id);
        }

        public async Task<List<DentistAvailability>> GetAllAsync()
        {
            return await _dentistAvailabilityRepository.GetAllAsync();
        }

        public async Task<DentistAvailability> GetByIdAsync(int id)
        {
            return await _dentistAvailabilityRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(DentistAvailability entity)
        {
            await _dentistAvailabilityRepository.UpdateAsync(entity);
        }
        public async Task<List<Dentist>> GetAvailableDentist(DateTime date, int slotRequired)
        {
            return await _dentistAvailabilityRepository.GetDentistAvailabilityAsync(date, slotRequired);
        }

    }
}
