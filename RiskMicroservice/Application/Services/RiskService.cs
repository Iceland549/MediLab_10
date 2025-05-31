using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using RiskMicroservice.Application.DTOs;
using RiskMicroservice.Infrastructure.Clients;
using RiskMicroservice.Application.Interfaces;


namespace RiskMicroservice.Application.Services
{
    public class RiskService : IRiskService
    {
        private readonly IPatientClient _patientClient;
        private readonly INoteClient _noteClient;

        public RiskService(IPatientClient patientClient, INoteClient noteClient)
        {
            _patientClient = patientClient;
            _noteClient = noteClient;
        }

        private static readonly string[] Triggers = new[]
        {
            "Hémoglobine A1C", "Microalbumine", "Poids", "Taille", "Fumeur", "Fumer", "Fume", "Anormale", "Anormales", "Cholestérol", "Vertige", "Rechute", "Réaction", "Anticorps"
        };

        public async Task<RiskAssessmentDto?> AssessmentRiskAsync(int patientId, string jwtToken)
        {
            var patient = await _patientClient.GetPatientByIdAsync(patientId, jwtToken);
            if (patient == null) return null;

            var today = DateTime.UtcNow;
            var age = today.Year - patient.DateOfBirth.Year;
            if (patient.DateOfBirth > today.AddYears(-age)) age--;

            var notes = await _noteClient.GetNotesByPatientIdAsync(patientId, jwtToken);
            // Extraire le contenu des notes dans une liste de string
            List<string> noteContents = notes
                .Where(note => note.Content != null)
                .Select(note => note.Content!)
                .ToList();

            int triggerCount = noteContents
                .SelectMany(content => Triggers.Where(trigger =>
                    content.IndexOf(trigger, StringComparison.OrdinalIgnoreCase) >= 0))
                .Count();


            string riskLevel = CalculateRiskLevel(age, patient.Gender ?? "", triggerCount);

            return new RiskAssessmentDto
            {
                PatientId = patientId,
                PatientFirstName = patient.FirstName ?? "",
                PatientLastName = patient.LastName ?? "",
                Age = age,
                Gender = patient.Gender ?? "",
                Notes = noteContents,
                RiskLevel = riskLevel
            };
        }

        private static string CalculateRiskLevel(int age, string gender, int triggerCount)
        {
            // Prise en compte simultanée de l'âge, du sexe et du nombre de triggers
            if (triggerCount == 0) return "None";

            // Early onset
            if (age < 30 && gender == "M" && triggerCount >= 5) return "Early onset";
            if (age < 30 && gender == "F" && triggerCount >= 7) return "Early onset";
            if (age >= 30 && triggerCount >= 8) return "Early onset";

            // In Danger
            if (age < 30 && gender == "M" && triggerCount >= 3) return "In Danger";
            if (age < 30 && gender == "F" && triggerCount >= 4) return "In Danger";
            if (age >= 30 && triggerCount >= 6) return "In Danger";

            // Borderline
            if (age >= 30 && triggerCount >= 2) return "Borderline";
            if (age < 30 && triggerCount >= 2) return "Borderline";

            return "None";
        }
    }
}
