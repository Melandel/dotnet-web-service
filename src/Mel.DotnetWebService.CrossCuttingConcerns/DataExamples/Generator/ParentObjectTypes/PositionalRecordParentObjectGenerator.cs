using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class PositionalRecordParentObjectGenerator : ParentObjectGenerator
{
	static readonly PositionalRecordParentObjectGenerator _instance = new();
	static readonly Dictionary<Type, ConstructorInfo> PropertyBasedConstructorsByPositionalRecordType = [];

	public static PositionalRecordParentObjectGenerator Instance(Type targetType, ConstructorInfo propertyBasedConstructor)
	{
		PropertyBasedConstructorsByPositionalRecordType.TryAdd(targetType, propertyBasedConstructor);
		return _instance;
	}

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var propertyBasedConstructor = FindPropertyBasedConstructor(type);
		var parameters = GenerateParameterExamplesFor(propertyBasedConstructor, salt);

		var instance = propertyBasedConstructor.Invoke(parameters);
		return instance;
	}

	ConstructorInfo FindPropertyBasedConstructor(Type type)
	{
		var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		var propertyBasedConstructor = constructors.First();
		return propertyBasedConstructor;
	}
}
