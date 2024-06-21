using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface IUserService
	{
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id); 
        Task<User?> LoginAsync(string userEmail, string password);
        Task<User> AddAsync(User entity);
        Task UpdateAsync(User entity);
        Task DeleteAsync(int id);
    }
}
