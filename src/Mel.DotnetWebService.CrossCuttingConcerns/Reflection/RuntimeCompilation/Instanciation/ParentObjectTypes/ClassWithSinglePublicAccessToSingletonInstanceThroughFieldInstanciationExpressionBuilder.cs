using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class ClassWithSinglePublicAccessToSingletonInstanceThroughFieldInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughFieldInstanciationExpressionBuilder _instance = new();
	static readonly Dictionary<Type, FieldInfo> SingletonAccessFieldByType = [];
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughFieldInstanciationExpressionBuilder Instance(Type type, FieldInfo singletonAccessField)
	{
		SingletonAccessFieldByType.TryAdd(type, singletonAccessField);
		return _instance;
	}
	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
		// var singletonAccess = SingletonAccessFieldByType[type];
		// var instance = singletonAccess.GetValue(null)!;
		// return instance;
	}
}

