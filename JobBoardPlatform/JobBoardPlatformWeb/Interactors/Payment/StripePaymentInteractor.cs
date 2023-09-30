using JobBoardPlatform.BLL.Commands.Offer;
using JobBoardPlatform.BLL.Query.Identity;
using JobBoardPlatform.PL.ViewModels.Factories.Offer.Payment;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Payment;
using Stripe.Checkout;

namespace JobBoardPlatform.PL.Interactors.Payment
{
    public class StripePaymentInteractor : IPaymentInteractor
    {
        private readonly IOfferQueryExecutor queryExecutor;
        private readonly IOfferManager offersManager;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IOfferPlanQueryExecutor plansQuery;


        public StripePaymentInteractor(
            IOfferQueryExecutor queryExecutor,
            IOfferManager offersManager,
            IHttpContextAccessor contextAccessor,
            IOfferPlanQueryExecutor plansQuery)
        {
            this.queryExecutor = queryExecutor;
            this.offersManager = offersManager;
            this.contextAccessor = contextAccessor;
            this.plansQuery = plansQuery;
        }

        public async Task ProcessCheckout(int offerId)
        {
            var options = await GetSessionOptions(offerId);
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

        private async Task<SessionCreateOptions> GetSessionOptions(int offerId)
        {
            return new SessionCreateOptions
            {
                SuccessUrl = GetSuccessUrl(),
                CancelUrl = GetCancelUrl(),
                LineItems = await GetSessionLineItemOptions(offerId),
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

        private async Task<List<SessionLineItemOptions>> GetSessionLineItemOptions(int offerId)
        {
            return new List<SessionLineItemOptions>() { await GetSingleLineItemOption(offerId) };
        }

        private async Task<SessionLineItemOptions> GetSingleLineItemOption(int offerId)
        {
            var offer = await queryExecutor.GetOfferById(offerId);
            var plan = (await GetSelectedPlan(offerId)).Plans.Single();
            var price = Convert.ToDouble(plan.Price);

            return new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmount = (long)price * 100,
                    Currency = "pln",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = $"Plan {plan.OfferType} for {offer.JobTitle}",
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

        private async Task<OfferPricingTableViewModel> GetSelectedPlan(int offerId)
        {
            var offer = await queryExecutor.GetOfferById(offerId);
            var factory = new OfferPricingTableViewModelFactory(plansQuery, offer.PlanId);
            return await factory.CreateAsync();
        }
    }
}
