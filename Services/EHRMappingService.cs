using clinical.APIs.Models;
using clinical.APIs.DTOs;

namespace clinical.APIs.Services
{
    public class EHRMappingService : IEHRMappingService
    {
        public EHRResponse MapToResponse(EHR ehr)
        {
            if (ehr == null)
                return null;

            return new EHRResponse
            {
                EHR_ID = ehr.EHR_ID,
                // Medical Information
                Allergies = ehr.Allergies,
                MedicalAlerts = ehr.MedicalAlerts,
                Medications = ehr.Medications,
                // Dental Information
                Diagnosis = ehr.Diagnosis,
                XRayFindings = ehr.XRayFindings,
                ClinicalNotes = ehr.ClinicalNotes,
                Recommendations = ehr.Recommendations,
                // Legacy fields
                History = ehr.History,
                Treatments = ehr.Treatments,
                // Metadata
                Last_Updated = ehr.Last_Updated,
                UpdatedBy = ehr.UpdatedBy,
                Patient_ID = ehr.Patient_ID,
                AppointmentId = ehr.AppointmentId,
                Patient = ehr.Patient != null ? new PatientBasicInfo
                {
                    Patient_ID = ehr.Patient.Patient_ID,
                    First = ehr.Patient.First,
                    Middle = ehr.Patient.Middle,
                    Last = ehr.Patient.Last,
                    Gender = ehr.Patient.Gender,
                    DOB = ehr.Patient.DOB
                } : null,
                Appointment = ehr.Appointment != null ? new AppointmentBasicInfo
                {
                    Appointment_ID = ehr.Appointment.Appointment_ID,
                    Date = ehr.Appointment.Date,
                    Time = ehr.Appointment.Time,
                    Ref_Num = ehr.Appointment.Ref_Num,
                    Type = ehr.Appointment.Type
                } : null
            };
        }

        public List<EHRResponse> MapToResponseList(List<EHR> ehrs)
        {
            if (ehrs == null)
                return new List<EHRResponse>();

            return ehrs.Select(e => MapToResponse(e)).ToList();
        }
    }
}
