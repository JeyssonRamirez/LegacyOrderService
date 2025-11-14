namespace Application.Implementation.Services
{
    public static class MapperHelper
    {
        /// <summary>
		/// Determines whether a given type is nullable.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>True if the type is nullable; otherwise, false.</returns>
		private static bool IsNullableType(Type type)
        {
            return !type.IsValueType || (Nullable.GetUnderlyingType(type) != null);
        }

        /// <summary>
        /// Determines whether a given value is the default value for its type.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the value is the default value; otherwise, false.</returns>
        private static bool IsDefaultValue(object value)
        {
            if (value == null)
                return true;

            var type = value.GetType();

            // Special case for strings
            if (type == typeof(string))
                return string.IsNullOrEmpty((string)value);

            // For value types, compare to default instance
            if (type.IsValueType)
                return value.Equals(Activator.CreateInstance(type));

            // For reference types (except string), null check is enough
            return false;
        }
        public static T Mapper<T>(object source, T destination)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Source object cannot be null.");
            if (destination == null)
                throw new ArgumentNullException(nameof(destination), "Destination object cannot be null.");

            // Validate that both objects are of the same type or at least share compatible properties
            var sourceType = source.GetType();
            var destinationType = destination.GetType();

            foreach (var sourceProperty in sourceType.GetProperties())
            {
                // Skip properties without a public getter
                if (!sourceProperty.CanRead)
                    continue;

                // Check if the destination object has the same property
                var destinationProperty = destinationType.GetProperty(sourceProperty.Name);
                if (destinationProperty == null)
                    continue;

                // Skip properties without a public setter
                if (!destinationProperty.CanWrite)
                    continue;

                // Ensure the destination property type is compatible with the source property type
                if (!destinationProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                    continue;

                // Skip if the destination property is already set (non-default value)
                var currentDestinationValue = destinationProperty.GetValue(destination);
                if (currentDestinationValue != null && !IsDefaultValue(currentDestinationValue))
                    continue;

                // Map the property value
                var value = sourceProperty.GetValue(source);

                // Validate that the value being set is not null, if the property is non-nullable
                if (value == null && !IsNullableType(destinationProperty.PropertyType))
                    throw new InvalidOperationException(
                        $"Property '{destinationProperty.Name}' cannot be set to null as it is a non-nullable type."
                    );

                destinationProperty.SetValue(destination, value);
            }

            return destination;
        }
    }
}
