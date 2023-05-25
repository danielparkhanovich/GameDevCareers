using JobBoardPlatform.DAL.Models.Company;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Requirements
{
    public interface IOfferPassRequirement : IAuthorizationRequirement
    {
        public string IdParameterName { get; }
        public bool IsRequirmentSucceded(JobOffer offer, ClaimsPrincipal user);
    }
}
