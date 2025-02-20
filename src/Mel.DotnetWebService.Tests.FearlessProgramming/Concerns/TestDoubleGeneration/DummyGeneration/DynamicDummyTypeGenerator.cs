using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDoubleGeneration.DummyGeneration;

static class DynamicDummyTypeGenerator
{
	public static Type GenerateConcreteTypeImplementing(Type interfaceType)
		=> InterfaceImplementionGenerator.GenerateFieldBasedConcreteTypeImplementing(interfaceType);
}
