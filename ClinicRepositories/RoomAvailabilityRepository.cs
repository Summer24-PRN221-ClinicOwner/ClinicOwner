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
        public async Task<List<Slot>> GetSlotsAvailabilityAsync(DateTime date, int slotRequired)
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
                    tmplist.Add(item);
                }
            }
            //Convert to available slots 
            List<Slot> slots = SlotDefiner.ConvertFromString("0000000000");
            foreach (var room in tmplist)
            {
                int availableSlot = SlotDefiner.CheckSlotRequired(room.AvailableSlots, slotRequired);
                while (availableSlot != -1)
                {
                    // Set available string 
                    slots.FirstOrDefault(item => item.Key == availableSlot).IsAvailable = true;

                    // Inactive slot 
                    List<Slot> tempSlot = SlotDefiner.ConvertFromString(room.AvailableSlots);
                    tempSlot.FirstOrDefault(item => item.Key == availableSlot).IsAvailable = false;
                    room.AvailableSlots = SlotDefiner.ConvertToString(tempSlot);

                    // Continue
                    availableSlot = SlotDefiner.CheckSlotRequired(room.AvailableSlots, slotRequired);
                }
            }
            return slots;


        }

        public async Task<Room> GetAvailableRoomAsync(DateTime date, int slotRequired)
        {
            var checklist = _context.RoomAvailabilities.ToList();
            List<RoomAvailability> item =  _context.RoomAvailabilities.Include(item => item.Room).
                Where(item => item.Day.Date == date.Date && item.AvailableSlots != "0000000000" ).ToList();
            foreach (var room in item) {

                int check = SlotDefiner.CheckSlotRequired(room.AvailableSlots, slotRequired);
                if( check != -1)
                {
                    return room.Room;
                }
            }

            return null;
        }

    }
}
