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
        /// <summary>
        /// Các dentist có thể làm (service + slot)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startSlot"></param>
        /// <param name="slotRequired"></param>
        /// <param name="serviceId"></param>
        /// <returns></returns>

        public async Task<List<Dentist>> GetDentistAvailabilityAsync(DateTime date, int startSlot, int slotRequired, int serviceId)
        {
            //Querry list of available dentist
            var listDentist = await _context.DentistAvailabilities.Include(item => item.Dentist).ThenInclude(item => item.Services).Where(item => item.Day.Date == date.Date).ToListAsync();

            if (listDentist.Count == 0) return await _context.Dentists.Include(item => item.Services).Where(item => item.Services.Any(serv => serv.Id == serviceId)).ToListAsync();
            //Chưa có lịch - có thể làm
            var result = await _context.Dentists.Include(item => item.Services).Include(item => item.DentistAvailabilities).Where(item => item.Services.Any(serv => serv.Id == serviceId) && item.DentistAvailabilities.Select(item => item.Day.Date == date.Date).ToList().Count == 0).ToListAsync();

            //Có lich - có thể làm
            listDentist = listDentist.Where(item => SlotDefiner.IsAvaiForSlot(item.AvailableSlots, slotRequired, startSlot)).ToList();
            var serv = await _context.Services.FirstOrDefaultAsync(item => item.Id == serviceId);

            //Có thể làm
            result.AddRange(listDentist.Select(item => item.Dentist).Where(item => item.Services.Any(serv => serv.Id == serviceId)).ToList());
            return result;
        }

        public async Task<bool> UpdateAvaialeString(int dentistId, DateTime date, int startSlot, int slotRequired)
        {
            var item = await _context.DentistAvailabilities.FirstOrDefaultAsync(item => item.Day.Date == date && item.DentistId == dentistId);

            if (item == null)
            {
                item = new() { AvailableSlots = "1111111111", Day = date.Date, DentistId = dentistId };
                _context.DentistAvailabilities.Add(item);
            }
            var slotList = SlotDefiner.ConvertFromString(item.AvailableSlots);

            for (int i = startSlot; i < startSlot + slotRequired; i++)
            {
                if (slotList.ElementAt(startSlot - 1).IsAvailable == true) slotList.ElementAt(startSlot - 1).IsAvailable = false;
                else return false;
            }
            item.AvailableSlots = SlotDefiner.ConvertToString(slotList);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
