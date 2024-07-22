using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class DentistService : IDentistService
    {
        private readonly IDentistRepository _repository;
        public readonly IUserService _userService;

        public DentistService(IDentistRepository repository, IUserService userService)
        {
            _repository = repository;
            _userService = userService;
        }

        public async Task<Dentist> AddAsync(Dentist entity, User userAccount)
        {
            if (!await _userService.IsUsernameExisted(userAccount.Username))
            {
                userAccount.Status = 1;
                var newAccount = await _userService.AddAsync(userAccount);
                entity.Id = newAccount.Id;
                return await _repository.AddAsync(entity);
            }
            else
            {
                throw new Exception($"Username: {userAccount.Username} already existed");
            }

        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<Dentist>> GetAllAsync()
        {
            return await _repository.GetDentistsAsync();
        }

        public async Task<Dentist> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Dentist entity)
        {
            if (!_repository.InformationIsUnique(entity.Phone, entity.Email)) throw new Exception("Email or password is duplicated!");
            await _repository.UpdateAsync(entity);
        }
        public Dentist GetDentistById(int id)
        {
            return _repository.GetDentistById(id);
        }
        public void UpdateDentistServices(Dentist dentist)
        {
            _repository.UpdateDentistServices(dentist);
        }
    }
}
