using JobBoardPlatform.BLL.Common.Formatter;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;
using JobBoardPlatform.PL.ViewModels.Utilities.Contracts;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationCardViewModelFactory : IFactory<CompanyApplicationCardViewModel>
    {
        private readonly OfferApplication application;
        private readonly DaysFormatter daysFormatter;



        public CompanyApplicationCardViewModelFactory(OfferApplication application)
        {
            this.application = application;
            this.daysFormatter = new DaysFormatter(true);
        }

        public Task<CompanyApplicationCardViewModel> Create()
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

            return Task.FromResult(viewModel);
        }
    }
}
