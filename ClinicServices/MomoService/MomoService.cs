﻿using BusinessObjects.Entities;
using BusinessObjects;
using ClinicRepositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.MomoService
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;

        private readonly IPaymentRepository paymentRepository;

        private readonly IAppointmentRepository appointmentRepository;

        public MomoService(IOptions<MomoOptionModel> options, IPaymentRepository paymentRepository, IAppointmentRepository appointmentRepository)
        {
            _options = options;
            this.paymentRepository = paymentRepository;
            this.appointmentRepository = appointmentRepository;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(OrderInfoModel model, string? returnUrl)
        {
            model.OrderId = Guid.NewGuid().ToString();
            model.OrderInfo =
                //"Khách hàng: " + model.FullName + 
                "Payment for schedule booking: " + model.OrderInfo;
            model.Amount = model.Amount;
            returnUrl = returnUrl == null ? _options.Value.ReturnUrl : returnUrl;
            var rawData =
                $"partnerCode={_options.Value.PartnerCode}&accessKey={_options.Value.AccessKey}&requestId={model.OrderId}&amount={model.Amount}&orderId={model.OrderId}&orderInfo={model.OrderInfo}&returnUrl={returnUrl}&notifyUrl={_options.Value.NotifyUrl}&extraData=";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = returnUrl,
                orderId = model.OrderId,
                amount = model.Amount.ToString(),
                orderInfo = model.OrderInfo,
                requestId = model.OrderId,
                extraData = "",
                signature = signature
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "Amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;
            var orderType = collection.First(s => s.Key == "orderType").Value;
            var transId = collection.First(s => s.Key == "transId").Value;
            var message = collection.First(s => s.Key == "message").Value;
            var errorCode = collection.First(s => s.Key == "errorCode").Value;
            //var payType = collection.First(s => s.Key == "payType").Value;

            // Save payment for booking
            if (int.Parse(errorCode) != 0)
            {
                throw new Exception("Your payment executed unsuccessfully.");
            }
            var bookingIdsStrings = orderInfo.ToString().Split(": ");
            var bookingIdsString = bookingIdsStrings[1].Split(',');
            var bookingIds = new List<int>();
            for (int i = 0; i < bookingIdsString.Length; i++)
            {
                bookingIds.Add(int.Parse(bookingIdsString[i]));
            }
            var payment = new Payment
            {
                Amount = long.Parse(amount),
                PaymentDate = DateTime.UtcNow.AddHours(7),
                PaymentStatus = "Paid",
                //Description = orderInfo,
                TransactionId = transId.ToString(),
            };
            paymentRepository.AddAsync(payment);
            foreach (var bookingId in bookingIds)
            {
                var booking = appointmentRepository.GetByIdAsync(bookingId);
                /////////
            }
            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo
            };
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
