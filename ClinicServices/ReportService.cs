using BusinessObjects.Entities;
using ClinicRepositories;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;

        public ReportService(IReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<Report> AddAsync(Report entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<Report>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Report> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Report entity)
        {

            await _repository.UpdateAsync(entity);

        }
        public async Task<Report> AddOrUpdateAsync(Report report)
        {
            var existingReport = await _repository.GetByIdAsync(report.Id);

            if (existingReport == null)
            {
                return await _repository.AddAsync(report);
            }
            else
            {
                existingReport.Name = report.Name;
                existingReport.Data = report.Data;
                existingReport.GeneratedDate = report.GeneratedDate;

                 await _repository.UpdateAsync(existingReport);
                return existingReport;
            }
        }
    }
}
