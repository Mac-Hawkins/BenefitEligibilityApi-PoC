namespace BenefitEligibilityApi.Data
{
    public class BenefitApplication
    {
        public int Id { get; set; }
        public decimal AnnualIncome { get; set; }
        public int HouseholdSize { get; set; }
        public bool IsEmployed { get; set; }
        public bool IsEligible { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}