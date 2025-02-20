using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingReflectiveCalls;

class UnconstrainedCollectionOfKeyValuePairsGenerator : ExampleValueGeneratorUsingReflectiveCalls
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
		object? firstKey = ExampleValueGeneratorUsingReflectiveCalls.GenerateExampleOf(keyType, firstKeySalt);
		while(firstKey == null)
		{
			firstKeySalt++;
			firstKey = ExampleValueGeneratorUsingReflectiveCalls.GenerateExampleOf(keyType, firstKeySalt);
		}
		collectionOfKeyValuePairsBuilder.Add(firstKey, ExampleValueGeneratorUsingReflectiveCalls.GenerateExampleOf(valueType, salt));

		var secondKeySalt = firstKeySalt + 1;
		var maxSecondKeySalt = salt+5;
		object? secondKey = ExampleValueGeneratorUsingReflectiveCalls.GenerateExampleOf(keyType, secondKeySalt);
		while(secondKey == null || secondKey.Equals(firstKey))
		{
			if (secondKeySalt++ == maxSecondKeySalt) { break; }
			secondKey = ExampleValueGeneratorUsingReflectiveCalls.GenerateExampleOf(keyType, secondKeySalt);
		}
		if (secondKeySalt < maxSecondKeySalt)
		{
			collectionOfKeyValuePairsBuilder.Add(secondKey!, ExampleValueGeneratorUsingReflectiveCalls.GenerateExampleOf(valueType, salt+1));
		}
		var list = collectionOfKeyValuePairsBuilder.BuildAsIList();

		return CollectionConverter.Convert(list, type);
	}
}
