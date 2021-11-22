using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StripeIntegration.Controllers
{
    [Route("webhook")]
    [ApiController]
    public class WebhookController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            // Replace this endpoint secret with your endpoint's unique secret
            // If you are testing with the CLI, find the secret by running 'stripe listen'
            // If you are using an endpoint defined with the API or dashboard, look in your webhook settings
            // at https://dashboard.stripe.com/webhooks
            const string endpointSecret = "whsec_12345";
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                var signatureHeader = Request.Headers["Stripe-Signature"];
                stripeEvent = EventUtility.ConstructEvent(json,
                        signatureHeader, endpointSecret);
                if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    Console.WriteLine("A subscription was canceled.", subscription.Id);
                    // Then define and call a method to handle the successful payment intent.
                    // handleSubscriptionCanceled(subscription);
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    Console.WriteLine("A subscription was updated.", subscription.Id);
                    // Then define and call a method to handle the successful payment intent.
                    // handleSubscriptionUpdated(subscription);
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    Console.WriteLine("A subscription was created.", subscription.Id);
                    // Then define and call a method to handle the successful payment intent.
                    // handleSubscriptionUpdated(subscription);
                }
                else if (stripeEvent.Type == Events.CustomerSubscriptionTrialWillEnd)
                {
                    var subscription = stripeEvent.Data.Object as Subscription;
                    Console.WriteLine("A subscription trial will end", subscription.Id);
                    // Then define and call a method to handle the successful payment intent.
                    // handleSubscriptionUpdated(subscription);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return BadRequest();
            }
        }
    }
}
