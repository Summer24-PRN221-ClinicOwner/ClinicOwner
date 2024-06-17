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
	public class PatientService : IPatientService
	{
		private readonly IPatientRepository _repository;	
		public PatientService(IPatientRepository repository)
		{
			_repository = repository;
		}

		public Task<Patient> AddAsync(Patient entity)
		{
			return _repository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _repository.DeleteAsync(id);
		}

		public Task<IEnumerable<Patient>> GetAllAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<Patient> GetByIdAsync(int id)
		{
			return _repository.GetByIdAsync(id);
		}

		public Task UpdateAsync(Patient entity)
		{
			return _repository.UpdateAsync(entity);
		}
	}
}
