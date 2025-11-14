using System.ComponentModel.DataAnnotations;

namespace Crosscutting.Util.CustomValidators
{
    public class ConditionalValidationAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;
        private readonly List<object> _expectedValues;

        public ConditionalValidationAttribute(string otherPropertyName, params object[] expectedValues)
        {
            _otherPropertyName = otherPropertyName;
            _expectedValues = expectedValues.ToList();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the other property value
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_otherPropertyName}");
            }

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            // Check if the other property value matches any value in the expected values list
            if (_expectedValues.Contains(otherPropertyValue))
            {
                // Apply validation logic for the current property
                if (value == null || value is string strValue && string.IsNullOrWhiteSpace(strValue))
                {
                    return new ValidationResult($"The {validationContext.DisplayName} field is required when {_otherPropertyName} has one of the specified values.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
