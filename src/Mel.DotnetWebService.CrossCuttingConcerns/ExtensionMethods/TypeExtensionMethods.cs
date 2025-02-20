using System.Collections;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

public static class TypeExtensionMethods
{
	static readonly string OrganizationNamespacePrefix
	= typeof(TypeExtensionMethods).Namespace!.Split('.').First();

	public static bool IsDefinedByOurOrganization(this Type type)
	=> type.Namespace is not null && type.Namespace.StartsWith(OrganizationNamespacePrefix);


	public static bool DefinesAValueObject(this Type type)
	=> type.IsDefinedByOurOrganization()
		&& type.GetMethods().Any(mi => mi.Name == "op_Implicit" || mi.Name == "op_Explicit");

	public static bool DefinesAFirstClassCollection(this Type type)
	=> type.IsDefinedByOurOrganization()
		&& type.DefinesConstructorWhoseParametersAllHaveAMatchingField(out var fields, out _)
		&& fields.Any(field => field.FieldType.IsEnumerableWithoutBeingString());

	public static bool DefinesUserDefinedConversions(this Type type, out MethodInfo[] converters)
	{
		var methods = type.GetMethods().ToArray();
		var implicitConverters = methods.Where(mi => mi.Name == "op_Implicit");
		var explicitConverters = methods.Where(mi => mi.Name == "op_Explicit");

		converters = implicitConverters
			.Concat(explicitConverters)
			.ToArray();
		return converters.Any();
	}

	public static MethodInfo[] GetUserDefinedConversions(this Type type)
	{
		var methods = type.GetMethods().ToArray();
		var implicitConverters = methods.Where(mi => mi.Name == "op_Implicit");
		var explicitConverters = methods.Where(mi => mi.Name == "op_Explicit");

		return implicitConverters
			.Concat(explicitConverters)
			.ToArray();
	}

	public static bool DefinesConstructorWhoseParametersAllHaveAMatchingField(this Type type, out FieldInfo[] fieldsToRender, out PropertyInfo[] propertiesToRender)
	{
		var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		var parametersOfEachConstructor = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.Select(ctor => ctor.GetParameters())
			.OrderByDescending(parameters => parameters.Length);

		foreach(var parametersOfOneConstructor in parametersOfEachConstructor)
		{
			var matchingProperties = new List<PropertyInfo>();
			var matchingFields = new List<FieldInfo>();
			foreach (var param in parametersOfOneConstructor)
			{
				var matchingProperty = properties.FirstOrDefault(propertyInfo => string.Equals(param.Name, propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase));
				if (matchingProperty != null)
				{
					matchingProperties.Add(matchingProperty);
					continue;
				}

				var matchingFieldName = fields.FirstOrDefault(field => string.Equals(param.Name, field.Name.TrimStart('_'), StringComparison.InvariantCultureIgnoreCase));
				if (matchingFieldName != null)
				{
					matchingFields.Add(matchingFieldName);
					continue;
				}

				break;
			}

			var numberOfMatchesFound = matchingProperties.Count + matchingFields.Count;
			var isConstructorWhoseParametersAllHaveAMatchingPropertyOrField = (numberOfMatchesFound == parametersOfOneConstructor.Length);
			if (isConstructorWhoseParametersAllHaveAMatchingPropertyOrField)
			{
				propertiesToRender = properties;
				fieldsToRender = matchingFields.ToArray();
				return true;
			}
		}

		propertiesToRender = Array.Empty<PropertyInfo>();
		fieldsToRender = Array.Empty<FieldInfo>();
		return false;
	}

	public static bool IsEnumerableWithoutBeingString(this Type type)
	=> type != typeof(string) && type.GetInterfaces().Contains(typeof(IEnumerable));

	public static bool IsReadOnlyCollectionWithoutBeingString(this Type type)
	=> type != typeof(string)
		&& type.GetInterfaces()
			.Where(i => i.IsGenericType)
			.Select(i => i.GetGenericTypeDefinition())
			.Any(i => i.IsAssignableFrom(typeof(IReadOnlyCollection<>)));
}
