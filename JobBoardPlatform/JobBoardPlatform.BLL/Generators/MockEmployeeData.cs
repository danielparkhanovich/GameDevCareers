using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;

namespace JobBoardPlatform.BLL.Generators
{
    internal class MockEmployeeData : IUserIdentityEntity
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;

        public int ProfileId { get; set; }
        public EmployeeProfile Profile { get; set; }
    }
}
