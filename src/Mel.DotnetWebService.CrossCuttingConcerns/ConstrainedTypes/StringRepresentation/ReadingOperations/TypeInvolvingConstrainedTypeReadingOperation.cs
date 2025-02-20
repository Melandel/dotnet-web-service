using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class TypeInvolvingConstrainedTypeReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly TypeInvolvingConstrainedTypeReadingOperation Instance = new();
	TypeInvolvingConstrainedTypeReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options)
	{
		var hasConstructorAndNoSetterProperty = targetType.DefinesInstanceConstructorsAndHasNoSetterProperty(out var constructors);
		if (!hasConstructorAndNoSetterProperty)
		{
			throw new NotImplementedException($"{GetType().Name} currently only handles building objects without properties and with a constructor, but is being called on type {targetType.FullName} instead");
		}

		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new InvalidOperationException($"{GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartObject} json token, but is being called on {reader.TokenType} json token instead");
		}
		reader.Read();

		List<object> constructorParameters = new List<object>();
		ConstructorInfo constructorToUse = constructors.First();

		foreach (var param in constructorToUse.GetParameters())
		{
			if (reader.TokenType == JsonTokenType.PropertyName)
			{
				reader.Read();
			}

			if (param.ParameterType.IsAConstrainedType())
			{
				var str = reader.GetString();
				if (param.ParameterType == typeof(string) || param.ParameterType == typeof(NonEmptyGuid))
				{
					str = $"\"{str}\"";
				}
				var obj = JsonSerializer.Deserialize(str!, param.ParameterType, options);
				constructorParameters.Add(obj);
				reader.Read();
			}
			else if (param.ParameterType == typeof(string))
			{
				var obj = reader.GetString();
				constructorParameters.Add(obj);
				reader.Read();
			}
			else if (param.ParameterType == typeof(Guid))
			{
				var obj = reader.GetString();
				constructorParameters.Add(new Guid(obj));
				reader.Read();
			}
			else if (reader.TokenType == JsonTokenType.StartArray)
			{
				var obj = CollectionReadingOperation.Instance.Execute(ref reader, param.ParameterType, options);
				constructorParameters.Add(obj);
			}
			else
			{
				var obj = Execute(ref reader, param.ParameterType, options);
				constructorParameters.Add(obj);
			}

		}

		if (reader.TokenType != JsonTokenType.EndObject)
		{
			throw new InvalidOperationException($"{GetType().Name} must complete on {JsonTokenType.EndObject} json token but is being completed on {reader.TokenType} json token instead");
		}
		reader.Read();

		return constructorToUse?.Invoke(constructorParameters.ToArray());

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
