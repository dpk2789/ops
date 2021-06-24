
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.UI.Controllers
{
    public class PaymentController : Controller
    {
        private RazorpayClient _razorpayClient;
        public PaymentController()
        {
            _razorpayClient = new RazorpayClient("rzp_test_PhqnP6sane3Ovm", "tWL6ajNZ58SXI7Q9O1ayoy5A");
        }

        public IActionResult Index()
        {
            return View();
        }
        public class ConfirmPaymentPayload
        {
            public string razorpay_payment_id { get; }
            public string razorpay_order_id { get; }
            public string razorpay_signature { get; }
        }


        [HttpPost]
        public async Task<IActionResult> InitializePayment()
        {
            var options = new Dictionary<string, object>
        {
            { "amount", 200 },
            { "currency", "INR" },
            { "receipt", "recipt_1" },
            // auto capture payments rather than manual capture
            // razor pay recommended option
            { "payment_capture", true }
        };

            var order = _razorpayClient.Order.Create(options);
            var orderId = order["id"].ToString();
            var orderJson = order.Attributes.ToString();
            return Ok(orderJson);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment([FromForm]ConfirmPaymentPayload confirmPayment)
        {
            var attributes = new Dictionary<string, string>
        {
            { "razorpay_payment_id", confirmPayment.razorpay_payment_id },
            { "razorpay_order_id", confirmPayment.razorpay_order_id },
            { "razorpay_signature", confirmPayment.razorpay_signature }
        };
            try
            {
                Utils.verifyPaymentSignature(attributes);
                // OR
                var isValid = Utils.ValidatePaymentSignature(attributes);
                if (isValid)
                {
                    var order = _razorpayClient.Order.Fetch(confirmPayment.razorpay_order_id);
                    var payment = _razorpayClient.Payment.Fetch(confirmPayment.razorpay_payment_id);
                    if (payment["status"] == "captured")
                    {
                        return Ok("Payment Successful");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
