using Microservice.One.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Microservice.One.Data
{
    public class BenefitContext : DbContext
    {
        public BenefitContext(DbContextOptions<BenefitContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region SeedData
            modelBuilder.Entity<Company>().HasData(
                    new Company { Id = 1, Name = "JHL Enterprises" },
                    new Company { Id = 2, Name = "HCSS" },
                    new Company { Id = 3, Name = "Hello Fresh" },
                    new Company { Id = 4, Name = "Walmart" },
                    new Company { Id = 5, Name = "Transafe" }
            );

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    FirstName = "Steve",
                    LastName = "Smith",
                    Ssn = "373637363",
                    Status = 0,
                    CompanyId = 5
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "John",
                    LastName = "Kesinger",
                    Ssn = "123333232",
                    Status = 0,
                    CompanyId = 1
                },
                new Employee
                {
                    Id = 3,
                    FirstName = "James",
                    LastName = "Doe",
                    Ssn = "878676767",
                    Status = 0,
                    CompanyId = 1
                },
                new Employee
                {
                    Id = 4,
                    FirstName = "Amanda",
                    LastName = "Know",
                    Ssn = "989887878",
                    Status = 1,
                    CompanyId = 1
                }
            );

            modelBuilder.Entity<Hsa>().HasData(
                new Hsa
                {
                    EmployeeId = 4,
                    ContributionAmount = 20,
                    Id = 1
                },
                new Hsa
                {
                    EmployeeId = 2,
                    ContributionAmount = 40,
                    Id = 2
                }
            );
            #endregion

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Hsa> Hsa { get; set; }
    }
}
