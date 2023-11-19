
namespace JobBoardPlatform.BLL.Generators
{
    public interface IShowcaseSnapshotGenerator
    {
        Task CreateAdmins();
        Task CreateCompanies();
        Task CreateOffersForEachCompany(int offersCount);
        Task CreateEmployees();
        Task CreateApplications();
    }
}
