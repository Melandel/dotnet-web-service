using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

class ClassWithPublicInstancesExposedAsPropertiesInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	static readonly ClassWithPublicInstancesExposedAsPropertiesInstanciationExpressionBuilder _instance = new();
	static readonly Dictionary<Type, Dictionary<int, PropertyInfo>> InstanceAccessPropertiesByType = [];
	public static ClassWithPublicInstancesExposedAsPropertiesInstanciationExpressionBuilder Instance(Type type, PropertyInfo[] staticInstanceAccessProperties)
	{
		if (!InstanceAccessPropertiesByType.ContainsKey(type))
		{
			InstanceAccessPropertiesByType.Add(
				type,
				staticInstanceAccessProperties
					.OrderBy(c => c.Name)
					.Select((value, index) => new { Salt = index, Property = value })
					.ToDictionary(
						kvp => kvp.Salt,
						kvp => kvp.Property));
		}
		return _instance;
	}
	ClassWithPublicInstancesExposedAsPropertiesInstanciationExpressionBuilder() { }

	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
		// var instanceAccessPropertiesBySalt = InstanceAccessPropertiesByType[type];
		// var singletonAccess = instanceAccessPropertiesBySalt[salt % instanceAccessPropertiesBySalt.Count];
		// var instance = singletonAccess.GetValue(null)!;
		// return instance;
	}
}
