using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class PaymentResponseModel
    {
		public decimal vnp_Amount { get; set; }
		public string vnp_BankCode { get; set; }
		public string vnp_BankTranNo { get; set; }
		public string vnp_CardType { get; set; }
		public string vnp_OrderInfo { get; set; }
		public DateTime vnp_PayDate { get; set; }
		public string vnp_ResponseCode { get; set; }
		public string vnp_TmnCode { get; set; }
		public string vnp_TransactionNo { get; set; }
		public string vnp_TransactionStatus { get; set; }
		public Guid vnp_TxnRef { get; set; }
		public string vnp_SecureHash { get; set; }
		public string Status { get; set; }
		public string Message { get; set; }
	}
}
