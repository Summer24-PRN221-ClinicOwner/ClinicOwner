using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
    public interface IRoomService
    {
        Task<List<Room>> GetAllAsync();
        Task<Room> GetByIdAsync(int id);
        Task<bool> AddAsync(Room entity);
        Task<bool> UpdateAsync(Room entity);
        Task<bool> DeleteAsync(int id);
    }
}
