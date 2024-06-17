using BusinessObjects.Entities;

namespace ClinicServices.Interfaces
{
    public interface IUserServices : IBaseInterfaceServices<User>
    {
        public Task<User> Login(string username, string password);
    }
}
