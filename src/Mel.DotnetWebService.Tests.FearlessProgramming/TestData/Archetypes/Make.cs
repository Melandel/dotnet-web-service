using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Core;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes.DotnetPrimitiveTypes;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

static class Make
{
	static Dictionary<ConstrainedTypeInfo, Dictionary<ArchetypeTitle, object>> ArchetypesByConstrainedTypeInfo;
	static Make()
	{
		ArchetypesByConstrainedTypeInfo = new Dictionary<ConstrainedTypeInfo, Dictionary<ArchetypeTitle, object>>();
	}
	public static TData SomeValue<TData>() where TData : class
	=> typeof(TData) switch
	{
		var t when ConstrainedTypeInfos.TryGet(typeof(TData), out var constrainedTypeInfo) => MakeArchetype(constrainedTypeInfo, ArchetypeTitle.SomeValue) as TData,
		var t when t == typeof(bool)           => BoolArchetype.SomeValue           as TData,
		var t when t == typeof(byte)           => ByteArchetype.SomeValue           as TData,
		var t when t == typeof(DateTime)       => DateTimeArchetype.SomeValue       as TData,
		var t when t == typeof(DateTimeOffset) => DateTimeOffsetArchetype.SomeValue as TData,
		var t when t == typeof(decimal)        => DecimalArchetype.SomeValue        as TData,
		var t when t == typeof(double)         => DoubleArchetype.SomeValue         as TData,
		var t when t == typeof(float)          => FloatArchetype.SomeValue          as TData,
		var t when t == typeof(Guid)           => GuidArchetype.SomeValue           as TData,
		var t when t == typeof(int)            => IntArchetype.SomeValue            as TData,
		var t when t == typeof(long)           => LongArchetype.SomeValue           as TData,
		var t when t == typeof(nint)           => NIntArchetype.SomeValue           as TData,
		var t when t == typeof(nuint)          => NUIntArchetype.SomeValue          as TData,
		var t when t == typeof(sbyte)          => SByteArchetype.SomeValue          as TData,
		var t when t == typeof(short)          => ShortArchetype.SomeValue          as TData,
		var t when t == typeof(string)         => StringArchetype.SomeValue         as TData,
		var t when t == typeof(uint)           => UIntArchetype.SomeValue           as TData,
		var t when t == typeof(ulong)          => ULongArchetype.SomeValue          as TData,
		var t when t == typeof(ushort)         => UShortArchetype.SomeValue         as TData,
		_ => throw new InvalidOperationException($"{nameof(Make)}.{nameof(SomeValue)}() only handles leaf data values")
	};

	public static TData SomeOtherValue<TData>() where TData : class
	=> typeof(TData) switch
	{
		var t when ConstrainedTypeInfos.TryGet(typeof(TData), out var constrainedTypeInfo) => MakeArchetype(constrainedTypeInfo, ArchetypeTitle.SomeOtherValue) as TData,
		var t when t == typeof(bool)           => BoolArchetype.SomeOtherValue           as TData,
		var t when t == typeof(byte)           => ByteArchetype.SomeOtherValue           as TData,
		var t when t == typeof(DateTime)       => DateTimeArchetype.SomeOtherValue       as TData,
		var t when t == typeof(DateTimeOffset) => DateTimeOffsetArchetype.SomeOtherValue as TData,
		var t when t == typeof(decimal)        => DecimalArchetype.SomeOtherValue        as TData,
		var t when t == typeof(double)         => DoubleArchetype.SomeOtherValue         as TData,
		var t when t == typeof(float)          => FloatArchetype.SomeOtherValue          as TData,
		var t when t == typeof(Guid)           => GuidArchetype.SomeOtherValue           as TData,
		var t when t == typeof(int)            => IntArchetype.SomeOtherValue            as TData,
		var t when t == typeof(long)           => LongArchetype.SomeOtherValue           as TData,
		var t when t == typeof(nint)           => NIntArchetype.SomeOtherValue           as TData,
		var t when t == typeof(nuint)          => NUIntArchetype.SomeOtherValue          as TData,
		var t when t == typeof(sbyte)          => SByteArchetype.SomeOtherValue          as TData,
		var t when t == typeof(short)          => ShortArchetype.SomeOtherValue          as TData,
		var t when t == typeof(string)         => StringArchetype.SomeOtherValue         as TData,
		var t when t == typeof(uint)           => UIntArchetype.SomeOtherValue           as TData,
		var t when t == typeof(ulong)          => ULongArchetype.SomeOtherValue          as TData,
		var t when t == typeof(ushort)         => UShortArchetype.SomeOtherValue         as TData,
		_ => throw new InvalidOperationException($"{nameof(Make)}.{nameof(SomeOtherValue)}() only handles leaf data values")
	};

