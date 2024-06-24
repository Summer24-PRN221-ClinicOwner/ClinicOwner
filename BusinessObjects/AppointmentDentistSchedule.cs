using BusinessObjects.Entities;

namespace BusinessObjects
{
    public class AppointmentDentistSchedule
    {
        public required List<Appointment> Monday { get; set; }
        public required List<Appointment> Tuesday { get; set; }
        public required List<Appointment> Wednesday { get; set; }
        public required List<Appointment> Thursday { get; set; }
        public required List<Appointment> Friday { get; set; }
        public required List<Appointment> Saturday { get; set; }
        public required List<Appointment> Sunday { get; set; }


    }
}
