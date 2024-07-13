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
            var existingUser = await _userService.GetByUsernameAsync(userAccount.Username);
            if (!_repository.InformationIsUnique(entity.Phone, entity.Email)) throw new Exception("Email or Phone is duplicated!");
            if (existingUser != null)
            {
                throw new Exception("A user with the same username already exists.");
            }
            userAccount.Status = 1;
            var newAccount = await _userService.AddAsync(userAccount);
            entity.Id = newAccount.Id;
            return await _repository.AddAsync(entity);
        }
        public async Task<Patient> StaffAddAsync(Patient entity, User userAccount)
        {
            var existingUser = await _userService.GetByUsernameAsync(userAccount.Username);
            if (existingUser != null)
            {
                throw new Exception("A user with the same username already exists.");
            }
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
            if (!_repository.InformationIsUnique(entity.Phone, entity.Email)) throw new Exception("Email or password is duplicated!");
            return _repository.UpdateAsync(entity);
        }

        public async Task<Patient> FindPatientAsync(string searchTerm)
        {
            var PatientList = await _repository.GetAllAsync();
            var patient = PatientList.FirstOrDefault(p => p.Email.Contains(searchTerm) || p.Phone.Contains(searchTerm));
            return patient;
        }

    }
}
