using System.Collections.Concurrent;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class InterfaceExampleInstantiationBuilder : ExampleInstantiationBuilder
{
	public static readonly InterfaceExampleInstantiationBuilder Instance = new();
	readonly ConcurrentDictionary<Type, Type> DynamicallyGeneratedConcreteImplementationsByInterface = [];

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var dynamicallyGeneratedConcreteImplementation = DynamicallyGeneratedConcreteImplementationsByInterface.GetOrAdd(
			type,
			_ => InterfaceImplementionGenerator.GenerateFieldBasedConcreteTypeImplementing(type));

		return ExampleInstantiationBuilder.BuildFor(dynamicallyGeneratedConcreteImplementation, currentSalt, parentObjectsAlreadyRecursedThrough);
	}
}
