using BusinessObjects.Entities;
using ClinicRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicRepositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository() : base()
        {
        }

        public async Task SavePaymentAsync(Payment payment)
        {
            // Check if the appointment exists
            var appointmentExists = await _context.Appointments.AnyAsync(a => a.Id == payment.AppointmentId);
            if (!appointmentExists)
            {
                throw new InvalidOperationException("Appointment does not exist.");
                // Or handle this scenario as per your application's requirements
            }
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }
    }
}
