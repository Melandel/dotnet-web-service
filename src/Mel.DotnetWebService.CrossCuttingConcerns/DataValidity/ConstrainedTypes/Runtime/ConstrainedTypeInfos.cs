using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

public static class ConstrainedTypeInfos
{
	public static void LoadConstrainedTypesDeclaredIn(Assembly assembly)
	{
		if (ConstrainedTypeInfos.HasLoaded(assembly))
		{
			return;
		}

		ConstrainedTypeInfoProvider.LoadConstrainedTypesDeclaredIn(assembly);
	}

	public static bool HasLoaded(Assembly assembly)
	=> ConstrainedTypeInfoProvider.Instance != null
		&& ConstrainedTypeInfoProvider.Instance.IsLoaded(out var loadedAssembliesNames)
		&& loadedAssembliesNames.Contains(assembly.FullName);

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
