using BusinessObjects;
using Microsoft.AspNetCore.Http;

namespace ClinicServices.VNPayService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
        Task<string> RefundPaymentAsync(string transactionId, decimal amount, string orderInfo, DateTime transactionDate, string transactionNo);

    }
}
