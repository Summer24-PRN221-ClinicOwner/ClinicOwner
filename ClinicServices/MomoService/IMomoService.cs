using BusinessObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.MomoService
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model, string? returnUrl);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
