using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace backend.Application.DTOs
{
    public class EmployeeHoursPeriodDto
    {
        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "EndDate is required")]
        [DateAfter(nameof(StartDate), ErrorMessage = "EndDate must be after StartDate.")]
        public DateTime EndDate { get; set; }
    }

    public class DateAfterAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;
        
        public DateAfterAttribute(string comparisonProperty)
            : this(comparisonProperty, null)
        {
        }

        public DateAfterAttribute(string comparisonProperty, string? errorMessage)
        {
            _comparisonProperty = comparisonProperty;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not DateTime date)
            {
                string typeErrorMessage = $"{validationContext.MemberName} must be a valid date.";
                return new ValidationResult(typeErrorMessage, new [] { validationContext.MemberName });
            }

            PropertyInfo? comparisonPropertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (comparisonPropertyInfo == null)
            {
                throw new ArgumentException($"Comparison property with name '{_comparisonProperty}' not found on type '{validationContext.ObjectType.Name}'");
            }

            object? comparisonValueObject = comparisonPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (comparisonValueObject is not DateTime comparisonDate)
            {
                string comparisonTypeErrorMessage = $"The comparison date ({comparisonPropertyInfo.Name}) must be a valid date.";
                return new ValidationResult(comparisonTypeErrorMessage, new[] { comparisonPropertyInfo.Name });
            }

            if (date <= comparisonDate)
            {
                string comparisonFailedMessage = ErrorMessage ?? $"{validationContext.MemberName} must be after {comparisonPropertyInfo.Name}";
                return new ValidationResult(comparisonFailedMessage, new[] { validationContext.MemberName });
            }

            return ValidationResult.Success;
        }
    }
}