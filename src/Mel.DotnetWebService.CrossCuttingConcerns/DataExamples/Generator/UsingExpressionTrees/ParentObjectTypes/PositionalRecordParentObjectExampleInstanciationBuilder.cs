using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class PositionalRecordParentObjectExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly PositionalRecordParentObjectExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = [];

	public static PositionalRecordParentObjectExampleInstanciationBuilder Instance(Type targetType, ConstructorInfo propertyBasedConstructor)
	{
		PropertyBasedConstructorsByPositionalRecordType.TryAdd(targetType, propertyBasedConstructor);
		return _instance;
	}

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	{
		var constructor = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).First();
		return Expression.New(
			constructor,
			GenerateParameterExamplesFor(constructor, currentSalt));
	}
}
