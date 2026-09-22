# Benefit Eligibility API (Proof of Concept)

This project is a proof of concept for creating REST APIs use ASP.NET Core and Azure to demonstrate eligibility logic for state benefit programs (Food Assistance, Unemployment).

## 📁 File Structure
- BenefitEligibilityApi/: Contains the API, Controller, Service, and Model.
- BenefitEligibilityApi.Tests/: Contains xUnit tests for the "BenefitEligibilityApi" project.

## 📊 Tech Stack
- **Backend:** .NET 8, ASP.NET Core
- **Database:** Azure SQL Database, Entity Framework Core
- **Security:** Azure Key Vault, Managed Identities
- **CI/CD:** GitHub Actions (Automated Build & Test)

## 🔒 Security & Configuration
- **Secrets Management:** Sensitive connection strings and credentials are retrieved at runtime from Azure Key Vault.
- **Environment Separation:** Local development uses launchSettings.json (excluded from Git).

*Note: `appsettings.Development.json` and `launchSettings.json` are current excluded from version control to protect local secrets.*

## 🧪 Testing Strategy
- **Unit Testing:** xUnit is used to isolate and test business logic (e.g., eligibility calculation rules) without external dependencies.
- **CI/CD Integration:** Every push triggers an automated pipeline that restores dependencies, builds the solution, and runs all tests.