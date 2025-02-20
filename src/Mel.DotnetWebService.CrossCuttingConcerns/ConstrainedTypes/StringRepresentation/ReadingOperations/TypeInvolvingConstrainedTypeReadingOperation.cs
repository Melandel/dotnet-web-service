using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class TypeInvolvingConstrainedTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly TypeInvolvingConstrainedTypeReadingOperation Instance = new();
	TypeInvolvingConstrainedTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		var hasConstructorAndNoSetterProperty = targetType.DefinesInstanceConstructorsAndHasNoSetterProperty(out var constructors);
		if (!hasConstructorAndNoSetterProperty)
		{
			throw new NotImplementedException($"{targetType.FullName} : {GetType().Name} currently only handles building objects without properties and with a constructor, but is being called on type {targetType.FullName} instead");
		}


		if (targetType.IsASingleCollectionEncapsulation(out var itemType))
		{
			var typedArrayType = itemType!.MakeArrayType();
			var deserializedArray = CollectionReadingOperation.Instance.Execute(ref reader, typedArrayType, options, preComputedOptionsWithoutConstrainedTypeConverter);
			var deserialized = targetType.CreateInstanceUsingConstructorOrFactoryMethod(deserializedArray!, BindingFlags.Public);
			return deserialized;
		}
		else
		{
			_ = reader.TokenType switch
			{
				JsonTokenType.StartObject => reader.Read(),
				_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartObject} json token, but is being called on {reader.TokenType} json token instead")
			};

			var constructorParameters = new List<object?>();
			ConstructorInfo constructorToUse = constructors.First();

			foreach (var param in constructorToUse.GetParameters())
			{
				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					reader.Read();
				}

				var deserialized = ConstrainedTypeConverterReadingOperation
					.For(param.ParameterType)
					.Execute(ref reader, param.ParameterType, options, preComputedOptionsWithoutConstrainedTypeConverter);

				constructorParameters.Add(deserialized);
			}

			_ = reader.TokenType switch
			{
				JsonTokenType.EndObject => reader.Read(),
				_ => throw new InvalidOperationException($"{GetType().Name} must complete on {JsonTokenType.EndObject} json token but is being completed on {reader.TokenType} json token instead")
			};
			return constructorToUse?.Invoke(constructorParameters.ToArray());
		}
	}

	object ReadSystemType(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
	=> type switch
	{
		var t when t == typeof(string) => ReadSystemType<string>(ref reader, type, options),
		var t when t == typeof(Guid) => ReadSystemType<Guid>(ref reader, type, options),
		_ => throw new NotImplementedException($"Type {type.Name} not supported by {nameof(ReadSystemType)}")
	};

	T ReadSystemType<T>(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
	{
		var res = ((JsonConverter<T>)JsonSerializerOptions.Default.GetConverter(type)).Read(ref reader, type, options)!;
		reader.Read();
		return res;
	}
}
