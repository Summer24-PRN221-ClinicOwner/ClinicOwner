using BusinessObjects.Entities;

namespace ClinicRepositories.Interfaces
{
    public interface IDentistAvailabilityRepository : IGenericRepository<DentistAvailability>
    {
        public Task<List<Dentist>> GetDentistAvailabilityAsync(DateTime date, int slotRequired);
    }
}
