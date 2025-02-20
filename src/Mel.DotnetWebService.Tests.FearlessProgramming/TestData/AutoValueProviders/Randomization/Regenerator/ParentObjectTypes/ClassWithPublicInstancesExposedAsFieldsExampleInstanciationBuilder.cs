using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

class ClassWithPublicInstancesExposedAsFieldsExampleInstanciationBuilder : ParentObjectExampleInstanciationBuilder
{
	static readonly ClassWithPublicInstancesExposedAsFieldsExampleInstanciationBuilder _instance = new();
	static readonly Dictionary<Type, Dictionary<int, FieldInfo>> InstanceAccessFieldsByType = [];
	public static ClassWithPublicInstancesExposedAsFieldsExampleInstanciationBuilder Instance(Type type, FieldInfo[] staticInstanceAccessFields)
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
	ClassWithPublicInstancesExposedAsFieldsExampleInstanciationBuilder() { }

	public override Expression BuildInstanciationExpressionFor(Type type, ParameterExpression generativeSalts)
	{
		var instanceAccessFieldsBySalt = InstanceAccessFieldsByType[type];
		var index = Expression.Modulo(GenerativeSalts.Dequeue(generativeSalts), Expression.Constant(instanceAccessFieldsBySalt.Count));
		var switchCases = instanceAccessFieldsBySalt.Select(
			kvp => Expression.SwitchCase(
				Expression.Convert(Expression.Field(null, kvp.Value), type),
				Expression.Constant(kvp.Key)))
			.ToArray();

		return Expression.Switch(
			index,
			Expression.Throw(Expression.New(typeof(NotImplementedException).GetConstructor(Type.EmptyTypes)), type),
			switchCases);
	}
}
