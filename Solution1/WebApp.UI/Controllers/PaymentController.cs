
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApp.UI.Helpers;
using WebApp.UI.Models;
using WebApp.UI.Models.Checkout;

namespace WebApp.UI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly RazorpayClient _razorpayClient;
        private string RazorPayKey { get; }
        private readonly ISessionManager _sessionManager;

        public PaymentController(IConfiguration config, ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            RazorPayKey = config["RazorPay:Key"].ToString();
            _razorpayClient = new RazorpayClient(RazorPayKey, "tWL6ajNZ58SXI7Q9O1ayoy5A");
        }

        public class ConfirmPaymentPayload
        {
            public string RazorpayPaymentId { get; set; }
            public string RazorpayOrderId { get; set; }
            public string RazorpaySignature { get; set; }
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
        public async Task<IActionResult> ConfirmPayment(ConfirmPaymentPayload confirmPayment)
        {
            var attributes = new Dictionary<string, string>
        {
            { "razorpay_payment_id", confirmPayment.RazorpayPaymentId },
            { "razorpay_order_id", confirmPayment.RazorpayOrderId },
            { "razorpay_signature", confirmPayment.RazorpaySignature }
        };
            try
            {
                Utils.verifyPaymentSignature(attributes);
                // OR
                bool isValid = Utils.ValidatePaymentSignature(attributes);
                if (isValid)
                {
                    var order = _razorpayClient.Order.Fetch(confirmPayment.RazorpayOrderId);
                    dynamic orderJson = order.Attributes.ToString();
                    var data = JsonConvert.DeserializeObject<RazorOrder>(orderJson);
                    var payment = _razorpayClient.Payment.Fetch(confirmPayment.RazorpayPaymentId);
                    if (payment["status"] == "captured")
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            using var client = new HttpClient();
                            var createOrderUri = new Uri(ApiUrls.Order.Create);
                            var userAccessToken = User.Claims.FirstOrDefault(x => x.Type == "AcessToken")?.Value;
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userAccessToken);

                            var orderRequest = new OrderViewModel();
                            orderRequest.UserId = User.Identity.Name;
                            orderRequest.RazorpayOrderId = confirmPayment.RazorpayOrderId;
                            orderRequest.RazorpayPaymentId = confirmPayment.RazorpayPaymentId;
                            orderRequest.RazorpaySignature = confirmPayment.RazorpaySignature;

                            var request = JsonConvert.SerializeObject( orderRequest );
                            var content = new StringContent(request, Encoding.UTF8, "application/json");
                            var postOrderResult = await client.PostAsync(createOrderUri, content);
                            string orderResult = postOrderResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        }
                        else
                        {
                            var list = _sessionManager.GetCart(x => new CartViewModel
                            {
                                Name = x.Name,
                                Value = x.Value,
                                RealValue = x.Value,
                                ProductId = x.Id,
                                Qty = x.Qty
                            });

                            var customerinformation = _sessionManager.GetCustomerInformation();
                        }

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
