using BusinessObjects;
using BusinessObjects.Entities;
using ClinicServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ClinicPresentationLayer.Pages
{
	public class PaymentReturnModel : PageModel
	{
		private readonly IAppointmentService _appointmentService;
		private readonly IPaymentService _paymentService;
		private readonly ILogger<PaymentReturnModel> _logger;

		public PaymentReturnModel(IAppointmentService appointmentService, IPaymentService paymentService, ILogger<PaymentReturnModel> logger)
		{
			_appointmentService = appointmentService;
			_paymentService = paymentService;
			_logger = logger;
		}

		public async Task<IActionResult> OnGetAsync(
	decimal vnp_Amount,
	string vnp_ResponseCode,
	string vnp_TxnRef,
	string vnp_SecureHash,
	string vnp_BankCode,
	string vnp_BankTranNo,
	string vnp_CardType,
	string vnp_OrderInfo,
	string vnp_PayDate,
	string vnp_TmnCode,
	string vnp_TransactionNo,
	string vnp_TransactionStatus)
		{
			_logger.LogInformation("OnGetAsync has been triggered.");
			_logger.LogInformation($"PaymentReturn OnGetAsync called with: vnp_Amount={vnp_Amount}, vnp_ResponseCode={vnp_ResponseCode}, vnp_TxnRef={vnp_TxnRef}, vnp_SecureHash={vnp_SecureHash}, vnp_BankCode={vnp_BankCode}, vnp_BankTranNo={vnp_BankTranNo}, vnp_CardType={vnp_CardType}, vnp_OrderInfo={vnp_OrderInfo}, vnp_PayDate={vnp_PayDate}, vnp_TmnCode={vnp_TmnCode}, vnp_TransactionNo={vnp_TransactionNo}, vnp_TransactionStatus={vnp_TransactionStatus}");
            var appointmentJson = TempData["Appointment"] as string;
            var appointment = JsonConvert.DeserializeObject<BusinessObjects.Entities.Appointment>(appointmentJson);
			
            if (appointment == null)
                Console.WriteLine(appointmentJson);
            // Validate the vnp_TxnRef
            if (string.IsNullOrEmpty(vnp_TxnRef))
			{
				TempData["ErrorMessage"] = "Transaction reference is missing.";
				_logger.LogError("Transaction reference is missing.");
				return RedirectToPage("/Error");
			}

			// Validate the vnp_SecureHash if necessary
			//int appointmentId;
			//if (!int.TryParse(vnp_TxnRef, out appointmentId))
			//{
			//	TempData["ErrorMessage"] = "Invalid transaction reference.";
			//	_logger.LogError("Invalid transaction reference.");
			//	return RedirectToPage("/Error");
			//}

			if (appointment == null)
			{
				TempData["ErrorMessage"] = "Appointment not found.";
				_logger.LogError("Appointment not found.");
				return RedirectToPage("/Error");
			}

			if (vnp_ResponseCode == "00") // Payment success
			{
				//appointment.Status = (int)AppointmentStatus.Waiting;
				var payment = new Payment
				{
					Amount = vnp_Amount / 100, // Assuming vnp_Amount is in the smallest currency unit (like cents)
					PaymentStatus = "Paid",
					PaymentDate = DateTime.UtcNow.AddHours(7),
					TransactionId = vnp_TxnRef
				};
				await _paymentService.AddAsync(payment);
				appointment.PaymentId = payment.Id;
                await _appointmentService.AddAsync(appointment);
				return RedirectToPage("/PatientHistory");
			}
			else
			{
				TempData["ErrorMessage"] = "Payment failed. Please try again.";
				_logger.LogError("Payment failed with response code: " + vnp_ResponseCode);
				return RedirectToPage("/Appointment/Create", new { id = appointment.ServiceId });
			}
		}
	}
}
