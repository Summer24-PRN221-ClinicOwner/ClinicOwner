using BusinessObjects;
using ClinicPresentationLayer.Extension.Libraries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Text;


namespace ClinicServices.VNPayService
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public VnPayService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }



        public PaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

            return response;
        }

        public async Task<string> RefundPaymentAsync(string transactionId, decimal amount, string orderInfo, DateTime transactionDate)
        {
            var vnpay = new VnPayLibrary();
            string requestId = Guid.NewGuid().ToString();
            string version = "2.1.0";
            string command = "refund";
            string tmnCode = _configuration["VnPay:TmnCode"];
            string transactionType = "02"; // Assuming full refund
            string createBy = "Admin"; // Replace with the actual user performing the refund
            string createDate = DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss");
            string ipAddr = "192.168.1.66"; // Replace with actual IP address if necessary

            vnpay.AddRequestData("vnp_RequestId", requestId);
            vnpay.AddRequestData("vnp_Version", version);
            vnpay.AddRequestData("vnp_Command", command);
            vnpay.AddRequestData("vnp_TmnCode", tmnCode);
            vnpay.AddRequestData("vnp_TransactionType", transactionType);
            vnpay.AddRequestData("vnp_TxnRef", transactionId);
            vnpay.AddRequestData("vnp_Amount", ((int)(amount * 100)).ToString());
            vnpay.AddRequestData("vnp_TransactionDate", transactionDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CreateBy", createBy);
            vnpay.AddRequestData("vnp_CreateDate", createDate);
            vnpay.AddRequestData("vnp_IpAddr", ipAddr);
            vnpay.AddRequestData("vnp_OrderInfo", orderInfo);
            vnpay.AddRequestData("vnp_TransactionNo", "14512160");// Thêm Trường vào DB !!!! NHỚ SỬA SECURE HASH

            // Generate checksum
            string secretKey = _configuration["VnPay:HashSecret"];
            string data = $"{requestId}|{version}|{command}|{tmnCode}|{transactionType}|{transactionId}|{((int)(amount * 100)).ToString()}|14512160|{transactionDate.ToString("yyyyMMddHHmmss")}|{createBy}|{createDate}|{ipAddr}|{orderInfo}";
            string secureHash = VnPayLibrary.HmacSHA512(secretKey, data);
            vnpay.AddRequestData("vnp_SecureHash", secureHash);

            // Convert to JSON
            var jsonContent = new JObject();
            foreach (var item in vnpay.GetRequestData())
            {
                jsonContent[item.Key] = item.Value;
            }
            var check = jsonContent.ToString();
            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(jsonContent.ToString(), Encoding.UTF8, "application/json");
            var check2 = content;
            var response = await client.PostAsync("https://sandbox.vnpayment.vn/merchant_webapi/api/transaction", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseContent);

            if (jsonResponse["vnp_ResponseCode"].ToString() == "00")
            {
                return "Refund successful.";
            }
            else
            {
                return $"Refund failed: {jsonResponse["vnp_ResponseMessage"]}";
            }
        }
    }
}

