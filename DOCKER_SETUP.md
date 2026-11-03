# RaceDayMVC Docker Setup Guide

## Prerequisites
Before running the Docker commands, ensure you have Docker installed:
- **Windows/Mac**: Install [Docker Desktop](https://www.docker.com/products/docker-desktop)
- **Linux**: Install Docker via your package manager

## Building the Docker Image

### Command 1: Build the Docker image
```powershell
docker build -t raceday-mvc:latest .
```

**Explanation:**
- `docker build`: Command to build a Docker image
- `-t raceday-mvc:latest`: Tags the image with name `raceday-mvc` and version `latest`
- `.`: Uses the Dockerfile in the current directory

**Expected Output:**
```
[+] Building 45.2s (12/12) FINISHED
 => [internal] load build definition from Dockerfile
 => [stage-1 1/7] FROM mcr.microsoft.com/dotnet/sdk:8.0
 => [stage-1 2/7] WORKDIR /src
 ...
 => exporting to image
 => => naming to docker.io/library/raceday-mvc:latest
```

---

## Running the Docker Container

### Command 2: Run the container (basic)
```powershell
docker run -d -p 8080:8080 --name raceday-app raceday-mvc:latest
```

**Explanation:**
- `docker run`: Command to create and run a new container
- `-d`: Run in detached mode (background)
- `-p 8080:8080`: Map port 8080 on host to port 8080 in container
- `--name raceday-app`: Give the container a friendly name
- `raceday-mvc:latest`: Image name and tag

**Access the application:**
- Open browser and go to: `http://localhost:8080`

---

### Command 3: Run the container with environment variables
```powershell
docker run -d `
  -p 8080:8080 `
  --name raceday-app `
  -e "ApiSettings__BaseUrl=https://api.example.com" `
  -e "ASPNETCORE_ENVIRONMENT=Production" `
  raceday-mvc:latest
```

**Explanation:**
- `-e`: Set environment variables
- Replace `https://api.example.com` with your actual API URL

---

### Command 4: Run the container with volume mounting (for development)
```powershell
docker run -d `
  -p 8080:8080 `
  --name raceday-app `
  -v ${PWD}:/app `
  raceday-mvc:latest
```

**Explanation:**
- `-v ${PWD}:/app`: Mount current directory into container
- Useful for development with live code changes

---

## Container Management Commands

### View running containers
```powershell
docker ps
```

### View all containers (including stopped ones)
```powershell
docker ps -a
```

### View container logs
```powershell
docker logs raceday-app
```

**Follow logs in real-time:**
```powershell
docker logs -f raceday-app
```

### Stop the container
```powershell
docker stop raceday-app
```

### Start a stopped container
```powershell
docker start raceday-app
```

### Remove the container
```powershell
docker rm raceday-app
```

**Note:** Container must be stopped before removing.

### View container details
```powershell
docker inspect raceday-app
```

---

## Image Management Commands

### List all images
```powershell
docker images
```

### Remove the image
```powershell
docker rmi raceday-mvc:latest
```

**Note:** All containers using this image must be stopped/removed first.

### Tag an image for registry
```powershell
docker tag raceday-mvc:latest myregistry.azurecr.io/raceday-mvc:latest
```

---

## Quick Start Workflow

### 1. First time setup (from project directory):
```powershell
# Navigate to solution directory
cd C:\Users\Admin\Downloads\PROGRAMMING-2B-POE

# Build the image
docker build -t raceday-mvc:latest .

# Run the container
docker run -d -p 8080:8080 --name raceday-app raceday-mvc:latest

# Check if running
docker ps
```

### 2. Access the application:
```
http://localhost:8080
```

### 3. View logs:
```powershell
docker logs -f raceday-app
```

### 4. Update application (after code changes):
```powershell
# Stop the running container
docker stop raceday-app

# Remove the container
docker rm raceday-app

# Rebuild the image
docker build -t raceday-mvc:latest .

# Run the new container
docker run -d -p 8080:8080 --name raceday-app raceday-mvc:latest
```

### 5. Cleanup (remove everything):
```powershell
# Stop and remove container
docker stop raceday-app
docker rm raceday-app

# Remove image
docker rmi raceday-mvc:latest
```

---

## Docker Compose (Optional - for multiple services)

If you have a separate API project, you can use Docker Compose to run both together.

### Create `docker-compose.yml`:
```yaml
version: '3.8'

services:
  raceday-mvc:
	build:
	  context: .
	  dockerfile: Dockerfile
	ports:
	  - "8080:8080"
	environment:
	  - ASPNETCORE_ENVIRONMENT=Production
	  - ApiSettings__BaseUrl=http://raceday-api:5001
	depends_on:
	  - raceday-api
	networks:
	  - raceday-network

  raceday-api:
	build:
	  context: ../RaceDayAPI
	  dockerfile: Dockerfile
	ports:
	  - "5001:5001"
	environment:
	  - ASPNETCORE_ENVIRONMENT=Production
	networks:
	  - raceday-network

networks:
  raceday-network:
	driver: bridge
```

### Run with Docker Compose:
```powershell
docker-compose up -d
```

### Stop with Docker Compose:
```powershell
docker-compose down
```

---

## Troubleshooting

### Port already in use
```powershell
# Find process using port 8080
netstat -ano | findstr :8080

# Kill the process (replace PID with actual process ID)
taskkill /PID <PID> /F
```

**Or use a different port:**
```powershell
docker run -d -p 8888:8080 --name raceday-app raceday-mvc:latest
```

### Container won't start
```powershell
# Check logs
docker logs raceday-app

# Inspect container
docker inspect raceday-app
```

### Remove dangling images
```powershell
docker image prune -a
```

### Rebuild without cache (for clean build)
```powershell
docker build --no-cache -t raceday-mvc:latest .
```

---

## File Structure
```
PROGRAMMING-2B-POE/
├── Dockerfile
├── .dockerignore
├── RaceDayMVC/
│   ├── RaceDayMVC.csproj
│   ├── Program.cs
│   ├── Controllers/
│   ├── Views/
│   ├── Services/
│   └── ...
└── RaceDayAPI/ (optional - separate project)
```

---

## Best Practices

1. **Keep images small**: Use multi-stage builds (already done)
2. **Use .dockerignore**: Exclude unnecessary files (already configured)
3. **Don't run as root**: The ASP.NET Core runtime image handles this
4. **Security**: Use environment variables for sensitive data (API URLs, connection strings)
5. **Health checks**: Included in the Dockerfile
6. **Logging**: Logs are streamed to stdout/stderr

---

## Production Deployment

For Azure Container Registry (ACR):
```powershell
# Login to ACR
az acr login --name <your-registry-name>

# Tag for ACR
docker tag raceday-mvc:latest <your-registry-name>.azurecr.io/raceday-mvc:latest

# Push to ACR
docker push <your-registry-name>.azurecr.io/raceday-mvc:latest
```

---

Generated for RaceDayMVC with .NET 8, ASP.NET Core, and Bootstrap
