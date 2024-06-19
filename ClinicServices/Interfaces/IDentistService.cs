using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface IDentistService
	{
        Task<List<Dentist>> GetAllAsync();
        Task<Dentist> GetByIdAsync(int id);
        Task<Dentist> AddAsync(Dentist entity);
        Task UpdateAsync(Dentist entity);
        Task DeleteAsync(int id);
    }
}