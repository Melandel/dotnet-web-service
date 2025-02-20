using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithPublicParameterlessConstructorAndPublicSettersExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithPublicParameterlessConstructorAndPublicSettersExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, PropertyInfo[]> PublicSettablePropertiesByType = new();
	public static ClassWithPublicParameterlessConstructorAndPublicSettersExampleInstantiationBuilder Instance(Type type, PropertyInfo[] settableProperties)
	{
		PublicSettablePropertiesByType.TryAdd(type, settableProperties);
		return _instance;
	}

	public override Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var instanceVariable = Expression.Variable(type, "instance");
		var blockExpressions = new List<Expression> { Expression.Assign(instanceVariable, Expression.New(type)) };

		var saltOffsetByType = new Dictionary<Type, int>();
		foreach (var property in PublicSettablePropertiesByType[type])
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

			var newSalt = Expression.Add(currentSalt, Expression.Constant(saltOffset, currentSalt.Type));
			var exampleValue = ExampleInstantiationBuilder.BuildFor(property.PropertyType, newSalt, parentObjectsAlreadyRecursedThrough);
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

