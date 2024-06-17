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
	public class ServiceService : IServiceService
	{
		private readonly IServiceRepository _repository;
		public ServiceService(IServiceRepository repository)
		{
			_repository = repository;
		}
		public Task<Service> AddAsync(Service entity)
		{
			return _repository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _repository.DeleteAsync(id);
		}

		public Task<IEnumerable<Service>> GetAllAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<Service> GetByIdAsync(int id)
		{
			return _repository.GetByIdAsync(id);
		}

		public Task UpdateAsync(Service entity)
		{
			return _repository.UpdateAsync(entity);
		}
	}
}
