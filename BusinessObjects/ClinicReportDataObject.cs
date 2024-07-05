using BusinessObjects.Entities;

namespace BusinessObjects
{
    public class ClinicReportDataObject
    {
        public List<Dentist> MostAppointmentDentist { get; set; }
        public int MostAppointmentAmountOfDentist { get; set; }
        public List<Dentist> LeastAppointmentDentist { get; set; }
        public int LeastAppointmentAmountOfDentist { get; set; }
        public int TotalAppointmentOfDentist { get; set; }
        public List<Service> MostPopularService { get; set; }
        public int MostAppointmentAmountOfService { get; set; }
        public List<Service> LeastPopularService { get; set; }
        public int LeastAppointmentAmountOfService { get; set; }
        public int TotalAppointmentOfService { get; set; }
        public Dictionary<int, int> ReportDentistAppointment { get; set; }
        public Dictionary<int, int> ReportServicesAppointment { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal RevenuePerAppointment { get; set; }
        public decimal RevenuePerCustomer { get; set; }
        public DateTime? Date { get; set; }
    }
}
