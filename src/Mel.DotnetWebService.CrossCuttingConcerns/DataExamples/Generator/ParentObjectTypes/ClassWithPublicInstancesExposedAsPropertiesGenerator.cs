using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithPublicInstancesExposedAsPropertiesGenerator : ParentObjectGenerator
{
	static readonly ClassWithPublicInstancesExposedAsPropertiesGenerator _instance = new();
	static readonly Dictionary<Type, Dictionary<int, PropertyInfo>> InstanceAccessPropertiesByType = [];
	public static ClassWithPublicInstancesExposedAsPropertiesGenerator Instance(Type type, PropertyInfo[] staticInstanceAccessProperties)
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
	ClassWithPublicInstancesExposedAsPropertiesGenerator() { }

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var instanceAccessPropertiesBySalt = InstanceAccessPropertiesByType[type];
		var singletonAccess = instanceAccessPropertiesBySalt[salt % instanceAccessPropertiesBySalt.Count];
		var instance = singletonAccess.GetValue(null)!;
		return instance;
	}
}
