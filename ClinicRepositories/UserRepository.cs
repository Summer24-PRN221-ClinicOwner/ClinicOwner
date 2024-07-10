using BusinessObjects.Entities;
using BusinessObjects;
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
        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(item => item.Username == username);
        }

        public async Task<bool> IsUsernameExisted(string username)
        {
            return await _context.Users.AnyAsync(item => item.Username == username);
        }

        public async Task<List<User>> GetAllStaffs()
        {
            return await _context.Users.Where(item => item.Role == UserRoles.Staff).ToListAsync();
        }

    }
}
