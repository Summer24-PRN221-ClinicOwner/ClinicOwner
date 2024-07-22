using System.Threading.Tasks;
using BusinessObjects;
using ClinicServices.Interfaces;
using ClinicServices.VNPayService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
            //check status cua appoint va payment truoc khi vao trang
            var appointment = await _appointmentService.GetByIdAsync(Id);
            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Appointment not found.";
                _logger.LogError("Appointment not found.");
                return Page();
            }
            else if(!await _appointmentService.IsValidStatusTransition(appointment.Status, (int)AppointmentStatus.Canceled, appointment.CreateDate, appointment.Payment.PaymentStatus, null))
            {
                TempData["ErrorMessage"] = "Appointment status and payment status is invalid.";
                _logger.LogError("Appointment status and payment status is invalid.");
                return Page();
            }
            

            var payment = await _paymentService.GetByIdAsync(appointment.PaymentId ?? throw new Exception("Invalid Payment Id"));
            if (payment == null)
            {
                TempData["ErrorMessage"] = "Payment not found.";
                _logger.LogError("Payment not found.");
                return RedirectToPage("/Error");
            }
            string transactionType = "02"; // full refund
            decimal refundAmount = payment.Amount;
            var cancellationTimeSpan = DateTime.UtcNow.Date - appointment.CreateDate.Date;
            if (cancellationTimeSpan.TotalDays >= 1)
            {
                transactionType = "03"; // half refund
                refundAmount = refundAmount/2;
            }
            
            var refundResult = await _vnPayService.RefundPaymentAsync(payment.TransactionId, refundAmount, "Refund request", appointment.CreateDate, payment.TransactionNo, transactionType);
            if(refundResult == "00" || refundResult == "94")
            {
            bool isUpdated = false;
                try
                {
                    if(transactionType == "03")
                    {
                         isUpdated = await _appointmentService.UpdateAppointmentStatus(appointment.Id, (int)AppointmentStatus.LateCanceled, null);
                    }
                    else
                    {
                         isUpdated = await _appointmentService.UpdateAppointmentStatus(appointment.Id, (int)AppointmentStatus.Canceled, null);
                    }
                    
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
            }
            else
            {
                TempData["ErrorMessage"] = "Fail VNPay Refund";
                return Page();
            }
            TempData["RefundMessage"] = refundResult;
            return RedirectToPage("/PatientHistory");
        }
    }
}
