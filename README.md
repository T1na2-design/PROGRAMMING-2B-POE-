# RaceDay - Event Management System

## Portfolio of Evidence (POE) - Part 1

---

## Project Overview

RaceDay is a full-stack web-based event management system designed specifically for the South African road running, walking, and cycling community. The platform allows Event Organisers to create and manage events, categories, and participant results, while Participants can browse upcoming events, enter events, and track their personal performance history.

**Part 1** focuses on **system planning and database design** before any application code is written.

---

## Repository Structure
RaceDay-POE/
│
├── .github/
│ └── workflows/
│ └── validate-repository.yml # GitHub Actions CI/CD workflow
│
├── docs/
│ ├── RaceDay ER Diagram.png # Section A - Entity Relationship Diagram
│ ├── Endpoint API Plan.docx # Section B - API Endpoint Plan
│ └── RaceDay Script.sql # Section C - SQL Database Script
│
├── .gitignore
└── README.md

text

---

## Sections Completed

| Section | Description | File |
|---------|-------------|------|
| **Section A** | Entity Relationship Diagram (ERD) | `docs/RaceDay ER Diagram.png` |
| **Section B** | API Endpoint Plan | `docs/Endpoint API Plan.docx` |
| **Section C** | SQL Database Script | `docs/RaceDay Script.sql` |

---

## Section A - Entity Relationship Diagram (ERD)

The ERD models the complete RaceDay data structure with **7 entities**:

| Entity | Description |
|--------|-------------|
| **Users** | All system users (Organisers and Participants) |
| **Organisers** | Event organisers who create and manage events |
| **Participants** | Athletes who participate in events |
| **Events** | Racing events created by organisers |
| **Categories** | Race categories (distances, age groups, gender) |
| **Enrolments** | Participant registrations for event categories |
| **Results** | Race results and performance records |

### Relationships
- Users → Organisers (1:1)
- Users → Participants (1:1)
- Organisers → Events (1:M)
- Events → Categories (1:M)
- Participants → Enrolments (1:M)
- Categories → Enrolments (1:M)
- Enrolments → Results (1:1)
- Users → Results (1:M)

---

## Section B - API Endpoint Plan

The API plan covers **39 endpoints** across 8 categories:

| Category | Endpoints | Description |
|----------|-----------|-------------|
| Authentication | 4 | Register, login, logout, current user |
| User Profile | 4 | View/update profile, change password |
| Events | 7 | CRUD operations, categories, statistics |
| Categories | 5 | CRUD operations for event categories |
| Enrolments | 5 | Event registration and management |
| Results | 7 | Race results and leaderboards |
| Statistics | 5 | Dashboard and analytics |
| Notifications/Webhooks | 2 | Email notifications and webhooks |

### Roles
- **Organiser** - Create/edit/delete events, manage categories, capture results, view all enrolments
- **Participant** - Browse events, enter events, view own enrolments, track personal results

---

## Section C - SQL Database Script

The SQL script creates the complete database schema for **SQL Server 2022**:

- ✅ 7 tables with all relationships
- ✅ Primary keys, foreign keys, and constraints
- ✅ CHECK constraints for data validation
- ✅ UNIQUE constraints
- ✅ DEFAULT values
- ✅ Indexes for performance
- ✅ Sample data: 2 organisers, 2 participants, 3 events, categories, and enrolments

### How to Run

1. Open **SQL Server Management Studio (SSMS)**
2. Connect to your SQL Server instance
3. Open `docs/RaceDay Script.sql`
4. Click **Execute** (F5)

The script will drop/create the database, create all tables, insert sample data, and display verification results.

---

## Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone https://github.com/T1na2-design/PROGRAMMING-2B-POE.git
Navigate to the docs folder:

bash
cd PROGRAMMING-2B-POE/docs

Run the SQL script in SSMS to create the database.

View the ERD by opening RaceDay_ERD.png.

View the API Plan by opening Endpoint_API_Plan.docx.

## CI/CD Workflow
This repository includes a GitHub Actions workflow that validates the repository structure on every push.

Checks performed:
✅ /docs folder exists

✅ ERD file (PNG/PDF) present in /docs

✅ API Endpoint Plan (DOCX/MD) present in /docs

✅ SQL Script (.sql) present in /docs

View workflow runs in the Actions tab.

##Technologies Used
SQL Server Management Studio (SSMS) -	Database development
Lucidchart -	ERD diagram creation
Microsoft Word -	API endpoint plan
GitHub -	Version control
GitHub Actions -	CI/CD workflow automation

## Author
Name :	Ndimuhulu Matamela
Student ID :	ST10495492
Date : 	September 2026
License : MIT
This project is submitted for academic purposes as part of the IIE Portfolio of Evidence (POE).

© 2026 Ndimuhulu Matamela. All rights reserved.


