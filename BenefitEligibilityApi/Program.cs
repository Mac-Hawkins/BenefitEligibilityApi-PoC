using BenefitEligibilityApi.Data;
using BenefitEligibilityApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------------------
// REGISTER CORE SERVICES
// -----------------------------------------------------------------------------
// Tells ASP.NET Core to handle incoming HTTP requests and route them to Controllers.
builder.Services.AddControllers();

// Enables Swagger UI (the web interface I use to test APIs in development).
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------------------------------------
// SECURE CONFIGURATION
// Commenting out for now because it's causing an issue with Azure container.
// -----------------------------------------------------------------------------
// Retrieve the Azure Key Vault URI configuration value from launchSettings.json
//var keyVaultUri = builder.Configuration["KeyVaultUri"];

//// If we successfully retrieved the URI...
//if (!string.IsNullOrEmpty(keyVaultUri))
//{
//    // Connect to Azure Key Vault.
//    builder.Configuration.AddAzureKeyVault(
//        new Uri(keyVaultUri),
//        new DefaultAzureCredential());

//    Console.WriteLine("Connected to Azure Key Vault!");
//}
//else
//{
//    Console.WriteLine("KeyVaultUri not found. Using local settings.");
//}

// -----------------------------------------------------------------------------
// DATABASE CONNECTION
// -----------------------------------------------------------------------------

// NOTE: In production, I would get the connection string for the Azure SQL DB from the Azure Key Vault.
// For this PoC, I am reading directly from Azure Application Settings for simplicity as it was causing issues with the container.
//var connectionStringValue = builder.Configuration["connection-string-asp-benefits-db-sql-auth-1"];

// Get the connection string from Azure Environmental Variables.
var connectionStringValue = builder.Configuration.GetConnectionString("DefaultConnection");

// Configures Entity Framework to use SQL Server and specifies options on how to retry.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionStringValue, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

// Register my service so it will be passed into the constructor of the controller through DI.
builder.Services.AddScoped<EligibilityService>();


// -----------------------------------------------------------------------------
// BUILD THE APPLICATION & INITIALIZE DB
// -----------------------------------------------------------------------------
var app = builder.Build();

// Create DB tables if they don't exist.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.EnsureCreated();
        Console.WriteLine("Database tables created successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating database: {ex.Message}");
    }
}

// -----------------------------------------------------------------------------
// REQUEST PIPELINE
// -----------------------------------------------------------------------------
// Enable Swagger UI.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection(); // Force HTTPS for security.
app.UseAuthorization();  // Enable authentication/authorization middleware.

// Scans all my controllers for methods decorated with [HttpGet], [HttpPost], etc.,
// and creates a map so that when a specific URL is hit, it runs that method.
app.MapControllers();

app.Run(); // Start the web server and listen for requests.
