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
	public class LicenseService : ILicenseService, IGenericService<License>
	{
		private readonly ILicenseRepository _repository;
		public LicenseService (ILicenseRepository repository)
		{
			_repository = repository;
		}
		public Task<License> AddAsync(License entity)
		{
			return _repository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _repository.DeleteAsync(id);
		}

		public Task<IEnumerable<License>> GetAllAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<License> GetByIdAsync(int id)
		{
			return _repository.GetByIdAsync(id);	
		}

		public Task UpdateAsync(License entity)
		{
			return _repository.UpdateAsync(entity);
		}
	}
}
