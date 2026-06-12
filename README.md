# 🏥 E-Birth — Digital Birth & Health Record System

> A secure, scalable RESTful API system for managing birth registrations, child health records, vaccinations, and multi-role healthcare workflows across hospitals, doctors, and parents.

---

## 📌 Overview

**E-Birth** is a backend API platform built with **ASP.NET Core** that digitizes and centralizes the birth registration process in Egypt. It enables hospitals to register newborns, doctors to manage medical records, and parents to track their children's health history — all through a unified, role-based system.

### 🎯 Key Capabilities

- Accurate registration of parents and children's data
- Management of official child vaccinations linked to hospitals
- Secure storage of medical records for both children and parents, with support for uploading medical images and files
- Linking medical records to the responsible doctor for each case
- Data integrity enforcement via strict database constraints and indexes
- Role-based access for **Parents**, **Doctors**, and **Hospitals**
- OTP-based authentication with password reset flow
- Doctor approval workflow managed by hospitals

---

## ⚙️ Business Rules

1. A medical record is linked to **either** a parent **or** a child — never both at the same time
2. Every medical record must be linked to a **responsible doctor**
3. **Deleting a doctor** who has linked medical records is **prohibited**
4. **Deleting a parent** automatically **cascades** to delete all their children
5. Every vaccination is linked to both a **hospital** and an **official vaccination type**
6. Medical images and files are uploaded **per medical record only**, with file paths stored in the database
7. The system guarantees **no data loss** through primary key constraints, foreign keys, and indexed fields

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Backend Framework | ASP.NET Core Web API |
| Language | C# / .NET |
| Authentication | JWT + ASP.NET Core Identity |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Architecture | Clean Architecture / Service Layer Pattern |
| API Versioning | `/api/v1/` |
| File Handling | Server-side storage + DB path mapping |

---

## 👥 System Roles

```
├── 🏥 Hospital      → Manages children, doctors, parents, and approvals
├── 👨‍⚕️ Doctor       → Views and adds medical records for assigned patients  
└── 👨‍👩‍👧 Parent       → Views children profiles, vaccinations, and health history
```

---

## 🚀 Advanced Concepts Applied

> These are concepts I hadn't worked with before this project — I learned and implemented them hands-on throughout the development process.

| Concept | What I Did |
|---|---|
| **API Versioning** | Implemented versioned routing (`/api/v1/`) to support future breaking changes without affecting existing clients |
| **Data Protection** | Used ASP.NET Core Data Protection API to encrypt sensitive fields stored in the database and protect auth tokens from tampering |
| **Cancellation Tokens** | Passed `CancellationToken` through async service and controller methods to gracefully cancel abandoned or timed-out requests |
| **Custom Middleware** | Built middleware pipeline for centralized error handling, request logging, and standardized API response formatting |
| **In-Memory Caching** | Applied `IMemoryCache` on frequently-read endpoints to reduce redundant database hits and improve response times |
| **Role-Based Authorization** | Configured ASP.NET Core Identity roles (Parent, Doctor, Hospital) with policy-based access control across all modules |
| **DB Call Optimization** | Applied `AsNoTracking()` for read-only queries, used `Select()` projections to fetch only required columns, and avoided N+1 query patterns |

---

## 📡 API Endpoints

### 🔐 Auth Services — `/api/v1/Auth`

| Method | Endpoint | Description |
|---|---|---|
| POST | `/CreateParent` | Register a new parent account |
| POST | `/CreateHospital` | Register a new hospital account |
| POST | `/CreateDoctor` | Register a new doctor (with attachment upload) |
| POST | `/UserLogin` | Unified login for all roles |
| POST | `/ForgetPassword` | Trigger OTP to email |
| POST | `/IsvalidOtp` | Validate the OTP code |
| POST | `/ResendOtp` | Resend OTP to email |
| POST | `/ResetPassword` | Set new password after OTP verification |

---

### 👨‍👩‍👧 Parent — `/api/v1/Parent`

