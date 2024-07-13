using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories
{
    public class DentistRepository : GenericRepository<Dentist>, IDentistRepository
    {
        public DentistRepository() : base()
        {
        }

        public async Task<List<Dentist>> GetDentistsAsync()
        {
            return await _context.Dentists
                 .Include(de => de.IdNavigation)
                 .Include(de => de.Clinic)
                 .ToListAsync();

        }
        public bool InformationIsUnique(string phone, string email)
        {
            return !_context.Dentists.Any(item => item.Phone == phone || item.Email == email);
        }
    }
}
