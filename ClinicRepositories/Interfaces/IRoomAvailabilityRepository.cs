using BusinessObjects;
using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IRoomAvailabilityRepository : IGenericRepository<RoomAvailability>
    {
        public Task<List<Slot>> GetSlotsAvailabilityAsync(DateTime date, int slotRequired);
        public Task<Room> GetAvailableRoomAsync(DateTime date, int slotRequired);
    }
}
