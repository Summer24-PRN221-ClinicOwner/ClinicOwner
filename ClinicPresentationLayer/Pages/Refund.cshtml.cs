using System.Threading.Tasks;
using BusinessObjects;
using ClinicServices.Interfaces;
using ClinicServices.VNPayService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ClinicPresentationLayer.Pages
{
    public class RefundModel : PageModel
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPaymentService _paymentService;
        private readonly VnPayService _vnPayService;
        private readonly ILogger<RefundModel> _logger;

        public RefundModel(IAppointmentService appointmentService, IPaymentService paymentService, VnPayService vnPayService, ILogger<RefundModel> logger)
        {
            _appointmentService = appointmentService;
            _paymentService = paymentService;
            _vnPayService = vnPayService;
            _logger = logger;
        }
        [BindProperty]
        public int AppointmentId { get; set; }
        public async Task<IActionResult> OnGetAsync(int Id)
        {
            var appointment = await _appointmentService.GetByIdAsync(Id);
            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Appointment not found.";
                _logger.LogError("Appointment not found.");
                return Page();
            }
            try
            {
                var isUpdated = await _appointmentService.UpdateAppointmentStatus(appointment.Id, (int)AppointmentStatus.Canceled, null);
                if (!isUpdated)
                {
                    TempData["ErrorMessage"] = "Failed to update appointment status.";
                    _logger.LogError("Failed to update appointment status.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Failed to update appointment status.");
                return Page();
            }
            AppointmentId = appointment.Id;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int appointmentId)
        {
            var appointment = await _appointmentService.GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Appointment not found.";
                _logger.LogError("Appointment not found.");
                return RedirectToPage("/Error");
            }

            var payment = await _paymentService.GetByIdAsync(appointment.PaymentId?? throw new Exception("Invalid Payment Id"));
            if (payment == null)
            {
                TempData["ErrorMessage"] = "Payment not found.";
                _logger.LogError("Payment not found.");
                return RedirectToPage("/Error");
            }

            var refundResult = await _vnPayService.RefundPaymentAsync(payment.TransactionId, payment.Amount, "Refund request");
            TempData["RefundMessage"] = refundResult;

            return RedirectToPage("/PatientHistory");
        }
    }
}
