# EHR Enhancement with Change Tracking

## Overview
This document describes the enhancements made to the Electronic Health Record (EHR) system to support comprehensive dental and medical information tracking, along with a complete change audit trail.

## What's New

### 1. Enhanced EHR Model
The EHR model has been expanded to include:

#### Medical Information
- **Allergies**: Patient allergies (medications, food, etc.)
- **MedicalAlerts**: Important medical conditions (diabetes, hypertension, etc.)
- **Medications**: Current medications the patient is taking

#### Dental Information
- **Diagnosis**: Dental diagnosis (caries, gum disease, etc.)
- **XRayFindings**: X-ray analysis results (decay, bone loss, etc.)
- **ClinicalNotes**: General clinical observations and notes
- **Recommendations**: Treatment recommendations for the patient

#### Metadata
- **UpdatedBy**: Name of the doctor who last updated the record
- **Last_Updated**: Timestamp of the last update

#### Legacy Fields
- **History**: Medical/dental history (kept for backwards compatibility)
- **Treatments**: Treatment information (kept for backwards compatibility)

### 2. EHR Change Log System
Every change to an EHR record is now automatically tracked with:

- **What changed**: Field name, old value, and new value
- **Who changed it**: Doctor ID and name (from JWT token)
- **When it changed**: Exact timestamp
- **In which appointment**: The appointment during which the change was made
- **Change type**: "Created" or "Updated"

### 3. New API Endpoints

#### Get EHR Change History
```
GET /EHR/{EHR_ID}/history
```
Returns a complete audit trail of all changes made to a specific EHR record, ordered by most recent first.

**Response Example:**
```json
[
  {
    "changeLog_ID": 1,
    "fieldName": "Diagnosis",
    "oldValue": "Healthy gums",
    "newValue": "Mild gingivitis",
    "changeType": "Updated",
    "changedAt": "2024-01-15T10:30:00",
    "changedByDoctorId": 5,
    "changedByDoctorName": "Dr. John Smith",
    "appointmentId": 123,
    "ehR_ID": 45
  }
]
```

### 4. Automatic Change Tracking

#### On EHR Creation
When a new EHR is created:
- All non-null fields are logged with ChangeType = "Created"
- The creating doctor's information is captured from the JWT token
- The appointment ID is recorded

#### On EHR Update
When an EHR is updated:
- Only changed fields are logged
- Each change shows the old value and new value
- ChangeType = "Updated"
- The modifying doctor and appointment are recorded

## Usage Examples

### Create a New EHR with Enhanced Fields

**Request:**
```http
POST /EHR
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "patient_ID": 123,
  "appointmentId": 456,
  "allergies": "Penicillin, Latex",
  "medicalAlerts": "Diabetes Type 2, Hypertension",
  "medications": "Metformin 500mg, Lisinopril 10mg",
  "diagnosis": "Dental caries on tooth #18",
  "xRayFindings": "Moderate decay in lower left molar, no bone loss detected",
  "clinicalNotes": "Patient reports sensitivity to cold. Recommended root canal treatment.",
  "recommendations": "Root canal therapy for tooth #18, follow-up in 2 weeks"
}
```

**Response:**
```json
{
  "ehR_ID": 789,
  "allergies": "Penicillin, Latex",
  "medicalAlerts": "Diabetes Type 2, Hypertension",
  "medications": "Metformin 500mg, Lisinopril 10mg",
  "diagnosis": "Dental caries on tooth #18",
  "xRayFindings": "Moderate decay in lower left molar, no bone loss detected",
  "clinicalNotes": "Patient reports sensitivity to cold. Recommended root canal treatment.",
  "recommendations": "Root canal therapy for tooth #18, follow-up in 2 weeks",
  "last_Updated": "2024-01-15T10:30:00",
  "updatedBy": "Dr. John Smith",
  "patient_ID": 123,
  "appointmentId": 456
}
```

### Update an Existing EHR

