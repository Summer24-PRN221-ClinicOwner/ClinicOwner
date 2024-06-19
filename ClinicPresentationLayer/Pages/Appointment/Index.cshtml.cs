using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Entities;
using ClinicRepositories;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class IndexModel : PageModel
    {
        private readonly ClinicRepositories.ClinicContext _context;

        public IndexModel(ClinicRepositories.ClinicContext context)
        {
            _context = context;
        }

        public IList<BusinessObjects.Entities.Appointment> Appointment { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Appointment = await _context.Appointments
                .Include(a => a.Room)
                .Include(a => a.Dentist)
                .Include(a => a.Patient)
                .Include(a => a.Service).ToListAsync();
        }
    }
}
