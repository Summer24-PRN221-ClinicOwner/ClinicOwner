using BusinessObjects;
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
    public class RoomAvailabilityRepository : GenericRepository<RoomAvailability>, IRoomAvailabilityRepository
    {
        public RoomAvailabilityRepository() : base()
        {
        }


        
        public async Task<List<Slot>> GetRoomsAvailabilityAsync(DateTime date, int slotRequired)
        {
            List<RoomAvailability> list = await GetAllAsync();
            list.Where(
                    item => item.Day == date.Date && item.AvailableSlots != "0000000000"  && checkSlotRequired(item, slotRequired) != -1             
                );
            List<Slot> slots = SlotDefiner.ConvertFromString("0000000000");
            
            foreach (var room in list) {
                // Set available string 
                slots.FirstOrDefault(item => item.Key == checkSlotRequired(room, slotRequired))
                 .IsAvailable = true;
                List<Slot> tempSlot = SlotDefiner.ConvertFromString(room.AvailableSlots);
                tempSlot.FirstOrDefault(item => item.Key == checkSlotRequired(room, slotRequired))
                    .IsAvailable = false;
                room.AvailableSlots = SlotDefiner.ConvertToString(tempSlot);
            }
        }

        public int checkSlotRequired(RoomAvailability roomAvailability, int slotRequired)
        {
            List<Slot> listSlot = SlotDefiner.ConvertFromString(roomAvailability.AvailableSlots);
            int checkedSlot = 0;
            foreach (Slot slot in listSlot)
            {
                if (slot.IsAvailable)
                {
                    checkedSlot++;
                    if (checkedSlot == slotRequired)
                    {
                        return slot.Key + 1 - slotRequired;
                    }
                }else {
                    checkedSlot = 0;
                }
            }
            return -1;
        }
    }
}
