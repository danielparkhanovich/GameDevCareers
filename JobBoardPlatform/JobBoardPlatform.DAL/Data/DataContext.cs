using JobBoardPlatform.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace JobBoardPlatform.DAL.Data
{
    public class DataContext : DbContext
    {
        public DbSet<EmployeeIdentity> EmployeeCredentials { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<CompanyIdentity> CompanyCredentials { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}
