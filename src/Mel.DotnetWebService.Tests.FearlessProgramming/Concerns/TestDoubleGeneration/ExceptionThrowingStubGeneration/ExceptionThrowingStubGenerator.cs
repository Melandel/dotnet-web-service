using System.Collections.Concurrent;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicCodeInvocation;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDoubleGeneration.ExceptionThrowingStubGeneration;

static class ExceptionThrowingStubGenerator
{
	static readonly ConcurrentDictionary<Type, CompiledInvokable> InstantiatorsByInterface = [];
	public static TInterface BuildStubThatThrowsOnEachCall<TInterface>(Exception exception)
	{
		var instantiator = typeof(TInterface) switch
		{
			var t when !t.IsInterface => throw new InvalidOperationException(),
			var t => InstantiatorsByInterface.GetOrAdd(t, _ => BuildExceptionThrowingStubInstantiatorFor(t))
		};

		var stub = (TInterface) instantiator.Invoke(exception);
		return stub;
	}

	static CompiledInvokable BuildExceptionThrowingStubInstantiatorFor(Type interfaceType)
	{
		var type = DynamicExceptionThrowingStubTypeGenerator.GenerateConcreteTypeImplementing(interfaceType);
		var constructor = type.GetConstructors().First();
		var exceptionParameter = Expression.Parameter(typeof(Exception), "exceptionToThrow");

		var instantiation = Expression.New(constructor, exceptionParameter);

		var instanciator = CompiledInvokable.FromExpression(instantiation, exceptionParameter);
		return instanciator;
	}
}
