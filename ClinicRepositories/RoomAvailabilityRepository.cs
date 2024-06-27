using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories
{
    public class RoomAvailabilityRepository : GenericRepository<RoomAvailability>, IRoomAvailabilityRepository
    {
        public RoomAvailabilityRepository() : base()
        {
        }


        /* Example
             Actual     11111 11111
             Clone      00011 11111
             Require    3
             Availale   11100 11100
                
             */
        public async Task<List<Slot>> GetRoomsAvailabilityAsync(DateTime date, int slotRequired)
        {
            //Querry list of available room
            List<RoomAvailability> list = await GetAllAsync();
            list = list.Where(
                    item => item.Day.Date == date.Date && item.AvailableSlots != "0000000000"
                ).ToList();
            List<RoomAvailability> tmplist = new List<RoomAvailability>();
            foreach (var item in list)
            {
                if (SlotDefiner.CheckSlotRequired(item.AvailableSlots, slotRequired) != -1)
                {
                    tmplist.Add(new() { Id = item.Id, AvailableSlots = item.AvailableSlots, Day = item.Day, RoomId = item.RoomId });
                }
            }
            //Convert to available slots 
            List<Slot> slots = SlotDefiner.ConvertFromString("0000000000");
            foreach (var room in tmplist)
            {
                if (SlotDefiner.ConvertToString(slots) == "11111111111") return slots;
                int availableSlot = SlotDefiner.CheckSlotRequired(room.AvailableSlots, slotRequired);
                while (availableSlot != -1)
                {
                    // Set available string 
                    slots.ElementAt(availableSlot - 1).IsAvailable = true;

                    // Inactive slot 
                    List<Slot> tempSlot = SlotDefiner.ConvertFromString(room.AvailableSlots);
                    tempSlot.ElementAt(availableSlot - 1).IsAvailable = false;
                    room.AvailableSlots = SlotDefiner.ConvertToString(tempSlot);

                    // Continue
                    availableSlot = SlotDefiner.CheckSlotRequired(room.AvailableSlots, slotRequired);
                }
            }
            return slots;


        }

        public Room GetAvailableRoomAsync(DateTime date, int slotRequired, int startSlot)
        {
            List<RoomAvailability> item = _context.RoomAvailabilities.Include(item => item.Room).
                Where(item => item.Day.Date == date.Date && item.AvailableSlots != "0000000000").ToList();
            foreach (var room in item)
            {
                if (SlotDefiner.IsAvaiRoom(room.AvailableSlots, slotRequired, startSlot)) return room.Room;
            }

            return null;
        }


        public async Task<bool> UpdateAvaialeString(int roomId, DateTime date, int startSlot, int slotRequired)
        {
            var item = await _context.RoomAvailabilities.FirstOrDefaultAsync(item => item.Day.Date == date && item.RoomId == roomId);
            var slotList = SlotDefiner.ConvertFromString(item.AvailableSlots);

            for (int i = startSlot; i < startSlot + slotRequired; i++)
            {
                slotList.ElementAt(startSlot - 1).IsAvailable = false;
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
    }
}
