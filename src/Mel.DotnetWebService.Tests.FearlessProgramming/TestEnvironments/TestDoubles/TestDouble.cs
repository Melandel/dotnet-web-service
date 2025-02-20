using System.Linq.Expressions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeCreation;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ExceptionThrowingStubGeneration;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

public abstract record TestDouble<TInterface, TSelf> where TSelf : TestDouble<TInterface, TSelf>, TInterface, new()
{
	protected TestDoubleCategory TestDoubleCategory;
	const int DummySalt = 0;
	static readonly Dictionary<Type, CompiledInvokable> CachedDummyInstanciatorsByInterface = [];
	static readonly Dictionary<Type, CompiledInvokable> CachedExceptionThrowingStubInstanciatorsByInterface = [];
	public static TInterface Dummy => GetOrBuildConcreteTypeImplementatingInterfaceUsingFields(typeof(TInterface)).Invoke();

	public static TInterface StubThatThrowsNotImplementedException => GetOrBuildInstanciatorForInterfaceImplementingTypeThatThrows(typeof(TInterface)).Invoke(new NotImplementedException());
	public static TInterface StubThatThrowsObjectConstructionException => GetOrBuildInstanciatorForInterfaceImplementingTypeThatThrows(typeof(TInterface)).Invoke(ObjectConstructionException.WhenConstructingAnInstanceOf<Exception>(new StackOverflowException()));

	public static TSelf Stub => new TSelf() { TestDoubleCategory = TestDoubleCategory.Stub };
	public static TSelf Fake => new TSelf() { TestDoubleCategory = TestDoubleCategory.Fake };
	public static TSelf Spy => new TSelf() { TestDoubleCategory = TestDoubleCategory.Spy };
	public static TSelf Mock => new TSelf() { TestDoubleCategory = TestDoubleCategory.Mock };
	public static TSelf Configure => new TSelf() { TestDoubleCategory = TestDoubleCategory.Other };

	static CompiledInvokable GetOrBuildConcreteTypeImplementatingInterfaceUsingFields(Type interfaceType)
	{
		if (!interfaceType.IsInterface)
		{
			throw new InvalidOperationException();
		}

		if (!CachedDummyInstanciatorsByInterface.TryGetValue(interfaceType, out var instanciator))
		{
			var type = InterfaceImplementionGenerator.GenerateFieldBasedConcreteTypeImplementing(interfaceType);
			var constructor = type.GetConstructors().First();
			var instanciation = Expression.New(
				constructor,
				constructor.GetParameters().Select(p => ExampleInstanciationBuilder.BuildFor(p.ParameterType, Expression.Constant(DummySalt))));

			instanciator = CompiledInvokable.FromExpression(instanciation);
			CachedDummyInstanciatorsByInterface.Add(interfaceType, instanciator);
		}

		return instanciator;
	}

	static CompiledInvokable GetOrBuildInstanciatorForInterfaceImplementingTypeThatThrows(Type interfaceType)
	{
		if (!interfaceType.IsInterface)
		{
			throw new InvalidOperationException();
		}

		if (!CachedExceptionThrowingStubInstanciatorsByInterface.TryGetValue(interfaceType, out var instanciator))
		{
			var type = ExceptionThrowingStubGenerator.GenerateConcreteTypeImplementing(interfaceType);
			var constructor = type.GetConstructors().First();
			var exceptionParameter = Expression.Parameter(typeof(Exception), "exceptionToThrow");
			var instanciation = Expression.New(constructor, exceptionParameter);

			instanciator = CompiledInvokable.FromExpression(instanciation, exceptionParameter);
			CachedExceptionThrowingStubInstanciatorsByInterface.Add(interfaceType, instanciator);
		}

		return instanciator;
	}
}
