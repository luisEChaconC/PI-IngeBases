using backend.Domain.Strategies;

namespace TestPayrollSystem
{
    public class WeeklyPaymentStrategyTest
    {
        private WeeklyPaymentStrategy _weeklyPaymentStrategy;

        [SetUp]
        public void Setup()
        {
            _weeklyPaymentStrategy = new WeeklyPaymentStrategy();
        }

        [Test]
        public void CalculateGrossPayment_WithValidHoursWorked_ReturnsCorrectAmount()
        {
            // Arrange
            var startDate = new DateTime(2025, 6, 1);
            var endDate = new DateTime(2025, 6, 7);
            decimal baseSalary = 10.5m;
            int hoursWorked = 40;

            // Act
            var result = _weeklyPaymentStrategy.CalculateGrossPayment(startDate, endDate, baseSalary, hoursWorked);

            // Assert
            Assert.AreEqual(420.000m, result);
        }

        [Test]
        public void CalculateGrossPayment_WithoutHoursWorked_ThrowsException()
        {
            // Arrange
            var startDate = new DateTime(2025, 6, 1);
            var endDate = new DateTime(2025, 6, 7);
            decimal baseSalary = 10.5m;

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
                _weeklyPaymentStrategy.CalculateGrossPayment(startDate, endDate, baseSalary, null));

            Assert.That(ex.Message, Is.EqualTo("Hours worked must be provided for weekly employees."));
        }
    }
}