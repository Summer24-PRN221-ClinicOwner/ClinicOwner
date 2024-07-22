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

        public Dentist GetDentistById(int id)
        {
            return _context.Dentists.Include(item => item.Licenses).Include(item => item.Services).FirstOrDefault(item => item.Id == id) ?? throw new Exception("404 Dentist Not Found");
        }
        public bool InformationIsUnique(string phone, string email)
        {
            return !_context.Dentists.Any(item => item.Phone == phone || item.Email == email);
        }
        public bool UpdateDentistServices(Dentist dentist)
        {
            var tar = _context.Dentists.Include(item => item.Services).FirstOrDefault(item => item.Id == dentist.Id);
            if (tar != null)
            {
                // Detach existing services
                foreach (var service in tar.Services.ToList())
                {
                    if (!dentist.Services.Any(item => item.Id == service.Id))
                    {
                        tar.Services.Remove(service);

                    }
                }

                foreach (var service in dentist.Services)
                {
                    if (!tar.Services.Any(item => item.Id == service.Id))
                    {
                        var newSer = new Service { Id = service.Id };
                        _context.Services.Attach(newSer);
                        tar.Services.Add(newSer);
                    }
                }

                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }
}
