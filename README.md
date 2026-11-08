# RaceDay - Event Management System

## Portfolio of Evidence (POE) - Part 2: Web Application Development

**Build Status:** ✅ **SUCCESSFUL**  
**Code Audit:** ✅ **COMPLETE - 5 Issues Fixed**  
**Ready for Testing:** ✅ **YES**

---

## 🎯 PART 2 CODE AUDIT SUMMARY

### Issues Fixed: 5/5 ✅

| Issue | Severity | Status |
|-------|----------|--------|
| Missing ViewModel Properties (DateOfBirth, Role) | 🔴 CRITICAL | ✅ FIXED |
| jQuery Validation JavaScript Error | 🔴 CRITICAL | ✅ FIXED |
| Missing CSRF Token on Forms | 🟡 MEDIUM | ✅ FIXED |
| Missing EventsController & Views | 🔴 CRITICAL | ✅ CREATED |
| Missing Validation Attributes | 🟡 MEDIUM | ✅ FIXED |

**See detailed breakdown below ⬇️**

---

## Project Overview

RaceDay is a full-stack web-based event management system designed specifically for the South African road running, walking, and cycling community. The platform allows Event Organisers to create and manage events, categories, and participant results, while Participants can browse upcoming events, enter events, and track their personal performance history.

- **Part 1:** System planning and database design (COMPLETE)
- **Part 2:** Web application development with ASP.NET Core 8 MVC (IN PROGRESS)

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

---

# PART 2: CODE AUDIT DETAILS

## Issue #1: Missing ViewModel Properties ✅
**File:** `ViewModels/RegisterViewModel.cs`

**Problem:** Form has `dateOfBirth` and `role` fields but ViewModel doesn't

**Fixed:**
```csharp
public DateTime DateOfBirth { get; set; }
public string Role { get; set; } = string.Empty;
```

---

## Issue #2: jQuery Validation JavaScript Error ✅
**File:** `Views/Account/Register.cshtml`

**Error:** `TypeError: Cannot read properties of undefined (reading 'type')`

**Fixed:** Added safe jQuery validation wrapper with double-initialization prevention

---

## Issue #3: Missing CSRF Token ✅
**Files:** `Register.cshtml`, `Events/Create.cshtml`

**Fixed:** Added `@Html.AntiForgeryToken()` to all POST forms

---

## Issue #4: Missing EventsController ✅
**Created:**
- `Controllers/EventsController.cs` with Index, Details, Create actions
- `Views/Events/Index.cshtml` - Event listing
- `Views/Events/Details.cshtml` - Event detail view
- `Views/Events/Create.cshtml` - Event creation form

**Updated:** `ViewModels/EventViewModel.cs` - Added 7 new properties

---

## Issue #5: Missing Validation Attributes ✅
**File:** `Register.cshtml` form fields

**Fixed:** Added `data-val`, `data-val-required`, and error message containers to dateOfBirth and role fields

---

## ✅ Testing & Deployment Ready

- [x] Build successful
- [x] All 5 issues fixed
- [x] Navigation complete
- [x] Form validation working
- [x] Security measures in place

Ready to proceed with backen✅ SQL Script (.sql) present in /docs

View workflow runs in the Actions tab.

---

## Project Screenshots

### API Documentation (Swagger UI)
![Swagger](docs/Screenshot%202026-11-06%20193518.png)

### Event Registration Form
![Event Registration](docs/Screenshot%202026-11-06%20193541.png)

### Events Listing
![Events List](docs/Screenshot%202026-11-06%20193554.png)

### Event Details with Image
![Event Details](docs/Screenshot%202026-11-06%20193830.png)

### Event Creation Form
![Event Creation](docs/Screenshot%202026-11-06%20193844.png)

### Dashboard View
![Dashboard](docs/Screenshot%202026-11-06%20193858.png)

### Azure Blob Storage Integration
![Azure Integration](docs/Screenshot%202026-11-06%20193914.png)

---

## Technologies Used

### Backend & API
| Technology | Version | Purpose |
|-----------|---------|---------|
| **.NET** | 8.0 | Backend framework |
| **ASP.NET Core** | 8.0 | Web API & MVC framework |
| **Entity Framework Core** | 8.0 | ORM for database access |
| **SQL Server** | 2022+ | Relational database |
| **Azure Blob Storage** | Latest | File/image storage |
| **Swashbuckle.AspNetCore** | 6.5.0 | Swagger API documentation |
| **BCrypt.Net-Next** | 4.0.2 | Password hashing |

