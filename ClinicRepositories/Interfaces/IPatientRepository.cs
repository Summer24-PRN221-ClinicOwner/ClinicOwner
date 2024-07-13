using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        public bool InformationIsUnique(string phone, string email);
    }
}
