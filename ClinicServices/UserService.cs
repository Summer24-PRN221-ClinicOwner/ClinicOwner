using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repository;
		public UserService(IUserRepository repository)
		{
			_repository = repository;
		}

		public Task<User> AddAsync(User entity)
		{
			return _repository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _repository.DeleteAsync(id);
		}

		public Task<IEnumerable<User>> GetAllAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<User> GetByIdAsync(int id)
		{
			return _repository.GetByIdAsync(id);
		}

		public Task UpdateAsync(User entity)
		{
			return _repository.UpdateAsync(entity);
		}
	}
}
