using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

class ClassWithPublicParameterlessConstructorAndSettersExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithPublicParameterlessConstructorAndSettersExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, PropertyInfo[]> SettablePropertiesByType = new();
	public static ClassWithPublicParameterlessConstructorAndSettersExampleInstantiationBuilder Instance(Type type, PropertyInfo[] settableProperties)
	{
		SettablePropertiesByType.TryAdd(type, settableProperties);
		return _instance;
	}

	public override Expression BuildInstantiationExpressionFor(Type type, ParameterExpression generativeSalts)
	{
		var instanceVariable = Expression.Variable(type, "instance");
		var blockExpressions = new List<Expression> { Expression.Assign(instanceVariable, Expression.New(type)) };

		var saltOffsetByType = new Dictionary<Type, int>();
		foreach (var property in SettablePropertiesByType[type])
		{
			int saltOffset;
			if (saltOffsetByType.TryGetValue(property.PropertyType, out var currentOffset))
			{
				saltOffset = currentOffset + 1;
				saltOffsetByType[property.PropertyType] = saltOffset;
			}
			else
			{
				saltOffset = 0;
				saltOffsetByType.Add(property.PropertyType, saltOffset);
			}

			var exampleValue = ExampleRegenerationBuilder.BuildFor(property.PropertyType, generativeSalts);
			var setProperty = Expression.Assign(Expression.Property(instanceVariable, property), exampleValue);

			var setPropertyWithSilentThrow = Expression.TryCatch(
				Expression.Block(setProperty, Expression.Empty()),
				Expression.Catch(
					Expression.Parameter(typeof(Exception), "ex"),
					Expression.Block(Expression.Empty())));

			blockExpressions.Add(setPropertyWithSilentThrow);
		}

		var returnVariable = instanceVariable;
		blockExpressions.Add(returnVariable);

		return Expression.Block([instanceVariable], blockExpressions);
	}
}

