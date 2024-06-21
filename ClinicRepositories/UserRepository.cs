using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicRepositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository() : base()
        {
        }

        public async Task<User?> GetUserByUsernamePass(string username, string password)
        {
             return await _context.Users.FirstOrDefaultAsync(item => item.Username == username && item.Password == password);
        }
    }
}
