using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Services.Offer.State
{
    // TODO: rename
    public class OfferStateFactory
    {
        public OfferStateType GetOfferState(JobOffer offer)
        {
            if (offer.IsDeleted)
            {
                return OfferStateType.Deleted;
            }
            else if (offer.IsPaid == false)
            {
                return OfferStateType.NotPaid;
            }
            else if (offer.IsSuspended)
            {
                return OfferStateType.Suspended;
            }
            else if (offer.IsShelved)
            {
                return OfferStateType.Shelved;
            }
            else if (offer.IsPublished)
            {
                return OfferStateType.Visible;
            }
            throw new Exception($"Wrong offer state, for offer id: {offer.Id}");
        }

        public bool IsOfferAvailable(JobOffer offer)
        {
            var state = GetOfferState(offer);

            if (state != OfferStateType.Deleted && state != OfferStateType.Suspended)
            {
                return true;
            }

            return false;
        }

        public bool IsOfferVisible(JobOffer offer)
        {
            return GetOfferState(offer) == OfferStateType.Visible;
        }
    }
}
