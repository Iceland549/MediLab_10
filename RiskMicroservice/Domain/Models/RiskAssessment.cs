namespace RiskMicroservice.Domain.Models
{
    public class RiskAssessment
    {
        public int PatientId { get; set; }
        public string PatientFirstName { get; set; } = string.Empty;
        public string PatientLastName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public List<string> Notes { get; set; } = new();
        public string RiskLevel { get; set; } = string.Empty;

    }
}
