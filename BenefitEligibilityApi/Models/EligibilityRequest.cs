namespace BenefitEligibilityApi.Models
{
    public class EligibilityRequest
    {
        public string ZipCode { get; set; } = string.Empty;
        public bool IsEmployed { get; set; }
        public decimal AnnualIncome { get; set; }
        public int HouseholdSize { get; set; }
    }
}
