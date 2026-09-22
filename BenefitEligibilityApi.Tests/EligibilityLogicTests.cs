using BenefitEligibilityApi.Models;
using BenefitEligibilityApi.Services;

namespace BenefitEligibilityApi.Tests
{
    public class EligibilityLogicTests
    {

        #region Data Members

        private readonly EligibilityService _service = new EligibilityService();

        #endregion

        [Fact]
        public void CheckEligibility_Employed_LowIncome_ReturnsTrue()
        {
            bool result = _service.CheckEligibility(20000, 3, true);

            Assert.True(result);
        }

        [Fact]
        public void CheckEligibility_Unemployed_HighIncome_ReturnsFalse()
        {
            // Act
            bool result = _service.CheckEligibility(40000, 2, false);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CheckEligibility_AtThreshold_ReturnsTrue()
        {
            // Act
            bool result = _service.CheckEligibility(30000, 1, false);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckEligibility_NoHouseHoldSize_ReturnsFalse()
        {
            // Act
            bool result = _service.CheckEligibility(40000, 0, true);

            // Assert
            Assert.False(result);
        }

    }
}