| Method | Endpoint | Description |
|---|---|---|
| POST | `/GetParentWithChilderen` | Get parent profile with all children |
| POST | `/GetParentDetailsAsync` | Get detailed parent info |
| POST | `/GetChildDetailsAsync` | Get specific child profile |
| POST | `/GetChildVaccinationsAsync` | Get all vaccinations for a child |
| POST | `/GetChildMedicalHistoryAsync` | Get child's full medical history |
| POST | `/GetSpecificChildMedicalHistoryAsync` | Get a specific medical record for a child |
| POST | `/GetParentMedicalHistoryAsync` | Get parent's medical history |
| POST | `/GetSpecificParentMedicalHistoryAsync` | Get a specific parent medical record |
| POST | `/UpdateParentProfile` | Update parent profile data |

---

### 👨‍⚕️ Doctor Dashboard — `/api/v1/DoctorDashboard`

| Method | Endpoint | Description |
|---|---|---|
| POST | `/GetDoctorForDashboard` | Get doctor dashboard summary |
| POST | `/GetDoctor` | Get doctor profile details |
| POST | `/UpdateDoctor` | Update doctor profile |
| POST | `/GetUserDetailsAsync` | Get details of a specific user |
| POST | `/GetChildVaccinationsAsync` | View child vaccination records |
| POST | `/GetChildMedicalHistoryAsync` | View child medical history |
| POST | `/GetSpecificChildMedicalHistoryAsync` | View specific child medical record |
| POST | `/AddChildMedicalRecord` | Add new medical record for a child |
| POST | `/GetParentMedicalHistoryAsync` | View parent medical history |
| POST | `/GetSpecificParentMedicalHistoryAsync` | View specific parent medical record |
| POST | `/AddParentMedicalRecord` | Add new medical record for a parent |

---

### 🏥 Hospital Dashboard — `/api/v1/HospitalDashboard`

| Method | Endpoint | Description |
|---|---|---|
| POST | `/GetHospitalDashboard` | Get full hospital dashboard overview |
| POST | `/GetHospital` | Get hospital profile |
| **Children** | | |
| GET | `/GetAllChilds` | Get all registered children |
| POST | `/CreateChild` | Register a new child (birth registration) |
| POST | `/UpdateChildProfile` | Update child's profile data |
| DELETE | `/DeleteChild` | Remove a child record |
| POST | `/GetChildDetailsByIdAsync` | Get child details by ID |
| POST | `/GetChildByNationalId` | Find child by National ID |
| POST | `/GetChildVaccinationsAsync` | View child vaccinations |
| POST | `/UpdateSpecificVaccinationAsync` | Update vaccination status |
| POST | `/GetChildMedicalHistoryAsync` | View child medical history |
| POST | `/GetSpecificChildMedicalHistoryAsync` | View specific child record |
| **Doctors** | | |
| GET | `/GetAllDoctors` | List all doctors in the hospital |
| POST | `/GetSpecificDoctorsById` | Find doctor by ID |
| POST | `/GetSpecificDoctorsByNationalId` | Find doctor by National ID |
| DELETE | `/DeleteDoctor` | Remove a doctor |
| POST | `/ApproveDoctor` | Approve a pending doctor registration |
| **Parents** | | |
| GET | `/GetAllParents` | List all registered parents |
| POST | `/GetParentDetailsByIdAsync` | Get parent details by ID |
| POST | `/GetParentDetailsByNationalIdAsync` | Find parent by National ID |
| POST | `/GetParentMedicalHistoryAsync` | View parent medical history |
| POST | `/GetSpecificParentMedicalHistoryAsync` | View specific parent record |

---

## 🔄 Core Workflows

### 1️⃣ Birth Registration Flow
```
Hospital Login
    └── CreateChild (birth registration)
        └── Assign to Parent (by National ID)
            └── Auto-generate vaccination schedule
                └── Doctor can add medical records
```

### 2️⃣ Doctor Onboarding Flow
```
Doctor registers (with credential attachment)
    └── Hospital reviews and ApproveDoctor
        └── Doctor gains access to assigned patients
```

### 3️⃣ Authentication Flow
```
Register (Parent / Hospital / Doctor)
    └── UserLogin → JWT Token issued
        ├── Forget Password → OTP sent to email
        ├── IsValidOtp → OTP verified
        └── ResetPassword → New password set
```

---

## 🔐 Authentication

All protected endpoints require a **JWT Bearer Token** in the Authorization header:

