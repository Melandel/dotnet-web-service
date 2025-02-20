using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

class ClassWithPublicInstancesExposedAsFieldsInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	static readonly ClassWithPublicInstancesExposedAsFieldsInstanciationExpressionBuilder _instance = new();
	static readonly Dictionary<Type, Dictionary<int, FieldInfo>> InstanceAccessFieldsByType = [];
	public static ClassWithPublicInstancesExposedAsFieldsInstanciationExpressionBuilder Instance(Type type, FieldInfo[] staticInstanceAccessFields)
	{
		if (!InstanceAccessFieldsByType.ContainsKey(type))
		{
			InstanceAccessFieldsByType.Add(
				type,
				staticInstanceAccessFields
					.OrderBy(c => c.Name)
					.Select((value, index) => new { Salt = index, Field = value })
					.ToDictionary(
						kvp => kvp.Salt,
						kvp => kvp.Field));
		}
		return _instance;
	}
	ClassWithPublicInstancesExposedAsFieldsInstanciationExpressionBuilder() { }

	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
		// var instanceAccessFieldsBySalt = InstanceAccessFieldsByType[type];
		// var singletonAccess = instanceAccessFieldsBySalt[salt % instanceAccessFieldsBySalt.Count];
		// var instance = singletonAccess.GetValue(null)!;
		// return instance;
	}
}
