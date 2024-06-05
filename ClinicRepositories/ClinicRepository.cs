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
    public class ClinicRepository : GenericRepository<Clinic>, IClinicRepository
    {
        private readonly Prn221Context _context;
        public ClinicRepository(DbContext context) : base(context)
        {
            _context = context;
        }
    }
}
