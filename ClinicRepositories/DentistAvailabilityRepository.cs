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
        public async Task<List<Dentist>> GetDentistAvailabilityAsync(DateTime date,int startSlot, int slotRequired, int serviceId)
        {
            //Querry list of available dentist

            var listDentist = await _context.DentistAvailabilities.Include(item => item.Dentist).ThenInclude(item => item.Services).Where(
                    item => item.Day.Date == date.Date && item.AvailableSlots != "0000000000"
                ).ToListAsync();
            listDentist = listDentist.Where(item => SlotDefiner.CheckSlotRequired(item.AvailableSlots, slotRequired) == startSlot).ToList();
            var serv = await _context.Services.FirstOrDefaultAsync(item => item.Id == serviceId);
            return listDentist.Select(item => item.Dentist).Where(item => item.Services.Contains(serv)).ToList();

        }
    }
}
