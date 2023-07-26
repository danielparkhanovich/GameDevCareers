using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.IntegrationTests.Common.Utils;

namespace JobBoardPlatform.IntegrationTests.Common.Assertions
{
    internal class CompanyProfileAssert
    {
        private readonly CompanyIntegrationTestsUtils testsUtils;
        private readonly ApplicationsIntegrationTestsUtils applicationsUtils;
        private readonly OfferIntegrationTestsUtils offerUtils;


        public CompanyProfileAssert(
            CompanyIntegrationTestsUtils testsUtils, 
            ApplicationsIntegrationTestsUtils applicationsUtils,
            OfferIntegrationTestsUtils offerUtils)
        {
            this.testsUtils = testsUtils;
            this.applicationsUtils = applicationsUtils;
            this.offerUtils = offerUtils;
        }

        public async Task ProfileImageExists(string imageUrl)
        {
            Assert.True(await testsUtils.IsProfileImageInStorage(imageUrl));
        }

        public async Task ProfileImageNotExists(string imageUrl)
        {
            Assert.False(await testsUtils.IsProfileImageInStorage(imageUrl));
        }

        public async Task OfferIsCreated(string companyEmail, string offerTitle)
        {
            var offer = await testsUtils.GetOfferAsync(companyEmail, offerTitle);
            Assert.NotNull(offer);
        }

        public async Task OfferIsPublished(string companyEmail, string offerTitle)
        {
            var offer = await testsUtils.GetOfferAsync(companyEmail, offerTitle);
            Assert.NotNull(offer);
            Assert.True(offer.IsPaid);
            Assert.True(offer.IsPublished);
        }

        public async Task OfferIsClosedAndResumesAreDeleted(JobOffer offer, string[] resumeUrls)
        {
            await OfferIsClosed(offer);
            Assert.False(await applicationsUtils.IsResumesInStorage(resumeUrls));
        }

        public async Task OffersAreClosed(ICollection<JobOffer> offers)
        {
            foreach (var offer in offers)
            {
                await OfferIsClosed(offer);
            }
        }

        public async Task OfferIsClosed(JobOffer offer)
        {
            var savedOffer = await testsUtils.GetOfferAsync(offer.Id);
            Assert.Null(savedOffer);
            await OfferContentIsDeleted(offer);
            await ApplicationsAreDeleted(offer.Id);
        }

        public async Task OfferContentIsDeleted(JobOffer offer)
        {
            var savedContactType = await offerUtils.GetOfferContactDetailsAsync(offer);
            Assert.Null(savedContactType);
            var savedOfferDetails = await offerUtils.GetOfferEmploymentDetailsAsync(offer);
            Assert.Empty(savedOfferDetails);
            var savedTechKeywords = await offerUtils.GetOfferTechKeywordsAsync(offer);
            Assert.Empty(savedTechKeywords);
            var savedApplications = await offerUtils.GetOfferApplicationsAsync(offer);
            Assert.Empty(savedApplications);
        }

        public async Task ApplicationsAreDeleted(int offerId)
        {
            var applications = await testsUtils.GetOfferApplicationsAsync(offerId);
            Assert.Empty(applications);
        }

        public async Task ApplicationsAreAdded(int offerId, int expectedCount, string[] resumeUrls)
        {
            var offer = await testsUtils.GetOfferAsync(offerId);
            Assert.Equal(expectedCount, offer.NumberOfApplications);

            var applications = await testsUtils.GetOfferApplicationsAsync(offer!.Id);
            Assert.Equal(expectedCount, applications.Length);
            Assert.Equal(expectedCount, resumeUrls.Length);

            Assert.True(await applicationsUtils.IsResumesInStorage(resumeUrls));
        }

        public async Task UserNotExists(string userEmail, int userProfileId)
        {
            var user = await testsUtils.GetCompanyByEmail(userEmail);
            var userProfile = await testsUtils.GetCompanyProfileById(userProfileId);

            Assert.Null(user);
            Assert.Null(userProfile);
        }
    }
}
