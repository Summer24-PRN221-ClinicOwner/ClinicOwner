﻿using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using ClinicServices.Interfaces;

namespace ClinicServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> AddAsync(User entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            item.Status = 0;
            await _repository.UpdateAsync(item);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateAsync(User entity)
        {
            await _repository.UpdateAsync(entity);
        }
        public async Task<User?> LoginAsync(string username, string password)
        {
           return await _repository.GetUserByUsernamePass(username, password);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _repository.GetUserByUsername(username);
        }

        public async Task<bool> IsUsernameExisted(string username)
        {
            return await _repository.IsUsernameExisted(username);
        }

        public async Task<List<User>> GetAllStaffAsync()
        {
            return await _repository.GetAllStaffs();
        }
    }
}
