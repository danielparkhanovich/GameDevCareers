using JobBoardPlatform.BLL.Services.Authorization.Utilities;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Company;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JobBoardPlatform.PL.ViewModels.JobOfferViewModels.Users;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.PL.Controllers.JobOffers
{
    public class JobOfferController : Controller
    {
        private readonly IRepository<JobOffer> offersRepository;


        public JobOfferController(IRepository<JobOffer> offersRepository)
        {
            this.offersRepository = offersRepository;
        }

        [Route("{companyname}-{offertitle}-{id}")]
        public async Task<IActionResult> Offer(int id, string companyname, string offertitle)
        {
            var content = await UpdateDisplayView(id);

            return View(content);
        }

        private async Task<JobOfferDisplayViewModel> UpdateDisplayView(int id)
        {
            var offersSet = await offersRepository.GetAllSet();
            // TODO: refactor .Where -> .FindAsync
            var offer = offersSet.Where(x => x.Id == id)
                .Include(x => x.CompanyProfile)
                .Include(x => x.WorkLocation)
                .Include(x => x.MainTechnologyType)
                .Include(x => x.TechKeywords)
                .Include(x => x.JobOfferEmploymentDetails)
                    .ThenInclude(y => y.SalaryRange != null ? y.SalaryRange.SalaryCurrency : null)
                .Include(x => x.JobOfferEmploymentDetails)
                    .ThenInclude(y => y.EmploymentType)
                .Include(x => x.ContactDetails)
                    .ThenInclude(y => y.ContactType)
                .Single();

            var salaryDisplayText = new List<string>(offer.JobOfferEmploymentDetails.Count);
            var employmentTypeDisplayText = new List<string>(offer.JobOfferEmploymentDetails.Count);

            foreach (var employmentDetails in offer.JobOfferEmploymentDetails)
            {
                string singleSalaryText = "Undisclosed Salary";

                var salaryRange = employmentDetails.SalaryRange;

                if (salaryRange != null)
                {
                    singleSalaryText = $"{salaryRange.From} - {salaryRange.To} {salaryRange.SalaryCurrency.Type}";
                }

                string employmentType = employmentDetails.EmploymentType.Type;

                salaryDisplayText.Add(singleSalaryText);
                employmentTypeDisplayText.Add(employmentType);
            }

            string fullAddress = $"{offer.City}, {offer.Country}";
            if (!string.IsNullOrEmpty(offer.Address))
            {
                fullAddress = $"{offer.Address}, {fullAddress}";
            }

            var techKeyWords = offer.TechKeywords.Select(x => x.Name).ToArray();

            var display = new JobOfferDisplayViewModel()
            {
                JobTitle = offer.JobTitle,
                JobDescription = offer.Description,
                CompanyName = offer.CompanyProfile.CompanyName,
                CompanyImageUrl = offer.CompanyProfile.ProfileImageUrl,
                FullAddress = fullAddress,
                EmploymentDetails = employmentTypeDisplayText.ToArray(),
                SalaryDetails = salaryDisplayText.ToArray(),
                MainTechnologyType = offer.MainTechnologyType.Type,
                TechKeywords = techKeyWords,
                WorkLocationType = offer.WorkLocation.Type,
                ContactForm = offer.ContactDetails.ContactType.Type
            };

            return display;
        }
    }
}
