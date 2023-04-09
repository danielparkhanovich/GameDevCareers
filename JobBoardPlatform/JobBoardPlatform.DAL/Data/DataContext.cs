using JobBoardPlatform.DAL.Models;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.DAL.Models.EnumTables;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data
{
    public class DataContext : DbContext
    {
        public DbSet<EmployeeIdentity> EmployeeCredentials { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        public DbSet<CompanyIdentity> CompanyCredentials { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        //public DbSet<JobOffer> JobOffers { get; set; }
        //public DbSet<EmploymentType> EmploymentTypes { get; set; }
        //public DbSet<CurrencyType> CurrencyTypes { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<EmploymentType>().HasData(
                new EmploymentType { Id = 1, Type = EmploymentTypeEnum.ContractOfEmployment },
                new EmploymentType { Id = 2, Type = EmploymentTypeEnum.B2B },
                new EmploymentType { Id = 3, Type = EmploymentTypeEnum.MandatoryContract }
            );

            modelBuilder.Entity<CurrencyType>().HasData(
                new CurrencyType { Id = 1, Type = CurrencyTypeEnum.PLN },
                new CurrencyType { Id = 2, Type = CurrencyTypeEnum.EUR }
            );*/
        }
    }
}
