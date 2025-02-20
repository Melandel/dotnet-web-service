using System.Collections;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class CollectionGenerator : ExampleValueGenerator
{
	public static readonly CollectionGenerator Instance = new();
	CollectionGenerator()
	{
	}

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var itemType = type.GetCollectionItemType();
		var listBuilder = GenericListBuilder.For(itemType);
		var list = GenericListBuilder.For(itemType)
			.Add(ExampleValueGenerator.GenerateExampleOf(itemType, salt  ))
			.Add(ExampleValueGenerator.GenerateExampleOf(itemType, salt+1))
			.BuildAsIList();

		var collectionWithExpectedType = DataValidity.ConstrainedTypes.Serialization.ReadingOperations.TypedListConverters.TypedListConverter
			.InstanceSuitedFor(type)
			.Convert(list, itemType, type);

		return collectionWithExpectedType;
	}
}
