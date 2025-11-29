using Xunit;
using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace PatientsBackend.Tests
{
    public class PatientRepositoryTests
    {
        
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddPatient()
        {
            var context = GetDbContext();
            var repo = new PatientRepository(context);

            var patient = new Patient
            {
                FirstName = "Juan",
                LastName = "Perez",
                DocumentType = "ID",
                DocumentNumber = "123"
            };

            await repo.AddAsync(patient);
            await context.SaveChangesAsync();

            var added = await context.Patients.FirstOrDefaultAsync(p => p.DocumentNumber == "123");
            Assert.NotNull(added);
            Assert.Equal("Juan", added.FirstName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemovePatient()
        {
            var context = GetDbContext();
            var repo = new PatientRepository(context);

            var patient = new Patient
            {
                FirstName = "Ana",
                LastName = "Lopez",
                DocumentType = "ID",
                DocumentNumber = "999"
            };

            await repo.AddAsync(patient);
            await context.SaveChangesAsync();

            await repo.DeleteAsync(patient);
            await context.SaveChangesAsync();

            var exists = await context.Patients.AnyAsync(p => p.DocumentNumber == "999");
            Assert.False(exists);
        }

        [Fact]
        public async Task ExistsDuplicateAsync_ShouldReturnTrue_WhenDuplicateExists()
        {
            var context = GetDbContext();
            var repo = new PatientRepository(context);

            var patient = new Patient
            {
                FirstName = "Ana",
                LastName = "Lopez",
                DocumentType = "ID",
                DocumentNumber = "999"
            };

            await repo.AddAsync(patient);
            await context.SaveChangesAsync();

            var exists = await repo.ExistsDuplicateAsync("ID", "999");
            Assert.True(exists);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPatient_WhenExists()
        {
            var context = GetDbContext();
            var repo = new PatientRepository(context);

            var patient = new Patient
            {
                FirstName = "Luis",
                LastName = "Diaz",
                DocumentType = "ID",
                DocumentNumber = "555"
            };

            await repo.AddAsync(patient);
            await context.SaveChangesAsync();

            var fetched = await repo.GetByIdAsync(patient.PatientId);
            Assert.NotNull(fetched);
            Assert.Equal("Luis", fetched.FirstName);
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnCorrectPage()
        {
            var context = GetDbContext();
            var repo = new PatientRepository(context);

            for (int i = 1; i <= 10; i++)
            {
                await repo.AddAsync(new Patient
                {
                    FirstName = $"Nombre{i}",
                    LastName = "Test",
                    DocumentType = "ID",
                    DocumentNumber = i.ToString()
                });
            }
            await context.SaveChangesAsync();

            var result = await repo.GetPagedAsync(page: 2, pageSize: 3, nameFilter: null, documentNumber: null);

            Assert.Equal(3, result.Items.Count());
            Assert.Equal(10, result.Total);
            Assert.Equal("Nombre4", result.Items.First().FirstName);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyPatient()
        {
            var context = GetDbContext();
            var repo = new PatientRepository(context);

            var patient = new Patient
            {
                FirstName = "OldName",
                LastName = "Last",
                DocumentType = "ID",
                DocumentNumber = "111"
            };

            await repo.AddAsync(patient);
            await context.SaveChangesAsync();

           
            patient.FirstName = "NewName";
            await repo.UpdateAsync(patient);
            await context.SaveChangesAsync();

            var updated = await context.Patients.FindAsync(patient.PatientId);
            Assert.Equal("NewName", updated.FirstName);
        }
    }
}
