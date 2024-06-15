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
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly ClinicContext _context;
        public PatientRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }
    }
}
