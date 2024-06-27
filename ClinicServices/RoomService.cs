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
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<bool> AddAsync(Room entity)
        {
            var result = await _roomRepository.AddAsync(entity);
            if(result == null)
            {
                throw new Exception("add Room fail");
            }
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _roomRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public async Task<List<Room>> GetAllAsync()
        {
           return  await _roomRepository.GetRoomsAsync();
        }

        public async Task<Room> GetByIdAsync(int id)
        {
            return await _roomRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Room entity)
        {
            try
            {
                await _roomRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }
    }
}
