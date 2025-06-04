using backend.Domain;
using backend.Domain.Strategies;
using NUnit.Framework;
using System;

namespace TestPayrollSystem
{
    [TestFixture]
    public class DeductionStrategyTest
    {
        private BenefitDeductionStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _strategy = new BenefitDeductionStrategy();
        }

        [Test]
        public void CalculateDeduction_BenefitIsNull_ReturnsZero()
        {
            var result = _strategy.CalculateDeduction(1000m, "FullTime", "M", null);
            Assert.AreEqual(0m, result);
        }

        [Test]
        public void CalculateDeduction_TypeIsFixedAmount_ReturnsFixedAmount()
        {
            var benefit = new Benefit
            {
                Type = "FixedAmount",
                FixedAmount = 150
            };

            var result = _strategy.CalculateDeduction(2000m, "FullTime", "F", benefit);
            Assert.AreEqual(150m, result);
        }

        [Test]
        public void CalculateDeduction_TypeIsFixedAmount_NullAmount_ReturnsZero()
        {
            var benefit = new Benefit
            {
                Type = "FixedAmount",
                FixedAmount = null
            };

            var result = _strategy.CalculateDeduction(2000m, "FullTime", "F", benefit);
            Assert.AreEqual(0m, result);
        }

        [Test]
        public void CalculateDeduction_TypeIsFixedPercentage_ReturnsCorrectPercentage()
        {
            var benefit = new Benefit
            {
                Type = "FixedPercentage",
                FixedPercentage = 5 // 5%
            };

            var grossSalary = 1000m;
            var expected = Math.Round(grossSalary * 0.05m, 2);

            var result = _strategy.CalculateDeduction(grossSalary, "Contractor", "M", benefit);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CalculateDeduction_TypeIsFixedPercentage_NullPercentage_ReturnsZero()
        {
            var benefit = new Benefit
            {
                Type = "FixedPercentage",
                FixedPercentage = null
            };

            var result = _strategy.CalculateDeduction(2000m, "FullTime", "F", benefit);
            Assert.AreEqual(0m, result);
        }

        [Test]
        public void CalculateDeduction_UnknownType_ReturnsZero()
        {
            var benefit = new Benefit
            {
                Type = "OtherUnknownType"
            };

            var result = _strategy.CalculateDeduction(2000m, "FullTime", "M", benefit);
            Assert.AreEqual(0m, result);
        }
    }
}
