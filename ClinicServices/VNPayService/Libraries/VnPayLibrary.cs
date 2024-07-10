using BusinessObjects;
using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace ClinicPresentationLayer.Extension.Libraries
{
    public class VnPayLibrary
    {
        private SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            var data = new StringBuilder();
            foreach (var kv in _requestData)
            {
                if (data.Length > 0)
                {
                    data.Append('&');
                }
                data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value));
            }

            var signData = HmacSHA512(vnp_HashSecret, data.ToString());
            return baseUrl + "?" + data + "&vnp_SecureHash=" + signData;
        }

        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            var keyByte = Encoding.UTF8.GetBytes(key);
            using (var hmacsha512 = new HMACSHA512(keyByte))
            {
                var hashValue = hmacsha512.ComputeHash(Encoding.UTF8.GetBytes(inputData));
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }

        public string GetIpAddress(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }

        public PaymentResponseModel GetFullResponseData(IQueryCollection collections, string vnp_HashSecret)
        {
            var responseData = new SortedList<string, string>(new VnPayCompare());
            foreach (var item in collections)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    responseData.Add(item.Key, item.Value);
                }
            }

            var vnp_SecureHash = collections["vnp_SecureHash"];
            if (string.IsNullOrEmpty(vnp_SecureHash))
            {
                return new PaymentResponseModel { Status = "97", Message = "Invalid signature" };
            }

            responseData.Remove("vnp_SecureHash");
            var inputRawData = new StringBuilder();
            foreach (var kv in responseData)
            {
                if (inputRawData.Length > 0)
                {
                    inputRawData.Append('&');
                }
                inputRawData.Append(kv.Key + "=" + kv.Value);
            }

            var checkSignature = HmacSHA512(vnp_HashSecret, inputRawData.ToString());
            if (checkSignature.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase))
            {
                return new PaymentResponseModel { Status = "00", Message = "Success" };
            }
            else
            {
                return new PaymentResponseModel { Status = "97", Message = "Invalid signature" };
            }
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            var xCharArray = x.ToCharArray();
            var yCharArray = y.ToCharArray();

            for (var i = 0; i < xCharArray.Length && i < yCharArray.Length; i++)
            {
                if (xCharArray[i] < yCharArray[i]) return -1;
                if (xCharArray[i] > yCharArray[i]) return 1;
            }
            return xCharArray.Length - yCharArray.Length;
        }
    }
}
