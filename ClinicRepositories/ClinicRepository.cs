using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class ClinicRepository : GenericRepository<Clinic>, IClinicRepository
    {
        public ClinicRepository() : base()
        {
        }
    }
}
