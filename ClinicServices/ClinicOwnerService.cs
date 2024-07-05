using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class ClinicOwnerService : IClinicOwnerService
    {
        private readonly IClinicOwnerRepository _clinicOwnerRepository;
        public readonly IUserService _userService;

        public ClinicOwnerService(IClinicOwnerRepository iClinicOwnerRepository, IUserService userService)
        {
            _clinicOwnerRepository = iClinicOwnerRepository;
            _userService = userService;
        }

        public async Task<ClinicOwner> AddAsync(ClinicOwner entity)
        {
            return await _clinicOwnerRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _userService.DeleteAsync(id);
        }

        public async Task<List<ClinicOwner>> GetAllAsync()
        {
            return await _clinicOwnerRepository.GetAllAsync();
        }

        public async Task<ClinicOwner> GetByIdAsync(int id)
        {
            return await _clinicOwnerRepository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(ClinicOwner entity)
        {
            await _clinicOwnerRepository.UpdateAsync(entity);
        }
        public List<ClinicReportDataObject> MakeClinicReport(DateTime startTime, DateTime endTime)
        {
            return _clinicOwnerRepository.GetClinicReport(startTime, endTime);
        }
    }
}
