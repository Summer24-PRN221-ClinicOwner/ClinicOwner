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
	public class DentistService : IDentistService
	{
		private readonly IDentistRepository _repository;
		public DentistService(IDentistRepository repository)
		{
			_repository = repository;
		}

		public Task<Dentist> AddAsync(Dentist entity)
		{
			return _repository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _repository.DeleteAsync(id);
		}

		public Task<IEnumerable<Dentist>> GetAllAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<Dentist> GetByIdAsync(int id)
		{
			return _repository.GetByIdAsync(id);
		}

		public Task UpdateAsync(Dentist entity)
		{
			return _repository.UpdateAsync(entity);
		}
	}
}
