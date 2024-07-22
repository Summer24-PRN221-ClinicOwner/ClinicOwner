using BusinessObjects.Entities;

namespace ClinicServices.Interfaces
{
    public interface IDentistService
    {
        Task<List<Dentist>> GetAllAsync();
        Task<Dentist> GetByIdAsync(int id);
        Task<Dentist> AddAsync(Dentist entity, User newUser);
        Task UpdateAsync(Dentist entity);
        Task DeleteAsync(int id);
        Dentist GetDentistById(int id);
        public void UpdateDentistServices(Dentist dentist);
    }
}