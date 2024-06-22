using BusinessObjects;
using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.EmailService;
using ClinicServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
            IDentistRepository dentistRepository,IServiceRepository serviceRepository, IRoomRepository roomRepository, IEmailSender emailSender)
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
            if(await _appointmentRepository.AddAsync(entity) != null)
            {
                //update dentist available
                UpdateDentistAvailabilityAsync(entity.DentistId, entity.AppointDate, entity.StartSlot, entity.EndSlot);
                //update room available
                UpdateRoomAvailabilityAsync(entity.RoomId, entity.AppointDate, entity.StartSlot, entity.EndSlot);
                //send email about the appointment to the patient
                SendEmailToPatient(entity.PatientId, entity);
            }
            else
            {
                throw new Exception("Fail to add new appointment");
            }
            return await _appointmentRepository.AddAsync(entity);
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
            return await _roomAvailabilityRepository.GetSlotsAvailabilityAsync(date, slotRequired);
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _appointmentRepository.GetByIdAsync(id);
        }

        public async Task<Room> GetRoomAvailable(DateTime date, int slotRequired)
        {
            return await _roomAvailabilityRepository.GetAvailableRoomAsync(date, slotRequired);
        }

        public async Task UpdateAsync(Appointment entity)
        {
            await _appointmentRepository.UpdateAsync(entity);
        }
        public async Task UpdateDentistAvailabilityAsync(int dentistId, DateTime date, int startSlot, int endSlot)
        {
            // Fetch the dentist's availability for the specified date
            var availList = await _dentistAvailabilityRepository.GetAllAsync();

            var dentistAvailability = availList.FirstOrDefault(da => da.DentistId == dentistId && da.Day.Date == date.Date);

            if (dentistAvailability == null)
            {
                throw new Exception("Dentist availability not found for the specified date.");
            }

            // Convert the AvailableSlots string to a char array for easier manipulation
            var slots = dentistAvailability.AvailableSlots.ToCharArray();

            // Mark the slots as unavailable
            for (int i = startSlot; i <= endSlot ; i++)
            {
                if (i >= slots.Length || slots[i - 1] == '0') // Ensure slot is within bounds and currently available
                {
                    throw new Exception("One or more slots are already unavailable.");
                }
                slots[i-1] = '0';
            }

            // Convert the char array back to a string and update the availability
            dentistAvailability.AvailableSlots = new string(slots);

            // Save the changes
            await _dentistAvailabilityRepository.UpdateAsync(dentistAvailability);
        }
        public async Task UpdateRoomAvailabilityAsync(int roomId, DateTime date, int startSlot, int endSlot)
        {
            // Fetch the dentist's availability for the specified date
            var availList = await _roomAvailabilityRepository.GetAllAsync();

            var roomAvailability = availList.FirstOrDefault(da => da.RoomId == roomId && da.Day.Date == date.Date);

            if (roomAvailability == null)
            {
                throw new Exception("Dentist availability not found for the specified date.");
            }

            // Convert the AvailableSlots string to a char array for easier manipulation
            var slots = roomAvailability.AvailableSlots.ToCharArray();

            // Mark the slots as unavailable
            for (int i = startSlot; i <= endSlot; i++)
            {
                if (i >= slots.Length || slots[i - 1] == '0') // Ensure slot is within bounds and currently available
                {
                    throw new Exception("One or more slots are already unavailable.");
                }
                slots[i-1] = '0';
            }

            // Convert the char array back to a string and update the availability
            roomAvailability.AvailableSlots = new string(slots);

            // Save the changes
            await _roomAvailabilityRepository.UpdateAsync(roomAvailability);
        }
        public async Task SendEmailToPatient(int patientId, Appointment details)
        {
            Patient patient =  await _patientRepository.GetByIdAsync(patientId);
            Dentist dentist = await _dentistRepository.GetByIdAsync(details.DentistId);
            Room room = await _roomRepository.GetByIdAsync(details.RoomId);
            Service serv =  await _serviceRepository.GetByIdAsync(details.ServiceId);
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
            EmailAddress patientEmail = new EmailAddress{ Email = patient.Email, DisplayName = patient.Name };
            EmailService.Message message = new EmailService.Message(new List<EmailAddress> { patientEmail }, subject, content);
            await _emailSender.SendEmailAsync(message);
        }
    }
}
