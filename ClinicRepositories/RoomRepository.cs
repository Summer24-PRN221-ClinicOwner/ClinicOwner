using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicRepositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository() : base()
        {
        }
        public async Task<List<Room>> GetRoomsAsync()
        {
            return await _context.Rooms
                .Include(r => r.Clinic)
                .Include(r => r.RoomAvailabilities)
                .ToListAsync();
        }
    }
}
