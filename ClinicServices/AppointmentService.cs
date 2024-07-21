﻿using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.EmailService;
using ClinicServices.Interfaces;
using System.Net.NetworkInformation;
using System.Transactions;

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
                throw new Exception($"No Appointment found for new {days} day(s)");
            }
            return result;
        }

        public async Task<bool> UpdateAppointmentStatus(int appointId, int newStatus, DateTime? newDate)
        {
            var appointment = await _appointmentRepository.GetAppointmentsByIdAsync(appointId);
            if (appointment == null)
            {
                throw new Exception("Can not found appointment");
            }
            if (appointment.Payment == null)
            {
                throw new Exception("Can not found payment for appointment");
            }
            string newPaymentStatus = appointment.Payment.PaymentStatus;
            switch ((AppointmentStatus)newStatus)
            {
                case AppointmentStatus.Canceled:
                    if (appointment.Payment.PaymentStatus == PaymentStatus.PAID)
                    {
                        newPaymentStatus = PaymentStatus.REFUNDED;
                    }
                    else if (appointment.Payment.PaymentStatus == PaymentStatus.CHECKOUT)
                    {
                        newPaymentStatus = PaymentStatus.CHECKOUT_REFUNDED;
                    }
                    break;
            }
            if (await IsValidStatusTransition(appointment.Status, newStatus, appointment.CreateDate,appointment.Payment.PaymentStatus, newDate))
            {
                if (newStatus == (int)AppointmentStatus.ReScheduled)
                {
                    appointment.AppointDate = newDate.Value;
                }
                appointment.ModifyDate = DateTime.UtcNow.AddHours(7);
                appointment.Status = newStatus;
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        await _appointmentRepository.UpdateAsync(appointment);
                        if (newStatus == (int)AppointmentStatus.Canceled && newPaymentStatus != appointment.Payment.PaymentStatus)
                        {
                            await _paymentService.UpdateStatus(appointment.Payment.Id, newPaymentStatus);
                        }
                        scope.Complete();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to update appointment or payment status.", ex);
                    }
                }
            }
            else
            {
                throw new Exception("Invalid status transition");
            }
        }
        public async Task<bool> IsValidStatusTransition(int currentStatus, int newStatus, DateTime createDate, string paymentStatus, DateTime? newDate)
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
            AppointmentStatus @new = (AppointmentStatus)newStatus;

            switch (current)
            {
                case AppointmentStatus.Waiting:
                    if (@new == AppointmentStatus.Canceled)
                    {
                        // ngay kham xa so voi now, tinh = date, cung ngay tao 
                        return (createDate.Date - DateTime.Now.Date).TotalDays < 1;
                    }
                    if (@new == AppointmentStatus.ReScheduled)
                    {
                        return newDate.HasValue;
                    }
                    return @new == AppointmentStatus.Checkin || @new == AppointmentStatus.Absent;

                case AppointmentStatus.ReScheduled:
                    if (@new == AppointmentStatus.ReScheduled)
                    {
                        return false;
                    }
                    if (@new == AppointmentStatus.Canceled)
                    {
                        return (createDate - DateTime.Now).TotalDays >= 3;
                    }
                    return @new == AppointmentStatus.Checkin || @new == AppointmentStatus.Absent;

                case AppointmentStatus.Checkin:

                    return @new == AppointmentStatus.Reported;

                case AppointmentStatus.Absent:
                case AppointmentStatus.Canceled:
                case AppointmentStatus.Reported:
                    return false;

                default:
                    return false;
            }
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

        public async Task<List<Appointment>> GetAppointmentsOfTodayAsync()
        {
            var appointList = await _appointmentRepository.GetAllAsync();
            var result =  appointList.Where(x=> x.AppointDate.Date == DateTime.Now.Date).ToList();
            return result;
        }
    }
}
