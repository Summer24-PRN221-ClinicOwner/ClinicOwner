using ClinicServices.VNPayService;
using Microsoft.AspNetCore.Mvc;

namespace ClinicPresentationLayer.Controller
{
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        public PaymentController (IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            return RedirectToPage("/PaymentReturn");
        }
    }
}
