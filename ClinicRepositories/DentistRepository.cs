using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class DentistRepository : GenericRepository<Dentist>, IDentistRepository
    {
        public DentistRepository() : base()
        {
        }
    }
}
