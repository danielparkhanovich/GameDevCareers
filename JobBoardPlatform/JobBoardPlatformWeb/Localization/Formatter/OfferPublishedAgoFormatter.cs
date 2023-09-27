using JobBoardPlatform.DAL.Models.Company;

namespace JobBoardPlatform.BLL.Common.Formatter
{
    public class OfferPublishedAgoFormatter : ITextFormatter<JobOffer>
    {
        public string GetString(JobOffer offer)
        {
            var daysFormatter = new PublishedAgoFormatter();
            if (offer.IsPublished)
            {
                return daysFormatter.GetString(offer.RefreshedOnPageAt);
            }
            else
            {
                return daysFormatter.GetString(offer.CreatedAt);
            }
        }
    }
}
