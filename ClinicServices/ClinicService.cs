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
	public class ClinicService : IClinicService
	{
		private readonly IClinicRepository _iClinicReposity;
		public ClinicService (IClinicRepository clinicRepository)
		{
			_iClinicReposity = clinicRepository;
		}
		public Task<Clinic> AddAsync(Clinic entity)
		{
			return _iClinicReposity.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _iClinicReposity.DeleteAsync(id);
		}

		public Task<IEnumerable<Clinic>> GetAllAsync()
		{
			return _iClinicReposity.GetAllAsync();
		}

		public Task<Clinic> GetByIdAsync(int id)
		{
			return _iClinicReposity.GetByIdAsync(id);
		}

		public Task UpdateAsync(Clinic entity)
		{
			return _iClinicReposity.UpdateAsync(entity);
		}
	}
}
