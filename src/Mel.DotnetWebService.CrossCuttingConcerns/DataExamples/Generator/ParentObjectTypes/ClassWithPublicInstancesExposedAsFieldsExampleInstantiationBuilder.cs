using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithPublicInstancesExposedAsFieldsExampleInstantiationBuilder : ParentObjectExampleInstantiationBuilder
{
	static readonly ClassWithPublicInstancesExposedAsFieldsExampleInstantiationBuilder _instance = new();
	static readonly ConcurrentDictionary<Type, Dictionary<int, FieldInfo>> InstanceAccessFieldsByType = [];
	public static ClassWithPublicInstancesExposedAsFieldsExampleInstantiationBuilder Instance(Type type, FieldInfo[] staticInstanceAccessFields)
	{
		InstanceAccessFieldsByType.GetOrAdd(
			type,
			_ => staticInstanceAccessFields
				.OrderBy(c => c.Name)
				.Select((value, index) => new { Salt = index, Field = value })
				.ToDictionary(
					kvp => kvp.Salt,
					kvp => kvp.Field));
		return _instance;
	}
	ClassWithPublicInstancesExposedAsFieldsExampleInstantiationBuilder() { }

	public override Expression BuildParentObjectInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	{
		var instanceAccessFieldsBySalt = InstanceAccessFieldsByType[type];
		var index = Expression.Modulo(currentSalt, Expression.Constant(instanceAccessFieldsBySalt.Count));
		var switchCases = instanceAccessFieldsBySalt.Select(
			kvp => Expression.SwitchCase(
				Expression.Convert(Expression.Field(null, kvp.Value), type),
				Expression.Constant(kvp.Key)))
			.ToArray();

		return Expression.Switch(
			index,
			Expression.Throw(Expression.New(typeof(NotImplementedException).GetConstructor(Type.EmptyTypes)!), type),
			switchCases);
	}
}
