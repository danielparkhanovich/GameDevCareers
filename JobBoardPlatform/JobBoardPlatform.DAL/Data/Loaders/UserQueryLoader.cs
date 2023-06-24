using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Contracts;
using JobBoardPlatform.DAL.Models.Employee;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data.Loaders
{
    public class UserQueryLoader<T> : IEntityLoader<T> where T : class, IUserIdentityEntity
    {
        public IQueryable<T> Load(IQueryable<T> queryable)
        {
            if (typeof(T) == typeof(EmployeeIdentity))
            {
                var employee = ((IQueryable<EmployeeIdentity>)queryable).Include(x => x.Profile);
                return (IQueryable<T>)employee;
            }
            else if (typeof(T) == typeof(CompanyIdentity))
            {
                var company = ((IQueryable<EmployeeIdentity>)queryable).Include(x => x.Profile);
                return (IQueryable<T>)company;
            }
            throw new NotImplementedException();
        }
    }
}
