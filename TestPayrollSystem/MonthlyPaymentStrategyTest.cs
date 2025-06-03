using backend.Domain.Strategies;

namespace TestPayrollSystem
{
    public class MonthlyPaymentStrategyTest
    {
        private MonthlyPaymentStrategy _monthlyPaymentStrategy;

        [SetUp]
        public void Setup()
        {
            _monthlyPaymentStrategy = new MonthlyPaymentStrategy();
        }

        [Test]
        public void CalculateGrossPayment_FullMonth_ReturnsFullBaseSalary()
        {
            // Arrange
            var startDate = new DateTime(2025, 6, 1);
            var endDate = new DateTime(2025, 6, 30);
            decimal baseSalary = 1500m;

            // Act
            var result = _monthlyPaymentStrategy.CalculateGrossPayment(startDate, endDate, baseSalary);

            // Assert
            Assert.AreEqual(1500.000m, result);
        }

        [Test]
        public void CalculateGrossPayment_HalfMonth_ReturnsHalfSalary()
        {
            // Arrange
            var startDate = new DateTime(2025, 6, 1);
            var endDate = new DateTime(2025, 6, 15);
            decimal baseSalary = 1500m;

            // Act
            var result = _monthlyPaymentStrategy.CalculateGrossPayment(startDate, endDate, baseSalary);

            // Assert
            Assert.AreEqual(750.000m, result);
        }

        [Test]
        public void CalculateGrossPayment_SingleDay_ReturnsOneDaySalary()
        {
            // Arrange
            var startDate = new DateTime(2025, 6, 10);
            var endDate = new DateTime(2025, 6, 10);
            decimal baseSalary = 1500m;

            // Act
            var result = _monthlyPaymentStrategy.CalculateGrossPayment(startDate, endDate, baseSalary);

            // Assert
            Assert.AreEqual(50.000m, result);
        }
    }
}
