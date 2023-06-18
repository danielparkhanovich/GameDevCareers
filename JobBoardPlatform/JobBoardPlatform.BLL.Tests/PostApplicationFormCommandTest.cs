using JobBoardPlatform.DAL.Data;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.BLL.IntegrationTests
{
    public class PostApplicationFormCommandTest
    {
        [Fact]
        public void Test1()
        {
            // TODO: write it when 90% functionality will be ready
            // Arrange
            /* var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DataContext(options))
            {
                IRepository<OfferApplication> applications = new CoreRepository<OfferApplication>(context);
                IRepository<JobOffer> offersRepository = new CoreRepository<JobOffer>(context);

                var user = new User { Name = "John", Email = "john@example.com" };

                // Act
                userRepository.Add(user);
                context.SaveChanges();

                // Assert
                var savedUser = context.Users.FirstOrDefault(u => u.Name == "John" && u.Email == "john@example.com");
                Assert.NotNull(savedUser);
                Assert.Equal(user.Name, savedUser.Name);
                Assert.Equal(user.Email, savedUser.Email);
            } */
        }
    }
}