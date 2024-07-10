using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        private readonly ClinicContext _context = new();



        public ServiceRepository() : base()
        {

        }

        public async Task<List<Service>> GetAllAvailService()
        {
            return await _context.Services
                .Where(s => s.Status == (int)ServiceStatus.Available)
                .ToListAsync();
        }

        public async Task<List<Service>> GetAllService()
        {
            return await _context.Services
                .ToListAsync();
        }
    }
}
