using JobBoardPlatform.BLL.Query.Identity;
using Stripe.Checkout;

namespace JobBoardPlatform.PL.Interactors.Payment
{
    public class StripePaymentInteractor : IPaymentInteractor
    {
        private readonly OfferQueryExecutor queryExecutor;
        private readonly IHttpContextAccessor contextAccessor;


        public StripePaymentInteractor(
            OfferQueryExecutor queryExecutor,
            IHttpContextAccessor contextAccessor)
        {
            this.queryExecutor = queryExecutor;
            this.contextAccessor = contextAccessor;
        }

        public async Task ProcessCheckout(int offerId)
        {
            var offer = queryExecutor.GetOfferById(offerId);

            var options = GetSessionOptions();
            var session = CreateSessionService(options);

            var context = contextAccessor.HttpContext;
            context.Response.Headers.Add("Location", session.Url);
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
            return $"{context.Request.Scheme}://{context.Request.Host}";
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
