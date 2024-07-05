using BusinessObjects;
using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IClinicOwnerRepository : IGenericRepository<ClinicOwner>
    {
        public List<ClinicReportDataObject> GetClinicReport(DateTime startDate, DateTime endDate);
    }
}
