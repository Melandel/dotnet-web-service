using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

public static class ConstrainedTypeInfos
{
	public static void LoadConstrainedTypesDeclaredIn(Assembly assembly, bool forceReload = false)
	{
		if (ConstrainedTypeInfos.IsLoaded(out var loadedAssembliesNames)
			&& loadedAssembliesNames.Contains(assembly.FullName)
			&& !forceReload)
		{
			return;
		}
		ConstrainedTypeInfoProvider.LoadConstrainedTypesDeclaredIn(assembly, forceReload);
	}

		

	public static bool IsLoaded(out IReadOnlyCollection<string> loadedAssembliesNames)
	{
		if (ConstrainedTypeInfoProvider.Instance == null)
		{
			loadedAssembliesNames = [];
			return false;
		}

		return ConstrainedTypeInfoProvider.Instance.IsLoaded(out loadedAssembliesNames);
	}

	public static ConstrainedTypeInfo Get(Type type)
	=> ConstrainedTypeInfoProvider.Instance!.TryGet(type, out var constrainedTypeInfo)
		? constrainedTypeInfo
		: throw new InvalidOperationException();

	public static bool TryGet(Type type, out ConstrainedTypeInfo constrainedTypeInfo)
	=> ConstrainedTypeInfoProvider.Instance!.TryGet(type, out constrainedTypeInfo);

	public static bool Include(Type type)
	=> ConstrainedTypeInfoProvider.Instance!.TryGet(type, out _);

	public static Type GetRootType(Type constrainedType)
	=> TryGet(constrainedType, out var constrainedTypeInfo)
		? constrainedTypeInfo.RootType
		: throw new InvalidOperationException($"{typeof(ConstrainedTypeInfos).GetName()}.{nameof(GetRootType)} : {constrainedType} is not a constrained type.");

	public static object ReconstituteFromRootTypeValue(Type constrainedType, object rootTypeValue)
	=> TryGet(constrainedType, out var constrainedTypeInfo)
		? constrainedTypeInfo.InvokeStaticFactoryMethod(rootTypeValue)!
		: throw new InvalidOperationException($"{typeof(ConstrainedTypeInfos).GetName()}.{nameof(ReconstituteFromRootTypeValue)} : {constrainedType} is not a constrained type.");
}
