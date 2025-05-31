using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using PatientMicroservice.Application.DTOs;
using PatientMicroservice.Application.Interfaces;
using PatientMicroservice.Application.Services;
using PatientMicroservice.Domain.Models;
using PatientMicroservice.Infrastructure.Repositories;
using Xunit;

namespace Tests.PatientMicroservice.Tests
{
    public class PatientTests
    {
        private readonly Mock<IPatientRepository> _repoMock;
        private readonly IPatientService _service;

        public PatientTests()
        {
            _repoMock = new Mock<IPatientRepository>();
            _service = new PatientService(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsPatientDtos()
        {
            // Arrange
            var patients = new List<Patient>
            {
                new Patient { Id = 1, FirstName = "Albert", LastName = "Bon", Gender = "M" }
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(patients);

            // Act
            var result = await _service.GetAllPatientsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Albert", result[0].FirstName);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsNull_IfNotFound()
        {
            // Arrange  
            _repoMock.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Patient?)null);

            // Act  
            var result = await _service.GetPatientByIdAsync(42);

            // Assert  
            Assert.Null(result);
        }

        [Fact]
        public async Task AddPatientAsync_CallsRepository()
        {
            // Arrange
            var dto = new PatientDto { Id = 2, FirstName = "Charline", LastName = "Dujardin", Gender = "F" };

            // Act
            await _service.AddPatientAsync(dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Patient>(p => p.FirstName == "Charline")), Times.Once());
        }
    }
}
