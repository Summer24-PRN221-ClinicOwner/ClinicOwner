using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class ClinicRepository : GenericRepository<Clinic>, IClinicRepository
    {
        public ClinicRepository() : base()
        {
        }
        public bool InformationIsUnique(string phone, string email)
        {
            return !_context.ClinicOwners.Any(item => item.Phone == phone || item.Email == email);
        }
    }
}
