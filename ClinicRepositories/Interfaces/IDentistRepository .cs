using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IDentistRepository : IGenericRepository<Dentist>
    {
        Task<List<Dentist>> GetDentistsAsync();
        public bool InformationIsUnique(string phone, string email);
        public Dentist GetDentistById(int id);

    }
}
