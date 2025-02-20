namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingReflectiveCalls;

class ExampleValueGeneratorUsingReflectiveCallsResolver
{
	public static ExampleValueGeneratorUsingReflectiveCalls Resolve(TypeCategory typeCategory, Type type)
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
