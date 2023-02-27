using JobBoardPlatform.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace JobBoardPlatform.DAL.Data
{
    public class DataContext : DbContext
    {
        public DbSet<EmployeeCredentials> Employees { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<CompanyCredentials> Companies { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
