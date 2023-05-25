using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobBoardPlatform.PL.Requirements
{
    public class OfferHandlerBase<T> : AuthorizationHandler<T>, IAuthorizationFilter
        where T: IOfferPassRequirement
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IRepository<JobOffer> offersRepository;


        public OfferHandlerBase(IHttpContextAccessor contextAccessor, IRepository<JobOffer> offersRepository)
        {
            this.contextAccessor = contextAccessor;
            this.offersRepository = offersRepository;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authorizationResult = context.HttpContext.Items[typeof(AuthorizationResult)] as AuthorizationResult;
            if (authorizationResult?.Succeeded == false)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
            else
            {
                // Authorization succeeded
            }
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, T requirement)
        {
            if (await IsAuthorizationSucceded(context, requirement))
            {
                context.Succeed(requirement);
                
            }
            else
            {
                context.Fail();
            }
        }

        private async Task<bool> IsAuthorizationSucceded(AuthorizationHandlerContext context, T requirement)
        {
            try
            {
                return await TryPassAuthorization(context, requirement);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> TryPassAuthorization(AuthorizationHandlerContext context, T requirement)
        {
            string routeId = GetIdFromRoute(requirement);
            int offerId = GetIdParsedToInt(routeId);
            return await IsRequirmentSucceded(offerId, context, requirement);
        }

        private string GetIdFromRoute(T requirement)
        {
            HttpContext? httpContext = contextAccessor.HttpContext;
            string? routeId = httpContext?.Request.RouteValues[requirement.IdParameterName]?.ToString();
            if (string.IsNullOrEmpty(routeId))
            {
                throw new Exception("Missing route id parameter");
            }
            return routeId!;
        }

        private int GetIdParsedToInt(string routeId)
        {
            int id = int.Parse(routeId);
            return id;
        }
        
        private async Task<bool> IsRequirmentSucceded(int offerId, AuthorizationHandlerContext context, 
            T requirement)
        {
            var offer = await TryGetOffer(offerId);
            return requirement.IsRequirmentSucceded(offer, context.User);
        }

        private async Task<JobOffer> TryGetOffer(int offerId)
        {
            var offer = await offersRepository.Get(offerId);
            if (offer == null)
            {
                throw new Exception($"Offer with id: {offerId} not found");
            }
            return offer!;
        }
    }
}
