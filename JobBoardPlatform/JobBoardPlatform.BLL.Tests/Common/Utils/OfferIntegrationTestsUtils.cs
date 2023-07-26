using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoardPlatform.IntegrationTests.Common.Utils
{
    internal class OfferIntegrationTestsUtils
    {
        private readonly IRepository<JobOffer> offersRepository;
        private readonly IRepository<JobOfferContactDetails> contactTypeRepository;
        private readonly IRepository<JobOfferEmploymentDetails> employmentDetailsRepository;
        private readonly IRepository<JobOfferTechKeyword> techKewordsRepository;
        private readonly IRepository<JobOfferApplication> applicationsRepository;


        public OfferIntegrationTestsUtils(IServiceProvider serviceProvider)
        {
            offersRepository = serviceProvider.GetService<IRepository<JobOffer>>()!;
            contactTypeRepository = serviceProvider.GetService<IRepository<JobOfferContactDetails>>()!;
            employmentDetailsRepository = serviceProvider.GetService<IRepository<JobOfferEmploymentDetails>>()!;
            techKewordsRepository = serviceProvider.GetService<IRepository<JobOfferTechKeyword>>()!;
            applicationsRepository = serviceProvider.GetService<IRepository<JobOfferApplication>>()!;
        }

        public Task<JobOffer> GetOfferAsync(int offerId)
        {
            return offersRepository.Get(offerId);
        }

        public Task<JobOfferContactDetails> GetOfferContactDetailsAsync(JobOffer offer)
        {
            return contactTypeRepository.Get(offer.ContactDetailsId);
        }

        public Task<List<JobOfferEmploymentDetails>> GetOfferEmploymentDetailsAsync(JobOffer offer)
        {
            return GetEntitiesFromRepository(offer.EmploymentDetails, employmentDetailsRepository);
        }

        public Task<List<JobOfferTechKeyword>> GetOfferTechKeywordsAsync(JobOffer offer)
        {
            return GetEntitiesFromRepository(offer.TechKeywords, techKewordsRepository);
        }

        public Task<List<JobOfferApplication>> GetOfferApplicationsAsync(JobOffer offer)
        {
            return GetEntitiesFromRepository(offer.OfferApplications, applicationsRepository);
        }

        private async Task<List<T>> GetEntitiesFromRepository<T>(ICollection<T> entities, IRepository<T> repository) 
            where T : class, IEntity
        {
            var result = new List<T>();
            if (entities == null)
            {
                return result;
            }

            var entitiesIds = entities.Select(x => x.Id);
            foreach (var id in entitiesIds)
            {
                var entity = await repository.Get(id);
                if (entity != null)
                {
                    result.Add(entity);
                }
            }
            return result;
        }
    }
}
