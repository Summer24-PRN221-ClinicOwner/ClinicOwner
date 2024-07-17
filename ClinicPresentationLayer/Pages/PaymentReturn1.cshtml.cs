using BusinessObjects.Entities;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ClinicPresentationLayer.Pages
{
    public class PaymentReturn1Model : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentReturnModel> _logger;

        public PaymentReturn1Model(IAppointmentService appointmentService, IPaymentService paymentService, ILogger<PaymentReturnModel> logger)
        {
            _appointmentService = appointmentService;
            _paymentService = paymentService;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync(
    string partnerCode,
    string accessKey,
    string requestId,
    string amount,
    string orderId,
    string orderInfo,
    string orderType,
    string transId,
    string message,
    string localMessage,
    string responseTime,
    string errorCode,
    string payType,
    string extraData,
    string signature)
        {
            _logger.LogInformation("OnGetAsync has been triggered.");
            var appointmentJson = TempData["Appointment"] as string;
            var appointment = JsonConvert.DeserializeObject<BusinessObjects.Entities.Appointment>(appointmentJson);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Appointment not found.";
                _logger.LogError("Appointment not found.");
                return RedirectToPage("/Error");
            }

            if (errorCode == "0") // Payment success
            {
                var payment = new Payment
                {
                    Amount = decimal.Parse(amount), 
                    PaymentStatus = "Paid",
                    PaymentDate = DateTime.UtcNow.AddHours(7),
                    TransactionId = transId
                };
                //await _paymentService.AddAsync(payment);
                //appointment.PaymentId = payment.Id;
                await _appointmentService.AddAsync(appointment, payment);
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = "Payment failed. Please try again.";
                _logger.LogError("Payment failed with error code: " + errorCode);
                return RedirectToPage("/Error");
            }
        }
    }
}
