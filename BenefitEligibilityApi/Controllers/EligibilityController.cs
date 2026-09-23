using BenefitEligibilityApi.Data;
using BenefitEligibilityApi.Models;
using BenefitEligibilityApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BenefitEligibilityApi.Controllers
{
    // [controller] will be "Eligibility" as it comes before "Controller" in the class name.
    [Route("api/[controller]")]
    [ApiController]
    public class EligibilityController : ControllerBase
    {

        #region Data Members

        private readonly ILogger<EligibilityController> _logger;
        private readonly AppDbContext _context;
        private readonly EligibilityService _eligibilityService;

        #endregion

        #region Constructor

        public EligibilityController(ILogger<EligibilityController> logger, AppDbContext context, EligibilityService eligibilityService)
        {
            _logger = logger;
            _context = context;
            _eligibilityService = eligibilityService;
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
            bool isEligible = _eligibilityService.CheckEligibility(request.AnnualIncome, request.HouseholdSize, request.IsEmployed);

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

            return Ok(new { IsEligible = isEligible, response.Message, ApplicationId = application.Id });
        }

        #endregion

    }
}
