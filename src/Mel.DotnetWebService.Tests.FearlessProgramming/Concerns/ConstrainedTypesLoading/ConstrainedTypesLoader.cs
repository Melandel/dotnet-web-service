using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.ConstrainedTypesLoading;

public static class ConstrainedTypesLoader
{
	static readonly Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();
	public static void LoadConstrainedTypesFromExecutingAssemblyIfNecessary()
	=> ConstrainedTypeInfos.LoadConstrainedTypesDeclaredIn(ExecutingAssembly);
}
