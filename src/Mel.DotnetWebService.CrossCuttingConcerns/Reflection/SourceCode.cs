using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection;

public static class SourceCode
{
	public static T[] GetAllPublicStaticFieldsValuesOfType<T>()
	{
		return typeof(T)
			.GetMembers(BindingFlags.Static | BindingFlags.Public)
			.OfType<FieldInfo>()
			.Where(f => typeof(T).IsAssignableFrom(f.FieldType))
			.Select(f => (T)f.GetValue(null)!)
			.ToArray();
	}
}
