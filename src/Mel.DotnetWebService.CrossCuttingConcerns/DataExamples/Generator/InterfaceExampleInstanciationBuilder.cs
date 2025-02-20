using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeCreation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class InterfaceExampleInstanciationBuilder : ExampleInstanciationBuilder
{
	public static readonly InterfaceExampleInstanciationBuilder Instance = new();
	readonly Dictionary<Type, Type> DynamicallyGeneratedConcreteImplementationsByInterface = [];

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		if (!DynamicallyGeneratedConcreteImplementationsByInterface.TryGetValue(type, out var dynamicallyGeneratedConcreteImplementation))
		{
			dynamicallyGeneratedConcreteImplementation = InterfaceImplementionGenerator.GenerateFieldBasedConcreteTypeImplementing(type);
			DynamicallyGeneratedConcreteImplementationsByInterface.Add(type, dynamicallyGeneratedConcreteImplementation);
		}

		return ExampleInstanciationBuilder.BuildFor(dynamicallyGeneratedConcreteImplementation, currentSalt);
	}
}
