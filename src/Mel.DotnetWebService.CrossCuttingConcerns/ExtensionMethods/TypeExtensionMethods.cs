using System.Collections;
using System.Data;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

public static class TypeExtensionMethods
{
	const string DeserializationFromJsonMethodName = nameof(ICanBeDeserializedFromJson<object>.DeserializeFromJson);
	static readonly Dictionary<Type, MethodInfo> EncounteredDeserializationMethodsByType = new Dictionary<Type, MethodInfo>();
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

	internal static bool IsOrInvolvesAConstrainedType(this Type type)
	=> type.IsAConstrainedType()
		|| type.IsGenericWithAConstrainedTypeArgument()
		|| type.HasFieldOrPropertieWithConstrainedType(browseRecursively: true);

	internal static bool IsAConstrainedType(this Type type)
	=> type switch
	{
		var _ when !type.IsDefinedByOurOrganization() => false,
		var _ when type.GetUserDefinedConversions().Any() => true,
		var _ when type.HasBaseType(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Constrained<>)) => true,
		_ => false
	};

	internal static Type GetConstrainedTypeRootType(this Type type)
	{
		var currentBaseType = type.BaseType;
		while (currentBaseType != null)
		{
			if (!currentBaseType.IsGenericType)
			{
				currentBaseType = currentBaseType!.BaseType;
				continue;
			}
			if (currentBaseType.GetGenericTypeDefinition() != typeof(Constrained<>))
			{
				currentBaseType = currentBaseType!.BaseType;
				continue;
			}

			return currentBaseType.GetGenericArguments().First();
		}

		throw new InvalidOperationException($"Parent type {typeof(Constrained<>).Name} not found in type {type.FullName}. Does it extend {typeof(Constrained<>).FullName}?");
	}

	internal static bool DefinesAFirstClassCollection(this Type type)
	=> type.IsDefinedByOurOrganization()
		&& type.DefinesConstructorWhoseParametersAllHaveAMatchingField(out var fields, out _, out _)
		&& fields.Any(field => field.FieldType.IsEnumerableWithoutBeingString());

	public static MethodInfo[] GetUserDefinedConversions(this Type type, bool browseParentTypes = false)
	{
		var converters = new List<MethodInfo>();
		var leafTypeMethods = type.GetMethods().ToArray();
		converters.AddRange(leafTypeMethods.Where(mi => mi.Name == "op_Implicit"));
		converters.AddRange(leafTypeMethods.Where(mi => mi.Name == "op_Explicit"));

		if (browseParentTypes)
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

	internal static bool DefinesConstructorWhoseParametersAllHaveAMatchingField(this Type type, out FieldInfo[] fieldsToRender, out PropertyInfo[] propertiesToRender, out int numberOfConstructorParameters)
	{
		var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		var parametersOfEachConstructor = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.Select(ctor => ctor.GetParameters())
			.OrderByDescending(parameters => parameters.Length);

		foreach (var parametersOfOneConstructor in parametersOfEachConstructor)
		{
			List<PropertyInfo> matchingProperties;
			List<FieldInfo> matchingFields;
			numberOfConstructorParameters = parametersOfOneConstructor.Length;
			var foundMatchingFieldsAndProperties = TryFindFieldsAndPropertiesWithNameMatchingParameterName(parametersOfOneConstructor, fields, properties, out matchingFields, out matchingProperties);
			if (!foundMatchingFieldsAndProperties)
			{
				foundMatchingFieldsAndProperties = TryFindFieldsAndPropertiesWithNameMatchingParameterTypeName(parametersOfOneConstructor, fields, properties, out matchingFields, out matchingProperties);
			}
			if (!foundMatchingFieldsAndProperties)
			{
				foundMatchingFieldsAndProperties = TryFindFieldsAndPropertiesWithTypeMatchingParameterType(parametersOfOneConstructor, fields, properties, out matchingFields, out matchingProperties);
			}

			if (foundMatchingFieldsAndProperties)
			{
				propertiesToRender = properties
					.Where(p => p.PropertyType != typeof(Type))
					.ToArray();
				fieldsToRender = matchingFields.ToArray();
				return true;
			}
		}

		propertiesToRender = Array.Empty<PropertyInfo>();
		fieldsToRender = Array.Empty<FieldInfo>();
		numberOfConstructorParameters = 0;
		return false;
	}

	static bool TryFindFieldsAndPropertiesWithNameMatchingParameterName(ParameterInfo[] parametersOfOneConstructor, FieldInfo[] fields, PropertyInfo[] properties, out List<FieldInfo> matchingFields, out List<PropertyInfo> matchingProperties)
	{
		matchingProperties = new List<PropertyInfo>();
		matchingFields = new List<FieldInfo>();
		foreach (var parameterName in parametersOfOneConstructor.Select(p => p.Name))
		{
			var propertiesWithNameMatchingCurrentParameterName = properties.Where(propertyInfo => string.Equals(parameterName, propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase));
			var fieldsWithNameMatchingCurrentParameterName = fields.Where(fieldInfo => string.Equals(parameterName, fieldInfo.Name.TrimStart('_'), StringComparison.InvariantCultureIgnoreCase));
			if (propertiesWithNameMatchingCurrentParameterName.Count() + matchingFields.Count() != 1)
			{
				return false;
			}

			matchingProperties.AddRange(propertiesWithNameMatchingCurrentParameterName);
			matchingFields.AddRange(fieldsWithNameMatchingCurrentParameterName);
		}

		return true;
	}

	static bool TryFindFieldsAndPropertiesWithNameMatchingParameterTypeName(ParameterInfo[] parametersOfOneConstructor, FieldInfo[] fields, PropertyInfo[] properties, out List<FieldInfo> matchingFields, out List<PropertyInfo> matchingProperties)
	{
		matchingProperties = new List<PropertyInfo>();
		matchingFields = new List<FieldInfo>();
		foreach (var parameterTypeName in parametersOfOneConstructor.Select(p => p.ParameterType.Name))
		{
			var propertiesMatchingCurrentParameterTypeName = properties.Where(propertyInfo => string.Equals(parameterTypeName, propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase));
			var fieldsMatchingCurrentParameterTypeName = fields.Where(fieldInfo => string.Equals(parameterTypeName, fieldInfo.Name.TrimStart('_'), StringComparison.InvariantCultureIgnoreCase));
			if (propertiesMatchingCurrentParameterTypeName.Count() + fieldsMatchingCurrentParameterTypeName.Count() != 1)
			{
				return false;
			}

			matchingProperties.AddRange(propertiesMatchingCurrentParameterTypeName);
			matchingFields.AddRange(fieldsMatchingCurrentParameterTypeName);
		}

		return true;
	}

	static bool TryFindFieldsAndPropertiesWithTypeMatchingParameterType(ParameterInfo[] parametersOfOneConstructor, FieldInfo[] fields, PropertyInfo[] properties, out List<FieldInfo> matchingFields, out List<PropertyInfo> matchingProperties)
	{
		matchingProperties = new List<PropertyInfo>();
		matchingFields = new List<FieldInfo>();
		foreach (var parameterType in parametersOfOneConstructor.Select(p => p.ParameterType))
		{
			var propertiesWithTypeMatchingCurrentParameterType = properties.Where(propertyInfo => propertyInfo.PropertyType == parameterType);
			var fieldsWithTypeMatchingCurrentParameterType = fields.Where(fieldInfo => fieldInfo.FieldType == parameterType);
			if (propertiesWithTypeMatchingCurrentParameterType.Count() + fieldsWithTypeMatchingCurrentParameterType.Count() != 1)
			{
				return false;
			}

			matchingProperties.AddRange(propertiesWithTypeMatchingCurrentParameterType);
			matchingFields.AddRange(fieldsWithTypeMatchingCurrentParameterType);
		}

		return true;
	}

	internal static bool IsEnumerableWithoutBeingString(this Type type)
	=> type != typeof(string) && type.GetInterfaces().Contains(typeof(IEnumerable));

	internal static bool IsReadOnlyCollectionWithoutBeingString(this Type type)
	=> type != typeof(string)
		&& type.GetInterfaces()
			.Where(i => i.IsGenericType)
			.Select(i => i.GetGenericTypeDefinition())
			.Any(i => i.IsAssignableFrom(typeof(IReadOnlyCollection<>)));

	public static bool Implements(this Type type, Type interfaceType)
	=> interfaceType switch
	{
		{ IsInterface: false } => false,
		{ IsGenericTypeDefinition: true } => type.GetInterfaces().Any(itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == interfaceType),
		{ IsGenericTypeDefinition: false } => type.GetInterfaces().Contains(interfaceType)
	};

	public static bool ImplementsGenericInterfaceWithGenericArgument(this Type type, Type expectedGenericInterfaceType, Type expectedGenericArgumentType)
	{
		var genericInterfaces = type.GetInterfaces().Where(itf => itf.IsGenericType);
		if (!genericInterfaces.Any())
		{
			return false;
		}

		var genericInterfacesWithExpectedGenericType = genericInterfaces.Where(itf => itf.GetGenericTypeDefinition() == expectedGenericInterfaceType);
		if (!genericInterfacesWithExpectedGenericType.Any())
		{
			return false;
		}

		foreach (var itf in genericInterfacesWithExpectedGenericType)
		{
			var genericArguments = itf.GetGenericArguments();
			if (genericArguments.Length != 1)
			{
				return false;
			}

			var genericArgumentType = genericArguments[0];
			if (genericArgumentType == expectedGenericArgumentType)
			{
				return true;
			}
		}

		return false;
	}

	public static bool ExtendsGenericClassWithGenericArgument(this Type type, Type expectedGenericClass, Func<Type, bool> expectedGenericArgumentTypePredicate)
	{
		var currentBaseType = type.BaseType;
		while (currentBaseType != null)
		{
			if (!currentBaseType.IsGenericType)
			{
				currentBaseType = currentBaseType!.BaseType;
				continue;
			}

			var currentBaseTypeGenericArgs = currentBaseType.GetGenericArguments();
			if (currentBaseTypeGenericArgs.Length != 1)
			{
				currentBaseType = currentBaseType!.BaseType;
				continue;
			}

			var currentBaseTypeSingleGenericArg = currentBaseTypeGenericArgs[0];
			if (expectedGenericArgumentTypePredicate.Invoke(currentBaseTypeSingleGenericArg) == true)
			{
				return true;
			}

			currentBaseType = currentBaseType!.BaseType;
		}

		return false;
	}

	public static object? DeserializeFromJson(this Type type, string? json)
	{
		if (json == null)
		{
			return null;
		}

		if (!EncounteredDeserializationMethodsByType.ContainsKey(type))
		{
			EncounteredDeserializationMethodsByType[type] = type.GetMethod(DeserializationFromJsonMethodName) switch
			{
				null => throw new InvalidOperationException($"Method {DeserializationFromJsonMethodName} not found in type {type.FullName}. Does it implement {typeof(ICanBeDeserializedFromJson<>).FullName}?"),
				var method => method
			};
		}

		var deserializationMethod = EncounteredDeserializationMethodsByType[type];
		return deserializationMethod.Invoke(null, new[] { json });
	}

	public static bool HasFieldOrPropertieWithConstrainedType(this Type type, bool browseRecursively = false)
	{
		var fieldTypes = type
		.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
		.Select(field => field.FieldType)
		.Distinct()
		.Where(fieldType => fieldType.Namespace is not null && !fieldType.Namespace.StartsWith("System"))
		.ToArray();

		if (fieldTypes.Any(fieldType => fieldType.IsAConstrainedType() || fieldType.IsArrayOfConstrainedTypeItems() || fieldType.IsGenericWithAConstrainedTypeArgument()))
		{
			return true;
		}

		var propertyTypes = type
		.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
		.Select(property => property.PropertyType)
		.Distinct()
		.Where(propertyType => propertyType.Namespace is not null && !propertyType.Namespace.StartsWith("System"))
		.ToArray();

		if (propertyTypes.Any(propertyType => propertyType.IsAConstrainedType() || propertyType.IsArrayOfConstrainedTypeItems()|| propertyType.IsGenericWithAConstrainedTypeArgument()))
		{
			return true;
		}

		if (browseRecursively)
		{
			var fieldsAndPropertiesTypes = fieldTypes.Union(propertyTypes);
			foreach (var fieldOrPropertyType in fieldsAndPropertiesTypes)
			{
				if (fieldOrPropertyType.HasFieldOrPropertieWithConstrainedType(browseRecursively: true))
				{
					return true;
				}
			}
		}

		return false;
	}

	public static bool IsArrayOfConstrainedTypeItems(this Type type)
	=> type.IsArray && type.GetElementType()!.IsAConstrainedType();

	public static bool IsGenericWithAConstrainedTypeArgument(this Type type)
	=> type.IsGenericType && type.GetGenericArguments().Any(arg => arg.IsAConstrainedType());

	internal static bool IsAGenericCollectionType(this Type type, Func<Type, bool> argTypePredicate)
	{
		var genericInterfaces = type.GetInterfaces().Where(itf => itf.IsGenericType);
		return genericInterfaces switch
		{
			var itfs when itfs.Any(itf => itf.GetGenericTypeDefinition() == typeof(IEnumerable<>)  && itf.GetGenericArguments().Any(argType => argTypePredicate.Invoke(argType) == true)) => true,
			var itfs when itfs.Any(itf => itf.GetGenericTypeDefinition() == typeof(IDictionary<,>) && itf.GetGenericArguments().Any(argType => argTypePredicate.Invoke(argType) == true)) => true,
			_ => false
		};
	}
}
