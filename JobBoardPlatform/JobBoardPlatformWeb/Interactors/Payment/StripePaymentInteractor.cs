using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using Stripe.Checkout;

namespace JobBoardPlatform.PL.Interactors.Payment
{
    public class StripePaymentInteractor : IPaymentInteractor
    {
        private readonly IOfferQueryExecutor queryExecutor;
        private readonly IOfferManager offersManager;
        private readonly IHttpContextAccessor contextAccessor;


        public StripePaymentInteractor(
            IOfferQueryExecutor queryExecutor,
            IOfferManager offersManager,
            IHttpContextAccessor contextAccessor)
        {
            this.queryExecutor = queryExecutor;
            this.offersManager = offersManager;
            this.contextAccessor = contextAccessor;
        }

        public async Task ProcessCheckout(int offerId)
        {
            var options = GetSessionOptions();
            var session = CreateSessionService(options);

            var context = contextAccessor.HttpContext;
            context.Response.Headers.Add("Location", session.Url);
        }

        public async Task ConfirmCheckout(int offerId, string checkoutSessionId)
        {
            var service = new SessionService();
            var session = service.Get(checkoutSessionId);

            if (session.PaymentStatus == "paid")
            {
                await offersManager.PassPaymentAsync(offerId);
            }
        }

        private SessionCreateOptions GetSessionOptions()
        {
            return new SessionCreateOptions
            {
                SuccessUrl = GetSuccessUrl(),
                CancelUrl = GetCancelUrl(),
                LineItems = GetSessionLineItemOptions(),
                Mode = "payment"
            };
        }

        private string GetSuccessUrl()
        {
            var context = contextAccessor.HttpContext;
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}/{{CHECKOUT_SESSION_ID}}";
        }

        private string GetCancelUrl()
        {
            var context = contextAccessor.HttpContext;
            return $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}/rejected";
        }

        private List<SessionLineItemOptions> GetSessionLineItemOptions()
        {
            return new List<SessionLineItemOptions>() { GetSingleLineItemOption() };
        }

        private SessionLineItemOptions GetSingleLineItemOption()
        {
            return new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmount = 1500,
                    Currency = "pln",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = "Senior 3D Animator",

                    }
                },
                Quantity = 1
            };
        }

        private Session CreateSessionService(SessionCreateOptions options)
        {
            var service = new SessionService();
            return service.Create(options);
        }
    }
}
