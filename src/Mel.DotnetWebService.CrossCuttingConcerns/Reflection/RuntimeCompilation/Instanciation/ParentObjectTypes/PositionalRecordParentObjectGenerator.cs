using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public class PositionalRecordParentObjectInstanciationExpressionBuilder : ParentObjectInstanciationExpressionBuilder
{
	static readonly PositionalRecordParentObjectInstanciationExpressionBuilder _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = [];

	public static PositionalRecordParentObjectInstanciationExpressionBuilder Instance(Type targetType, ConstructorInfo propertyBasedConstructor)
	{
		PropertyBasedConstructorsByPositionalRecordType.TryAdd(targetType, propertyBasedConstructor);
		return _instance;
	}

	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
		// var propertyBasedConstructor = FindPropertyBasedConstructor(type);
		// var constructorParameterInfos = propertyBasedConstructor.GetParameters().ToList();

		// var constructorParameterValues = new List<object>(constructorParameterInfos.Count);
		// var saltOffsetByType = new Dictionary<Type, int>();
		// foreach (var param in propertyBasedConstructor.GetParameters())
		// {
		// 	if (saltOffsetByType.TryGetValue(param.ParameterType, out var currentSaltOffsetForThisType))
		// 	{
		// 		saltOffsetByType[param.ParameterType] = ++currentSaltOffsetForThisType;
		// 	}
		// 	else
		// 	{
		// 		saltOffsetByType.Add(param.ParameterType, 0);
		// 	}
		// 	var saltOffset = saltOffsetByType[param.ParameterType];
		// 	var value = InstanciationExpressionBuilder.BuildInstanciationExpressionFor(param.ParameterType, salt+saltOffset);
		// 	constructorParameterValues.Add(value);
		// }

		// var instance =  propertyBasedConstructor.Invoke(constructorParameterValues.ToArray());
		// return instance;
	}

	ConstructorInfo FindPropertyBasedConstructor(Type type)
	{
		var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		var propertyBasedConstructor = constructors.First();
		return propertyBasedConstructor;
	}
}