**Request:**
```http
PUT /EHR/789
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "ehR_ID": 789,
  "patient_ID": 123,
  "appointmentId": 457,
  "allergies": "Penicillin, Latex",
  "medicalAlerts": "Diabetes Type 2, Hypertension",
  "medications": "Metformin 500mg, Lisinopril 10mg",
  "diagnosis": "Root canal completed on tooth #18",
  "xRayFindings": "Post-treatment X-ray shows successful root canal filling",
  "clinicalNotes": "Root canal procedure completed successfully. Patient tolerated well.",
  "recommendations": "Follow-up in 6 months for routine checkup"
}
```

### View Change History

**Request:**
```http
GET /EHR/789/history
Authorization: Bearer <jwt-token>
```

**Response:**
```json
[
  {
    "changeLog_ID": 5,
    "fieldName": "Diagnosis",
    "oldValue": "Dental caries on tooth #18",
    "newValue": "Root canal completed on tooth #18",
    "changeType": "Updated",
    "changedAt": "2024-01-20T14:00:00",
    "changedByDoctorId": 5,
    "changedByDoctorName": "Dr. John Smith",
    "appointmentId": 457,
    "ehR_ID": 789
  },
  {
    "changeLog_ID": 4,
    "fieldName": "XRayFindings",
    "oldValue": "Moderate decay in lower left molar, no bone loss detected",
    "newValue": "Post-treatment X-ray shows successful root canal filling",
    "changeType": "Updated",
    "changedAt": "2024-01-20T14:00:00",
    "changedByDoctorId": 5,
    "changedByDoctorName": "Dr. John Smith",
    "appointmentId": 457,
    "ehR_ID": 789
  }
]
```

## Database Schema

### EHRs Table
- Added columns: `MedicalAlerts`, `Diagnosis`, `XRayFindings`, `ClinicalNotes`, `Recommendations`, `UpdatedBy`
- Existing columns: `EHR_ID`, `Allergies`, `Medications`, `History`, `Treatments`, `Last_Updated`, `Patient_ID`, `AppointmentId`

### EHRChangeLogs Table (New)
- `ChangeLog_ID`: Primary key
- `EHR_ID`: Foreign key to EHRs
- `FieldName`: Name of the field that was changed
- `OldValue`: Previous value
- `NewValue`: New value
- `ChangeType`: "Created" or "Updated"
- `ChangedAt`: Timestamp of the change
- `ChangedByDoctorId`: ID of the doctor who made the change
- `ChangedByDoctorName`: Name of the doctor
- `AppointmentId`: ID of the appointment during which the change was made
- `DoctorId`: Foreign key to Doctors
- `Appointment_ID`: Foreign key to Appointments

## Security & Authentication

- All EHR operations require authentication (JWT token)
- Only users with "Doctor" role can create or update EHRs (`[Authorize(Policy = "DoctorOnly")]`)
- Doctor information is automatically extracted from the JWT token
- No manual entry of doctor information is required

## Technical Implementation

### New Services
1. **IEHRChangeLogService / EHRChangeLogService**
   - `LogCreationAsync()`: Logs all fields when a new EHR is created
   - `LogChangesAsync()`: Compares old and new values, logs only changed fields

### Updated Services
2. **EHRMappingService**: Updated to map all new fields

### Updated Controllers
3. **EHRController**: 
   - Integrated with `IEHRChangeLogService`
   - Extracts doctor information from JWT token
   - Automatically logs all changes
   - New endpoint: `GET /EHR/{EHR_ID}/history`

## Benefits

1. **Complete Audit Trail**: Every change is tracked with full context
2. **Accountability**: Know exactly who made what changes and when
3. **Regulatory Compliance**: Meets healthcare data audit requirements
4. **Better Patient Care**: Track patient progress over time with detailed history
5. **Dispute Resolution**: Clear record of all modifications to patient records
6. **Medical-Legal Protection**: Comprehensive documentation of all clinical decisions

## Migration

The database migration `EnhanceEHRWithChangeTracking` has been created and applied. It:
- Adds new columns to the EHRs table
- Creates the EHRChangeLogs table
- Sets up appropriate foreign key relationships
- Maintains backward compatibility with existing data

## Future Enhancements

Potential future improvements:
1. Add filtering options for change history (by date range, field name, doctor)
2. Export change history to PDF for official records
3. Add notifications when critical fields are changed
4. Implement change approval workflow for sensitive fields
5. Add rollback capability to restore previous values
