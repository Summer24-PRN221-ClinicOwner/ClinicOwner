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
	public class DentistAvailabilityService: IDentistAvailabilityService
	{
		private IDentistAvailabilityRepository _iDentistAvailabilityRepository;
		public DentistAvailabilityService(IDentistAvailabilityRepository iDentistAvailabilityRepository)
		{
			_iDentistAvailabilityRepository = iDentistAvailabilityRepository;
		}
		public Task<DentistAvailability> AddAsync(DentistAvailability entity)
		{
			return _iDentistAvailabilityRepository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _iDentistAvailabilityRepository.DeleteAsync(id);
		}

		public Task<IEnumerable<DentistAvailability>> GetAllAsync()
		{
			return _iDentistAvailabilityRepository.GetAllAsync();
		}

		public Task<DentistAvailability> GetByIdAsync(int id)
		{
			return _iDentistAvailabilityRepository.GetByIdAsync(id);
		}

		public Task UpdateAsync(DentistAvailability entity)
		{
			return _iDentistAvailabilityRepository.UpdateAsync(entity);
		}
	}
}
