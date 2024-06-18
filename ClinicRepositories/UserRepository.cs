using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;

namespace ClinicRepositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository() : base()
        {
        }
    }
}
