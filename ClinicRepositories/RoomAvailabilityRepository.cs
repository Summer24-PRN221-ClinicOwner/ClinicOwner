using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories
{
    public class RoomAvailabilityRepository : GenericRepository<RoomAvailability>, IRoomAvailabilityRepository
    {
        private ClinicContext localContext;
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
            List<Slot> slots = SlotDefiner.ConvertFromString("0000000000");
            //Querry list of available room
            List<RoomAvailability> list = [.. _context.RoomAvailabilities.Include(item => item.Room)];

            if (_context.Rooms.Any(item => item.Status == 1 && !item.RoomAvailabilities
            .Any(item => item.Day.Date == date.Date))) return SlotDefiner.ConvertFromString("1111111111");

            list = list.Where(
                    item => item.Day.Date == date.Date
                && item.Room.Status == 1).ToList();
            if (list.Count != 0)
            {
                List<RoomAvailability> tmplist = new List<RoomAvailability>();
                foreach (var item in list)
                {
                    if (SlotDefiner.CheckSlotRequired(item.AvailableSlots, slotRequired) != -1)
                    {
                        tmplist.Add(new() { Id = item.Id, AvailableSlots = item.AvailableSlots, Day = item.Day, RoomId = item.RoomId });
                    }
                }

                //Convert to available slots 
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
            }
            else
            {
                string standard = "1111111111";
                int availableSlot = SlotDefiner.CheckSlotRequired(standard, slotRequired);
                while (availableSlot != -1)
                {
                    // Set available string 
                    slots.ElementAt(availableSlot - 1).IsAvailable = true;

                    // Inactive slot 
                    List<Slot> tempSlot = SlotDefiner.ConvertFromString(standard);
                    tempSlot.ElementAt(availableSlot - 1).IsAvailable = false;
                    standard = SlotDefiner.ConvertToString(tempSlot);

                    // Continue
                    availableSlot = SlotDefiner.CheckSlotRequired(standard, slotRequired);
                }
            }
            foreach (Slot slot in slots)
            {
                if (slot.Key <= 5)
                {
                    if (slot.Key + 5 < DateTime.Now.Hour)
                    {
                        slot.IsAvailable = false;
                    }
                }
                else
                {
                    if (slot.Key + 6 < DateTime.Now.Hour)
                    {
                        slot.IsAvailable = false;
                    }
                }
            }
            return slots;
        }

        public Room GetAvailableRoomAsync(DateTime date, int slotRequired, int startSlot)
        {
            //Room co lich
            List<RoomAvailability> listRoom = _context.RoomAvailabilities.Include(item => item.Room).
            Where(item => item.Day.Date == date.Date && item.AvailableSlots != "0000000000" && item.Room.Status == 1).ToList();
            foreach (var room in listRoom)
            {
                if (SlotDefiner.IsAvaiForSlot(room.AvailableSlots, slotRequired, startSlot)) return room.Room;
            }
            //neu ko co room da co lich return room dau tien khong co lich
            var result = _context.Rooms.Include(item => item.RoomAvailabilities).Where(item =>
            !item.RoomAvailabilities.Any(item => item.Day.Date == date.Date) && item.Status == 1).ToList();
            if (result.Count != 0)
            {
                return result.FirstOrDefault();
            }
            else
            {
                throw new Exception("No room found");
            }
        }


        public async Task<bool> UpdateAvaialeString(int roomId, DateTime date, int startSlot, int slotRequired)
        {
            localContext = new ClinicContext();
            try
            {
                var item = await localContext.RoomAvailabilities.FirstOrDefaultAsync(item => item.Day.Date == date && item.RoomId == roomId);
                if (item == null)
                {
                    item = new() { AvailableSlots = "1111111111", Day = date.Date, RoomId = roomId };
                    localContext.RoomAvailabilities.Add(item);
                }
                var slotList = SlotDefiner.ConvertFromString(item.AvailableSlots);

                for (int i = startSlot; i < startSlot + slotRequired; i++)
                {
                    if (slotList.ElementAt(i - 1).IsAvailable == true) slotList.ElementAt(i - 1).IsAvailable = false;
                    else return false;
                }
                item.AvailableSlots = SlotDefiner.ConvertToString(slotList);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool CancelAppointment(Appointment appointment)
        {
            if (appointment.Status == (int)AppointmentStatus.LateCanceled) return true;
            try
            {
                var tar = _context.RoomAvailabilities.FirstOrDefault(item => item.Day.Date == appointment.AppointDate.Date && item.RoomId == appointment.RoomId);
                var temp = SlotDefiner.ConvertFromString(tar.AvailableSlots);
                for (int i = appointment.StartSlot; i <= appointment.EndSlot; i++)
                {
                    if (temp.ElementAt(i - 1).IsAvailable = true) throw new Exception("Invalid Update");
                    temp.ElementAt(i - 1).IsAvailable = true;
                }
                tar.AvailableSlots = SlotDefiner.ConvertToString(temp);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public void SaveChanges()
        {
            localContext.SaveChanges();
        }
        public void Dispose()
        {
            localContext.Dispose();
        }
    }
}
