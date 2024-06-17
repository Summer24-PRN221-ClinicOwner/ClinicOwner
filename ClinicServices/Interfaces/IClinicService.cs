using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface IClinicService
	{
        Task<IEnumerable<Clinic>> GetAllAsync();
        Task<Clinic> GetByIdAsync(int id);
        Task<Clinic> AddAsync(Clinic entity);
        Task UpdateAsync(Clinic entity);
        Task DeleteAsync(int id);
    }
}