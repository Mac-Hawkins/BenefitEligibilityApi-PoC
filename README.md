# Benefit Eligibility API (Proof of Concept)

This project is a proof of concept for creating REST APIs use ASP.NET Core and Azure to demonstrate eligibility logic for state benefit programs (Food Assistance, Unemployment).

## 📊 Tech Stack
- **Backend:** .NET 8, ASP.NET Core
- **Database:** Azure SQL, Entity Framework Core
- **Security:** Azure Key Vault, Managed Identities

## 🔒 Security & Configuration

This project uses **Azure Key Vault** to store sensitive connection strings. Below are the steps I took.

1.  Created an Azure Key Vault and add a secret with my SQL connection string.
2.  Added myself to the Key Vault's **IAM** with the **Key Vault Secrets Adminstrator** and **Key Vault Secrets User** role.
3.  Set the environment variable `KeyVaultUri` in `Properties/launchSettings.json` to my vault's URI.

*Note: `appsettings.Development.json` and `launchSettings.json` are current excluded from version control to protect local secrets.*
