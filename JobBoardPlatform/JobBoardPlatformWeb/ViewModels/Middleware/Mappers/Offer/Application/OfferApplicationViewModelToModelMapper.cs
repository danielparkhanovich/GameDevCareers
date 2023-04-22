using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.PL.ViewModels.OfferViewModels.Users;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Mappers.Offer.Application
{
    internal class OfferApplicationViewModelToModelMapper : IMapper<OfferApplicationUpdateViewModel, OfferApplication>
    {
        public void Map(OfferApplicationUpdateViewModel from, OfferApplication to)
        {
            to.FullName = from.FullName;
            to.Email = from.Email;
            to.Description = from.AdditionalInformation;
        }
    }
}
