using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.EmailService;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDentistAvailabilityRepository _dentistAvailabilityRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IDentistRepository _dentistRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IEmailSender _emailSender;

        public AppointmentService(IAppointmentRepository iAppointmentRepository, IRoomAvailabilityRepository roomAvailabilityRepository,
            IDentistAvailabilityRepository dentistAvailabilityRepository, IPatientRepository patientRepository,
            IDentistRepository dentistRepository, IServiceRepository serviceRepository, IRoomRepository roomRepository, IEmailSender emailSender)
        {
            _appointmentRepository = iAppointmentRepository;
            _roomAvailabilityRepository = roomAvailabilityRepository;
            _dentistAvailabilityRepository = dentistAvailabilityRepository;
            _patientRepository = patientRepository;
            _serviceRepository = serviceRepository;
            _dentistRepository = dentistRepository;
            _roomRepository = roomRepository;
            _emailSender = emailSender;
        }

        public async Task<Appointment> AddAsync(Appointment entity)
        {

            //update dentist available
            var updateDentist = await _dentistAvailabilityRepository.UpdateAvaialeString(entity.DentistId, entity.AppointDate.Date, entity.StartSlot, entity.EndSlot - entity.StartSlot + 1);
            //update room available
            var updateRoom = await _roomAvailabilityRepository.UpdateAvaialeString(entity.RoomId, entity.AppointDate, entity.StartSlot, entity.EndSlot - entity.StartSlot + 1);
            var updateAppointment = await _appointmentRepository.AddAppointmentAsync(entity);
            //send email about the appointment to the patient
            if (updateDentist && updateRoom && updateAppointment)
            {
                SendEmailToPatient(entity.PatientId, entity);
            }
            else
            {
                throw new Exception("Fail to add new appointment");
            }
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _appointmentRepository.DeleteAsync(id);
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _appointmentRepository.GetAllAsync();
        }

        public async Task<List<Slot>> GetAvailableSlotAsync(DateTime date, int slotRequired)
        {
            return await _roomAvailabilityRepository.GetRoomsAvailabilityAsync(date, slotRequired);
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _appointmentRepository.GetByIdAsync(id);
        }

        public Room GetRoomAvailable(DateTime date, int slotRequired)
        {
            return _roomAvailabilityRepository.GetAvailableRoomAsync(date, slotRequired);
        }

        public async Task UpdateAsync(Appointment entity)
        {
            await _appointmentRepository.UpdateAsync(entity);
        }
        public async Task SendEmailToPatient(int patientId, Appointment details)
        {
            Patient patient = await _patientRepository.GetByIdAsync(patientId);
            Dentist dentist = await _dentistRepository.GetByIdAsync(details.DentistId);
            Room room = await _roomRepository.GetByIdAsync(details.RoomId);
            Service serv = await _serviceRepository.GetByIdAsync(details.ServiceId);
            Slot startSlot = SlotDefiner.NewSlot(details.StartSlot);
            var subject = "Your Appointment Details";
            var content = $"Dear {patient.Name},<br/><br/>" +
                          $"Here are the details of your upcoming appointment:<br/>" +
                          $"<b>Appointment Date:</b> {details.AppointDate.Date}<br/>" +
                          $"<b>Service name:</b> {serv.Name}<br/>" +
                          $"<b>Dentist name:</b> {dentist.Name}<br/>" +
                          $"<b>Start Slot:</b> {startSlot.DisplayTime}<br/>" +
                          $"<b>Room ID:</b> {room.RoomNumber}<br/>" +
                          $"Thank you,<br/>";
            EmailAddress emailAddress = new() { Email = patient.Email, DisplayName = patient.Name };
            EmailAddress patientEmail = emailAddress;
            EmailService.Message message = new([patientEmail], subject, content);
            await _emailSender.SendEmailAsync(message);
        }

        public async Task<AppointmentDentistSchedule> GetAppoinmentSchedule(int pageWeek)
        {
            var date = DateTime.Now;
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }
            var Monday = await _appointmentRepository.GetByDate(date);
            var Tuesday = await _appointmentRepository.GetByDate(date.AddDays(1));
            var Wednesday = await _appointmentRepository.GetByDate(date.AddDays(2));
            var Thursday = await _appointmentRepository.GetByDate(date.AddDays(3));
            var Friday = await _appointmentRepository.GetByDate(date.AddDays(4));
            var Saturday = await _appointmentRepository.GetByDate(date.AddDays(5));
            var Sunday = await _appointmentRepository.GetByDate(date.AddDays(6));

            AppointmentDentistSchedule result = new()
            {
                Monday = Monday,
                Tuesday = Tuesday,
                Wednesday = Wednesday,
                Thursday = Thursday,
                Friday = Friday,
                Saturday = Saturday,
                Sunday = Sunday
            };
            return result;
        }
    }
}
