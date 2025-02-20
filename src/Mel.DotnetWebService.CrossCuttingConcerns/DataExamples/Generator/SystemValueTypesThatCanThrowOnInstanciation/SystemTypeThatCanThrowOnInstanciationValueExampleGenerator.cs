using System.Globalization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public abstract class SystemTypeThatCanThrowOnInstanciationValueExampleGenerator : ExampleValueGenerator
{
	static readonly Dictionary<Type, SystemTypeThatCanThrowOnInstanciationValueExampleGenerator> SystemTypeThatCanThrowOnInstanciationValueExampleGeneratorsByType = new()
	{
		{ typeof(CultureInfo),    CultureInfoExampleValueGenerator.Instance },
		{ typeof(RegionInfo),     RegionInfoExampleValueGenerator.Instance },
		{ typeof(Version),        VersionExampleValueGenerator.Instance },
		{ typeof(Uri),            UriExampleValueGenerator.Instance },
	};

	public static SystemTypeThatCanThrowOnInstanciationValueExampleGenerator InstanceSuitedFor(Type type)
	=> SystemTypeThatCanThrowOnInstanciationValueExampleGeneratorsByType.TryGetValue(type, out var generator)
		? generator
		: throw new InvalidOperationException($"{nameof(SystemTypeThatCanThrowOnInstanciationValueExampleGenerator)}.{nameof(InstanceSuitedFor)}() does not handle data type {type.GetName()}");
}
