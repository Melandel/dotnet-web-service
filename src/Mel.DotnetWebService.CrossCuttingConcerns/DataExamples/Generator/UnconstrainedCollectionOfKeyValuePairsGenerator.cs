using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class UnconstrainedCollectionOfKeyValuePairsGenerator : ExampleValueGenerator
{
	public static readonly UnconstrainedCollectionOfKeyValuePairsGenerator Instance = new();
	UnconstrainedCollectionOfKeyValuePairsGenerator()
	{
	}

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var kvpTypes = type.GetCollectionItemType().GetGenericArguments();
		var keyType = kvpTypes.First();
		var valueType = kvpTypes.Last();

		// var collectionOfKeyValuePairs = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(typeof(KeyValuePair<,>).MakeGenericType(keyType, valueType)))!;
		var collectionOfKeyValuePairsBuilder = GenericListOfKeyValuePairsBuilder.For(keyType, valueType);

		var firstKeySalt = salt;
		object? firstKey = ExampleValueGenerator.GenerateExampleOf(keyType, firstKeySalt);
		while(firstKey == null)
		{
			firstKeySalt++;
			firstKey = ExampleValueGenerator.GenerateExampleOf(keyType, firstKeySalt);
		}
		collectionOfKeyValuePairsBuilder.Add(firstKey, ExampleValueGenerator.GenerateExampleOf(valueType, salt));

		var secondKeySalt = firstKeySalt + 1;
		var maxSecondKeySalt = salt+5;
		object? secondKey = ExampleValueGenerator.GenerateExampleOf(keyType, secondKeySalt);
		while(secondKey == null || secondKey.Equals(firstKey))
		{
			if (secondKeySalt++ == maxSecondKeySalt) { break; }
			secondKey = ExampleValueGenerator.GenerateExampleOf(keyType, secondKeySalt);
		}
		if (secondKeySalt < maxSecondKeySalt)
		{
			collectionOfKeyValuePairsBuilder.Add(secondKey!, ExampleValueGenerator.GenerateExampleOf(valueType, salt+1));
		}
		var list = collectionOfKeyValuePairsBuilder.BuildAsIList();

		return CollectionConverter.Convert(list, type);
	}
}
