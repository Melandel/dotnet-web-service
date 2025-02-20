using System.Reflection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.TestCases;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public sealed class TestCasesAggregationAttribute : NUnitAttribute, ITestBuilder, IImplyFixture
{
	readonly Type _testCaseSource;
	readonly string _testCaseSourceProperty;
	static readonly Dictionary<Type, MethodInfo> TestCasesAggregationInstanciatorsByPropertyType = [];


	public TestCasesAggregationAttribute(Type testCaseSource, string testCaseSourceProperty)
	{
		_testCaseSource = testCaseSource ?? throw new ArgumentNullException(nameof(testCaseSource));
		_testCaseSourceProperty = testCaseSourceProperty ?? throw new ArgumentNullException(nameof(testCaseSourceProperty));
	}

	public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
	{
		var methodInfo = method.MethodInfo;
		ValidateTestMethod(methodInfo);

		var property = _testCaseSource.GetProperty(_testCaseSourceProperty, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
		if (property == null)
		{
			throw new InvalidTestFixtureException($"Test case source property '{_testCaseSource.FullName}.{_testCaseSourceProperty}' was not found.");
		}
		if (property.GetMethod == null)
		{
			throw new InvalidTestFixtureException($"Test case source property '{_testCaseSource.FullName}.{_testCaseSourceProperty}' does not have a getter.");
		}

		if (!property.PropertyType.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out _))
		{
			throw new InvalidTestFixtureException($"Test case source property '{_testCaseSource.FullName}.{_testCaseSourceProperty}' must be IEnumerable<T>.");
		}

		var parameterType = methodInfo.GetParameters()[0].ParameterType;
		var testCaseType = GetTestCaseType(property);
		var expectedParameterType = typeof(TestCasesAggregation<>).MakeGenericType(testCaseType);
		if (parameterType != expectedParameterType)
		{
			throw new InvalidTestFixtureException($"Test method parameter type '{parameterType.FullName}' must be {expectedParameterType.GetName()}.");
		}

		var testCaseSourceValue = GetSourceValue(property, testCaseType);
		var parameters = new TestCaseParameters(new object[] { testCaseSourceValue });
		var test = new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parameters);
		test.Name = $"{_testCaseSource.Name}.{_testCaseSourceProperty}";

		yield return test;
	}

	static Type GetTestCaseType(PropertyInfo property)
	{
		var testCaseType = property.PropertyType.GetGenericArguments()[0];
		while (testCaseType.IsGenericType && testCaseType.GetGenericTypeDefinition() == typeof(TestCasesAggregation<>))
		{
			testCaseType = testCaseType.GetGenericArguments()[0];
		}

		return testCaseType;
	}

	static void ValidateTestMethod(MethodInfo method)
	{
		var parameters = method.GetParameters();
		if (parameters.Length != 1)
		{
			throw new InvalidTestFixtureException($"'{method.DeclaringType?.FullName}.{method.Name}' must have exactly one parameter.");
		}

		var parameterType = parameters[0].ParameterType;
		if (!parameterType.IsGenericType || parameterType.GetGenericTypeDefinition() != typeof(TestCasesAggregation<>))
		{
			throw new InvalidTestFixtureException($"The parameter of '{method.Name}' must be {typeof(TestCasesAggregation<>).GetName()}.");
		}
	}

	static object GetSourceValue(PropertyInfo property, Type testCaseType)
	{
		object instance = null;
		if (!property.GetMethod!.IsStatic)
		{
			instance = Activator.CreateInstance(property.DeclaringType!);
		}

		var value = property.GetValue(null);
		if (value == null)
		{
			throw new InvalidTestFixtureException($"Test case source property '{property.DeclaringType?.FullName}.{property.Name}' returned null.");
		}

		if (!TestCasesAggregationInstanciatorsByPropertyType.TryGetValue(property.PropertyType, out var testCasesAggregationInstanciator))
		{
			var instanciationOperationName = property.PropertyType.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes) && argTypes[0].IsGenericType && argTypes[0].GetGenericTypeDefinition() == typeof(TestCasesAggregation<>)
				? nameof(TestCasesAggregation<object>.CreateFromTestAggregations)
				: nameof(TestCasesAggregation<object>.CreateFromTestCases);
			testCasesAggregationInstanciator = typeof(TestCasesAggregation<>).MakeGenericType(testCaseType)
				.GetMethod(
					instanciationOperationName,
					BindingFlags.Public | BindingFlags.Static);

			TestCasesAggregationInstanciatorsByPropertyType.Add(property.PropertyType, testCasesAggregationInstanciator);
		}

		return testCasesAggregationInstanciator.GetParameters().Length switch
		{
			1 => testCasesAggregationInstanciator.Invoke(null, [value]),
			2 => testCasesAggregationInstanciator.Invoke(null, [value, property.Name]),
			_ => throw new InvalidOperationException()
		};
	}
}
