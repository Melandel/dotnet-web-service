using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypeInvolvingConstrainedTypeReadingOperations;

class GetSetStyleClassReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	public static readonly GetSetStyleClassReadingOperation Instance = new();
	GetSetStyleClassReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		_ = reader.TokenType switch
		{
			JsonTokenType.StartObject => reader.Read(),
			_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartObject} json token, but is being called on {reader.TokenType} json token instead")
		};


		var instanceUnderConstruction = Activator.CreateInstance(targetType);
		var setProperties = targetType.GetProperties().Where(prop => prop.GetSetMethod() != null);

		while (reader.TokenType != JsonTokenType.EndObject)
		{
			var propertyNameFoundInsideJson = reader.TokenType switch
			{
				JsonTokenType.PropertyName => reader.GetString()!,
				_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.PropertyName} json token, but is being called on {reader.TokenType} json token instead")
			};
			reader.Read();

			var matchingProperty = ResolveBestSettablePropertyFor(propertyNameFoundInsideJson, setProperties);
			if (matchingProperty != null)
			{
				var value = For(matchingProperty.PropertyType).Execute(ref reader, matchingProperty.PropertyType, options, preComputedOptionsWithoutConstrainedTypeConverter);
				matchingProperty.SetValue(instanceUnderConstruction, value, null);
			}
		}

		_ = reader.TokenType switch
		{
			JsonTokenType.EndObject => reader.Read(),
			_ => throw new InvalidOperationException($"{GetType().Name} must complete on {JsonTokenType.EndObject} json token but is being completed on {reader.TokenType} json token instead")
		};

		return instanceUnderConstruction;
	}

	PropertyInfo? ResolveBestSettablePropertyFor(string propertyNameFoundInsideJson, IEnumerable<PropertyInfo> settableProperties)
	{
		var propertyCandidateMatchingExactly = settableProperties.FirstOrDefault(settable => string.Equals(settable.Name, propertyNameFoundInsideJson, StringComparison.InvariantCulture));
		if (propertyCandidateMatchingExactly != null)
		{
			return propertyCandidateMatchingExactly;
		}

		var propertyCandidateMatchingCaseInsensitive = settableProperties.FirstOrDefault(settable => string.Equals(settable.Name, propertyNameFoundInsideJson, StringComparison.InvariantCultureIgnoreCase));
		if (propertyCandidateMatchingCaseInsensitive != null)
		{
			return propertyCandidateMatchingCaseInsensitive;
		}

		return null;
	}
}
