# PROGRAMMING-2B-POE Repository

A comprehensive solution for the RaceDay event management system, featuring both a REST API backend and an ASP.NET Core MVC frontend.

## 📁 Repository Structure

### `/RaceDayAPI`
The backend REST API built with ASP.NET Core 8.

**Key Components:**
- **Controllers/** - HTTP endpoint controllers for all API resources (Events, Users, Categories, Enrolments, Results)
- **Models/** - Domain entities (Event, User, Participant, Category, Enrolment, Result, Organiser)
- **DTOs/** - Data Transfer Objects for request/response serialization
- **Data/** - Entity Framework Core `ApplicationDbContext` for database operations
- **Services/** - Business logic layer (AuthService, IAuthService, SessionAuthenticationHandler)
- **Migrations/** - Database migrations and SQL scripts
- **Properties/** - Launch settings and configuration
- **Program.cs** - Application configuration and startup
- **appsettings.json** - Application settings and configuration
- **RaceDayAPI.csproj** - Project file

**Database:**
- SQL Server via Entity Framework Core
- Initial migration creates schema for Users, Events, Categories, Enrolments, Results, and Organisers

**Key Endpoints:**
- `/api/auth` - Authentication (Login, Register)
- `/api/events` - Event management (GET, POST, PUT, DELETE)
- `/api/users` - User management
- `/api/categories` - Category management
- `/api/enrolments` - Course/race enrolments
- `/api/results` - Race results tracking

---

### `/RaceDayMVC`
The frontend ASP.NET Core MVC application that consumes the RaceDayAPI.

**Key Components:**
- **ViewModels/** - MVC view models (LoginViewModel, RegisterViewModel, EventViewModel, DashboardViewModel)
- **Controllers/** - MVC controllers (to be implemented)
- **Views/** - Razor templates (to be implemented)
- **Services/** - API client integration via HttpClient
- **RaceDayMVC.slnx** - Solution file for MVC project

**Session Management:**
- Configured with 30-minute session timeout
- HttpOnly and Essential cookies for security
- Distributed memory cache backing

**API Integration:**
- `Services/ApiService` - HttpClient-based service for calling RaceDayAPI
- Base URL: `https://localhost:5001/` (configurable in appsettings.json)
- Placeholder methods for `LoginAsync()` and `GetEventsAsync()`

---

### `/RaceDayTests`
Consolidated test projects for validating API functionality.

**Contents:**
- **Tests/** - Example and reference test files
- **RaceDay_Tests/** - Integration tests (EventsControllerTests, etc.)

---

### `/docs` & Supporting Files
- **Race Day Assignment/** - Assignment documentation and requirements
- **.gitignore** - Git exclusion rules
- **.github/workflows/** - CI/CD pipeline configuration
- **LICENSE** - Project licensing

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Visual Studio 2026 or later (or VS Code)
- SQL Server (LocalDB or full instance)

### Setup

#### 1. RaceDayAPI
```bash
cd RaceDayAPI
dotnet restore
dotnet build
dotnet run
```
API will be available at `https://localhost:5001`

#### 2. RaceDayMVC
```bash
cd RaceDayMVC
dotnet restore
dotnet build
dotnet run
```
MVC will be available at `https://localhost:7xxx` (see Properties/launchSettings.json)

**Important:** Ensure RaceDayAPI is running before starting RaceDayMVC, as it depends on API calls.

---

## 📝 Architecture Notes

### API (RaceDayAPI)
- **Pattern:** ASP.NET Core 8 REST API
- **Database:** Entity Framework Core with SQL Server
- **Authentication:** Session-based (handled by SessionAuthenticationHandler)
- **Controllers:** Stateless HTTP controllers returning JSON/DTOs

### MVC (RaceDayMVC)
- **Pattern:** ASP.NET Core 8 MVC (NO direct database access)
- **Database:** ❌ No direct DbContext - all data via HTTP calls to API
- **Authentication:** Session management for storing login state
- **Services:** ApiService wraps HttpClient for API communication
- **Session:** Distributed memory cache with 30-minute timeout

**Key Rule:** The MVC application must ONLY communicate with data via the RaceDayAPI. No direct database connections.

---

## 🔄 Data Flow

```
User Browser
	↓
RaceDayMVC (Controllers + Views)
	↓
RaceDayMVC Services (ApiService with HttpClient)
	↓
RaceDayAPI (REST Endpoints)
	↓
SQL Server Database
```

---

## 🛠️ Development

### Adding a New Feature to the MVC

1. Create ViewModel in `RaceDayMVC/ViewModels/`
2. Add method to `RaceDayMVC/Services/ApiService.cs` for API calls
3. Create Controller in `RaceDayMVC/Controllers/` to handle requests
4. Create Razor templates in `RaceDayMVC/Views/` for UI
5. Ensure session management is used for state

### Adding a New API Endpoint

1. Create Model in `RaceDayAPI/Models/` if needed
2. Create DTO in `RaceDayAPI/DTOs/` for request/response
3. Create Controller in `RaceDayAPI/Controllers/`
4. Implement service logic in `RaceDayAPI/Services/`
5. Create migration: `dotnet ef migrations add <MigrationName>`
6. Update `RaceDayMVC/Services/ApiService.cs` to call new endpoint

---

## 📦 Dependencies

### RaceDayAPI
- EntityFrameworkCore (EF Core)
- SQL Server provider
- ASP.NET Core (built-in)

### RaceDayMVC
- HttpClientFactory (built-in dependency injection)
- Session middleware (built-in)
- Bootstrap (already included for styling)

---

## 🧪 Testing

Run tests via:
```bash
cd RaceDayTests
dotnet test
```

---

## 📄 License

See LICENSE file for details.

---

## ✅ Latest Updates

- **Repository Reorganized:** Separated API and MVC into distinct folders for clarity
- **MVC Foundation Added:** ViewModels, ApiService, and session configuration ready
- **API Structure:** Controllers, DTOs, Models, and Data access layer organized
- **Tests Consolidated:** All test projects moved to RaceDayTests folder

