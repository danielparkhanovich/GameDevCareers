using JobBoardPlatform.IntegrationTests.Common.Fixtures;

namespace JobBoardPlatform.IntegrationTests.Endpoints
{
    public class PublicEndpointsTest : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<Program> factory;


        public PublicEndpointsTest(CustomWebApplicationFactory<Program> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData("")]
        [InlineData("terms-of-service")]
        [InlineData("privacy-policy")]
        public async Task GetEndpointsReturnSuccessAndCorrectContentType(string url)
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        public void Dispose()
        {
            factory.Dispose();
        }
    }
}