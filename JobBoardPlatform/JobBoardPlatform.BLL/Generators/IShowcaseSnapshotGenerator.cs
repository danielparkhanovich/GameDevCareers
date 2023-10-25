
namespace JobBoardPlatform.BLL.Generators
{
    public interface IShowcaseSnapshotGenerator
    {
        Task CreateAdmins();
        Task CreateCompanies();
        Task CreateOffers(int offersCount);
        Task CreateEmployees();
        Task CreateApplications();
    }
}
