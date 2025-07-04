using backend.Domain.Strategies;
using NUnit.Framework;

namespace TestPayrollSystem
{
    [TestFixture]
    public class IncomeTaxDeductionStrategyTest
    {
        private IncomeTaxDeductionStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _strategy = new IncomeTaxDeductionStrategy();
        }

        [Test]
        public void CalculateDeduction_Between922kAnd1352k_Applies10Percent()
        {
            var salary = 1_000_000m;
            var expected = (salary - 922_000m) * 0.10m;
            var result = _strategy.CalculateDeduction(salary, "Permanent", "F");
            Assert.AreEqual(Math.Round(expected, 2), result);
        }
        [Test]
        public void CalculateDeduction_ForSalary1700000_Returns95200()
        {
            
            decimal salary = 1_700_000m;
            string contractType = "Permanent";
            string gender = "F";

            decimal expected = 95_200m;

            
            var result = _strategy.CalculateDeduction(salary, contractType, gender);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