### Frontend
| Technology | Purpose |
|-----------|---------|
| **ASP.NET Core MVC** | Server-side rendering |
| **jQuery** | Form validation & interactions |
| **Bootstrap** | Responsive UI framework |
| **HTML5 / CSS3** | Markup & styling |
| **Razor Views** | Dynamic templating |

### Development Tools
| Tool | Purpose |
|------|---------|
| **Visual Studio Community 2026** | IDE |
| **SQL Server Management Studio (SSMS)** | Database development & management |
| **Postman** | API testing |
| **Git** | Version control |
| **GitHub** | Repository hosting |
| **GitHub Actions** | CI/CD pipeline automation |

### Documentation & Design
| Tool | Purpose |
|------|---------|
| **Lucidchart** | Entity Relationship Diagram (ERD) |
| **Microsoft Word** | API Endpoint Plan documentation |
| **Draw.io / Visio** | Architecture diagrams |

### NuGet Packages
```
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.AspNetCore.OpenApi
Swashbuckle.AspNetCore.Annotations
BCrypt.Net-Next
Azure.Storage.Blobs (v12.20.0)
Azure.Identity
System.Security.Cryptography.Pkcs
System.Text.Json
System.IdentityModel.Tokens.Jwt
Microsoft.Extensions.Caching.Memory
Microsoft.Data.SqlClient
```
# RaceDayMVC Azure Deployment Guide

## Prerequisites

Before deploying to Azure, you need:

1. **Azure Account** - Create free at: https://azure.microsoft.com/en-us/free/
2. **Azure CLI** - Download from: https://learn.microsoft.com/en-us/cli/azure/install-azure-cli-windows
3. **Docker Desktop** - Already installed on your machine

### Verify Prerequisites

```powershell
# Check Azure CLI
az --version

# Check Docker
docker --version

# Login to Azure
az login
```

---

## Step 1: Create an Azure Container Registry (ACR)

An Azure Container Registry is where you store your Docker images in the cloud.

### Create Resource Group

```powershell
# Define variables
$resourceGroup = "raceday-rg"
$location = "eastus"

# Create resource group
az group create --name $resourceGroup --location $location
```

**Note:** 
- `eastus` is the Azure region (closest to most of North America)
- Other options: `westus`, `northeurope`, `southafrika` (Cape Town)

### Create Container Registry

```powershell
# Define variables
$registryName = "racedayregistry"  # Must be globally unique, lowercase, alphanumeric only
$resourceGroup = "raceday-rg"

# Create container registry
az acr create `
  --resource-group $resourceGroup `
  --name $registryName `
  --sku Basic

# Get login server
az acr show --name $registryName --query loginServer --output tsv
```

**Output Example:** `racedayregistry.azurecr.io`

---

## Step 2: Login to Azure Container Registry

```powershell
# Login to ACR
az acr login --name racedayregistry

# Or login with Docker credentials
$password = az acr credential show --name racedayregistry --query "passwords[0].value" --output tsv
$loginServer = "racedayregistry.azurecr.io"

docker login $loginServer -u racedayregistry -p $password
```

---

## Step 3: Build and Push Docker Image to Azure

Navigate to your project directory and build:

```powershell
# Navigate to project
cd C:\Users\Admin\Downloads\PROGRAMMING-2B-POE

# Build locally first (optional)
docker build -t raceday-mvc:latest .

# Tag image for ACR
docker tag raceday-mvc:latest racedayregistry.azurecr.io/raceday-mvc:latest

# Push to Azure Container Registry
docker push racedayregistry.azurecr.io/raceday-mvc:latest

# Verify push
az acr repository list --name racedayregistry --output table
```

**Expected Output:**
```
Result
------
raceday-mvc
```

---

## Step 4: Deploy to Azure Container Instances (ACI)

The simplest and most cost-effective way to run your container in Azure.

### Option A: Deploy via Azure CLI

```powershell
$resourceGroup = "raceday-rg"
$containerName = "raceday-app"
$registryName = "racedayregistry"
$registryPassword = az acr credential show --name $registryName --query "passwords[0].value" --output tsv

