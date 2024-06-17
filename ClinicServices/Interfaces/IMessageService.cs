using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
	public interface IMessageService
	{
        Task<IEnumerable<Message>> GetAllAsync();
        Task<Message> GetByIdAsync(int id);
        Task<Message> AddAsync(Message entity);
        Task UpdateAsync(Message entity);
        Task DeleteAsync(int id);
    }
}
