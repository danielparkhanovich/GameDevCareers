using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;

namespace JobBoardPlatform.BLL.Generators
{
    internal class MockCompanyData : IUserIdentityEntity
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;

        public int ProfileId { get; set; }
        public CompanyProfile Profile { get; set; }
    }
}
