using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

class ConstrainedTypeInfoProvider : IConstrainedTypeInfoProvider
{
	static readonly string CurrentNamespacePrefix;
	readonly Dictionary<Type, ConstrainedTypeInfo> _constrainedTypesByType;
	static ConstrainedTypeInfoProvider()
	{
		var currentNamespace = typeof(ConstrainedTypeInfoProvider).Namespace!;
		CurrentNamespacePrefix = currentNamespace.Substring(0, currentNamespace.IndexOf('.'));
	}
	public static ConstrainedTypeInfoProvider? Instance { get; private set; }
	ConstrainedTypeInfoProvider(Dictionary<Type, ConstrainedTypeInfo> constrainedTypeInfosByType)
	{
		_constrainedTypesByType = constrainedTypeInfosByType;
		Instance = this;
	}
	public static ConstrainedTypeInfoProvider LoadConstrainedTypesDeclaredIn(Assembly assembly, bool forceReload = false)
	{
		if (Instance != null && !forceReload)
		{
			return Instance;
		}

		var assemblyNamesReferendByEntryAssembly = assembly.GetReferencedAssemblies().Where(ass => ass.FullName.StartsWith(CurrentNamespacePrefix));
		var assembliesReferendByEntryAssembly = assemblyNamesReferendByEntryAssembly.Distinct().Select(referencedAssemblyName =>
		{
			Assembly? loadedAssembly = null;
			try { loadedAssembly = Assembly.Load(referencedAssemblyName); }
			catch { }
			return loadedAssembly;
		})
		.Where(loadedAssembly => loadedAssembly != null);

		var assembliesThatCanDefineConstrainedTypes = assembliesReferendByEntryAssembly;
		var count = 0;
		while (count != assembliesThatCanDefineConstrainedTypes.Count())
		{
			count = assembliesThatCanDefineConstrainedTypes.Count();

			var referencedAssemblies = assembliesThatCanDefineConstrainedTypes
				.SelectMany(ass => ass!.GetReferencedAssemblies())
				.Distinct()
				.Where(ass => ass!.FullName!.StartsWith(CurrentNamespacePrefix))
				.Select(referencedAssemblyName =>
				{
					Assembly? loadedAssembly = null;
					try { loadedAssembly = Assembly.Load(referencedAssemblyName); }
					catch { }
					return loadedAssembly;
				})
				.Where(loadedAssembly => loadedAssembly != null);

			assembliesThatCanDefineConstrainedTypes = assembliesThatCanDefineConstrainedTypes.Union(referencedAssemblies);
		}

		assembliesThatCanDefineConstrainedTypes = assembliesThatCanDefineConstrainedTypes.Prepend(assembly);

		var x = assembliesThatCanDefineConstrainedTypes
			.SelectMany(ass => ass!.GetTypes())
			.Distinct()
			.Where(t => t.ImplementsInterface(typeof(IConstrainedType)) && !t.IsAbstract && !t.IsInterface)
			.ToArray();

		var constrainedTypesDetectedAtRuntime = x.ToDictionary(
			t => t,
			t => ConstrainedTypeInfoBuilder.For(t).Build());

		return new(constrainedTypesDetectedAtRuntime);
	}

	public bool TryGet(Type type, out ConstrainedTypeInfo constrainedType)
	{
		if (type.Namespace is null || !type.Namespace.StartsWith(CurrentNamespacePrefix) || !_constrainedTypesByType.ContainsKey(type))
		{
			constrainedType = null!;
			return false;
		}

		constrainedType = _constrainedTypesByType[type];
		return true;
	}
}
