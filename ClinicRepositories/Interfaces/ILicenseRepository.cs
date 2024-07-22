using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface ILicenseRepository : IGenericRepository<License>
    {
        public void UpdateLicense(License license);

    }
}
