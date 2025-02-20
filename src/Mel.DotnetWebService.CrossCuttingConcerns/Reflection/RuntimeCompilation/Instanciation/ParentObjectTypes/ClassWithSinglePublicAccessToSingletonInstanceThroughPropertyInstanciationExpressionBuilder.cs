using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	static readonly ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyInstanciationExpressionBuilder _instance = new();
	static readonly Dictionary<Type, PropertyInfo> SingletonAccessPropertyByType = new();
	public static ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyInstanciationExpressionBuilder Instance(Type type, PropertyInfo singletonAccessProperty)
	{
		SingletonAccessPropertyByType.TryAdd(type, singletonAccessProperty);
		return _instance;
	}
	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
		// var singletonAccess = SingletonAccessPropertyByType[type];
		// var instance = singletonAccess.GetValue(null)!;
		// return instance;
	}
}

