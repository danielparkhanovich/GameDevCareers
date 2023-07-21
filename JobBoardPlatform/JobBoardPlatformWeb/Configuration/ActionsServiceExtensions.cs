using JobBoardPlatform.BLL.Services.Actions.Offers.Factory;

namespace JobBoardPlatform.PL.Configuration
{
    public static class ActionsServiceExtensions
    {
        public static void AddActionsServices(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (!environment.IsDevelopment())
            {
                services.AddTransient<IOfferActionHandlerFactory, OfferActionHandlerFactory>();
            }
            else
            {
                services.AddTransient<IOfferActionHandlerFactory, OfferActionEmptyHandlerFactory>();
            }
        }
    }
}