az container create `
  --resource-group $resourceGroup `
  --name $containerName `
  --image racedayregistry.azurecr.io/raceday-mvc:latest `
  --cpu 1 `
  --memory 2 `
  --registry-login-server racedayregistry.azurecr.io `
  --registry-username racedayregistry `
  --registry-password $registryPassword `
  --ports 8080 `
  --environment-variables `
	ASPNETCORE_ENVIRONMENT="Production" `
	ApiSettings__BaseUrl="https://your-api-url.com" `
  --dns-name-label racedayapp
```

**Replace:**
- `https://your-api-url.com` with your actual API endpoint

### Check Deployment Status

```powershell
# Get container status
az container show `
  --resource-group raceday-rg `
  --name raceday-app `
  --query "containers[0].instanceView.currentState"

# Get container details
az container show `
  --resource-group raceday-rg `
  --name raceday-app `
  --query "{FQDN:ipAddress.fqdn, ProvisioningState:provisioningState}"

# View logs
az container logs `
  --resource-group raceday-rg `
  --name raceday-app

# Follow logs in real-time
az container attach `
  --resource-group raceday-rg `
  --name raceday-app
```

### Access Your Application

Once deployed, your application will be available at:

```
http://racedayapp.eastus.azurecontainers.io:8080
```

Or use the FQDN from the container details above.

---

## Step 5: Deploy to Azure App Service (Alternative)

Better for production with auto-scaling and monitoring.

### Create App Service Plan

```powershell
$resourceGroup = "raceday-rg"
$appServicePlan = "raceday-plan"
$location = "eastus"

az appservice plan create `
  --name $appServicePlan `
  --resource-group $resourceGroup `
  --is-linux `
  --sku B2
```

### Create Web App

```powershell
$webAppName = "raceday-app-prod"
$registryName = "racedayregistry"
$appServicePlan = "raceday-plan"
$resourceGroup = "raceday-rg"
$registryPassword = az acr credential show --name $registryName --query "passwords[0].value" --output tsv

az webapp create `
  --resource-group $resourceGroup `
  --plan $appServicePlan `
  --name $webAppName `
  --deployment-container-image-name racedayregistry.azurecr.io/raceday-mvc:latest

# Configure container settings
az webapp config container set `
  --name $webAppName `
  --resource-group $resourceGroup `
  --docker-custom-image-name racedayregistry.azurecr.io/raceday-mvc:latest `
  --docker-registry-server-url https://racedayregistry.azurecr.io `
  --docker-registry-server-user racedayregistry `
  --docker-registry-server-password $registryPassword

# Set environment variables
az webapp config appsettings set `
  --resource-group $resourceGroup `
  --name $webAppName `
  --settings `
	ASPNETCORE_ENVIRONMENT="Production" `
	ApiSettings__BaseUrl="https://your-api-url.com" `
	WEBSITES_PORT=8080
```

### Access App Service

Your application will be available at:

```
https://raceday-app-prod.azurewebsites.net
```

---

## Step 6: Update Application (After Code Changes)

When you update your code:

```powershell
# Stop running container
az container stop `
  --resource-group raceday-rg `
  --name raceday-app

# Delete container
az container delete `
  --resource-group raceday-rg `
  --name raceday-app `
  --yes

# Rebuild and push image
cd C:\Users\Admin\Downloads\PROGRAMMING-2B-POE
docker build -t racedayregistry.azurecr.io/raceday-mvc:latest .
docker push racedayregistry.azurecr.io/raceday-mvc:latest

# Redeploy container
az container create `
  --resource-group raceday-rg `
  --name raceday-app `
  --image racedayregistry.azurecr.io/raceday-mvc:latest `
  --cpu 1 `
  --memory 2 `
  --registry-login-server racedayregistry.azurecr.io `
  --registry-username racedayregistry `
  --registry-password $registryPassword `
  --ports 8080 `
  --environment-variables `
	ASPNETCORE_ENVIRONMENT="Production" `
	ApiSettings__BaseUrl="https://your-api-url.com" `
  --dns-name-label racedayapp
```

### For App Service (easier update)

```powershell
# App Service automatically pulls latest image when image tag is updated
# Just push new image to ACR:

docker build -t racedayregistry.azurecr.io/raceday-mvc:latest .
docker push racedayregistry.azurecr.io/raceday-mvc:latest

# Restart the web app to pull new image
az webapp restart `
  --resource-group raceday-rg `
  --name raceday-app-prod
```

---

## Step 7: Monitor Your Application

### View Application Logs

```powershell
# Container Instances logs
az container logs `
  --resource-group raceday-rg `
  --name raceday-app `
  --follow

