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
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        private readonly Prn221Context _context;
        public ReportRepository(Prn221Context context) : base(context)
        {
            _context = context;
        }
    }
}
