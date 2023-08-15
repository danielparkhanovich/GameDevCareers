using JobBoardPlatform.BLL.Services.Offer.State;
using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Services.Background
{
    public class OfferExpirationService : IExpirationService
    {
        private const int NotPaidOfferExpirationTimeInDays = 2;
        private const int PublishedOfferExpirationTimeInDays = 30;

        private readonly JobOffer offer;


        public OfferExpirationService(JobOffer offer)
        {
            this.offer = offer;
        }

        public bool IsExpired()
        {
            return GetDaysLeft() <= 0;
        }

        public int GetDaysLeft()
        {
            if (IsOfferNotPaid())
            {
                return GetNotPaidOfferDaysLeft();
            }
            else
            {
                return GetPublishedOfferDaysLeft();
            }
        }

        private bool IsOfferNotPaid()
        {
            var offerState = new OfferState(offer);
            return offerState.GetState() == OfferStateType.NotPaid;
        }

        private int GetNotPaidOfferDaysLeft()
        {
            return GetSubstractedExpirationDateInDays(offer.CreatedAt, NotPaidOfferExpirationTimeInDays);
        }

        private int GetPublishedOfferDaysLeft()
        {
            return GetSubstractedExpirationDateInDays(offer.PublishedAt, PublishedOfferExpirationTimeInDays);
        }

        private int GetSubstractedExpirationDateInDays(DateTime createdAt, int expirationTimeInDays)
        {
            DateTime expirationAt = createdAt.AddDays(expirationTimeInDays);
            return (expirationAt - DateTime.Now).Days;
        }
    }
}
