namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;

public class InterfaceImplementionGenerator
{
	public static Type GenerateFieldBasedConcreteTypeImplementing(Type type)
	=> InterfaceImplementationGeneratorUsingReflectionEmit.GenerateConcreteTypeImplementing(type);
}
