using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IDentistAvailabilityRepository : IGenericRepository<DentistAvailability>
    {
        public Task<List<Dentist>> GetDentistAvailabilityAsync(DateTime date, int startSlot, int slotRequired, int serviceId);
        public Task<bool> UpdateAvaialeString(int dentistId, DateTime date, int startSlot, int slotRequired);
        public void SaveChanges();

    }
}
