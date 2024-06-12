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
	public class ClinicOwnerService : IGenericService<ClinicOwner>, IClinicOwnerService
	{
		private readonly IClinicOwnerRepository _clinicOwnerRepository;
		public ClinicOwnerService(IClinicOwnerRepository iClinicOwnerRepository)
		{
			_clinicOwnerRepository = iClinicOwnerRepository;
		}
		public Task<ClinicOwner> AddAsync(ClinicOwner entity)
		{
			return _clinicOwnerRepository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _clinicOwnerRepository.DeleteAsync(id);
		}

		public Task<IEnumerable<ClinicOwner>> GetAllAsync()
		{
			return _clinicOwnerRepository.GetAllAsync();
		}

		public Task<ClinicOwner> GetByIdAsync(int id)
		{
			return _clinicOwnerRepository.GetByIdAsync(id);
		}

		public Task UpdateAsync(ClinicOwner entity)
		{
			return _clinicOwnerRepository.UpdateAsync(entity);
		}
	}
}
