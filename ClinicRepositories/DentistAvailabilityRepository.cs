using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories
{
    public class DentistAvailabilityRepository : GenericRepository<DentistAvailability>, IDentistAvailabilityRepository
    {
        public DentistAvailabilityRepository() : base()
        {
        }
        public async Task<List<Dentist>> GetDentistAvailabilityAsync(DateTime date, int slotRequired)
        {
            //Querry list of available dentist

            var listDentist = await _context.DentistAvailabilities.Include(item => item.Dentist).Where(
                    item => item.Day == date.Date && item.AvailableSlots != "0000000000" && SlotDefiner.CheckSlotRequired(item.AvailableSlots, slotRequired) != -1
                ).ToListAsync();

            return listDentist.Select(item => item.Dentist).ToList();

        }
    }
}
