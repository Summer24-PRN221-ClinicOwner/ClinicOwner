using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository() : base()
        {
        }

        public bool InformationIsUnique(string phone, string email)
        {
            return !_context.Patients.Any(item => item.Phone == phone || item.Email == email);
        }
    }
}
