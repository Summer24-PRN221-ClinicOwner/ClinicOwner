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

        public async Task<Dentist> AddAsync(Dentist entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Dentist>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Dentist> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Dentist entity)
        {
            await _repository.UpdateAsync(entity);
        }
    }
}
