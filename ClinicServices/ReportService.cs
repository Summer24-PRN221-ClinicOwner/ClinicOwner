using BusinessObjects.Entities;
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
            // Corrected the return type to match the repository method
            await _repository.UpdateAsync(entity);
            // UpdateAsync doesn't typically return anything, 
            // so we don't need to return a value here.
        }
    }
}
