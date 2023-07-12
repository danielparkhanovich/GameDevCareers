using JobBoardPlatform.BLL.Boundaries;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.PL.ViewModels.Models.Offer.Users
{
    public class OfferApplicationUpdateViewModel : IApplicationForm
    {
        public int OfferId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public IAttachedResume AttachedResume { get; set; }

        public string? AdditionalInformation { get; set; }
    }
}
