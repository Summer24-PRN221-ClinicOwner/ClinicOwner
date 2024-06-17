using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface IDentistAvailabilityService
	{
        Task<IEnumerable<DentistAvailability>> GetAllAsync();
        Task<DentistAvailability> GetByIdAsync(int id);
        Task<DentistAvailability> AddAsync(DentistAvailability entity);
        Task UpdateAsync(DentistAvailability entity);
        Task DeleteAsync(int id);
    }
}