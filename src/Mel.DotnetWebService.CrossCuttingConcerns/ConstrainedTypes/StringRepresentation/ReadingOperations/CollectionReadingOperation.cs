using System.Collections;
using System.Reflection;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class CollectionReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly CollectionReadingOperation Instance = new();
	CollectionReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.StartArray)
		{
			throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartArray} json token, but is being called on {reader.TokenType} json token instead");
		}
		reader.Read();

		var elementType = targetType switch
		{
			{ IsArray: true } => targetType.GetElementType()!,
			{ IsGenericType: true } => targetType.GetGenericArguments().First()!,
			_ => throw new NotImplementedException()
		};
		var listType = typeof(List<>).MakeGenericType(elementType);
		dynamic collection = Activator.CreateInstance(listType)!;

		while (reader.TokenType != JsonTokenType.EndArray)
		{
			var deserialized = DeserializeCurrentElement(ref reader, elementType, options);
			listType.InvokeMember(
				nameof(IList.Add),
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null,
				collection,
				new object?[] { deserialized });
		}

		if (reader.TokenType != JsonTokenType.EndArray)
		{
			throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must complete on {JsonTokenType.EndArray} json token, but is being completed on {reader.TokenType} json token instead");
		}
		reader.Read();

		return Transform(collection, listType, targetType);
	}

	object? Transform(dynamic collection, Type currentListType, Type targetType)
	{
		if (targetType.IsArray)
		{
			var array = currentListType.InvokeMember(
				nameof(List<object>.ToArray),
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null,
				collection,
				Array.Empty<object>());
			return array;
		}

		if (targetType.IsGenericType)
		{
			if (targetType.GetGenericTypeDefinition() == typeof(List<>))
			{
				return collection;
			}
		}
		return collection;
	}

	object? DeserializeCurrentElement(ref Utf8JsonReader reader, Type elementType, JsonSerializerOptions options)
	{
		if (elementType.ImplementsGenericInterface(typeof(ICanBeDeserializedFromJson<,>), out var argumentTypes))
		{
			var expectedType = argumentTypes.First();
			var deserialized = reader.GetNativelySupportedValue(expectedType) switch
			{
				null => throw new InvalidOperationException($"{elementType.FullName} expected to be deserialized from a {expectedType.FullName}, but serialized data does not match the latter type."),
				var value => elementType.ReconstituteFromValueFoundInsideJson(value)
			};
			reader.Read();
			return deserialized;
		}

		if (reader.TokenType == JsonTokenType.StartArray)
		{
			var deserialized = Execute(ref reader, elementType, options);
			return deserialized;
		}

		if (reader.TokenType == JsonTokenType.StartObject)
		{
			var deserialized = TypeInvolvingConstrainedTypeReadingOperation.Instance.Execute(ref reader, elementType, options);
			return deserialized;
		}

		throw new NotImplementedException();
	}
}
