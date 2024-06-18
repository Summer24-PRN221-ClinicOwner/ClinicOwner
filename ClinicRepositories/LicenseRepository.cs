using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicRepositories
{
    public class LicenseRepository : GenericRepository<BusinessObjects.Entities.License>, ILicenseRepository
    {
        private readonly ClinicContext _context;
        public LicenseRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }
    }
}
