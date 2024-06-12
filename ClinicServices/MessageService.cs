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
	public class MessageService : IMessageService, IGenericService<Message>
	{
		private readonly IMessageRepository _repository;
		public MessageService(IMessageRepository repository) 
		{
			_repository = repository;	
		}
		public Task<Message> AddAsync(Message entity)
		{
			return _repository.AddAsync(entity);
		}

		public Task DeleteAsync(int id)
		{
			return _repository.DeleteAsync(id);
		}

		public Task<IEnumerable<Message>> GetAllAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<Message> GetByIdAsync(int id)
		{
			return _repository.GetByIdAsync(id);
		}

		public Task UpdateAsync(Message entity)
		{
			return _repository.UpdateAsync(entity);	
		}
	}
}
