using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class ClassWithPublicParameterlessConstructorAndSettersExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithPublicParameterlessConstructorAndSettersExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, PropertyInfo[]> SettablePropertiesByType = new();
	public static ClassWithPublicParameterlessConstructorAndSettersExampleInstantiationBuilder Instance(Type type, PropertyInfo[] settableProperties)
	{
		SettablePropertiesByType.TryAdd(type, settableProperties);
		return _instance;
	}

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var instanceVariable = Expression.Variable(type, "instance");
		var blockExpressions = new List<Expression> { Expression.Assign(instanceVariable, Expression.New(type)) };

		foreach (var property in SettablePropertiesByType[type])
		{
			var newSaltVariable = Expression.Variable(RandomSalt.Type);
			var newSalt = Expression.Assign(newSaltVariable, RandomSalt.Select);
			var exampleValue = RandomizedExampleInstantiationBuilder.BuildFor(property.PropertyType, newSalt, generativeSalts);
			var setProperty = Expression.Assign(Expression.Property(instanceVariable, property), exampleValue);

			var setPropertyWithSilentThrow = Expression.TryCatch(
				Expression.Block(setProperty, Expression.Empty()),
				Expression.Catch(
					Expression.Parameter(typeof(Exception), "ex"),
					Expression.Block(Expression.Empty())));

			blockExpressions.Add(
				Expression.Block(
					[newSaltVariable],
					setPropertyWithSilentThrow));
		}

		var returnVariable = instanceVariable;
		blockExpressions.Add(returnVariable);

		return Expression.Block([instanceVariable], blockExpressions);
	}
}

