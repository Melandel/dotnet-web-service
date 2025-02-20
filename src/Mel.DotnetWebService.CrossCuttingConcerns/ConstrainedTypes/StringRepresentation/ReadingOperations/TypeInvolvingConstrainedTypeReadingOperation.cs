using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;

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
			throw new NotImplementedException($"{targetType.FullName} : {GetType().Name} currently only handles building objects without properties and with a constructor, but is being called on type {targetType.FullName} instead");
		}

		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartObject} json token, but is being called on {reader.TokenType} json token instead");
		}
		reader.Read();

		var constructorParameters = new List<object?>();
		ConstructorInfo constructorToUse = constructors.First();

		foreach (var param in constructorToUse.GetParameters())
		{
			if (reader.TokenType == JsonTokenType.PropertyName)
			{
				reader.Read();
			}

			if (param.ParameterType.IsAConstrainedType(out var rootType))
			{
				var valueFoundInsideJson = reader.GetNativelySupportedValue(rootType!);
				var obj = param.ParameterType.ReconstituteFromValueFoundInsideJson(valueFoundInsideJson);
				reader.Read();
				constructorParameters.Add(obj);
			}
			else if (reader.TokenType == JsonTokenType.StartArray)
			{
				var obj = CollectionReadingOperation.Instance.Execute(ref reader, param.ParameterType, options);
				constructorParameters.Add(obj);
			}
			else if (reader.NativelySupports(param.ParameterType))
			{
				var obj = reader.GetNativelySupportedValue(param.ParameterType);
				reader.Read();
				constructorParameters.Add(obj);
			}

			//var deserialized = param.ParameterType switch
			//	{
			//		var t when t == typeof(string) => reader.GetString(),
			//		var t when t == typeof(bool) => reader.GetBoolean(),
			//		var t when t == typeof(byte) => reader.GetByte(),
			//		var t when t == typeof(byte[]) => reader.GetBytesFromBase64(),
			//		var t when t == typeof(DateTime) => reader.GetDateTime(),
			//		var t when t == typeof(DateTimeOffset) => reader.GetDateTimeOffset(),
			//		var t when t == typeof(decimal) => reader.GetDecimal(),
			//		var t when t == typeof(double) => reader.GetDouble(),
			//		var t when t == typeof(Guid) => reader.GetGuid(),
			//		var t when t == typeof(short) => reader.GetInt16(),
			//		var t when t == typeof(int) => reader.GetInt32(),
			//		var t when t == typeof(long) => reader.GetInt64(),
			//		var t when t == typeof(sbyte) => reader.GetSByte(),
			//		var t when t == typeof(float) => reader.GetSingle(),
			//		var t when t == typeof(ushort) => reader.GetUInt16(),
			//		var t when t == typeof(uint) => reader.GetUInt32(),
			//		var t when t == typeof(ulong) => reader.GetUInt64(),
			//		var t => Execute(ref reader, t, options)
			//	};
			//	constructorParameters.Add(deserialized);

			//}


			//else if (param.ParameterType == typeof(string))
			//{
			//	var obj = reader.GetString();
			//	constructorParameters.Add(obj);
			//	reader.Read();
			//}
			//else if (param.ParameterType == typeof(Guid))
			//{
			//	var obj = reader.GetString()!;
			//	constructorParameters.Add(new Guid(obj));
			//	reader.Read();
			//}
			//else if (reader.TokenType == JsonTokenType.StartArray)
			//{
			//	var obj = CollectionReadingOperation.Instance.Execute(ref reader, param.ParameterType, options);
			//	constructorParameters.Add(obj);
			//}
			//else if (param.ParameterType != null)
			//{
			//	var valueFoundInsideJson = reader.GetValue(param.ParameterType);
			//	constructorParameters.Add(valueFoundInsideJson);
			//}
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
