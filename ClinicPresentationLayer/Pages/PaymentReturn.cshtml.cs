using BusinessObjects.Entities;
using BusinessObjects;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicPresentationLayer.Pages.Appointment
{
    public class PaymentReturnModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPaymentService _paymentService;

        public PaymentReturnModel(IAppointmentService appointmentService, IPaymentService paymentService)
        {
            _appointmentService = appointmentService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> OnGetAsync(string vnp_TxnRef, string vnp_TransactionStatus)
        {
            if (string.IsNullOrEmpty(vnp_TxnRef) || string.IsNullOrEmpty(vnp_TransactionStatus))
            {
                TempData["ErrorMessage"] = "Invalid payment return data.";
                return RedirectToPage("/Error");
            }

            int appointmentId = int.Parse(vnp_TxnRef);
            var appointment = await _appointmentService.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Appointment not found.";
                return RedirectToPage("/Error");
            }

            if (vnp_TransactionStatus == "00")
            {
                appointment.Status = (int)AppointmentStatus.Waiting;
                var payment = new Payment
                {
                    AppointmentId = appointmentId,
                    Amount = (decimal)appointment.Service.Cost,
                    PaymentStatus = "Paid",
                    PaymentDate = DateTime.UtcNow.AddHours(7),
                    TransactionId = vnp_TxnRef
                };
                await _paymentService.AddAsync(payment);
                await _appointmentService.UpdateAsync(appointment);
                return RedirectToPage("/PatientHistory");
            }
            else
            {
                TempData["ErrorMessage"] = "Payment failed. Please try again.";
                return RedirectToPage("/Appointment/Create", new { id = appointment.ServiceId });
            }
        }
    }
}
