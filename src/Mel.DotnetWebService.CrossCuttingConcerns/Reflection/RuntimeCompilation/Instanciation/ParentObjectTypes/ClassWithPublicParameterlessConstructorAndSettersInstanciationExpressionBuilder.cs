using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class ClassWithPublicParameterlessConstructorAndSettersInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	static readonly ClassWithPublicParameterlessConstructorAndSettersInstanciationExpressionBuilder _instance = new();
	static readonly Dictionary<Type, PropertyInfo[]> SettablePropertiesByType = new();
	public static ClassWithPublicParameterlessConstructorAndSettersInstanciationExpressionBuilder Instance(Type type, PropertyInfo[] settableProperties)
	{
		SettablePropertiesByType.TryAdd(type, settableProperties);
		return _instance;
	}
	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
		// var instanceUnderConstruction = Activator.CreateInstance(type)!;
		// var setProperties = SettablePropertiesByType[type];
		// var saltOffsetByType = new Dictionary<Type, int>();
		// foreach (var setProperty in setProperties)
		// {
		// 	if (saltOffsetByType.TryGetValue(setProperty.PropertyType, out var currentSaltOffsetForThisType))
		// 	{
		// 		saltOffsetByType[setProperty.PropertyType] = ++currentSaltOffsetForThisType;
		// 	}
		// 	else
		// 	{
		// 		saltOffsetByType.Add(setProperty.PropertyType, 0);
		// 	}
		// 	var saltOffset = saltOffsetByType[setProperty.PropertyType];
		// 	var setPropertyExampleValue = InstanciationExpressionBuilder.BuildInstanciationExpressionFor(setProperty.PropertyType, salt+saltOffset);
		// 	setProperty.SetValue(instanceUnderConstruction, setPropertyExampleValue, null);
		// }

		// return instanceUnderConstruction;
	}
}

