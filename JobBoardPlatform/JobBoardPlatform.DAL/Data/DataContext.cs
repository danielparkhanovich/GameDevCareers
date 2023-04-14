using JobBoardPlatform.DAL.Data.Enums;
using JobBoardPlatform.DAL.Models.Company;
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
        public DbSet<ContactDetails> ContactDetails { get; set; }

        // Common
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<TechKeyword> TechKeyWords { get; set; }
        public DbSet<WorkLocationType> WorkLocationTypes { get; set; }
        public DbSet<MainTechnologyType> MainFieldTypes { get; set; }
        public DbSet<ContactType> ContactTypes { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var enumCreator = new EnumModelCreator(modelBuilder);

            enumCreator.SetDataForEntity<EmploymentType, EmploymentTypeEnum>();

            enumCreator.SetDataForEntity<CurrencyType, CurrencyTypeEnum>();

            enumCreator.SetDataForEntity<WorkLocationType, WorkLocationTypeEnum>();

            enumCreator.SetDataForEntity<MainTechnologyType, MainTechnologyTypeEnum>();

            enumCreator.SetDataForEntity<ContactType, ContactTypeEnum>();
        }
    }
}
