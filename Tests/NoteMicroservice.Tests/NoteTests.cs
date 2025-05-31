using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NoteMicroservice.Application.DTOs;
using NoteMicroservice.Application.Interfaces;
using NoteMicroservice.Application.Services;
using NoteMicroservice.Domain.Models;
using NoteMicroservice.Infrastructure.Repositories;
using Xunit;

namespace Tests.NoteMicroservice.Tests
{
    public class NoteTests
    {
        private readonly Mock<INoteRepository> _repoMock;
        private readonly INoteService _service;

        public NoteTests()
        {
            _repoMock = new Mock<INoteRepository>();
            _service = new NoteService(_repoMock.Object);
        }

        [Fact]
        public async Task GetNotesByPatientIdAsync_ReturnsNotes()
        {
            // Arrange
            var notes = new List<Note>
            {
                new Note { Id = "1", PatientId = 1, Content = "test" }
            };
            _repoMock.Setup(r => r.GetNotesByPatientIdAsync(1)).ReturnsAsync(notes);

            // Act
            var result = await _service.GetNotesByPatientIdAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal("test", result[0].Content);
        }

        [Fact]
        public async Task AddNoteAsync_CallsRepository()
        {
            // Arrange
            var dto = new NoteDto { PatientId = 1, Content = "abc" };

            // Act
            await _service.AddNoteAsync(dto);

            // Assert
            _repoMock.Verify(r => r.AddAsync(It.Is<Note>(n => n.Content == "abc")), Times.Once());
        }
    }
}
