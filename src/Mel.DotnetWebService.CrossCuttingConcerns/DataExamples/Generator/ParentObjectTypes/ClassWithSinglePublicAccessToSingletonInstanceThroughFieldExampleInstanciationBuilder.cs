using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, FieldInfo> SingletonAccessFieldByType = [];
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughFieldExampleInstanciationBuilder Instance(Type type, FieldInfo singletonAccessField)
	{
		SingletonAccessFieldByType.TryAdd(type, singletonAccessField);
		return _instance;
	}
	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
	=> Expression.Field(null, SingletonAccessFieldByType[type]);
}

