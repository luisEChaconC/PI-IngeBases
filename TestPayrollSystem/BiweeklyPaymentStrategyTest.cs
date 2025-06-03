using backend.Domain.Strategies;

namespace TestPayrollSystem
{
    public class BiweeklyPaymentStrategyTest
    {
        private BiweeklyPaymentStrategy _biweeklyPaymentStrategy;

        [SetUp]
        public void Setup()
        {
            _biweeklyPaymentStrategy = new BiweeklyPaymentStrategy();
        }

        [Test]
        public void CalculateGrossPayment_FullBiweek_ReturnsHalfSalary()
        {
            // Arrange
            var startDate = new DateTime(2025, 6, 1);
            var endDate = new DateTime(2025, 6, 15);
            decimal baseSalary = 1500m;

            // Act
            var result = _biweeklyPaymentStrategy.CalculateGrossPayment(startDate, endDate, baseSalary);

            // Assert
            Assert.AreEqual(750.000m, result);
        }

        [Test]
        public void CalculateGrossPayment_OneWeek_ReturnsProportionalAmount()
        {
            // Arrange
            var startDate = new DateTime(2025, 6, 1);
            var endDate = new DateTime(2025, 6, 7);
            decimal baseSalary = 1500m;

            // Act
            var result = _biweeklyPaymentStrategy.CalculateGrossPayment(startDate, endDate, baseSalary);

            // Assert
            Assert.AreEqual(350.000m, result); // (1500 / 2) / 15 * 7 = 750 / 15 * 7 = 50 * 7 = 350
        }

        [Test]
        public void CalculateGrossPayment_SingleDay_ReturnsOneDayAmount()
        {
            // Arrange
            var startDate = new DateTime(2025, 6, 1);
            var endDate = new DateTime(2025, 6, 1);
            decimal baseSalary = 1500m;

            // Act
            var result = _biweeklyPaymentStrategy.CalculateGrossPayment(startDate, endDate, baseSalary);

            // Assert
            Assert.AreEqual(50.000m, result);
        }
    }
}
