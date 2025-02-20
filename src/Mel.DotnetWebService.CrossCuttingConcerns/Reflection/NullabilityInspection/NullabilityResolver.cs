using System.Collections.Concurrent;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.NullabilityInspection;

public static class NullabilityResolver
{
	static readonly ConcurrentDictionary<MethodBase, ConcurrentDictionary<ParameterInfo, bool>> NullabilityByConstructorOrMethodParameter = [];
	static readonly NullabilityInfoContext NullabilityContext = new NullabilityInfoContext();
	static readonly Lock NullabilityInfoCreationLock = new Lock();
	public static bool Resolve(ParameterInfo parameter, MethodBase constructorOrMethod)
	=> NullabilityByConstructorOrMethodParameter
		.GetOrAdd(constructorOrMethod, _ => new ConcurrentDictionary<ParameterInfo, bool>())
		.GetOrAdd(parameter, _ =>
		{
			var isNullable = parameter.ParameterType switch
			{
				var t when Nullable.GetUnderlyingType(t) != null => true,
				var t when t.IsValueType => false,
				_ => GetNullability(parameter)
			};

			return isNullable;
		});

	static bool GetNullability(ParameterInfo parameter)
	{
		NullabilityInfo? nullabilityInfo = null;
		lock (NullabilityInfoCreationLock)
		{
			nullabilityInfo ??= NullabilityContext.Create(parameter);
		}

		return nullabilityInfo.ReadState switch
		{
			NullabilityState.Nullable => true,
			NullabilityState.Unknown => true,
			_ => false
		};
	}
}
