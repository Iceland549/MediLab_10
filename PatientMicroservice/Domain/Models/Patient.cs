using System.Runtime.InteropServices;

namespace PatientMicroservice.Domain.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Adress {  get; set; }
        public string? PhoneNumber { get; set; }
    }
}
