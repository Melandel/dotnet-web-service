using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class ClassWithPublicParameterlessConstructorAndSettersGenerator : ParentObjectGenerator
{
	static readonly ClassWithPublicParameterlessConstructorAndSettersGenerator _instance = new();
	static readonly Dictionary<Type, PropertyInfo[]> SettablePropertiesByType = new();
	public static ClassWithPublicParameterlessConstructorAndSettersGenerator Instance(Type type, PropertyInfo[] settableProperties)
	{
		SettablePropertiesByType.TryAdd(type, settableProperties);
		return _instance;
	}
	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var instanceUnderConstruction = Activator.CreateInstance(type)!;
		var setProperties = SettablePropertiesByType[type];
		var saltOffsetByType = new Dictionary<Type, int>();
		foreach (var setProperty in setProperties)
		{
			if (saltOffsetByType.TryGetValue(setProperty.PropertyType, out var currentSaltOffsetForThisType))
			{
				saltOffsetByType[setProperty.PropertyType] = ++currentSaltOffsetForThisType;
			}
			else
			{
				saltOffsetByType.Add(setProperty.PropertyType, 0);
			}
			var saltOffset = saltOffsetByType[setProperty.PropertyType];

			var setPropertyExampleValue = ExampleValueGenerator.GenerateExampleOf(setProperty.PropertyType, salt+saltOffset);
			SetPropertyValue(setProperty, setPropertyExampleValue, instanceUnderConstruction);
		}

		return instanceUnderConstruction;
	}
	void SetPropertyValue(PropertyInfo settableProperty, object value, object instance)
	{
		try
		{
			settableProperty.SetValue(instance, value, null);
		}
		catch
		{
		}
	}
}