# App Service logs
az webapp log tail `
  --resource-group raceday-rg `
  --name raceday-app-prod
```

### Check Application Health

```powershell
# Container status
az container show `
  --resource-group raceday-rg `
  --name raceday-app

# App Service details
az webapp show `
  --resource-group raceday-rg `
  --name raceday-app-prod
```

---

## Environment Variables

Configure these in your Azure deployment:

| Variable | Purpose | Example |
|----------|---------|---------|
| `ASPNETCORE_ENVIRONMENT` | Environment type | `Production` |
| `ApiSettings__BaseUrl` | API endpoint | `https://api.raceday.com` |
| `ASPNETCORE_URLS` | Listening URL | `http://+:8080` |

---

## Cost Estimation

### Container Instances Costs (as of 2024)
- **CPU**: $0.0000015 per second (~$1.30/month per vCore)
- **Memory**: $0.000000125 per GB/second (~$3.24/month per GB)
- **Estimate for 1 CPU, 2GB RAM**: ~$5-10/month (with limited traffic)

### App Service Costs
- **B2 Plan**: ~$30-50/month
- **Better for high traffic applications**
- **Includes auto-scaling, monitoring, backup**

### Free Trial
- **$200 free credit** for first 30 days
- **12 months free tier** for many services
- Deploy test and see costs!

---

## Troubleshooting

### Container won't start

```powershell
# Check logs
az container logs `
  --resource-group raceday-rg `
  --name raceday-app

# Check events
az container show `
  --resource-group raceday-rg `
  --name raceday-app
```

### Image not found

```powershell
# Verify image exists in ACR
az acr repository show `
  --name racedayregistry `
  --repository raceday-mvc

# List all images
az acr repository list --name racedayregistry
```

### Connection refused

```powershell
# Verify port mapping
az container show `
  --resource-group raceday-rg `
  --name raceday-app `
  --query "ipAddress.ports"

# Check ASPNETCORE_URLS environment variable is set to http://+:8080
```

### Authentication failed

```powershell
# Re-login to ACR
az acr login --name racedayregistry

# Or refresh credentials
az acr credential renew --name racedayregistry --password-name password1
```

---

## Cleanup (Remove Everything)

When you want to delete resources to save costs:

```powershell
# Delete container
az container delete `
  --resource-group raceday-rg `
  --name raceday-app `
  --yes

# Delete app service
az webapp delete `
  --resource-group raceday-rg `
  --name raceday-app-prod

# Delete registry
az acr delete `
  --resource-group raceday-rg `
  --name racedayregistry `
  --yes

# Delete resource group (deletes everything)
az group delete `
  --resource-group raceday-rg `
  --yes
```

---

## Quick Reference

| Task | Command |
|------|---------|
| Login to Azure | `az login` |
| Create resource group | `az group create --name raceday-rg --location eastus` |
| Create registry | `az acr create --resource-group raceday-rg --name racedayregistry --sku Basic` |
| Push image | `docker push racedayregistry.azurecr.io/raceday-mvc:latest` |
| Deploy to ACI | `az container create ...` |
| Deploy to App Service | `az webapp create ...` |
| View logs | `az container logs --resource-group raceday-rg --name raceday-app` |
| Get app URL | `az container show --resource-group raceday-rg --name raceday-app --query ipAddress.fqdn` |

---

## Next Steps

1. ✅ Create Azure Account
2. ✅ Install Azure CLI
3. ✅ Create Resource Group
4. ✅ Create Container Registry
5. ✅ Push Docker Image
6. ✅ Deploy to Azure (ACI or App Service)
7. ✅ Test application
8. ✅ Configure custom domain (optional)
9. ✅ Setup monitoring and alerts (optional)

---

## Support and Documentation

- **Azure Container Instances**: https://learn.microsoft.com/en-us/azure/container-instances/
- **Azure App Service**: https://learn.microsoft.com/en-us/azure/app-service/
- **Azure CLI Reference**: https://learn.microsoft.com/en-us/cli/azure/
- **Azure Pricing Calculator**: https://azure.microsoft.com/en-us/pricing/calculator/

---

**Last Updated:** Generated for RaceDayMVC with .NET 8, ASP.NET Core, and Azure

---

## Author
Name :	Ndimuhulu Matamela
Student ID :	ST10495492
Date : 	September 2026
License : MIT
This project is submitted for academic purposes as part of the IIE Portfolio of Evidence (POE).

© 2026 Ndimuhulu Matamela. All rights reserved.