	public static TData YetAnotherValue<TData>() where TData : class
	=> typeof(TData) switch
	{
		var t when ConstrainedTypeInfos.TryGet(typeof(TData), out var constrainedTypeInfo) => MakeArchetype(constrainedTypeInfo, ArchetypeTitle.YetAnotherValue) as TData,
		var t when t == typeof(bool)           => BoolArchetype.YetAnotherValue           as TData,
		var t when t == typeof(byte)           => ByteArchetype.YetAnotherValue           as TData,
		var t when t == typeof(DateTime)       => DateTimeArchetype.YetAnotherValue       as TData,
		var t when t == typeof(DateTimeOffset) => DateTimeOffsetArchetype.YetAnotherValue as TData,
		var t when t == typeof(decimal)        => DecimalArchetype.YetAnotherValue        as TData,
		var t when t == typeof(double)         => DoubleArchetype.YetAnotherValue         as TData,
		var t when t == typeof(float)          => FloatArchetype.YetAnotherValue          as TData,
		var t when t == typeof(Guid)           => GuidArchetype.YetAnotherValue           as TData,
		var t when t == typeof(int)            => IntArchetype.YetAnotherValue            as TData,
		var t when t == typeof(long)           => LongArchetype.YetAnotherValue           as TData,
		var t when t == typeof(nint)           => NIntArchetype.YetAnotherValue           as TData,
		var t when t == typeof(nuint)          => NUIntArchetype.YetAnotherValue          as TData,
		var t when t == typeof(sbyte)          => SByteArchetype.YetAnotherValue          as TData,
		var t when t == typeof(short)          => ShortArchetype.YetAnotherValue          as TData,
		var t when t == typeof(string)         => StringArchetype.YetAnotherValue         as TData,
		var t when t == typeof(uint)           => UIntArchetype.YetAnotherValue           as TData,
		var t when t == typeof(ulong)          => ULongArchetype.YetAnotherValue          as TData,
		var t when t == typeof(ushort)         => UShortArchetype.YetAnotherValue         as TData,
		_ => throw new InvalidOperationException($"{nameof(Make)}.{nameof(YetAnotherValue)}() only handles leaf data values")
	};

	public static IReadOnlyCollection<TData> SomeValues<TData>(CollectionCase size = CollectionCase.ManyElements) where TData : class
	=> size switch
	{
		CollectionCase.Null => null,
		CollectionCase.ZeroElement => Array.Empty<TData>(),
		CollectionCase.OneElement => new[] { SomeValue<TData>() },
		CollectionCase.ManyElements => new[] { SomeValue<TData>(), SomeOtherValue<TData>() },
		_ => throw new NotImplementedException()
	};

	static object MakeArchetype(ConstrainedTypeInfo constrainedTypeInfo, ArchetypeTitle title)
	{
		if (!ArchetypesByConstrainedTypeInfo.ContainsKey(constrainedTypeInfo))
		{
			var someValue = constrainedTypeInfo.InvokeStaticFactoryMethod(constrainedTypeInfo.ValidValueExamples[0]);
			var someOtherValue = constrainedTypeInfo.InvokeStaticFactoryMethod(constrainedTypeInfo.ValidValueExamples[1]);
			var yetAnotherValue = constrainedTypeInfo.InvokeStaticFactoryMethod(constrainedTypeInfo.ValidValueExamples[2 % constrainedTypeInfo.ValidValueExamples.Length]);

			ArchetypesByConstrainedTypeInfo.Add(
				constrainedTypeInfo,
				new Dictionary<ArchetypeTitle, object>
				{
					{ ArchetypeTitle.SomeValue, someValue },
					{ ArchetypeTitle.SomeOtherValue, someOtherValue },
					{ ArchetypeTitle.YetAnotherValue, yetAnotherValue }
				});
		}

		return ArchetypesByConstrainedTypeInfo[constrainedTypeInfo][title];
	}
}
