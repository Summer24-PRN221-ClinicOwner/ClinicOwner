using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        public readonly IUserService _userService;

        public PatientService(IPatientRepository repository, IUserService userService)
        {
            _repository = repository;
            _userService = userService;
        }

        public async Task<Patient> AddAsync(Patient entity, User userAccount)
        {
            userAccount.Status = 1;
            var newAccount = await _userService.AddAsync(userAccount);
            entity.Id = newAccount.Id;
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _userService.DeleteAsync(id);
        }

        public Task<List<Patient>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Patient> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task UpdateAsync(Patient entity)
        {
            return _repository.UpdateAsync(entity);
        }

    }
}
