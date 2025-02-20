using System.Collections.Concurrent;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicCodeInvocation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDoubleGeneration.DummyGeneration;

static class DummyGenerator
{
	const int DummySalt = 0;
	static readonly ConcurrentDictionary<Type, CompiledInvokable> InstantiatorsByInterface = [];

	public static TInterface Generate<TInterface>()
	{
		var instanciator = typeof(TInterface) switch
		{
			var t when !t.IsInterface => throw new InvalidOperationException(),
			var t => InstantiatorsByInterface.GetOrAdd(t, _ => BuildDummyInstantiatorFor(t))
		};

		var dummy = (TInterface)instanciator.Invoke();
		return dummy;
	}

	static CompiledInvokable BuildDummyInstantiatorFor(Type interfaceType)
	{
		var type = DynamicDummyTypeGenerator.GenerateConcreteTypeImplementing(interfaceType);
		var constructor = type.GetConstructors().First();
		var constructorParameters = constructor.GetParameters().Select(p => ExampleInstantiationBuilder.BuildFor(p.ParameterType, Expression.Constant(DummySalt), []));

		var instantiation = Expression.New(constructor, constructorParameters);

		var instantiator = CompiledInvokable.FromExpression(instantiation);
		return instantiator;
	}
}
