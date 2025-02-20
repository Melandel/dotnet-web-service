using System.Collections.Concurrent;
using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicCodeInvocation;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ExceptionThrowingStubGeneration;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

public abstract record TestDouble<TInterface, TSelf> where TSelf : TestDouble<TInterface, TSelf>, TInterface, new()
{
	protected TestDoubleCategory TestDoubleCategory;
	const int DummySalt = 0;
	static readonly ConcurrentDictionary<Type, CompiledInvokable> CachedDummyInstantiatorsByInterface = [];
	static readonly ConcurrentDictionary<Type, CompiledInvokable> CachedExceptionThrowingStubInstantiatorsByInterface = [];
	public static TInterface Dummy => (TInterface)InstantiatorForValidExamplesBasedImplementationOf(typeof(TInterface)).Invoke();

	public static TInterface StubThatThrows
	=> BuildStubThatThrowsOnEachCall(
		(Exception)ExceptionUnknownAtCompileTime.Instantiator.Invoke($"{typeof(TSelf).GetName()}.{nameof(StubThatThrows)} has thrown. Shocker!"));

	public static TSelf Stub => new TSelf() { TestDoubleCategory = TestDoubleCategory.Stub };
	public static TSelf Fake => new TSelf() { TestDoubleCategory = TestDoubleCategory.Fake };
	public static TSelf Spy => new TSelf() { TestDoubleCategory = TestDoubleCategory.Spy };
	public static TSelf Mock => new TSelf() { TestDoubleCategory = TestDoubleCategory.Mock };
	public static TSelf Configure => new TSelf() { TestDoubleCategory = TestDoubleCategory.Other };

	static CompiledInvokable InstantiatorForValidExamplesBasedImplementationOf(Type interfaceType)
	{
		if (!interfaceType.IsInterface)
		{
			throw new InvalidOperationException();
		}

		return CachedDummyInstantiatorsByInterface.GetOrAdd(interfaceType, _ => BuildDummyInstantiatorFor(interfaceType));
	}

	protected static TInterface BuildStubThatThrowsOnEachCall(Exception exception)
	{
		var instantiator = InstantiatorForExceptionThrowingImplementionOf(typeof(TInterface));
		return (TInterface) instantiator.Invoke(exception);
	}

	static CompiledInvokable InstantiatorForExceptionThrowingImplementionOf(Type interfaceType)
	{
		if (!interfaceType.IsInterface)
		{
			throw new InvalidOperationException();
		}

		return CachedExceptionThrowingStubInstantiatorsByInterface.GetOrAdd(interfaceType, _ => BuildExceptionThrowingStubInstantiatorFor(interfaceType)); ;
	}

	static CompiledInvokable BuildDummyInstantiatorFor(Type interfaceType)
	{
		var type = InterfaceImplementionGenerator.GenerateFieldBasedConcreteTypeImplementing(interfaceType);
		var constructor = type.GetConstructors().First();
		var instantiation = Expression.New(
			constructor,
			constructor.GetParameters().Select(p => ExampleInstantiationBuilder.BuildFor(p.ParameterType, Expression.Constant(DummySalt), [])));

		return CompiledInvokable.FromExpression(instantiation);
	}
	static CompiledInvokable BuildExceptionThrowingStubInstantiatorFor(Type interfaceType)
	{
		var type = ExceptionThrowingStubGenerator.GenerateConcreteTypeImplementing(interfaceType);
		var constructor = type.GetConstructors().First();
		var exceptionParameter = Expression.Parameter(typeof(Exception), "exceptionToThrow");
		var instantiation = Expression.New(constructor, exceptionParameter);

		return CompiledInvokable.FromExpression(instantiation, exceptionParameter);
	}
}
