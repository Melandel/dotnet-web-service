using System.Collections;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

public static class TypeExtensionMethods
{
	static readonly string OrganizationNamespacePrefix
	= typeof(TypeExtensionMethods).Namespace!.Split('.').First();

	public static bool IsDefinedByOurOrganization(this Type type)
	=> type.Namespace is not null && type.Namespace.StartsWith(OrganizationNamespacePrefix);

	public static string GetName(this Type type)
	=> type.GetGenericArguments() switch
	{
		{ Length: 0 } => GetCSharpTypeName(type),
		var genericArgumentTypes => String.Format(
			"{0}<{1}>",
			type.Name[..type.Name.IndexOf('`')],
			string.Join(", ", genericArgumentTypes.Select(arg => arg.GetName())))
	};

	static string GetCSharpTypeName(Type type)
	=> type.FullName switch
	{
		"System.Boolean" => "bool",
		"System.Byte" => "byte",
		"System.SByte" => "sbyte",
		"System.Char" => "char",
		"System.Decimal" => "decimal",
		"System.Double" => "double",
		"System.Single" => "float",
		"System.Int32" => "int",
		"System.UInt32" => "uint",
		"System.IntPtr" => "nint",
		"System.UIntPtr" => "nuint",
		"System.Int64" => "long",
		"System.UInt64" => "ulong",
		"System.Int16" => "short",
		"System.UInt16" => "ushort",
		"System.Object" => "object",
		"System.String" => "string",
		"System.Delegate" => "delegate",
		_ => type.Name
	};

	internal static bool DefinesAValueObject(this Type type)
	=> type switch
	{
		var _ when !type.IsDefinedByOurOrganization() => false,
		var _ when type.GetUserDefinedConversions().Any() => true,
		var _ when type.HasBaseType(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(EncapsulationOf<>)) => true,
		_ => false
	};

	internal static bool DefinesAFirstClassCollection(this Type type)
	=> type.IsDefinedByOurOrganization()
		&& type.DefinesConstructorWhoseParametersAllHaveAMatchingField(out var fields, out _)
		&& fields.Any(field => field.FieldType.IsEnumerableWithoutBeingString());

	public static MethodInfo[] GetUserDefinedConversions(this Type type, bool includeParentTypes = false)
	{
		var converters = new List<MethodInfo>();
		var leafTypeMethods = type.GetMethods().ToArray();
		converters.AddRange(leafTypeMethods.Where(mi => mi.Name == "op_Implicit"));
		converters.AddRange(leafTypeMethods.Where(mi => mi.Name == "op_Explicit"));

		if (includeParentTypes)
		{
			var currentBaseType = type.BaseType;
			while (currentBaseType != null)
			{
				converters.AddRange(currentBaseType.GetUserDefinedConversions());
				currentBaseType = currentBaseType!.BaseType;
			}
		}
		return converters.ToArray();
	}

	static bool HasBaseType(this Type type, Func<Type, bool> predicate)
	{
		var currentBaseType = type.BaseType;
		while (currentBaseType != null)
		{
			if (predicate.Invoke(currentBaseType) == true)
			{
				return true;
			}

			currentBaseType = currentBaseType!.BaseType;
		}

		return false;
	}

	internal static bool DefinesConstructorWhoseParametersAllHaveAMatchingField(this Type type, out FieldInfo[] fieldsToRender, out PropertyInfo[] propertiesToRender)
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

	internal static bool IsEnumerableWithoutBeingString(this Type type)
	=> type != typeof(string) && type.GetInterfaces().Contains(typeof(IEnumerable));

	internal static bool IsReadOnlyCollectionWithoutBeingString(this Type type)
	=> type != typeof(string)
		&& type.GetInterfaces()
			.Where(i => i.IsGenericType)
			.Select(i => i.GetGenericTypeDefinition())
			.Any(i => i.IsAssignableFrom(typeof(IReadOnlyCollection<>)));
}
