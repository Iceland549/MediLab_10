namespace RiskMicroservice.Application.DTOs
{
        public class NoteDto
        {
            public string Id { get; set; } = string.Empty;
            public int PatientId { get; set; }
            public string? Content { get; set; }
            public DateTime DateCreated { get; set; }
        }
}


