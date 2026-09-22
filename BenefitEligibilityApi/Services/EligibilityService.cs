namespace BenefitEligibilityApi.Services
{
    public class EligibilityService
    {
        #region Constants

        private const int MaxAllowedUnemployedAnnualIncome = 30000;
        private const int MaxAllowedEmployedAnnualIncome = 25000;
        private const int MinAllowedHouseholdSize = 1;

        #endregion

        #region Methods

        // Helps to make code cleaner and more testable.
        public bool CheckEligibility(decimal annualIncome, int householdSize, bool isEmployed)
        {
            bool isEligible;
            if (isEmployed)
            {
                isEligible = annualIncome <= MaxAllowedEmployedAnnualIncome && householdSize >= MinAllowedHouseholdSize;
            }
            else
            {
                isEligible = annualIncome <= MaxAllowedUnemployedAnnualIncome && householdSize >= MinAllowedHouseholdSize;
            }
            return isEligible;
        }

        #endregion
    }
}
