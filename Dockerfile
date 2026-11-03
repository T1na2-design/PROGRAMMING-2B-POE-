# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file
COPY ["RaceDayMVC/RaceDayMVC.csproj", "RaceDayMVC/"]

# Restore dependencies
RUN dotnet restore "RaceDayMVC/RaceDayMVC.csproj"

# Copy the entire project
COPY ["RaceDayMVC/", "RaceDayMVC/"]

# Build the application
WORKDIR "/src/RaceDayMVC"
RUN dotnet build "RaceDayMVC.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "RaceDayMVC.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published application from publish stage
COPY --from=publish /app/publish .

# Expose port 8080
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
	CMD curl -f http://localhost:8080/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "RaceDayMVC.dll"]
