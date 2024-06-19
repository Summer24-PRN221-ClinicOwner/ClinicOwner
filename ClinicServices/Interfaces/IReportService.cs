using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface IReportService 
	{
        Task<List<Report>> GetAllAsync();
        Task<Report> GetByIdAsync(int id);
        Task<Report> AddAsync(Report entity);
        Task UpdateAsync(Report entity);
        Task DeleteAsync(int id);
    }
}
