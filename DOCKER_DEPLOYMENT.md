# RaceDayMVC Docker Deployment Guide

## Step 1: Install Docker Desktop

**Windows:**
1. Download Docker Desktop from: https://www.docker.com/products/docker-desktop
2. Run the installer and follow the setup wizard
3. Restart your computer when prompted
4. Verify installation by opening PowerShell and running:
   ```powershell
   docker --version
   ```

**Expected Output:**
```
Docker version 20.10.x, build xxxxxxx
```

---

## Step 2: Build the Docker Image

Once Docker is installed, navigate to your project directory and build the image:

```powershell
# Navigate to project directory
cd C:\Users\Admin\Downloads\PROGRAMMING-2B-POE

# Build the Docker image
docker build -t raceday-mvc:latest .
```

**What this does:**
- Builds a Docker image from the Dockerfile
- Names it `raceday-mvc` with tag `latest`
- Multi-stage build: optimizes image size by discarding build artifacts

**Expected Output:**
```
[+] Building 90.5s (12/12) FINISHED
 => [internal] load build definition from Dockerfile
 => [stage-1 1/7] FROM mcr.microsoft.com/dotnet/sdk:8.0
 => [stage-2 2/3] COPY --from=publish /app/publish
 => => naming to docker.io/library/raceday-mvc:latest
```

**Build Time:** ~2-5 minutes (first build takes longer due to downloading base images)

---

## Step 3: Run the Docker Container

### Option A: Basic Run (Recommended for first-time)

```powershell
docker run -d -p 8080:8080 --name raceday-app raceday-mvc:latest
```

**Explanation:**
- `-d`: Run in detached mode (background)
- `-p 8080:8080`: Map port 8080 on your computer to port 8080 in container
- `--name raceday-app`: Give the container a friendly name

**Access the application:**
```
http://localhost:8080
```

**Check if running:**
```powershell
docker ps
```

**Expected Output:**
```
CONTAINER ID   IMAGE                 COMMAND                  CREATED       STATUS                  PORTS                  NAMES
abc123def456   raceday-mvc:latest    "dotnet RaceDayMVC.d…"   1 minute ago  Up 1 minute (healthy)  0.0.0.0:8080->8080/tcp  raceday-app
```

---

### Option B: Run with Custom API URL

If you have the RaceDayAPI running on a different server:

```powershell
docker run -d `
  -p 8080:8080 `
  --name raceday-app `
  -e "ApiSettings__BaseUrl=https://your-api-url.com" `
  -e "ASPNETCORE_ENVIRONMENT=Production" `
  raceday-mvc:latest
```

**Replace:**
- `https://your-api-url.com` with your actual API base URL
- Example: `https://api.raceday.com` or `https://192.168.1.100:5001`

---

### Option C: Run with Volume Mounting (Development)

For development with live code reload:

```powershell
docker run -d `
  -p 8080:8080 `
  --name raceday-app `
  -v C:\Users\Admin\Downloads\PROGRAMMING-2B-POE:/app `
  raceday-mvc:latest
```

**Note:** Volume mounting requires Docker for Windows with WSL 2 backend.

---

## Step 4: Verify the Application

### Check Container Logs

```powershell
# View logs
docker logs raceday-app

# Follow logs in real-time (Ctrl+C to stop)
docker logs -f raceday-app
```

**Expected Log Output:**
```
info: Microsoft.Hosting.Lifetime[14]
	  Now listening on: http://[::]:8080
info: Microsoft.Hosting.Lifetime[0]
	  Application started. Press Ctrl+C to shut down.
```

### Health Check Status

```powershell
docker inspect --format='{{.State.Health.Status}}' raceday-app
```

**Expected Output:**
```
healthy
```

### Open in Browser

- Navigate to: `http://localhost:8080`
- You should see the RaceDay application homepage

---

## Step 5: Container Management

### Stop the Container

```powershell
docker stop raceday-app
```

### Start the Container Again

```powershell
docker start raceday-app
```

### View Container Details

```powershell
docker inspect raceday-app
```

### Remove Container (after stopping)

```powershell
docker rm raceday-app
```

### View All Running Containers

```powershell
docker ps
```

### View All Containers (including stopped)

```powershell
docker ps -a
```

---

## Step 6: Rebuild After Code Changes

After making changes to your code:

