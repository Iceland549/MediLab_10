using Microsoft.EntityFrameworkCore;
using PatientMicroservice.Domain.Models;

namespace PatientMicroservice.Infrastructure.Data
{
    public class PatientDbContext : DbContext 
    {
        public DbSet<Patient> Patients { get; set; }
        public PatientDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().HasKey(p => p.Id);

            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    Id = 1,
                    FirstName = "TestNone",
                    LastName = "Test",
                    DateOfBirth = new DateTime(1966, 12, 31),
                    Gender = "F",
                    Adress = "1 Brookside St",
                    PhoneNumber = "100-222-3333"
                },
                new Patient
                {
                    Id = 2,
                    FirstName = "TestBorderline",
                    LastName = "Test",
                    DateOfBirth = new DateTime(1945, 6, 24),
                    Gender = "M",
                    Adress = "2 High St",
                    PhoneNumber = "200-333-4444"
                },
                new Patient
                {
                    Id = 3,
                    FirstName = "TestInDanger",
                    LastName = "Test",
                    DateOfBirth = new DateTime(2004, 6, 18),
                    Gender = "M",
                    Adress = "3 Club Road",
                    PhoneNumber = "300-444-5555"
                },
                new Patient
                {
                    Id = 4,
                    FirstName = "TestEarlyOnSet",
                    LastName = "Test",
                    DateOfBirth = new DateTime(2002, 6, 28),
                    Gender = "F",
                    Adress = "4 Valley Dr",
                    PhoneNumber = "400-555-6666"
                }
            );
        }
    }
}

