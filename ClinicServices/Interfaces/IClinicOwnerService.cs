using BusinessObjects;
using BusinessObjects.Entities;

namespace ClinicServices.Interfaces
{
    public interface IClinicOwnerService
    {
        Task<List<ClinicOwner>> GetAllAsync();
        Task<ClinicOwner> GetByIdAsync(int id);
        Task<ClinicOwner> AddAsync(ClinicOwner entity);
        Task UpdateAsync(ClinicOwner entity);
        Task DeleteAsync(int id);
        public List<ClinicReportDataObject> MakeClinicReport(DateTime startTime, DateTime endTime);
        public ClinicReportDataObject MakeClinicReportTotal(DateTime startTime, DateTime endTime);

    }
}
