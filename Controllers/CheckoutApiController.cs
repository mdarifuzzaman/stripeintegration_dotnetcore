using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeIntegration.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutApiController : Controller
    {
        public CheckoutApiController()
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("success")]
        public IActionResult Success()
        {
            return View();
        }

        [HttpGet("cancel")]
        public IActionResult Cancel()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create()
        {
            var domain = "https://localhost:44343";

            var rdo = Request.Form["rdoPriceOption"];
            var priceOption = rdo.First();
            //var lookupKeys = new List<string> {
            //        Request.Form["lookup_key"]
            //    };

            //StripeList<Price> prices = priceService.List(priceOptions);
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>
                  {
                    new SessionLineItemOptions
                    {
                      Price = priceOption,
                      // For metered billing, do not pass quantity
                      Quantity = 1,
                    },
                  },
                Mode = "subscription",
                SuccessUrl = domain + "/create-checkout-session/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = domain + "/create-checkout-session/cancel",
            };
            var service = new SessionService();
            Session session = service.Create(options);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}
