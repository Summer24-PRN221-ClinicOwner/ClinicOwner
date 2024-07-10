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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _iPaymentRepository;

        public PaymentService(IPaymentRepository iPaymentRepository)
        {
            _iPaymentRepository = iPaymentRepository;
        }
        public Task<Payment> AddAsync(Payment entity)
        {
            return _iPaymentRepository.AddAsync(entity);
        }

        public Task DeleteAsync(int id)
        {
            return _iPaymentRepository.DeleteAsync(id);
        }

        public Task<List<Payment>> GetAllAsync()
        {
           return _iPaymentRepository.GetAllAsync();
        }

        public Task<Payment> GetByIdAsync(int id)
        {
            return _iPaymentRepository.GetByIdAsync(id);
        }

        public Task UpdateAsync(Payment entity)
        {
            return (_iPaymentRepository.UpdateAsync(entity));
        }
    }
}
