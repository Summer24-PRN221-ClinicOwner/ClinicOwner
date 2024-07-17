using BusinessObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace ClinicServices.MomoService.Libraries
{
    public class MoMoLibrary
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<MomoOptionModel> _options;

        public MoMoLibrary(IConfiguration configuration, IOptions<MomoOptionModel> options)
        {
            _options = options;
        }

        public string GenerateSignature(Dictionary<string, string> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            string secretKey = parameters["secretKey"];
            parameters.Remove("secretKey");

            var rawData = string.Join("&", parameters.OrderBy(kvp => kvp.Key).Select(kvp => $"{kvp.Key}={kvp.Value}"));

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public async Task<string> CreatePaymentUrl(string orderId, string requestId, long amount, string orderInfo, string returnUrl)
        {
            var endpoint = _configuration["MoMo:Endpoint"];
            var partnerCode = _configuration["MoMo:PartnerCode"];
            var accessKey = _configuration["MoMo:AccessKey"];
            var secretKey = _configuration["MoMo:SecretKey"];
            var notifyUrl = _configuration["MoMo:NotifyUrl"];
            var extraData = ""; // Use actual extra data if needed

            // Log configuration values
            Console.WriteLine("MoMo Configuration:");
            Console.WriteLine($"Endpoint: {endpoint}");
            Console.WriteLine($"PartnerCode: {partnerCode}");
            Console.WriteLine($"AccessKey: {accessKey}");
            Console.WriteLine($"SecretKey: {secretKey}");
            Console.WriteLine($"NotifyUrl: {notifyUrl}");
            Console.WriteLine($"ExtraData: {extraData}");

            var requestParameters = new Dictionary<string, string>
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount.ToString() },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyUrl },
                { "extraData", extraData },
                { "secretKey", secretKey },
                { "requestType", "captureMoMoWallet" }
            };

            var signature = GenerateSignature(requestParameters);
            requestParameters.Add("signature", signature);

            var jsonRequest = JsonConvert.SerializeObject(requestParameters);

            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                try
                {
                    var response = await httpClient.PostAsync(endpoint, content);
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Log the request and response content for debugging
                    Console.WriteLine("Request to MoMo API: " + jsonRequest);
                    Console.WriteLine("Response from MoMo API: " + jsonResponse);

                    var responseObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                    if (responseObject != null && responseObject.ContainsKey("payUrl"))
                    {
                        return responseObject["payUrl"];
                    }
                    else
                    {
                        throw new Exception("Invalid response format from MoMo API: " + jsonResponse);
                    }
                }
                catch (JsonReaderException ex)
                {
                    // Handle non-JSON response
                    Console.WriteLine("Failed to parse response as JSON: " + ex.Message);
                    Console.WriteLine("Response content: " + ex.ToString());
                    throw new Exception("Invalid response format from MoMo API.");
                }
                catch (Exception ex)
                {
                    // Log other exceptions
                    Console.WriteLine("An error occurred: " + ex.Message);
                    throw;
                }
            }
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
    }
}
