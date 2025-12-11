using System.ComponentModel.DataAnnotations;

namespace clinical.APIs.DTOs
{
    public class EHRCreateRequest
    {
        // Medical Information
        public string? Allergies { get; set; }
        public string? MedicalAlerts { get; set; }
        public string? Medications { get; set; }

        // Dental Information
        public string? Diagnosis { get; set; }
        public string? XRayFindings { get; set; }
        public string? ClinicalNotes { get; set; }
        public string? Recommendations { get; set; }

        // Legacy fields (optional for backwards compatibility)
        public string? History { get; set; }
        public string? Treatments { get; set; }

        [Required(ErrorMessage = "Patient_ID is required")]
        public int Patient_ID { get; set; }

        [Required(ErrorMessage = "AppointmentId is required")]
        public int AppointmentId { get; set; }
    }
}
