using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.DAL.Models.Company;
using System.Security.Claims;

namespace JobBoardPlatform.PL.Requirements
{
    public class OfferPublishedOrOwnerOrAdminRequirement : IOfferPassRequirement
    {
        public string IdParameterName { get; }


        public OfferPublishedOrOwnerOrAdminRequirement(string idParameterName)
        {
            this.IdParameterName = idParameterName;
        }

        public bool IsRequirmentSucceded(JobOffer offer, ClaimsPrincipal user)
        {
            return IsOfferVisible(offer) || IsAuthorized(offer, user);
        }

        private bool IsOfferVisible(JobOffer offer)
        {
            var offerState = new OfferState(offer);
            return offerState.IsVisibleOnMainPage();
        }

        private bool IsAuthorized(JobOffer offer, ClaimsPrincipal user)
        {
            return UserSessionUtils.IsLoggedIn(user) && IsHasPermit(offer, user);
        }

        private bool IsHasPermit(JobOffer offer, ClaimsPrincipal user)
        {
            return UserRolesUtils.IsUserOwner(user, offer) || UserRolesUtils.IsUserAdmin(user);
        }
    }
}
