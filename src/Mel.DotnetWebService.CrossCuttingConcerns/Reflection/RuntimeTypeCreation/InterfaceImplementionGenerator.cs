using System.Reflection;
using System.Reflection.Emit;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeCreation;

public class InterfaceImplementionGenerator
{
	public const string DynamicallyGeneratedTypesAssembly = nameof(DynamicallyGeneratedTypesAssembly);
	public static readonly AssemblyBuilder Assembly;
	public static readonly ModuleBuilder Module;

	static InterfaceImplementionGenerator()
	{
		var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(DynamicallyGeneratedTypesAssembly), AssemblyBuilderAccess.Run);
		var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName != null && !a.FullName.StartsWith("System"));
		var ignoresAccessChecksToAttributeConstructor = typeof(IgnoresAccessChecksToAttribute).GetConstructor([typeof(string)])!;
		foreach (var assembly in assemblies)
		{
			assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(ignoresAccessChecksToAttributeConstructor, [assembly.GetName().Name]));
		}
		Assembly = assemblyBuilder;
		Module = Assembly.DefineDynamicModule("MainModule");
	}
	public static Type GenerateFieldBasedConcreteTypeImplementing(Type type)
	=> InterfaceImplementationGeneratorUsingReflectionEmit.GenerateConcreteTypeImplementing(type);
}
