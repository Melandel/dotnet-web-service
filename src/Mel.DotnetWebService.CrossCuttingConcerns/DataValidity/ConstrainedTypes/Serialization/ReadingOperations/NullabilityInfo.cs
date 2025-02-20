using System.Collections.Concurrent;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;

static class NullabilityInfo
{
	public static readonly ConcurrentDictionary<MethodBase, ConcurrentDictionary<ParameterInfo, bool>> NullabilityByConstructorOrMethodParameter = [];
}
