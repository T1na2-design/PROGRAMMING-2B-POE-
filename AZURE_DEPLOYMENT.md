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

