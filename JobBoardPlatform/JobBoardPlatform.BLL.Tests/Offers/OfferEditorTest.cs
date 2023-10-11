using JobBoardPlatform.IntegrationTests.Common.Fixtures;
using JobBoardPlatform.IntegrationTests.Common.Utils;

namespace JobBoardPlatform.IntegrationTests.Offers
{
    public class OfferEditorTest : IClassFixture<AppFixture>, IDisposable
    {
        private readonly AppFixture fixture;
        private readonly CompanyIntegrationTestsUtils testsUtils;


        public OfferEditorTest(AppFixture fixture)
        {
            this.fixture = fixture;
            this.testsUtils = new CompanyIntegrationTestsUtils(fixture.ServiceProvider);
        }

        [Fact]
        public async Task AddNewOfferTest()
        {
            string userEmail = testsUtils.GetExampleCompanyEmail();
            await testsUtils.AddExampleCompanyAsync(userEmail);

            string offerName = testsUtils.GetExampleOfferTitle();
            await testsUtils.AddNewOfferAsync(userEmail, offerName);
        }

        [Fact]
        public async Task EditOfferTest()
        {
            string userEmail = testsUtils.GetExampleCompanyEmail();
            await testsUtils.AddExampleCompanyAsync(userEmail);

        }

        public void Dispose()
        {
            fixture.Dispose();
        }
    }
}