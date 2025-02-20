using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ExampleValueGeneratorResolver
{
	public static ExampleValueGenerator Resolve(TypeCategory typeCategory, Type type)
	=> typeCategory switch
	{
		TypeCategory.NativeValueType => NativeValueExampleGenerator.InstanceSuitedFor(type),
		TypeCategory.SystemTypeThatCanThrowOnInstanciation => SystemTypeThatCanThrowOnInstanciationValueExampleGenerator.InstanceSuitedFor(type),
		TypeCategory.EnumType => EnumValueGenerator.Instance,
		TypeCategory.CollectionType => CollectionGenerator.Instance,
		TypeCategory.UnconstrainedCollectionOfKeyValuePairsType => UnconstrainedCollectionOfKeyValuePairsGenerator.Instance,
		TypeCategory.ParentObjectType => ParentObjectGenerator.InstanceSuitedFor(type),
		_ => throw new NotImplementedException()
	};
}
