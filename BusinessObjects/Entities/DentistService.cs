using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Entities
{
    public partial class DentistService
    {
        public int DentistId { get; set; }

        public int ServiceId { get; set; }

        public Dentist Dentist { get; set; }

        public Service Service { get; set; }
    }
}
