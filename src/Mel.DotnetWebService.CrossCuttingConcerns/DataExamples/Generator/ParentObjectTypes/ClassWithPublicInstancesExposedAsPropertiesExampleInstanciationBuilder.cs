using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithPublicInstancesExposedAsPropertiesExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithPublicInstancesExposedAsPropertiesExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, Dictionary<int, PropertyInfo>> InstanceAccessPropertiesByType = [];
	public static ClassWithPublicInstancesExposedAsPropertiesExampleInstantiationBuilder Instance(Type type, PropertyInfo[] staticInstanceAccessProperties)
	{
		InstanceAccessPropertiesByType.GetOrAdd(
			type,
			_ => staticInstanceAccessProperties
				.OrderBy(c => c.Name)
				.Select((value, index) => new { Salt = index, Property = value })
				.ToDictionary(
					kvp => kvp.Salt,
					kvp => kvp.Property));
		return _instance;
	}
	ClassWithPublicInstancesExposedAsPropertiesExampleInstantiationBuilder() { }

	public override Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var instanceAccessFieldsBySalt = InstanceAccessPropertiesByType[type];
		var index = Expression.Modulo(currentSalt, Expression.Constant(instanceAccessFieldsBySalt.Count));
		var switchCases = instanceAccessFieldsBySalt.Select(
			kvp => Expression.SwitchCase(
				Expression.Convert(Expression.Property(null, kvp.Value), type),
				Expression.Constant(kvp.Key)))
			.ToArray();

		return Expression.Switch(
			index,
			Expression.Throw(Expression.New(typeof(NotImplementedException).GetConstructor(Type.EmptyTypes)!), type),
			switchCases);
	}
}
