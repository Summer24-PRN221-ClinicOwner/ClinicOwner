using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.EmailService;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        public readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public PatientService(IPatientRepository repository, IUserService userService, IEmailSender emailSender)
        {
            _repository = repository;
            _userService = userService;
            _emailSender = emailSender;
        }

        public async Task<Patient> AddAsync(Patient entity, User userAccount)
        {
            var existingUser = await _userService.GetByUsernameAsync(userAccount.Username);
            if (!_repository.InformationIsUnique(entity.Phone, entity.Email)) throw new Exception("Email or Phone is duplicated!");
            if (existingUser != null)
            {
                throw new Exception("A user with the same username already exists.");
            }
            userAccount.Status = 1;
            var newAccount = await _userService.AddAsync(userAccount);
            entity.Id = newAccount.Id;
            return await _repository.AddAsync(entity);
        }
        public async Task<Patient> StaffAddAsync(Patient entity, User userAccount)
        {
            var existingUser = await _userService.GetByUsernameAsync(userAccount.Username);
            if (existingUser != null)
            {
                throw new Exception("A user with the same username already exists.");
            }
            if (!_repository.InformationIsUnique(entity.Phone, entity.Email)) throw new Exception("Email or phone is duplicated!");
            userAccount.Status = 1;
            var newAccount = await _userService.AddAsync(userAccount);
            entity.Id = newAccount.Id;
            var addedPatient = await _repository.AddAsync(entity);
            await SendEmailToPatientAsync(addedPatient, newAccount);
            return addedPatient;
        }
        public async Task DeleteAsync(int id)
        {
            await _userService.DeleteAsync(id);
        }

        public Task<List<Patient>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Patient> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task UpdateAsync(Patient entity)
        {
            if (!_repository.InformationIsUnique(entity.Phone, entity.Email)) throw new Exception("Email or phone is duplicated!");
            return _repository.UpdateAsync(entity);
        }

        public async Task<Patient> FindPatientAsync(string searchTerm)
        {
            var PatientList = await _repository.GetAllAsync();
            var patient = PatientList.FirstOrDefault(p => p.Email.Contains(searchTerm) || p.Phone.Contains(searchTerm));
            return patient;
        }
        private async Task SendEmailToPatientAsync(Patient patient, User userAccount)
        {
            var subject = "Welcome to Our Clinic!";
            var content = $"Dear {patient.Name},<br/><br/>" +
                          $"Thank you for registering with our clinic. Here are your account details:<br/>" +
                          $"<b>Email:</b> {patient.Email}<br/>" +
                          $"<b>Phone:</b> {patient.Phone}<br/>" +
                          $"<b>Username:</b> {userAccount.Username}<br/>" +
                          $"<b>Password:</b> {userAccount.Password}<br/><br/>";
                          

            EmailAddress emailAddress = new() { Email = patient.Email, DisplayName = patient.Name };
            EmailService.Message message = new(new List<EmailAddress> { emailAddress }, subject, content);

            await _emailSender.SendEmailAsync(message);
        }
    }
}
