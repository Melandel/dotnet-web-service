using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;

public static class DynamicallyGeneratedTypes
{
	static readonly ConstructorInfo IgnoresAccessChecksToAttributeConstructor = typeof(IgnoresAccessChecksToAttribute).GetConstructor([typeof(string)])!;
	static readonly AssemblyBuilder AssemblyBuilder;
	static readonly ModuleBuilder ModuleBuilder;
	static DynamicallyGeneratedTypes()
	{
		AssemblyBuilder = CreateAssemblyBuilder();
		ModuleBuilder = CreateModuleBuilder(AssemblyBuilder);
	}

 public static TypeBuilder DefineType(string name)
	=> ModuleBuilder.DefineType(name);

 public static TypeBuilder DefineType(string name, TypeAttributes attr)
	=> ModuleBuilder.DefineType(name, attr);

 public static TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] Type? parent)
	=> ModuleBuilder.DefineType(name, attr, parent);

 public static TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] Type? parent, PackingSize packingSize, int typesize)
	=> ModuleBuilder.DefineType(name, attr, parent, packingSize, typesize);

 public static TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] Type? parent, PackingSize packsize)
	=> ModuleBuilder.DefineType(name, attr, parent, packsize);

 public static TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] Type? parent, Type[]? interfaces)
	=> ModuleBuilder.DefineType(name, attr, parent, interfaces);

 public static TypeBuilder DefineType(string name, TypeAttributes attr, [DynamicallyAccessedMembers((DynamicallyAccessedMemberTypes)(-1))] Type? parent, int typesize)
	=> ModuleBuilder.DefineType(name, attr, parent, typesize);

	static AssemblyBuilder CreateAssemblyBuilder()
	{
		var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
			new AssemblyName($"{nameof(DynamicallyGeneratedTypes)}Assembly"),
			AssemblyBuilderAccess.Run);

		var assemblies = AppDomain.CurrentDomain
			.GetAssemblies()
			.Where(a => a.FullName != null && !a.FullName.StartsWith("System"));

		foreach (var assembly in assemblies)
		{
			GiveVisibilityToAllTypesInside(assembly, assemblyBuilder);
		}

		return assemblyBuilder;
	}

	static void GiveVisibilityToAllTypesInside(Assembly assembly, AssemblyBuilder assemblyBuilder)
	=> assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(IgnoresAccessChecksToAttributeConstructor, [assembly.GetName().Name]));

	static ModuleBuilder CreateModuleBuilder(AssemblyBuilder assemblyBuilder)
	=> assemblyBuilder.DefineDynamicModule("MainModule");
}
