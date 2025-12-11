namespace clinical.APIs.DTOs
{
    public class EHRResponse
    {
        public int EHR_ID { get; set; }
        
        // Medical Information
        public string? Allergies { get; set; }
        public string? MedicalAlerts { get; set; }
        public string? Medications { get; set; }

        // Dental Information
        public string? Diagnosis { get; set; }
        public string? XRayFindings { get; set; }
        public string? ClinicalNotes { get; set; }
        public string? Recommendations { get; set; }

        // Legacy fields
        public string? History { get; set; }
        public string? Treatments { get; set; }

        // Metadata
        public DateTime Last_Updated { get; set; }
        public string? UpdatedBy { get; set; }
        
        public int Patient_ID { get; set; }
        public int AppointmentId { get; set; }
        public PatientBasicInfo? Patient { get; set; }
        public AppointmentBasicInfo? Appointment { get; set; }
    }
}