```powershell
# Stop the running container
docker stop raceday-app

# Remove the container
docker rm raceday-app

# Rebuild the image (for clean build, use --no-cache)
docker build -t raceday-mvc:latest .

# Run the new container
docker run -d -p 8080:8080 --name raceday-app raceday-mvc:latest
```

**Or in one command:**
```powershell
docker stop raceday-app; docker rm raceday-app; docker build -t raceday-mvc:latest .; docker run -d -p 8080:8080 --name raceday-app raceday-mvc:latest
```

---

## Troubleshooting

### Port 8080 Already in Use

```powershell
# Find what's using port 8080
netstat -ano | findstr :8080

# Kill the process (replace PID with actual ID)
taskkill /PID <PID> /F

# Or use a different port
docker run -d -p 8888:8080 --name raceday-app raceday-mvc:latest
# Then access at: http://localhost:8888
```

### Container Won't Start

```powershell
# Check logs for errors
docker logs raceday-app

# View detailed container information
docker inspect raceday-app

# Try removing and rebuilding
docker rm raceday-app
docker build --no-cache -t raceday-mvc:latest .
docker run -d -p 8080:8080 --name raceday-app raceday-mvc:latest
```

### "Docker daemon is not running"

- Open Docker Desktop from your Start Menu
- Wait for Docker to fully start (watch the taskbar icon)
- Try your docker command again

### Remove All Docker Images and Containers

```powershell
# Stop all running containers
docker stop $(docker ps -q)

# Remove all containers
docker rm $(docker ps -aq)

# Remove all images
docker rmi $(docker images -q)
```

---

## Environment Variables (Production)

When deploying to production, set these environment variables:

```powershell
docker run -d `
  -p 8080:8080 `
  --name raceday-app `
  -e "ApiSettings__BaseUrl=https://production-api.com" `
  -e "ASPNETCORE_ENVIRONMENT=Production" `
  raceday-mvc:latest
```

**Available Variables:**
- `ApiSettings__BaseUrl`: The API endpoint URL
- `ASPNETCORE_ENVIRONMENT`: Set to `Production` for production deployments
- `ASPNETCORE_URLS`: Default is `http://+:8080` (usually no need to change)

---

## Docker Compose (Multiple Services)

If you have both RaceDayMVC and RaceDayAPI, you can use Docker Compose:

**Create `docker-compose.yml` in your workspace root:**

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

**Run with Docker Compose:**

```powershell
docker-compose up -d
```

**Stop with Docker Compose:**

```powershell
docker-compose down
```

---

## Production Deployment to Azure Container Registry

### Step 1: Create Azure Container Registry

```powershell
az acr create --resource-group myResourceGroup --name myregistry --sku Basic
```

### Step 2: Login to ACR

```powershell
az acr login --name myregistry
```

### Step 3: Tag Image for ACR

```powershell
docker tag raceday-mvc:latest myregistry.azurecr.io/raceday-mvc:latest
```

### Step 4: Push to ACR

```powershell
docker push myregistry.azurecr.io/raceday-mvc:latest
```

### Step 5: Deploy to Azure Container Instances

```powershell
az container create `
  --resource-group myResourceGroup `
  --name raceday-app `
  --image myregistry.azurecr.io/raceday-mvc:latest `
  --ports 8080 `
  --dns-name-label raceday-app `
  --registry-login-server myregistry.azurecr.io `
  --registry-username myregistry `
  --registry-password <password>
```

---

## Quick Reference

| Command | Purpose |
|---------|---------|
| `docker build -t raceday-mvc:latest .` | Build image |
| `docker run -d -p 8080:8080 --name raceday-app raceday-mvc:latest` | Run container |
| `docker ps` | List running containers |
| `docker logs raceday-app` | View logs |
| `docker logs -f raceday-app` | Follow logs live |
| `docker stop raceday-app` | Stop container |
| `docker start raceday-app` | Start container |
| `docker rm raceday-app` | Remove container |
| `docker images` | List images |
| `docker rmi raceday-mvc:latest` | Remove image |

---

## Next Steps After Deployment

1. ✅ Test the application at `http://localhost:8080`
2. ✅ Verify registration error messages display correctly
3. ✅ Test all major features (Events, Enrolment, Results)
4. ✅ Check Docker logs for any errors
5. Push changes to GitHub when satisfied
6. Deploy to production environment (Azure, AWS, etc.)

---

**Last Updated:** Generated for RaceDayMVC with .NET 8, ASP.NET Core, and Docker

