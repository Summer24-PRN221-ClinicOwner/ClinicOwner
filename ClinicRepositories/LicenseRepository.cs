using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class LicenseRepository : GenericRepository<BusinessObjects.Entities.License>, ILicenseRepository
    {
        public LicenseRepository() : base()
        {
        }
    }
}
