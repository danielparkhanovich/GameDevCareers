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

        public OfferStateType GetState()
        {
            if (offer.IsDeleted)
            {
                return OfferStateType.Deleted;
            }
            else if (offer.IsSuspended)
            {
                return OfferStateType.Suspended;
            }
            else if (!offer.IsPaid)
            {
                return OfferStateType.NotPaid;
            }
            else if (offer.IsShelved)
            {
                return OfferStateType.Shelved;
            }
            else if (offer.IsPublished)
            {
                return OfferStateType.Visible;
            }

            return OfferStateType.Suspended;
            //throw new Exception($"Wrong offer state, for offer id: {offer.Id}");
        }

        public bool IsAvailableForEdit()
        {
            var state = GetState();

            if (state != OfferStateType.Deleted)
            {
                return true;
            }

            return false;
        }

        public bool IsVisibleOnMainPage()
        {
            return GetState() == OfferStateType.Visible;
        }
    }
}
