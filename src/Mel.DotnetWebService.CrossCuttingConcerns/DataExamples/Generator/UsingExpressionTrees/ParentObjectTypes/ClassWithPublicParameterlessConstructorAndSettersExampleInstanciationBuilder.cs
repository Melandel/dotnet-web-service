using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class ClassWithPublicParameterlessConstructorAndSettersExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly ClassWithPublicParameterlessConstructorAndSettersExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, PropertyInfo[]> SettablePropertiesByType = new();
	public static ClassWithPublicParameterlessConstructorAndSettersExampleInstanciationBuilder Instance(Type type, PropertyInfo[] settableProperties)
	{
		SettablePropertiesByType.TryAdd(type, settableProperties);
		return _instance;
	}

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
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

			var salt = Expression.Add(currentSalt, Expression.Constant(saltOffset, currentSalt.Type));
			var exampleValue = ExampleInstanciationBuilder.BuildFor(property.PropertyType, salt);
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

