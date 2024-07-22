using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class LicenseRepository : GenericRepository<BusinessObjects.Entities.License>, ILicenseRepository
    {
        public LicenseRepository() : base()
        {
        }
        public void UpdateLicense(License license)
        {
            var tar = _context.Licenses.FirstOrDefault(item => item.Id == license.Id);
            if (tar != null)
            {
                tar.LicenseNumber = license.LicenseNumber;
                tar.IssueDate = license.IssueDate;
                tar.ExpireDate = license.ExpireDate;
            }
            _context.SaveChanges();
        }
    }
}
