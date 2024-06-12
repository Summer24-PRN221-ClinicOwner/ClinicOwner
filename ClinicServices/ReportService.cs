using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices
{
	public class ReportService : IReportService, IGenericService<Report>
	{
		private readonly IReportRepository _repository;
		public ReportService(IReportRepository repository) { _repository = repository; }
		public Task<Report> AddAsync(Report entity)
		{
			return _repository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _repository.DeleteAsync(id);
		}

		public Task<IEnumerable<Report>> GetAllAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<Report> GetByIdAsync(int id)
		{
			return _repository.GetByIdAsync(id);
		}

		public Task UpdateAsync(Report entity)
		{
			return _repository.GetAllAsync();
		}
	}
}
