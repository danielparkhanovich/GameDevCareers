using JobBoardPlatform.IntegrationTests.Common.Utils;

namespace JobBoardPlatform.IntegrationTests.Common.Assertions
{
    internal class CompanyProfileAssert
    {
        private readonly CompanyIntegrationTestsUtils testsUtils;
        private readonly ApplicationsIntegrationTestsUtils applicationsUtils;


        public CompanyProfileAssert(
            CompanyIntegrationTestsUtils testsUtils, 
            ApplicationsIntegrationTestsUtils applicationsUtils)
        {
            this.testsUtils = testsUtils;
            this.applicationsUtils = applicationsUtils;
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

        public async Task OfferIsClosedAndResumesAreDeleted(int offerId, string[] resumeUrls)
        {
            await OfferIsClosed(offerId);
            Assert.False(await applicationsUtils.IsResumesInStorage(resumeUrls));
        }

        public async Task OfferIsClosed(int offerId)
        {
            var offer = await testsUtils.GetOfferAsync(offerId);
            Assert.Null(offer);
            await ApplicationsAreDeleted(offerId);
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
            var user = await testsUtils.GetCompanyProfileByEmail(userEmail);
            var userProfile = await testsUtils.GetCompanyProfileProfileById(userProfileId);

            Assert.Null(user);
            Assert.Null(userProfile);
        }
    }
}
