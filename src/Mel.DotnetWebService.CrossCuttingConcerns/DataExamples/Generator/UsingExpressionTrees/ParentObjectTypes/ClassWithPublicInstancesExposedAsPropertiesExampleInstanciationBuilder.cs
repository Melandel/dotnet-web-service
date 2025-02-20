using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;

class ClassWithPublicInstancesExposedAsPropertiesExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly ClassWithPublicInstancesExposedAsPropertiesExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, Dictionary<int, PropertyInfo>> InstanceAccessPropertiesByType = [];
	public static ClassWithPublicInstancesExposedAsPropertiesExampleInstanciationBuilder Instance(Type type, PropertyInfo[] staticInstanceAccessProperties)
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
	ClassWithPublicInstancesExposedAsPropertiesExampleInstanciationBuilder() { }

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt)
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
