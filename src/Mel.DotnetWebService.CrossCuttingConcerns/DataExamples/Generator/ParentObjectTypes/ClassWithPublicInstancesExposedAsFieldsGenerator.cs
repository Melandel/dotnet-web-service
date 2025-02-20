using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ClassWithPublicInstancesExposedAsFieldsGenerator : ParentObjectGenerator
{
	static readonly ClassWithPublicInstancesExposedAsFieldsGenerator _instance = new();
	static readonly Dictionary<Type, Dictionary<int, FieldInfo>> InstanceAccessFieldsByType = [];
	public static ClassWithPublicInstancesExposedAsFieldsGenerator Instance(Type type, FieldInfo[] staticInstanceAccessFields)
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
	ClassWithPublicInstancesExposedAsFieldsGenerator() { }

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var instanceAccessFieldsBySalt = InstanceAccessFieldsByType[type];
		var singletonAccess = instanceAccessFieldsBySalt[salt % instanceAccessFieldsBySalt.Count];
		var instance = singletonAccess.GetValue(null)!;
		return instance;
	}
}
