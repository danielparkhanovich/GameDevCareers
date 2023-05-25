using JobBoardPlatform.BLL.Common.Formatter;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationCardViewModelFactory : IViewModelFactory<OfferApplication, IContainerCard>
    {
        private readonly PublishedAgoFormatter daysFormatter;


        public CompanyApplicationCardViewModelFactory()
        {
            this.daysFormatter = new PublishedAgoFormatter(true);
        }

        public IContainerCard CreateViewModel(OfferApplication application)
        {
            string applicatedAgo = daysFormatter.GetString(application.CreatedAt);
            string? linkedInUrl = application.EmployeeProfile?.LinkedInUrl;
            string? profileImageUrl = application.EmployeeProfile?.ProfileImageUrl;
            string? yearsOfExperience = application.EmployeeProfile?.YearsOfExperience;
            string? city = application.EmployeeProfile?.City;
            string? country = application.EmployeeProfile?.Country;

            var viewModel = new CompanyApplicationCardViewModel()
            {
                Id = application.Id,
                PriorityFlagId = application.ApplicationFlagTypeId,
                FullName = application.FullName,
                Email = application.Email,
                ProfileImageUrl = profileImageUrl,
                ResumeUrl = application.ResumeUrl,
                YearsOfExperience = yearsOfExperience,
                Description = application.Description,
                ApplicatedAgo = applicatedAgo,
                LinkedInUrl = linkedInUrl,
                City = city,
                Country = country
            };

            return viewModel;
        }
    }
}
