using JobBoardPlatform.DAL.Data;
using JobBoardPlatform.DAL.Models;

namespace JobBoardPlatform.DAL.Repositories
{
    public class EmployeeRepository : CoreRepository<EmployeeCredentials, DataContext>
    {
        public EmployeeRepository(DataContext context) : base(context)
        {

        }

        // We can add new methods specific to the movie repository here in the future
    }
}
