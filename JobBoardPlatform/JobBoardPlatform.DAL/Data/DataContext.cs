using JobBoardPlatform.DAL.Models.Company;
using JobBoardPlatform.DAL.Models.Company.Company;
using JobBoardPlatform.DAL.Models.Employee;
using JobBoardPlatform.DAL.Models.Enums;
using JobBoardPlatform.DAL.Models.EnumTables;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.DAL.Data
{
    public class DataContext : DbContext
    {
        // Employee
        public DbSet<EmployeeIdentity> EmployeeCredentials { get; set; }
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }

        // Company
        public DbSet<CompanyIdentity> CompanyCredentials { get; set; }
        public DbSet<CompanyProfile> CompanyProfiles { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<JobOfferEmploymentDetails> JobOfferEmploymentDetails { get; set; }
        public DbSet<JobOfferSalariesRange> JobOfferSalariesRange { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }

        // Common
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<TechKeyWord> TechKeyWords { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmploymentType>().HasData(
                new EmploymentType { Id = 1, Type = EmploymentTypeEnum.Permanent.ToString() },
                new EmploymentType { Id = 2, Type = EmploymentTypeEnum.B2B.ToString() },
                new EmploymentType { Id = 3, Type = EmploymentTypeEnum.MandatoryContract.ToString() }
            );

            modelBuilder.Entity<CurrencyType>().HasData(
                new CurrencyType { Id = 1, Type = CurrencyTypeEnum.PLN.ToString() },
                new CurrencyType { Id = 2, Type = CurrencyTypeEnum.EUR.ToString() }
            );
        }
    }
}
