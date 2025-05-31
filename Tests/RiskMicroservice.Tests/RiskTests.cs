using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using RiskMicroservice.Application.DTOs;
using RiskMicroservice.Application.Interfaces;
using RiskMicroservice.Application.Services;
using RiskMicroservice.Infrastructure.Clients;
using Xunit;

namespace Tests.RiskMicroservice.Tests
{
    public class RiskTests
    {
        private readonly Mock<IPatientClient> _patientClientMock;
        private readonly Mock<INoteClient> _noteClientMock;
        private readonly IRiskService _riskService;

        public RiskTests()
        {
            _patientClientMock = new Mock<IPatientClient>();
            _noteClientMock = new Mock<INoteClient>();
            _riskService = new RiskService(_patientClientMock.Object, _noteClientMock.Object);
        }

        [Fact]
        public async Task AssessRiskAsync_ReturnsNone_WhenNoTriggers()
        {
            // Arrange
            var patient = new PatientDto
            {
                Id = 1,
                FirstName = "Albert",
                LastName = "Bon",
                Gender = "F",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            var notes = new List<NoteDto>
            {
                new NoteDto { Content = "Pas de mot clé déclencheur ici" }
            };
            _patientClientMock.Setup(p => p.GetPatientByIdAsync(1, It.IsAny<string>())).ReturnsAsync(patient);
            _noteClientMock.Setup(n => n.GetNotesByPatientIdAsync(1, It.IsAny<string>())).ReturnsAsync(notes);

            // Act
            var result = await _riskService.AssessmentRiskAsync(1, "dummy-token");

            // Assert
            Assert.NotNull(result); 
            Assert.Equal("None", result.RiskLevel);
            Assert.Equal(1, result.PatientId);
            Assert.Equal("Albert", result.PatientFirstName);
            Assert.Equal("Bon", result.PatientLastName);
            Assert.Equal(45, result.Age); 
            Assert.Equal("F", result.Gender);
            Assert.Single(result.Notes);
            Assert.Equal("Pas de mot clé déclencheur ici", result.Notes[0]);
        }

        [Fact]
        public async Task AssessRiskAsync_ReturnsInDanger_WhenTriggersDetected()
        {
            // Arrange
            var patient = new PatientDto
            {
                Id = 2,
                FirstName = "Charline",
                LastName = "Dujardin",
                Gender = "M",
                DateOfBirth = new DateTime(1960, 1, 1)
            };
            var notes = new List<NoteDto>
            {
                new NoteDto { Content = "Poids, Fumeur, Cholestérol" },
                new NoteDto { Content = "Anticorps" }
            };
            _patientClientMock.Setup(p => p.GetPatientByIdAsync(2, It.IsAny<string>())).ReturnsAsync(patient);
            _noteClientMock.Setup(n => n.GetNotesByPatientIdAsync(2, It.IsAny<string>())).ReturnsAsync(notes);

            // Act
            var result = await _riskService.AssessmentRiskAsync(2, "dummy-token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Borderline", result.RiskLevel);
            Assert.Equal(2, result.PatientId);
            Assert.Equal("Charline", result.PatientFirstName);
            Assert.Equal("Dujardin", result.PatientLastName);
            Assert.Equal(65, result.Age); 
            Assert.Equal("M", result.Gender);
            Assert.Equal(2, result.Notes.Count);
            Assert.Contains("Poids, Fumeur, Cholestérol", result.Notes);
            Assert.Contains("Anticorps", result.Notes);
        }

        [Fact]
        public async Task AssessRiskAsync_ReturnsEarlyOnset_ForYoungWithManyTriggers()
        {
            // Arrange
            var patient = new PatientDto
            {
                Id = 3,
                FirstName = "Elsa",
                LastName = "Fabian",
                Gender = "F",
                DateOfBirth = DateTime.Today.AddYears(-30)
            };
            var notes = new List<NoteDto>
            {
                new NoteDto { Content = "Fumeur, Cholestérol, Anticorps, Vertige, Taille, Poids, Microalbumine, Réaction" }
            };
            _patientClientMock.Setup(p => p.GetPatientByIdAsync(3, It.IsAny<string>())).ReturnsAsync(patient);
            _noteClientMock.Setup(n => n.GetNotesByPatientIdAsync(3, It.IsAny<string>())).ReturnsAsync(notes);

            // Act
            var result = await _riskService.AssessmentRiskAsync(3, "dummy-token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Early onset", result.RiskLevel);
            Assert.Equal(3, result.PatientId);
            Assert.Equal("Elsa", result.PatientFirstName);
            Assert.Equal("Fabian", result.PatientLastName);
            Assert.Equal(30, result.Age); 
            Assert.Equal("F", result.Gender);
            Assert.Single(result.Notes);
            Assert.Equal("Fumeur, Cholestérol, Anticorps, Vertige, Taille, Poids, Microalbumine, Réaction", result.Notes[0]);
        }
    }
}
