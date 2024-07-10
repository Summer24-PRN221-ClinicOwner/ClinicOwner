using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface IServiceService
	{
        Task<List<Service>> GetAllAsync();
        Task<List<Service>> GetAllAvailAsync();
        Task<Service> GetByIdAsync(int id);
        Task<Service> AddAsync(Service entity);
        Task UpdateAsync(Service entity);
        Task DeleteAsync(int id);
    }
}
