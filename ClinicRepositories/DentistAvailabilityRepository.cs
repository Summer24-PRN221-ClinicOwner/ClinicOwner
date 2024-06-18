using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class DentistAvailabilityRepository : GenericRepository<DentistAvailability>, IDentistAvailabilityRepository
    {
        public DentistAvailabilityRepository() : base()
        {
        }
    }
}
