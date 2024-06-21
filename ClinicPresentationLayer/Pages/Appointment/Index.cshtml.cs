using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class IndexModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDentistService _dentistService;
        private readonly IPatientService _petientService;
        private readonly IServiceService _serviceService;

        public IndexModel(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public IList<BusinessObjects.Entities.Appointment> Appointment { get; set; } = default!;

        public async Task OnGetAsync()
        {

        }
    }
}
