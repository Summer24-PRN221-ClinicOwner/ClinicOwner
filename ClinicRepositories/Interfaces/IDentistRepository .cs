using BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicRepositories.Interfaces
{
    public interface IDentistRepository : IGenericRepository<Dentist>
    {
        Task<List<Dentist>> GetDentistsAsync();
    }
}
