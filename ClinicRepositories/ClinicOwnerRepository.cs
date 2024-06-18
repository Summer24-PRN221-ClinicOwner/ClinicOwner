using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class ClinicOwnerRepository : GenericRepository<ClinicOwner>, IClinicOwnerRepository
    {
        public ClinicOwnerRepository() : base()
        {
        }
    }
}
