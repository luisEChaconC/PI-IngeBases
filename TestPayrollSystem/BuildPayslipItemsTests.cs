using backend.Application.Payslip.Services;
using backend.Domain;
using NUnit.Framework;
using System.Collections.Generic;

namespace backend.Tests.Application.Payslip.Services
{
    public class BuildPayslipItemsTests
    {
        private BuildPayslipItems _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new BuildPayslipItems();
        }

        [Test]
        public void Build_WithVoluntaryAndMandatoryItems_ReturnsCorrectPayslipItems()
        {
            // Arrange
            var deductions = new List<DeductionDetailModel>
            {
                new DeductionDetailModel { Name = "Seguro de salud", AmountDeduced = 1000, DeductionType = "mandatory" },
                new DeductionDetailModel { Name = "Asociación Solidarista", AmountDeduced = 500, DeductionType = "voluntary" }
            };

            // Act
            var result = _builder.Build(deductions);

            // Assert
            Assert.That(result.Count, Is.EqualTo(4));

            // Voluntary item
            Assert.That(result[0].Label, Is.EqualTo("Asociación Solidarista"));
            Assert.That(result[0].Amount, Is.EqualTo(-500));
            Assert.That(result[0].IsBold, Is.False);

            // Voluntary total
            Assert.That(result[1].Label, Is.EqualTo("Total Deducciones Voluntarias"));
            Assert.That(result[1].Amount, Is.EqualTo(-500));
            Assert.That(result[1].IsBold, Is.True);

            // Mandatory item
            Assert.That(result[2].Label, Is.EqualTo("Seguro de salud"));
            Assert.That(result[2].Amount, Is.EqualTo(-1000));
            Assert.That(result[2].IsBold, Is.False);

            // Mandatory total
            Assert.That(result[3].Label, Is.EqualTo("Total Deducciones Obligatorias"));
            Assert.That(result[3].Amount, Is.EqualTo(-1000));
            Assert.That(result[3].IsBold, Is.True);
        }
    }
}
