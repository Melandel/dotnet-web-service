using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class PositionalRecordParentObjectExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly PositionalRecordParentObjectExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = [];

	public static PositionalRecordParentObjectExampleInstantiationBuilder Instance(Type targetType, ConstructorInfo propertyBasedConstructor)
	{
		PropertyBasedConstructorsByPositionalRecordType.TryAdd(targetType, propertyBasedConstructor);
		return _instance;
	}

	public override Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var constructor = PropertyBasedConstructorsByPositionalRecordType[type];
		return Expression.New(
			constructor,
			GenerateParameterExamplesFor(constructor, currentSalt, parentObjectsAlreadyRecursedThrough));
	}
}
