using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

class ConstrainedTypeInfoProvider : IConstrainedTypeInfoProvider
{
	static readonly string CurrentNamespacePrefix;
	readonly Dictionary<Type, ConstrainedTypeInfo> _constrainedTypeInfosByFullyResolvedType;
	readonly HashSet<Type> _unresolvedGenericConstrainedTypes;
	readonly string _loadedAssemblyName;
	public bool IsLoaded(out string loadedAssemblyName)
	{
		var isLoaded = Instance != null;
		if (Instance != null)
		{
			loadedAssemblyName = Instance._loadedAssemblyName;
			return true;
		}
		loadedAssemblyName = "";
		return false;
	}

	static ConstrainedTypeInfoProvider()
	{
		var currentNamespace = typeof(ConstrainedTypeInfoProvider).Namespace!;
		CurrentNamespacePrefix = currentNamespace.Substring(0, currentNamespace.IndexOf('.'));
	}
	public static ConstrainedTypeInfoProvider? Instance { get; private set; }
	ConstrainedTypeInfoProvider(
		string loadedAssemblyName,
		Dictionary<Type, ConstrainedTypeInfo> constrainedTypeInfosByFullyResolvedType,
		HashSet<Type> unresolvedGenericConstrainedTypes)
	{
		_loadedAssemblyName = loadedAssemblyName;
		_constrainedTypeInfosByFullyResolvedType = constrainedTypeInfosByFullyResolvedType;
		_unresolvedGenericConstrainedTypes = unresolvedGenericConstrainedTypes;
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

		GetConcreteConstrainedTypes(assembliesThatCanDefineConstrainedTypes, out var fullyResolvedTypes, out var unresolvedGenericTypes);

		var constrainedTypeInfosByConstrainedTypeDetectedAtRuntime = fullyResolvedTypes.ToDictionary(
			t => t,
			t => ConstrainedTypeInfoBuilder.For(t).Build());

		return new(assembly.FullName!, constrainedTypeInfosByConstrainedTypeDetectedAtRuntime, unresolvedGenericTypes);
	}

	static void GetConcreteConstrainedTypes(IEnumerable<Assembly?> assembliesThatCanDefineConstrainedTypes, out List<Type> fullyResolvedTypes, out HashSet<Type> unresolvedGenericTypes)
	{
		fullyResolvedTypes = [];
		unresolvedGenericTypes = [];
		var distinctTypes = assembliesThatCanDefineConstrainedTypes
			.SelectMany(ass => ass!.GetTypes())
			.Where(t => t.ImplementsInterface(typeof(IConstrainedType)) && !t.IsAbstract && !t.IsInterface)
			.Distinct();
		foreach (var type in distinctTypes)
		{
			if (type.IsGenericType)
			{
				unresolvedGenericTypes.Add(type);
			}
			else
			{
				fullyResolvedTypes.Add(type);
			}
		}
	}

	public bool TryGet(Type type, out ConstrainedTypeInfo constrainedTypeInfo)
	{
		if (type.Namespace is null || !type.Namespace.StartsWith(CurrentNamespacePrefix))
		{
			constrainedTypeInfo = null!;
			return false;
		}

		var isAlreadyIncluded = _constrainedTypeInfosByFullyResolvedType.TryGetValue(type, out var value);
		if (isAlreadyIncluded)
		{
			constrainedTypeInfo = value!;
			return true;
		}

		var isFullyResolvedFromConstrainedGenericType = type.IsGenericType && _unresolvedGenericConstrainedTypes.Contains(type.GetGenericTypeDefinition());
		if (isFullyResolvedFromConstrainedGenericType)
		{
			constrainedTypeInfo = ConstrainedTypeInfoBuilder.For(type).Build();
			_constrainedTypeInfosByFullyResolvedType.Add(type, constrainedTypeInfo);
			return true;
		}

		constrainedTypeInfo = null!;
		return false;
	}
}
