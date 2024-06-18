using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository() : base()
        {
        }
    }
}
