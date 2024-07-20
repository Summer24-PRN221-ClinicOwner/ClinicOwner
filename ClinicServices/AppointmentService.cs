using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.EmailService;
using ClinicServices.Interfaces;
using System.Net.NetworkInformation;

namespace ClinicServices
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDentistAvailabilityRepository _dentistAvailabilityRepository;
        private readonly IDentistRepository _dentistRepository;
        private readonly IEmailSender _emailSender;
        private readonly IPaymentService _paymentService;

        public AppointmentService(IAppointmentRepository iAppointmentRepository, IRoomAvailabilityRepository roomAvailabilityRepository,
            IDentistAvailabilityRepository dentistAvailabilityRepository, IDentistRepository dentistRepository, IEmailSender emailSender
            , IPaymentService paymentService)
        {
            _appointmentRepository = iAppointmentRepository;
            _roomAvailabilityRepository = roomAvailabilityRepository;
            _dentistAvailabilityRepository = dentistAvailabilityRepository;
            _dentistRepository = dentistRepository;
            _emailSender = emailSender;
            _paymentService = paymentService;
        }

        public async Task<Appointment> AddAsync(Appointment entity, Payment payment)
        {

            //update dentist available
            var updateDentist = await _dentistAvailabilityRepository.UpdateAvaialeString(entity.DentistId, entity.AppointDate.Date, entity.StartSlot, entity.EndSlot - entity.StartSlot + 1);
            //update room available
            var updateRoom = await _roomAvailabilityRepository.UpdateAvaialeString(entity.RoomId, entity.AppointDate, entity.StartSlot, entity.EndSlot - entity.StartSlot + 1);
            var updateAppointment = await _appointmentRepository.AddAppointmentAsync(entity);
            //send email about the appointment to the patient
            if (updateDentist && updateRoom && updateAppointment)
            {

                _dentistAvailabilityRepository.SaveChanges();
                _roomAvailabilityRepository.SaveChanges();
                await _paymentService.AddAsync(payment);
                entity.PaymentId = payment.Id;
                _appointmentRepository.SaveChanges();

                await SendEmailToPatient(entity);
            }
            else
            {
                _dentistAvailabilityRepository.Dispose();
                _roomAvailabilityRepository.Dispose();
                _appointmentRepository.Dispose();
                throw new Exception("Fail to add new appointment!!");
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
            return await _appointmentRepository.GetAppointmentsByIdAsync(id);
        }

        public Room GetRoomAvailable(DateTime date, int slotRequired, int startSlot)
        {
            return _roomAvailabilityRepository.GetAvailableRoomAsync(date, slotRequired, startSlot);
        }

        public async Task UpdateAsync(Appointment entity)
        {
            await _appointmentRepository.UpdateAsync(entity);
        }
        public async Task SendEmailToPatient(Appointment appointmentId)
        {
           var appointment = await _appointmentRepository.GetAppointmentsByIdAsync(appointmentId.Id);
            if (appointment == null)
            {
                throw new Exception("Can not find appointment to send email");
            }

            Patient patient = appointment.Patient;
            Dentist dentist = appointment.Dentist;
            Room room = appointment.Room;
            Service service = appointment.Service;
            Slot startSlot = SlotDefiner.NewSlot(appointment.StartSlot);

            var subject = "Your Appointment Details";
            var content = $"Dear {patient.Name},<br/><br/>" +
                          $"Here are the details of your upcoming appointment:<br/>" +
                          $"<b>Appointment Date:</b> {appointment.AppointDate:dd/MM/yyyy}<br/>" +
                          $"<b>Service name:</b> {service.Name}<br/>" +
                          $"<b>Dentist name:</b> {dentist.Name}<br/>" +
                          $"<b>Start Slot:</b> {SlotDefiner.GetSlotDisplayTime(startSlot, service.Duration)}<br/>" +
                          $"<b>Room ID:</b> {room.RoomNumber}<br/>" +
                          $"Thank you,<br/>";

            EmailAddress emailAddress = new() { Email = patient.Email, DisplayName = patient.Name };
            EmailService.Message message = new(new List<EmailAddress> { emailAddress }, subject, content);

            await _emailSender.SendEmailAsync(message);
        }

        public async Task<AppointmentDentistSchedule> GetAppoinmentSchedule(int pageWeek, int dentistId)
        {
            Dentist dentist = await _dentistRepository.GetByIdAsync(dentistId);
            var date = DateTime.Now;
            date = date.AddDays(7 * pageWeek);
            while (date.DayOfWeek != DayOfWeek.Monday)
            {
                date = date.AddDays(-1);
            }

            AppointmentDentistSchedule result = new()
            {
                Monday = await _appointmentRepository.GetByDate(date, dentistId),
                Tuesday = await _appointmentRepository.GetByDate(date.AddDays(1), dentistId),
                Wednesday = await _appointmentRepository.GetByDate(date.AddDays(2), dentistId),
                Thursday = await _appointmentRepository.GetByDate(date.AddDays(3), dentistId),
                Friday = await _appointmentRepository.GetByDate(date.AddDays(4), dentistId),
                Saturday = await _appointmentRepository.GetByDate(date.AddDays(5), dentistId),
                Sunday = await _appointmentRepository.GetByDate(date.AddDays(6), dentistId)
            };
            return result;

        }

        public async Task<List<Appointment>> GetAppoinmentHistoryAsync(int patientId)
        {
            try
            {
                var result = await _appointmentRepository.GetByPatientId(patientId);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error when get appointment history", ex);
            }

        }
            public async Task<List<Appointment>> GetAppointmentsBeforeDaysAsync(int days)
        {
            var result = await _appointmentRepository.GetAppointmentsBeforeDaysAsync(days);
            if (result == null)
            {
                throw new Exception($"No Appointment found for next {days} day(s)");
            }
            return result;
        }

        public async Task<bool> UpdateAppointmentStatus(int appointId, int newStatus, DateTime? newDate)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(appointId);
            if (appointment == null)
            {
                throw new Exception("Can not found appointment");
            }
            if(IsValidStatusTransition(appointment.Status, newStatus, appointment.AppointDate, newDate))
            {
                if (newStatus == (int)AppointmentStatus.ReScheduled)
                {
                    appointment.AppointDate = newDate.Value;
                }
                appointment.ModifyDate = DateTime.UtcNow.AddHours(7);
                appointment.Status = newStatus;
                try
                {
                    await _appointmentRepository.UpdateAsync(appointment);
                    return true;
                }catch (Exception ex)
                {
                    throw new Exception("Failed to update appointment.", ex);
                }
            }
            else
            {
                throw new Exception("Invalid status transition");
            }
        }
        private bool IsValidStatusTransition(int currentStatus, int newStatus, DateTime appointmentDate, DateTime? newDate)
        {
            if (!Enum.IsDefined(typeof(AppointmentStatus), currentStatus))
            {
                return false;
            }

            // Validate new newStatus
            if (!Enum.IsDefined(typeof(AppointmentStatus), newStatus))
            {
                return false;
            }
            AppointmentStatus current = (AppointmentStatus)currentStatus;
            AppointmentStatus next = (AppointmentStatus)newStatus;

            switch (current)
            {
                case AppointmentStatus.Waiting:
                    if (next == AppointmentStatus.Canceled)
                    {
                        // Can only cancel if at least 3 days before the appointment date
                        return (appointmentDate.Date - DateTime.Now.Date).TotalDays >= 3;
                    }
                    if (next == AppointmentStatus.ReScheduled)
                    {
                        return newDate.HasValue;
                    }
                    return next == AppointmentStatus.Checkin || next == AppointmentStatus.Absent;

                case AppointmentStatus.ReScheduled:
                    if (next == AppointmentStatus.ReScheduled)
                    {
                        return false;
                    }
                    if (next == AppointmentStatus.Canceled)
                    {
                        return (appointmentDate - DateTime.Now).TotalDays >= 3;
                    }
                    return next == AppointmentStatus.Checkin || next == AppointmentStatus.Absent;

                case AppointmentStatus.Checkin:
                    return next == AppointmentStatus.Reported;

                case AppointmentStatus.Absent:
                case AppointmentStatus.Canceled:
                case AppointmentStatus.Reported:
                    return false;

                default:
                    return false;
            }
        }
        public async Task<int> GetAppointmentCountAsync()
        {
            return await _appointmentRepository.GetAppointmentCountAsync();
        }

        public async Task<int> GetTodayAppointmentCountAsync()
        {
            return await _appointmentRepository.GetTodayAppointmentCountAsync();
        }

        public async Task<decimal> GetTodayTotalEarningsAsync()
        {
            return await _appointmentRepository.GetTodayTotalEarningsAsync();
        }
        public async Task<Appointment> GetAppointmentsByIdAsync(int id)
        {
            var result = await _appointmentRepository.GetAppointmentsByIdAsync(id);
            if (result == null)
            {
                throw new Exception($"No Appointment found");
            }
            return result;
        }
    }
}
