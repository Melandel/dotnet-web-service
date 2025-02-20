namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public abstract class NativeValueInstanciationExpressionBuilder : InstanciationExpressionBuilder
{
	static readonly Dictionary<Type, NativeValueInstanciationExpressionBuilder> NativeValueInstanciationExpressionBuilderByType = new()
	{
		{ typeof(bool),           BoolInstanciationExpressionBuilder.Instance },
		{ typeof(byte),           ByteInstanciationExpressionBuilder.Instance },
		{ typeof(DateTime),       DateTimeInstanciationExpressionBuilder.Instance },
		{ typeof(DateTimeOffset), DateTimeOffsetInstanciationExpressionBuilder.Instance },
		{ typeof(decimal),        DecimalInstanciationExpressionBuilder.Instance },
		{ typeof(double),         DoubleInstanciationExpressionBuilder.Instance },
		{ typeof(float),          FloatInstanciationExpressionBuilder.Instance },
		{ typeof(Guid),           GuidInstanciationExpressionBuilder.Instance },
		{ typeof(int),            IntInstanciationExpressionBuilder.Instance },
		{ typeof(long),           LongInstanciationExpressionBuilder.Instance },
		{ typeof(nint),           NIntInstanciationExpressionBuilder.Instance },
		{ typeof(nuint),          NUIntInstanciationExpressionBuilder.Instance },
		{ typeof(sbyte),          SByteInstanciationExpressionBuilder.Instance },
		{ typeof(short),          ShortInstanciationExpressionBuilder.Instance },
		{ typeof(string),         StringInstanciationExpressionBuilder.Instance },
		{ typeof(uint),           UIntInstanciationExpressionBuilder.Instance },
		{ typeof(ulong),          ULongInstanciationExpressionBuilder.Instance },
		{ typeof(ushort),         UShortInstanciationExpressionBuilder.Instance },
	};

	public static NativeValueInstanciationExpressionBuilder InstanceSuitedFor(Type type)
	=> NativeValueInstanciationExpressionBuilderByType.TryGetValue(type, out var builder)
		? builder
		: throw new InvalidOperationException($"{nameof(NativeValueInstanciationExpressionBuilder)}.{nameof(InstanceSuitedFor)}() does not handle data type {type.GetName()}");
}
