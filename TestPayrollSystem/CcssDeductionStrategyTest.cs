using backend.Domain.Strategies;
using NUnit.Framework;

namespace TestPayrollSystem
{
    [TestFixture]
    public class CcssDeductionStrategyTest
    {
        private CcssDeductionStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _strategy = new CcssDeductionStrategy();
        }

        [Test]
        public void CalculateDeduction_ProfessionalServices_ReturnsZero()
        {
            var result = _strategy.CalculateDeduction(1_000_000m, "Professional Services", "F");
            Assert.AreEqual(0m, result);
        }
        [Test]
        public void CalculateDeduction_AboveMinimumBase_UsesGrossSalary()
        {
            var gross = 1_000_000m;
            var expected = Math.Round(gross * 0.0550m + gross * 0.0417m, 2);
            var result = _strategy.CalculateDeduction(gross, "Permanent", "F");
            Assert.AreEqual(expected, result);
        }
         [Test]
        public void CalculateDeduction_CorrectForSalary3910000()
        {
            
            decimal grossSalary = 3_910_000m;
            string contractType = "Permanent";
            string gender = "F";

            decimal expectedTotal = 378_097m;

            var result = _strategy.CalculateDeduction(grossSalary, contractType, gender);

            Assert.AreEqual(expectedTotal, result);
        }
    }
}

