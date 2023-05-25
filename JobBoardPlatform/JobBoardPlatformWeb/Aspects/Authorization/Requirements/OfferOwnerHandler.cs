using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JobBoardPlatform.PL.Requirements
{
    public class OfferOwnerHandler : OfferHandlerBase<OfferOwnerOrAdminRequirement>, IAuthorizationFilter
    {
        public OfferOwnerHandler(IHttpContextAccessor contextAccessor, IRepository<JobOffer> offersRepository) 
            : base(contextAccessor, offersRepository)
        {

        }
    }
}
