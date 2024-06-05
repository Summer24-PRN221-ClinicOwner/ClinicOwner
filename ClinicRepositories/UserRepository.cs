using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicRepositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly Prn221Context _context;
        public UserRepository(DbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
