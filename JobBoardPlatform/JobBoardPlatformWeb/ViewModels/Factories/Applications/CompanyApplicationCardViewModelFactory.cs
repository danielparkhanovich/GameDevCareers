using JobBoardPlatform.BLL.Common.Formatter;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.PL.Aspects.DataValidators.Common;
using JobBoardPlatform.PL.Controllers.Utils;
using JobBoardPlatform.PL.ViewModels.Contracts;
using JobBoardPlatform.PL.ViewModels.Factories.Templates;
using JobBoardPlatform.PL.ViewModels.Models.Offer.Company;

namespace JobBoardPlatform.PL.ViewModels.Middleware.Factories.Applications
{
    public class CompanyApplicationCardViewModelFactory : IContainerCardFactory<JobOfferApplication>
    {
        private readonly PublishedAgoFormatter daysFormatter;


        public CompanyApplicationCardViewModelFactory()
        {
            this.daysFormatter = new PublishedAgoFormatter();
        }

        public IContainerCard CreateCard(JobOfferApplication application)
        {
            string applicatedAgo = daysFormatter.GetString(application.CreatedAt);
            string? linkedInUrl = application.EmployeeProfile?.LinkedInUrl;
            string? profileImageUrl = application.EmployeeProfile?.ProfileImageUrl;
            string? yearsOfExperience = application.EmployeeProfile?.YearsOfExperience;
            string? city = application.EmployeeProfile?.City;
            string? country = application.EmployeeProfile?.Country;

            var viewModel = new ApplicationCardViewModel()
            {
                Id = application.Id,
                OfferId = application.JobOfferId,
                PriorityFlagId = application.ApplicationFlagTypeId,
                FullName = application.FullName,
                Email = application.Email,
                ProfileImageUrl = GetProfileImageUri(profileImageUrl),
                ResumeUrl = application.ResumeUrl,
                YearsOfExperience = yearsOfExperience,
                Description = application.Description,
                ApplicatedAgo = applicatedAgo,
                LinkedInUrl = linkedInUrl,
                City = city,
                Country = country
            };

            ParseCoverLetterAndSetUrls(viewModel);

            return viewModel;
        }

        private string GetProfileImageUri(string? profileImageUrl)
        {
            return StaticFilesUtils.GetEmployeeDefaultAvatarUriIfEmpty(profileImageUrl);
        }

        private void ParseCoverLetterAndSetUrls(ApplicationCardViewModel application)
        {
            if (string.IsNullOrEmpty(application.Description))
            {
                return;
            }

            var urlValidator = new UrlValidator();

            var separators = new string[] { " ", "\r\n" };
            var tokens = application.Description.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens) 
            {
                TryParseUrlToken(urlValidator, application, token);
            }

            application.CoverLetterWordsCount = tokens.Length;
        }

        private void TryParseUrlToken(UrlValidator urlValidator, ApplicationCardViewModel application, string token)
        {
            var urlValidation = urlValidator.Validate(token);
            if (!urlValidation.IsValid)
            {
                return;
            }

            if (token.Contains("github"))
            {
                application.GitHubUrl = token;
            }
            else if (token.Contains("linkedin"))
            {
                application.LinkedInUrl = token;
            }
        }
    }
}
