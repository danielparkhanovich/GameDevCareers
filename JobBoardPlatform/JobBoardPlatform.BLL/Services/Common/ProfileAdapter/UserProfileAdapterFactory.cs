using JobBoardPlatform.BLL.Services.Common.ProfileAdapter.Contracts;
using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;

namespace JobBoardPlatform.BLL.Services.Common.ProfileAdapter
{
    internal class UserProfileAdapterFactory
    {
        public static IUserProfileAdapter CreateProfileAdapter(IUserProfileEntity profile)
        {
            if (profile is EmployeeProfile)
            {
                return new EmployeeProfileAdapter(profile as EmployeeProfile);
            }
            else if (profile is CompanyProfile)
            {
                return new CompanyProfileAdapter(profile as CompanyProfile);
            }
            else
            {
                throw new ArgumentException("Unsupported profile type");
            }
        }
    }
}
