using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Services.Offer.State
{
    public class OfferState
    {
        private readonly JobOffer offer;


        public OfferState(JobOffer offer)
        {
            this.offer = offer;
        }

        public OfferStateType GetOfferState()
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

        public bool IsOfferAvailableForEdit()
        {
            var state = GetOfferState();

            if (state != OfferStateType.Deleted && state != OfferStateType.Suspended)
            {
                return true;
            }

            return false;
        }

        public bool IsOfferVisible()
        {
            return GetOfferState() == OfferStateType.Visible;
        }
    }
}
