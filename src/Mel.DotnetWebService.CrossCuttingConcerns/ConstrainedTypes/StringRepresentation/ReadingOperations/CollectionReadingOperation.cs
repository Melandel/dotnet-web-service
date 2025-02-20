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

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		_ = reader.TokenType switch
		{
			JsonTokenType.StartArray => reader.Read(),
			_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must {nameof(Execute)} on {JsonTokenType.StartArray} json token, but is being called on {reader.TokenType} json token instead")
		};

		var itemType = targetType.GetCollectionItemType();
		var listType = typeof(List<>).MakeGenericType(itemType);
		dynamic collection = Activator.CreateInstance(listType)!;

		while (reader.TokenType != JsonTokenType.EndArray)
		{
			var deserialized = ConstrainedTypeConverterReadingOperation
				.For(itemType)
				.Execute(ref reader, itemType, options, preComputedOptionsWithoutConstrainedTypeConverter);

			listType.InvokeMember(
				nameof(IList.Add),
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null,
				collection,
				new object?[] { deserialized });
		}

		_ = reader.TokenType switch
		{
			JsonTokenType.EndArray => reader.Read(),
			_ => throw new InvalidOperationException($"{targetType.FullName} : {GetType().Name} must complete on {JsonTokenType.EndArray} json token, but is being completed on {reader.TokenType} json token instead")
		};

		return Transform(collection, listType, targetType, itemType);
	}

	object? Transform(dynamic collectionAsTypedList, Type currentListType, Type targetType, Type itemType)
	{
		return targetType switch
		{
			{ IsArray: true } => TransformListTypeToArray(collectionAsTypedList, currentListType),
			{ IsInterface: true, Namespace: var ns } when ns != null && ns.StartsWith("System") => collectionAsTypedList,
			{ IsInterface: true } => throw new NotImplementedException($"{nameof(CollectionReadingOperation)}.{nameof(Transform)} does not support type {targetType.FullName}. Supported types inclue arrays, interfaces provided by the System namespace, List<>, HashSet<>, and objects with a single collection parameter in a public constructor or factory method"),
			{ IsGenericType: true } => targetType.GetGenericTypeDefinition() switch
			{
				var t when t == typeof(List<>) => collectionAsTypedList,
				var t when t == typeof(HashSet<>) => TransformListTypeToHashSet(collectionAsTypedList, itemType),
				_ => throw new NotImplementedException($"{nameof(CollectionReadingOperation)}.{nameof(Transform)} does not support type {targetType.FullName}. Supported types inclue arrays, interfaces provided by the System namespace, List<>, HashSet<>, and objects with a single collection parameter in a public constructor or factory method")
			},
			_ => TransformListTypeIntoTargetType(collectionAsTypedList, currentListType, targetType)
		};
	}

	object TransformListTypeToArray(dynamic collectionAsTypedList, Type currentListType)
	=> currentListType.InvokeMember(nameof(List<object>.ToArray), BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, collectionAsTypedList, Array.Empty<object>());

	object TransformListTypeToHashSet(dynamic collectionAsTypedList, Type itemType)
	{
		var hashSetType = typeof(HashSet<>).MakeGenericType(itemType);
		dynamic hashSetInstance = hashSetType
			.GetConstructor(new[] { typeof(IEnumerable<>).MakeGenericType(itemType) })
			!.Invoke(new object[] { collectionAsTypedList });
		return hashSetInstance;
	}
	object TransformListTypeIntoTargetType(dynamic collectionAsTypedList, Type currentListType, Type targetType)
	{
		object array = TransformListTypeToArray(collectionAsTypedList, currentListType);
		return targetType.CreateInstanceUsingConstructorOrFactoryMethod(array, BindingFlags.Public);
	}
}
