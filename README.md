# Clinical Dentist System

![.NET](https://img.shields.io/badge/.NET-8.0%2B-blueviolet)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-red)
![Ollama](https://img.shields.io/badge/AI-Ollama%20LLaMA%203.1-orange)
![MediatR](https://img.shields.io/badge/MediatR-Event_Driven-green)

A **RESTful Web API** built with **ASP.NET Core (C#)** designed to manage the complete clinical workflow of a dental clinic — from patient registration and appointments to AI-assisted clinical documentation, radiology, and prosthodontic lab integration.

---

## 💡 Project Idea

The **Clinical Dentist System** was built to solve a real operational problem in dental clinics: fragmented and manual clinical processes. Instead of managing patients, appointments, records, and stock through disconnected tools, this system centralizes everything.

The core idea is to give dental staff a **single backend system** that:
- Tracks the full patient journey — from booking to completed treatment.
- Stores rich **Electronic Health Records (EHR)** with structured dental data (teeth, procedures, X-rays, medications).
- Connects the main clinic with **Radiology** and **Prosthodontic Lab** modules via event-driven FHIR integration.
- Introduces **AI assistance** (via a local LLaMA model through Ollama) to help doctors write clinical notes, look up dental terminology, suggest treatments, and extract structured data from free-form text.
- Enforces **role-based access** so that admins, doctors, nurses, lab technicians, and radiologists each have appropriate permissions.
- Manages **clinic supplies and stock** with full transaction history.

This project is intended to serve as a backend API, designed to integrate with a frontend (e.g., a Next.js application).

---

## ✨ Features

### 👤 Authentication & Authorization
- Separate **Admin**, **Doctor**, **Nurse**, **Radiologist**, and **Lab Technician** registration and login flows.
- **JWT-based authentication** with specific role claims.
- **Role-based access control** — endpoints are restricted to specific roles (e.g., `DoctorOnly` or `AdminOnly` policies).

### 🧑‍⚕️ Patient Management
- Create, read, update, and delete patient profiles.
- Patient fields: first name, middle name, last name, gender, date of birth, phone.

### 📅 Appointment Scheduling
- Create and manage appointments linked to a patient, doctor, and nurse.
- Auto-generated reference numbers per appointment.

### 🗂️ Electronic Health Records (EHR)
- Full EHR per appointment/patient including:
  - **Medical info**: allergies, medical alerts.
  - **Dental info**: diagnosis, X-ray findings, periodontal status, clinical notes, recommendations, treatment history.
  - **Structured collections**: medications, procedures (with procedure codes), and per-tooth records.
- **Audit trail / change log**: every field-level change is logged with who changed it, when, and from which appointment.

### 🩻 Radiology Module (Event-Driven)
- Manage radiology equipment, radiologists, and imaging appointments.
- Event-driven FHIR integration: Doctor creates an imaging request which automatically schedules an appointment.
- Seamless diagnostic report tracking and retrieval.

### 🦷 Prosthodontic Lab Module (Event-Driven)
- Manage lab technicians.
- Doctors create **Lab Orders** and **Prescriptions**.
- FHIR standards used for data exchange (e.g., `DeviceRequest`).
- Background synchronization with fallback queuing and retry mechanism for resilience against outages.

### 🤖 AI-Assisted Clinical Documentation (LLaMA via Ollama)
- **Auto-complete** for partial clinical note text.
- **Dental terminology suggestions** from partial terms.
- **Generate full clinical notes** from bullet points.
- **Treatment suggestions** based on diagnosis and patient history.
- **Full EHR parsing** — doctor writes a large block of text, and the AI extracts all EHR fields automatically.

### 🏥 Staff Management
- Admins can manage **Doctor**, **Nurse**, **Radiologist**, and **Lab Technician** profiles.

### 📦 Supply & Inventory Management
- Manage dental supplies with categories, units, and quantities.
- Track **stock transactions** (who used what and when).

---

## 🛠 Tech Stack

| Layer | Technology |
|---|---|
| **Framework** | ASP.NET Core (C#) |
| **Database** | SQL Server |
| **ORM** | Entity Framework Core (with migrations) |
| **Event Bus** | MediatR |
| **Interoperability** | FHIR |
| **Authentication** | JWT Bearer tokens |
| **AI** | LLaMA 3.1 8B via [Ollama](https://ollama.com) |
| **API Docs** | Swagger / OpenAPI |

---

## 📂 Project Structure

```text
ClinicalDentistSystem/
├── Migrations/          # EF Core database migrations
├── Modules/             # Domain modules
│   ├── DentalClinic/    # Core operations (Patients, Appointments, EHR, Inventory, AI)
│   ├── ProsthodonticLab/# Lab technicians, orders, and prescriptions
│   └── Radiology/       # Radiology imaging appointments and diagnostic reports
├── Shared/              # Shared data context, models, and cross-module contracts
├── Tests/               # Automated tests
├── Program.cs           # App configuration and dependency injection
└── appsettings.json     # Configuration (JWT, DB connection, AI settings)
```

---

## 🚀 API Endpoints Overview

| Area | Endpoints |
|---|---|
| **Admin Auth** | `POST /api/v1/clinic/admin/login`, `POST /api/v1/clinic/admin/register` |
| **Doctor Auth** | `POST /api/v1/clinic/doctorauth/register`, `POST /api/v1/clinic/doctorauth/login` |
| **Nurse Auth** | `POST /api/v1/clinic/nurseauth/register`, `POST /api/v1/clinic/nurseauth/login` |
| **Patients** | `GET/POST/PUT/DELETE /api/v1/clinic/patient` |
| **Appointments** | `GET/POST/PUT/DELETE /api/v1/clinic/appointment` |
| **EHR** | `GET/POST/PUT /api/v1/clinic/ehr`, `GET /api/v1/clinic/ehr/{id}/history` |
| **Doctors / Nurses** | `GET/POST/PUT/DELETE /api/v1/clinic/admin/...` |
| **Supplies & Stock** | `GET/POST/PUT/DELETE /api/v1/clinic/supply`, `/api/v1/clinic/stocktransaction` |
| **AI** | `/api/v1/clinic/ai/autocomplete`, `/terminology`, `/generate-notes`, `/suggest-treatments`, `/extract-clinical-data`, `/parse-ehr` |
| **Radiology** | `/api/v1/radiology/imagingappointment`, `/api/v1/radiology/report` |
| **Prosthodontic Lab**| `/api/v1/prosthodonticlab/labtechnician`, `/api/v1/prosthodonticlab/order`, `/api/v1/prosthodonticlab/prescription` |

---

## ⚙️ Getting Started

### Prerequisites

1. **[.NET SDK 8.0+](https://dotnet.microsoft.com/download)**.
2. **SQL Server** (local or remote).
3. **[Ollama](https://ollama.com)** with the `llama3.1:8b` model pulled (for AI features):
   ```bash
   ollama pull llama3.1:8b
   ```

### Setup & Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/GoldenBoy13420/ClinicalDentistSystem.git
   cd ClinicalDentistSystem
   ```

2. **Configure Database & Secrets:**
   Update the `appsettings.json` or `appsettings.Development.json` file with your SQL Server connection string and JWT settings:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=ClinicalDentistDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Apply Migrations:**
   ```bash
   dotnet ef database update
   ```

4. **Run the API:**
   ```bash
   dotnet run
   ```
   Navigate to `https://localhost:<port>/swagger` to view and test the endpoints.