```
Authorization: Bearer <your_token_here>
```

Tokens are issued upon successful login via `/api/v1/Auth/UserLogin`.

---

## 🗄️ Database Schema

The system uses **SQL Server** with **ASP.NET Core Identity** as the foundation, extended with custom domain tables.

### 🔑 Identity Tables (ASP.NET Core)

| Table | Purpose |
|---|---|
| `AspNetUsers` | Base user table — shared by Parents, Doctors, Hospitals |
| `AspNetRoles` | Role definitions (Parent, Doctor, Hospital) |
| `AspNetUserRoles` | Many-to-many user-role assignments |
| `AspNetUserClaims` | Custom claims per user |
| `AspNetUserLogins` | External login providers |
| `AspNetUserTokens` | Auth tokens storage |
| `AspNetRoleClaims` | Claims attached to roles |
| `OtpCodes` | OTP records with expiration for password reset flow |

### 🏥 Domain Tables

| Table | Key Fields | Relationships |
|---|---|---|
| `Parents` | FullName, NationalId, BirthDate, Village, City, Gender, Governorate, BloodType, PhoneNumber, Email | → AspNetUsers (1:1) |
| `Children` | FullName, NationalId, BirthDate, Village, City, Gender, Governorate, BloodType, ParentId | → Parents (many:1) |
| `Doctors` | FullName, NationalId, BirthDate, Village, Gender, BloodType, PhoneNumber, Email, IsApproved, IsDeleted | → AspNetUsers (1:1) |
| `Hospitals` | Name, Longitude, Latitude, PhoneNumber, Email, Governorate, IsApproved, IsDeleted | → AspNetUsers (1:1) |
| `ChildVaccinations` | DateGiven, ChildId, HospitalId | → Children (many:1), Hospitals (many:1) |
| `AllOfficialVaccination` | VaccinationType, AgeMonths (schedule reference) | — |
| `UserMedicalRecords` | Description, ParentId, ChildId, DoctorId, Email, Medicine | → Parents / Children / Doctors |
| `MedicalRecordImages` | ImagePath, UserMedicalRecordId | → UserMedicalRecords (many:1) |
| `DoctorAttachments` | FilePath, DoctorId | → Doctors (many:1) |

### 🔗 Key Relationships

```
AspNetUsers
    ├── Parents (1:1)
    │       └── Children (1:many)
    │               ├── ChildVaccinations (1:many)
    │               └── UserMedicalRecords (1:many)
    │                       └── MedicalRecordImages (1:many)
    ├── Doctors (1:1)
    │       └── DoctorAttachments (1:many)
    └── Hospitals (1:1)
            └── ChildVaccinations (1:many)
```

---

## 📦 Sample Request Bodies

### Create Parent
```json
{
  "FullName": "Ahmed Mohamed",
  "NationalId": "12345678901234",
  "BirthDate": "1990-05-01",
  "Village": "El-Mahalla El-Kubra",
  "City": "Gharbia",
  "Gender": 1,
  "Governorate": 3,
  "BloodType": 2,
  "PhoneNumber": "01012345678",
  "Email": "parent@example.com",
  "Passworded": "MySecret123!",
  "ConfirmPassworded": "MySecret123!"
}
```

### User Login
```json
{
  "EmailOrNationalId": "parent@example.com",
  "Password": "MySecret123!"
}
```

### Reset Password
```json
{
  "email": "parent@example.com",
  "Password": "NewPassword123#",
  "confirmPassword": "NewPassword123#"
}
```

---

## 📊 Stats

| Metric | Value |
|---|---|
| Total Endpoints | 51 |
| API Modules | 4 (Auth, Parent, Doctor, Hospital) |
| User Roles | 3 (Parent, Doctor, Hospital) |
| Database Tables | 16 (7 Identity + 9 Domain) |
| Auth Mechanism | JWT + OTP |

---

## 👨‍💻 Author

**Hesham Ahmed Hassan**  
.NET Full-Stack Developer  
📧 hesham.ahmed.hassan.fci.tu@gmail.com  
🔗 [LinkedIn](https://www.linkedin.com/in/hesham-ahmed-hassan/) | [GitHub](https://github.com/HeshamAhmed0)
"# E_Birth-system-graduation-project-" 
