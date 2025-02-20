using System.Globalization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public abstract class NativeValueExampleGenerator : ExampleValueGenerator
{
	static readonly Dictionary<Type, NativeValueExampleGenerator> NativeValueExampleGeneratorsByType = new()
	{
		{ typeof(bool),           BoolExampleValueGenerator.Instance },
		{ typeof(byte),           ByteExampleValueGenerator.Instance },
		{ typeof(DateTime),       DateTimeExampleValueGenerator.Instance },
		{ typeof(DateTimeOffset), DateTimeOffsetExampleValueGenerator.Instance },
		{ typeof(decimal),        DecimalExampleValueGenerator.Instance },
		{ typeof(double),         DoubleExampleValueGenerator.Instance },
		{ typeof(float),          FloatExampleValueGenerator.Instance },
		{ typeof(Guid),           GuidExampleValueGenerator.Instance },
		{ typeof(int),            IntExampleValueGenerator.Instance },
		{ typeof(long),           LongExampleValueGenerator.Instance },
		{ typeof(nint),           NIntExampleValueGenerator.Instance },
		{ typeof(nuint),          NUIntExampleValueGenerator.Instance },
		{ typeof(sbyte),          SByteExampleValueGenerator.Instance },
		{ typeof(short),          ShortExampleValueGenerator.Instance },
		{ typeof(string),         StringExampleValueGenerator.Instance },
		{ typeof(uint),           UIntExampleValueGenerator.Instance },
		{ typeof(ulong),          ULongExampleValueGenerator.Instance },
		{ typeof(ushort),         UShortExampleValueGenerator.Instance },
	};

	public static NativeValueExampleGenerator InstanceSuitedFor(Type type)
	=> NativeValueExampleGeneratorsByType.TryGetValue(type, out var generator)
		? generator
		: throw new InvalidOperationException($"{nameof(NativeValueExampleGenerator)}.{nameof(InstanceSuitedFor)}() does not handle data type {type.GetName()}");
}
