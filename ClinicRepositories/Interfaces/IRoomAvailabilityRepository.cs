using BusinessObjects;
using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IRoomAvailabilityRepository : IGenericRepository<RoomAvailability>
    {
        public Task<List<Slot>> GetRoomsAvailabilityAsync(DateTime date, int slotRequired);
        public Room GetAvailableRoomAsync(DateTime date, int slotRequired, int startSlot);
        public Task<bool> UpdateAvaialeString(int roomId, DateTime date, int startSlot, int slotRequired);
        public void SaveChanges();
    }
}
