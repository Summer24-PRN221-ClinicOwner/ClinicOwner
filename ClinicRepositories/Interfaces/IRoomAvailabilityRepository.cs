using BusinessObjects;
using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IRoomAvailabilityRepository : IGenericRepository<RoomAvailability>
    {
        public Task<List<Slot>> GetRoomsAvailabilityAsync(DateTime date, int slotRequired);
        public Task<Room> GetAvailableRoomAsync(DateTime date, int slotRequired);
    }
}
