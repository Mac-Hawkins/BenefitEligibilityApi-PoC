# Stage 1: Build
# Uses the .NET SDK image to compile the code
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the SOLUTION file first
COPY ["BenefitEligibilityApi.sln", "./"]

# Copy the solution file and restore dependencies first (caches this layer)
COPY ["BenefitEligibilityApi/BenefitEligibilityApi.csproj", "BenefitEligibilityApi/"]
COPY ["BenefitEligibilityApi.Tests/BenefitEligibilityApi.Tests.csproj", "BenefitEligibilityApi.Tests/"]

# Restore dependencies using the solution file
RUN dotnet restore "BenefitEligibilityApi.sln"

# Copy the rest of the source code
COPY . .

# Build the project in Release mode
WORKDIR "/src/BenefitEligibilityApi"
RUN dotnet build "BenefitEligibilityApi.csproj" -c Release -o /app/build

# Stage 2: Publish
# Publishes the app to a folder for the final image
FROM build AS publish
RUN dotnet publish "BenefitEligibilityApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
# Uses the smaller ASP.NET runtime image (no compiler needed)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published output from the 'publish' stage
COPY --from=publish /app/publish .

# Expose the port the app runs on (default ASP.NET port)
EXPOSE 8080

# Set the environment variable for the URL
ENV ASPNETCORE_URLS=http://+:8080

# Run the application
ENTRYPOINT ["dotnet", "BenefitEligibilityApi.dll"]