using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface IClinicOwnerService 
	{
        Task<IEnumerable<ClinicOwner>> GetAllAsync();
        Task<ClinicOwner> GetByIdAsync(int id);
        Task<ClinicOwner> AddAsync(ClinicOwner entity);
        Task UpdateAsync(ClinicOwner entity);
        Task DeleteAsync(int id);
    }
}
