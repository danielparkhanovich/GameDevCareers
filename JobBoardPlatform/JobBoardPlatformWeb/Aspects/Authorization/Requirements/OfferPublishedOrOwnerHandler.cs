using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobBoardPlatform.PL.Requirements
{
    /// <summary>
    /// Pass all users only if offer is published or it's owner or admin
    /// </summary>
    public class OfferPublishedOrOwnerHandler : OfferHandlerBase<OfferPublishedOrOwnerOrAdminRequirement>, IAuthorizationFilter
    {
        public OfferPublishedOrOwnerHandler(IHttpContextAccessor contextAccessor, IRepository<JobOffer> offersRepository) 
            : base(contextAccessor, offersRepository)
        {

        }
    }
}
