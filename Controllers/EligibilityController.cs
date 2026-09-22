using BenefitEligibilityApi.Data;
using BenefitEligibilityApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BenefitEligibilityApi.Controllers
{
    // [controller] will be "Eligibility" as it comes before "Controller" in the class name.
    [Route("api/[controller]")]
    [ApiController]
    public class EligibilityController : ControllerBase
    {

        #region Constants

        private const int MaxAllowedUnemployedAnnualIncome = 30000;
        private const int MaxAllowedEmployedAnnualIncome = 25000;
        private const int MinAllowedHouseholdSize = 1;

        #endregion

        #region Data Members

        private readonly ILogger<EligibilityController> _logger;
        private readonly AppDbContext _context;

        #endregion

        #region Constructor

        public EligibilityController(ILogger<EligibilityController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        #endregion

        #region Endpoints

        // Route for this would be api/Eligibility/check
        [HttpPost("check")]
        public async Task<IActionResult> CheckEligibility([FromBody] EligibilityRequest request)
        {
            // Log the request
            _logger.LogInformation($"Checking eligibility for benefits. Annual income: {request.AnnualIncome}, Household size: {request.HouseholdSize}");

            // Determine elibility based on employment status, income, and household size.
            bool isEligible;
            if (request.IsEmployed)
            {
                isEligible = request.AnnualIncome <= MaxAllowedEmployedAnnualIncome && request.HouseholdSize >= MinAllowedHouseholdSize;
            }
            else
            {
                isEligible = request.AnnualIncome <= MaxAllowedUnemployedAnnualIncome && request.HouseholdSize >= MinAllowedHouseholdSize;
            }

            // Form response.
            var response = new
            {
                IsEligible = isEligible,
                Message = isEligible ? "Is eligible for food assistance." : "Is ineligible for food assistance.",
                Timestamp = DateTime.UtcNow
            };

            // Creates a BenefitApplication to store in the DB based on the EligibilityRequest.
            var application = new BenefitApplication
            {
                AnnualIncome = request.AnnualIncome,
                HouseholdSize = request.HouseholdSize,
                IsEmployed = request.IsEmployed,
                IsEligible = isEligible,
                SubmittedAt = DateTime.UtcNow
            };

            // Actually saves the data to the DB.
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return Ok(new { IsEligible = isEligible, Message = "...", ApplicationId = application.Id });
        }

        #endregion

    }
}
