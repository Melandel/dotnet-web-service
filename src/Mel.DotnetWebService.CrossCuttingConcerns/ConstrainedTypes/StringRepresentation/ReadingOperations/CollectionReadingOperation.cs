using System.Collections;
using System.Reflection;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;

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
			throw new InvalidOperationException($"{GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartArray} json token, but is being called on {reader.TokenType} json token instead");
		}
		reader.Read();

		var elementType = targetType switch
		{
			{ IsArray: true } => targetType.GetElementType()!,
			{ IsGenericType: true } => targetType.GetGenericArguments().First()!,
			_ => throw new NotImplementedException()
		};
		var typedList = typeof(List<>).MakeGenericType(elementType);
		dynamic collection = Activator.CreateInstance(typedList)!;

		while (reader.TokenType != JsonTokenType.EndArray)
		{
			if (elementType.IsAConstrainedType())
			{ 
				var str = reader.GetString();
				if (elementType == typeof(NonEmptyGuid))
				{
					str = $"\"{str}\"";
				}
				var obj = JsonSerializer.Deserialize(str!, elementType, options);
				typedList.InvokeMember(
					nameof(IList.Add),
					BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
					null,
					collection,
					new object[] { obj });
				reader.Read();
			}
			else if (reader.TokenType == JsonTokenType.StartArray)
			{
				var obj = Execute(ref reader, elementType, options);
				typedList.InvokeMember(
					nameof(IList.Add),
					BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
					null,
					collection,
					new object[] { obj });
			}
			else
			{

				var obj = TypeInvolvingConstrainedTypeReadingOperation.Instance.Execute(ref reader, elementType, options);
				typedList.InvokeMember(
					nameof(IList.Add),
					BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
					null,
					collection,
					new object[] { obj });
			}
		}

		if (reader.TokenType != JsonTokenType.EndArray)
		{
			throw new InvalidOperationException($"{GetType().Name} must complete on {JsonTokenType.EndArray} json token, but is being completed on {reader.TokenType} json token instead");
		}
		reader.Read();

		if (targetType.IsArray)
		{
			var array = typedList.InvokeMember(
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
}
