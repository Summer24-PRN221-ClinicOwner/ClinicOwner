using BusinessObjects.Entities;

namespace BusinessObjects
{
    public class ReportDataObject
    {
        public Dentist MostAppointmentDentist { get; set; }
        public int MostAppointmentAmountOfDentist { get; set; }
        public Dentist LeastAppointmentDentist { get; set; }
        public int LeastAppointmentAmountOfDentist { get; set; }
        public int TotalAppointmentOfDentist { get; set; }
        public Service MostPopularService { get; set; }
        public int MostAppointmentAmountOfService { get; set; }
        public Service LeastPopularService { get; set; }
        public int LeastAppointmentAmountOfService { get; set; }
        public int TotalAppointmentOfService { get; set; }
        public Dictionary<int, int> ReportDentistAppointment { get; set; }
        public Dictionary<int, int> ReportServicesAppointment { get; set; }
        public double TotalRevenue { get; set; }
        public double RevenuePerAppointment { get; set; }
    }
}
