using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface IPatientService
	{
        Task<List<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(int id);
        Task<Patient> AddAsync(Patient entity, User user);
        Task<Patient> StaffAddAsync(Patient entity, User user);
        Task UpdateAsync(Patient entity);
        Task DeleteAsync(int id);
        Task<Patient> FindPatientAsync(string searchTerm);
    }
}
