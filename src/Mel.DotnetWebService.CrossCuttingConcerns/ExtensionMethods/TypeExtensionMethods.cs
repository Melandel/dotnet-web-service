using System.Collections;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

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
		{ Length: 0 } => type.IsArray ? $"{type.GetElementType()!.GetName()}[]" : GetCSharpTypeName(type),
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

	static readonly HashSet<Type> TypesConsideredNativeAndScalar = new HashSet<Type>
	{
		typeof(bool),
		typeof(byte),
		typeof(DateTime),
		typeof(DateTimeOffset),
		typeof(decimal),
		typeof(double),
		typeof(float),
		typeof(Guid),
		typeof(int),
		typeof(long),
		typeof(nint),
		typeof(nuint),
		typeof(sbyte),
		typeof(short),
		typeof(string),
		typeof(uint),
		typeof(ulong),
		typeof(ushort)
	};

	static readonly HashSet<Type> SystemTypesThatCanThrowOnInstanciation = new HashSet<Type>
	{
		typeof(CultureInfo),
		typeof(RegionInfo),
		typeof(Version),
		typeof(Uri),
	};

	public static bool IsANativeScalarType(this Type type)
	=> TypesConsideredNativeAndScalar.Contains(type);

	public static bool IsASystemTypeThatCanThrowOnInstanciation(this Type type)
	=> SystemTypesThatCanThrowOnInstanciation.Contains(type);

	public static bool IsOrInvolvesAConstrainedType(this Type type)
	=> ConstrainedTypeInfos.Include(type)
		|| type.IsGenericWithATypeArgumentBeingOrInvolvingAConstrainedType()
		|| type.HasFieldOrPropertyWithConstrainedType(browseRecursively: true);

	public static MethodInfo[] GetUserDefinedConversions(this Type type, bool browseParentTypes = false)
	=> GetUserDefinedConversions(type, browseParentTypes, new List<Type>());
	static MethodInfo[] GetUserDefinedConversions(Type type, bool browseParentTypes, List<Type> typesAlreadyBrowsed)
	{
		if (typesAlreadyBrowsed.Contains(type))
		{
			return Array.Empty<MethodInfo>();
		}
		typesAlreadyBrowsed.Add(type);

		var converters = new List<MethodInfo>();
		var leafTypeMethods = type.GetMethods().ToArray();
		converters.AddRange(leafTypeMethods.Where(mi => mi.Name == "op_Implicit"));
		converters.AddRange(leafTypeMethods.Where(mi => mi.Name == "op_Explicit"));

		if (browseParentTypes)
		{
			var currentBaseType = type.BaseType;
			while (currentBaseType != null)
			{
				if (!currentBaseType.IsGenericType)
				{
					converters.AddRange(GetUserDefinedConversions(currentBaseType, false, typesAlreadyBrowsed));
				}
				else
				{
					var genericTypeDefinition = currentBaseType.GetGenericTypeDefinition();
					if (genericTypeDefinition == typeof(ConstrainedFurthermore<>))
					{
						var baseType = typeof(ConstrainedFurthermore<>).MakeGenericType(currentBaseType.GenericTypeArguments);
						converters.AddRange(GetUserDefinedConversions(baseType, true, typesAlreadyBrowsed));

						var furtherConstrainedType = currentBaseType.GetGenericArguments().First();
						converters.AddRange(GetUserDefinedConversions(furtherConstrainedType, true, typesAlreadyBrowsed));
					}
					else
					{
						converters.AddRange(GetUserDefinedConversions(currentBaseType, true, typesAlreadyBrowsed));
					}
				}

				currentBaseType = currentBaseType!.BaseType;
			}
		}
		return converters.ToArray();
	}

	public static MethodInfo[] GetStaticFactoryMethods(this Type type, BindingFlags accessModifiers)
	=> type
		.GetMethods(accessModifiers | BindingFlags.Static)
		.Where(method => method.ReturnParameter.ParameterType.IsAssignableFrom(type))
		.ToArray();

	internal static bool IsDeclaredAsAPositionalRecord(this Type type, out ConstructorInfo? constructor)
	{
		var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		if (constructors.Length != 2)
		{
			constructor = null;
			return false;
		}

		var cloningConstructor = constructors[1];
		var cloningConstructorParameters = cloningConstructor.GetParameters();
		if (cloningConstructorParameters.Length != 1)
		{
			constructor = null;
			return false;
		}

		if (cloningConstructorParameters.Single().ParameterType != type)
		{
			constructor = null;
			return false;
		}

		var setterProperties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.Where(prop => prop.PropertyType != typeof(Type))
			.ToArray();

		if (setterProperties.Length == 0)
		{
			constructor = null;
			return false;
		}

		if (!setterProperties.All(prop => prop.IsInitOnly()))
		{
			constructor = null;
			return false;
		}

		var propertyBasedConstructor = constructors[0];
		if (propertyBasedConstructor.GetParameters().Length != setterProperties.Length)
		{
			constructor = null;
			return false;
		}

		constructor = propertyBasedConstructor;
		return true;
	}

	internal static bool DefinesConstructorWhoseParametersAllHaveAMatchingField(this Type type, out FieldInfo[] fieldsToRender, out PropertyInfo[] propertiesToRender, out int numberOfConstructorParameters)
	{
		var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(prop => !prop.Name.Contains("k__BackingField")).ToArray();
		var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(prop => !prop.Name.Contains("k__BackingField")).ToArray();
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
			if (propertiesWithNameMatchingCurrentParameterName.Count() + fieldsWithNameMatchingCurrentParameterName.Count() != 1)
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
	=> type != typeof(string) && (type.GetInterfaces().Contains(typeof(IEnumerable)) || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable));

	internal static bool IsReadOnlyCollectionWithoutBeingString(this Type type)
	=> type != typeof(string)
		&& type.GetInterfaces()
			.Where(i => i.IsGenericType)
			.Select(i => i.GetGenericTypeDefinition())
			.Any(i => i.IsAssignableFrom(typeof(IReadOnlyCollection<>)));

	public static bool IsOrExtends(this Type type, Type baseType)
	{
		if (!type.IsGenericType && !baseType.IsGenericType && type == baseType)
		{
			return true;
		}

		if (type.IsGenericType && type.IsGenericTypeDefinition && baseType.IsGenericType && baseType.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == baseType.GetGenericTypeDefinition())
		{
			return true;
		}

		if (type.IsGenericType && !type.IsGenericTypeDefinition && baseType.IsGenericType && !baseType.IsGenericTypeDefinition && type == baseType)
		{
			return true;
		}

		if (type.IsGenericType && !type.IsGenericTypeDefinition && baseType.IsGenericType && baseType.IsGenericTypeDefinition && type.GetGenericTypeDefinition() == baseType.GetGenericTypeDefinition())
		{
			return true;
		}
		var currentBaseType = type.BaseType;
		while (currentBaseType != null)
		{
			if (currentBaseType.IsOrExtends(baseType))
			{
				return true;
			}

			currentBaseType = currentBaseType!.BaseType;
		}

		return false;
	}

	public static bool IsOrImplementsInterface(this Type type, Type interfaceType)
	=> interfaceType switch
	{
		{ IsInterface: true } => type == interfaceType || type.GetInterfaces().Any(itf => itf.ImplementsInterface(interfaceType)),
		var itf => itf.ImplementsInterface(interfaceType)
	};

	public static bool ImplementsInterface(this Type type, Type interfaceType)
	=> interfaceType switch
	{
		//{ IsInterface: false } => false,
		{ IsGenericTypeDefinition: true } => type.GetInterfaces().Any(itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == interfaceType),
		{ IsGenericTypeDefinition: false } => type.GetInterfaces().Contains(interfaceType)
	};

	public static bool IsOrImplementsGenericInterface(this Type type, Type interfaceType, out Type[] argumentTypes)
	{
		if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == interfaceType)
		{
			argumentTypes = type.GetGenericArguments();
			return true;
		}

		return type.ImplementsGenericInterface(interfaceType, out argumentTypes);
	}

	public static bool ImplementsGenericInterface(this Type type, Type interfaceType, out Type[] argumentTypes)
	{
		if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == interfaceType)
		{
			argumentTypes = type.GetGenericArguments();
			return true;
		}

		var matchingInterface = type
			.GetInterfaces()
			.FirstOrDefault(itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == interfaceType);
		if (matchingInterface == null)
		{
			argumentTypes = Array.Empty<Type>();
			return false;
		}

		argumentTypes = matchingInterface.GetGenericArguments();
		return true;
	}

	public static bool ImplementsGenericDictionary(this Type type, out Type keyType, out Type valueType)
	{
		keyType = typeof(object);
		valueType = typeof(object);
		if (type == typeof(Type))
		{
			return false;
		}

		if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
		{
			var kvpTypes = type.GetGenericArguments();
			keyType = kvpTypes.First();
			valueType = kvpTypes.Last();
			return true;
		}

		var iDictionaryInterfaces = type.GetInterfaces().Where(itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IDictionary<,>)).ToArray();
		if (!iDictionaryInterfaces.Any())
		{
			return false;
		}

		var dictionaryInterfacePreferredCandidate = iDictionaryInterfaces.First();
		var argTypes = dictionaryInterfacePreferredCandidate.GetGenericArguments();
		keyType = argTypes.First();
		valueType = argTypes.Last();
		return true;
	}

	public static bool ImplementsGenericIEnumerableOfKeyPairValues(this Type type, out Type keyType, out Type valueType)
	{
		keyType = typeof(object);
		valueType = typeof(object);
		if (type == typeof(Type))
		{
			return false;
		}

		if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
		{
			var argTypes = type.GetGenericArguments();
			var singleArgType = argTypes.Single();
			if (!singleArgType.IsGenericType)
			{
				return false;
			}
			var singleArgTypeArgType = singleArgType.GetGenericTypeDefinition();
			if (singleArgTypeArgType != typeof(KeyValuePair<,>))
			{
				return false;
			}

			var kvpTypes = singleArgType.GetGenericArguments();
			keyType = kvpTypes.First();
			valueType = kvpTypes.Last();
			return true;
		}

		var ienumerableInterfaces = type.GetInterfaces().Where(itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IEnumerable<>)).ToArray();
		if (!ienumerableInterfaces.Any())
		{
			return false;
		}

		foreach (var itf in ienumerableInterfaces)
		{
			var argTypes = itf.GetGenericArguments();
			var singleArgType = argTypes.Single();
			if (!singleArgType.IsGenericType)
			{
				continue;
			}
			var singleArgTypeArgType = singleArgType.GetGenericTypeDefinition();
			if (singleArgTypeArgType != typeof(KeyValuePair<,>))
			{
				continue;
			}

			var kvpTypes = singleArgType.GetGenericArguments();
			keyType = kvpTypes.First();
			valueType = kvpTypes.Last();
			return true;
		}

		return false;
	}
	public static bool ImplementsGenericIEnumerableWithAnArgumentTypeThatVerifies(this Type type, Func<Type, bool> predicate)
	{
		if (type == typeof(Type))
		{
			return false;
		}

		if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) && type.GetGenericArguments().Any(arg => predicate.Invoke(arg) == true))
		{
			return true;
		}

		var ienumerableInterfaces = type.GetInterfaces().Where(itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IEnumerable<>)).ToArray();
		if (!ienumerableInterfaces.Any())
		{
			return false;
		}

		var genericInterfacesSatisfyingThePredicate = ienumerableInterfaces.Where(itf => itf.GetGenericArguments().Any(arg => predicate.Invoke(arg) == true)).ToArray();
		if (genericInterfacesSatisfyingThePredicate.Any())
		{
			return true;
		}

		return false;
	}

	public static bool HasFieldOrPropertyWithConstrainedType(this Type type, bool browseRecursively = false, HashSet<Type>? alreadyRecursedThrough = null)
	{
		if (type.IsEnum)
		{
			return false;
		}

		if (type.Namespace != null && type.Namespace.StartsWith("System.Collections"))
		{
			if (!type.IsGenericType)
			{
				return false;
			}
			alreadyRecursedThrough ??= new HashSet<Type>();
			alreadyRecursedThrough.Add(type);
			var v = type.GetGenericArguments().Any(fieldType => ConstrainedTypeInfos.Include(fieldType) || fieldType.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) || fieldType.IsAGenericDictionaryType(argType => argType.IsOrInvolvesAConstrainedType()));
			return v;
		}

		if (type.Namespace != null && type.Namespace.StartsWith("System"))
		{
			return false;
		}

		alreadyRecursedThrough ??= new HashSet<Type>();
		alreadyRecursedThrough.Add(type);
		var fs = type
		.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
		.Where(f => !alreadyRecursedThrough.Contains(f.FieldType) && !f.Name.Contains("k__BackingField")).ToArray();

		var fieldTypes = fs.Select(field => field.FieldType).Distinct()
		.ToArray();

		if (fieldTypes.Any(fieldType => ConstrainedTypeInfos.Include(fieldType) || fieldType.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) || fieldType.IsAGenericDictionaryType(argType => argType.IsOrInvolvesAConstrainedType())))
		{
			return true;
		}

		var ps = type
		.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
		.Where(prop => !alreadyRecursedThrough.Contains(prop.PropertyType)).ToArray();
		var propertyTypes = ps.Select(property => property.PropertyType).Distinct()
		.ToArray();

		if (propertyTypes.Any(propertyType => ConstrainedTypeInfos.Include(propertyType) || propertyType.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) || propertyType.IsAGenericDictionaryType(argType => argType.IsOrInvolvesAConstrainedType())))
		{
			return true;
		}

		var parentTypeName = type.IsGenericType
			? $"{type.GetGenericTypeDefinition()}<{string.Join(",", type.GetGenericArguments().Select(t => t.Name))}>"
			: type.Name;
		if (browseRecursively)
		{
			var fieldsAndPropertiesTypes = fieldTypes.Union(propertyTypes).Except(alreadyRecursedThrough);
			foreach (var fieldOrPropertyType in fieldsAndPropertiesTypes)
			{
		var typeName = fieldOrPropertyType.IsGenericType
			? $"{fieldOrPropertyType.GetGenericTypeDefinition()}<{string.Join(",", fieldOrPropertyType.GetGenericArguments().Select(t => t.Name))}>"
			: fieldOrPropertyType.Name;
				if (fieldOrPropertyType.HasFieldOrPropertyWithConstrainedType(browseRecursively: true, alreadyRecursedThrough))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsGenericWithATypeArgumentBeingOrInvolvingAConstrainedType(this Type type)
	=> type != typeof(Type) && type.IsGenericType && type.GetGenericArguments().Any(arg => arg.IsOrInvolvesAConstrainedType() || arg.IsGenericWithATypeArgumentBeingOrInvolvingAConstrainedType());

	internal static bool IsAGenericCollectionType(this Type type, Func<Type, bool> argTypePredicate)
	{
		if (type == typeof(Type))
		{
			return false;
		}

		if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>) && type.GetGenericArguments().Any(arg => argTypePredicate.Invoke(arg) == true))
		{
			return true;
		}

		var genericInterfaces = type.GetInterfaces().Where(itf => itf.IsGenericType);
		return genericInterfaces switch
		{
			var itfs when itfs.Any(itf => itf.GetGenericTypeDefinition() == typeof(IEnumerable<>)  && itf.GetGenericArguments().Any(argType =>
			{
				return argTypePredicate.Invoke(argType) == true;
			})) => true,
			_ => false
		};
	}

	internal static bool IsAGenericDictionaryType(this Type type, Func<Type, bool> argTypePredicate)
	{
		var interfaces = new List<Type>();
		if (type.IsInterface)
		{
			interfaces.Add(type);
		}
		interfaces.AddRange(type.GetInterfaces());
		var itfs = interfaces
			.Where(itf => itf.IsGenericType);

		foreach (var itf in itfs.Where(itf => itf.GetGenericTypeDefinition() == typeof(IDictionary<,>)))
		{
			foreach (var argType in itf.GetGenericArguments())
			{
				var v = argTypePredicate.Invoke(argType);
				if (v)
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static Type GetCollectionItemType(this Type collectionType)
	{
		return collectionType switch
		{
			{ IsArray: true } => collectionType.GetElementType()!,
			var t when t.ImplementsGenericInterface(typeof(IEnumerable<>), out var argTypes) => argTypes.First(),
			var t when t.ImplementsGenericInterface(typeof(IConstrainedCollection<,>), out var argTypes) => argTypes.First(),
			_ => throw new InvalidOperationException($"{nameof(Type)}.{nameof(GetCollectionItemType)} should be called on a collection, but was called on {collectionType.FullName} instead.")
		};
	}

	public static bool IsAConstrainedDictionary(this Type type, out Type? collectionTypeUsedAsConstructorParameter, out Type[] collectionTypesUsedAsInstanciationParameter)
	{
		var b = type.IsAFirstClassCollection(out collectionTypeUsedAsConstructorParameter, out collectionTypesUsedAsInstanciationParameter);
		if (type.IsAGenericDictionaryType(arg => true))
		{
			return true;
		}
		return false;
	}

	public static bool IsClassWithPublicStaticFactoryMethod(this Type type, out MethodInfo[] publicStaticFactoryMethods)
	{
		publicStaticFactoryMethods = type
			.GetMethods(BindingFlags.Public | BindingFlags.Static)
			.Where(method => method.ReturnParameter.ParameterType.IsAssignableFrom(type))
			.Where(method => !method.IsSpecialName)
			.ToArray();

		return publicStaticFactoryMethods.Length != 0;
	}

	public static bool IsClassWithPublicConstructorHavingParameters(this Type type, out ConstructorInfo[] constructorsWithParameters)
	{
		constructorsWithParameters = type
			.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
			.Where(c => c.GetParameters().Length > 0)
			.ToArray();
		return constructorsWithParameters.Length > 0;

	}

	public static bool IsClassWithPublicParameterlessConstructorAndSetters(this Type type, out PropertyInfo[] settableProperties)
	{
		settableProperties = type.GetProperties().Where(prop => prop.GetSetMethod() != null).ToArray();
		var parameterlessConstructors = type
			.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
			.Where(c => c.GetParameters().Length == 0)
			.ToArray();
		return parameterlessConstructors.Length == 1;
	}

	public static bool IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(this Type type, out PropertyInfo? singletonAccessProperty)
	{
		var singletonAccessProperties = type
			.GetProperties(BindingFlags.Public | BindingFlags.Static)
			.Where(p =>
					p.PropertyType.IsAssignableFrom(type) &&
					p.GetMethod != null &&
					p.GetMethod.IsPublic &&
					p.GetMethod.IsStatic &&
					p.SetMethod == null)
				.ToArray();
		if (singletonAccessProperties.Length != 1)
		{
			singletonAccessProperty = null;
			return false;
		}
		singletonAccessProperty = singletonAccessProperties.First();
		return true;
	}

	public static bool IsClassWithSinglePublicAccessToSingletonInstanceThroughField(this Type type, out FieldInfo? singletonAccessField)
	{
		var singletonAccessFields = type
			.GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(f => f.FieldType == type)
			.ToArray();
		if (singletonAccessFields.Length != 1)
		{
			singletonAccessField = null;
			return false;
		}
		singletonAccessField = singletonAccessFields.First();
		return true;
	}

	public static bool IsClassWithPublicInstancesExposedAsStaticReadonlyFields(this Type type, out FieldInfo[] instanceAccessStaticReadonlyFields)
	{
		instanceAccessStaticReadonlyFields = type
			.GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(f => f.FieldType.IsAssignableTo(type) && f.IsInitOnly)
			.ToArray();
		return instanceAccessStaticReadonlyFields.Length > 0;
	}

	public static bool IsClassWithPublicInstancesExposedAsStaticFields(this Type type, out FieldInfo[] instanceAccessStaticFields)
	{
		instanceAccessStaticFields = type
			.GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(f => f.FieldType.IsAssignableTo(type))
			.ToArray();
		return instanceAccessStaticFields.Length > 0;
	}

	public static bool IsClassWithPublicInstancesExposedAsProperties(this Type type, out PropertyInfo[] instanceAccessStaticProperties)
	{
		instanceAccessStaticProperties = type
			.GetProperties(BindingFlags.Public | BindingFlags.Static)
			.Where(p => p.PropertyType.IsAssignableTo(type))
			.ToArray();
		return instanceAccessStaticProperties.Length > 0;
	}

	public static bool IsAFirstClassCollection(this Type type, out Type? collectionTypeUsedAsConstructorParameter, out Type[] collectionTypesUsedAsInstanciationParameter)
	{
		if (type.HasSingleConstructorAndThatConstructorHasSingleParameterAndThatParameterIsACollectionType(BindingFlags.Public, out collectionTypeUsedAsConstructorParameter)
			&& type.HasCollectionFieldOrPropertyOf(collectionTypeUsedAsConstructorParameter!))
		{
			collectionTypesUsedAsInstanciationParameter = new[] { collectionTypeUsedAsConstructorParameter! };
			return true;
		}

		if(type.HasSingleConstructorAndThatConstructorHasSingleParameterAndThatParameterIsACollectionType(BindingFlags.NonPublic, out collectionTypeUsedAsConstructorParameter)
			&& type.HasCollectionFieldOrPropertyOf(collectionTypeUsedAsConstructorParameter!)
			&& type.HasStaticFactoryMethodsTakingSingleParameterThatIsCompatible(collectionTypeUsedAsConstructorParameter!, BindingFlags.Public, out var factoryMethods))
		{
			collectionTypesUsedAsInstanciationParameter = factoryMethods.Select(m => m.GetParameters().Single().ParameterType).ToArray();
			return true;
		}

		collectionTypeUsedAsConstructorParameter = null;
		collectionTypesUsedAsInstanciationParameter = Array.Empty<Type>();
		return false;
	}

	static bool HasSingleConstructorAndThatConstructorHasSingleParameterAndThatParameterIsACollectionType(this Type type, BindingFlags accessModifier, out Type? collectionTypeUsedAsConstructorParameter)
	{
		var constructors = type.GetConstructors(accessModifier | BindingFlags.Instance);
		if (constructors.Length != 1)
		{
			collectionTypeUsedAsConstructorParameter = null;
			return false;
		}
		var constructor = constructors.First();
		var parameters = constructor.GetParameters();
		if (parameters.Length != 1)
		{
			collectionTypeUsedAsConstructorParameter = null;
			return false;
		}

		var parameter = parameters.First();
		if (parameter.ParameterType.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argumentTypes))
		{
			collectionTypeUsedAsConstructorParameter = parameter.ParameterType;
			return true;
		}

		collectionTypeUsedAsConstructorParameter = null;
		return false;
	}

	public static bool HasConstructorsTakingSingleParameterAssignableFrom(this Type type, Type parameterType, BindingFlags accessModifiers, out ConstructorInfo[] constructors)
	{
		var allPublicConstructors = type
			.GetConstructors(accessModifiers | BindingFlags.Instance);

		constructors = allPublicConstructors.Where(ctr => ctr.GetParameters().Count() == 1).ToArray();
		return constructors.Any();
	}

	public static bool HasStaticFactoryMethodsTakingSingleParameterThatIsCompatible(this Type type, Type expectedParameterType, BindingFlags accessModifiers, out MethodInfo[] factoryMethods)
	{
		var allPublicOrInternalStaticFactoryMethods = type
			.GetMethods(accessModifiers | BindingFlags.Static)
			.Where(method => method.ReturnParameter.ParameterType.IsAssignableFrom(type))
			.Where(method => method.GetParameters() switch
			{
				[var p] when p.ParameterType.IsAssignableFrom(expectedParameterType) => true,
				[var p] when p.ParameterType != typeof(object) && p.ParameterType.IsAssignableTo(expectedParameterType) => true,
				[var p] when
					expectedParameterType.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var paramTypeArgumentTypes)
					&& p.ParameterType.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var staticMethodParameterTypeArgumentTypes)
					&& (staticMethodParameterTypeArgumentTypes.First() == paramTypeArgumentTypes.First()
						|| ConstrainedTypeInfos.TryGet(paramTypeArgumentTypes.First(), out var constrainedTypeInfo) && constrainedTypeInfo.RootType == staticMethodParameterTypeArgumentTypes.First()) => true,
				_ => false
			});

		factoryMethods = allPublicOrInternalStaticFactoryMethods.Where(factoryMethod => factoryMethod.GetParameters().Count() == 1).ToArray();
		return factoryMethods.Any();
	}

	public static object CreateInstanceUsingConstructorOrFactoryMethod(this Type type, object instanciationParameterValue, BindingFlags accessModifiers)
	{
		return instanciationParameterValue.GetType() switch
		{
			Type paramType when type.HasConstructorsTakingSingleParameterAssignableFrom(paramType, accessModifiers, out var constructors) => type.CreateInstanceUsingAConstructor(instanciationParameterValue, constructors),
			Type paramType when type.HasStaticFactoryMethodsTakingSingleParameterThatIsCompatible(paramType, accessModifiers, out var factoryMethods) => type.CreateInstanceUsingAFactoryMethod(instanciationParameterValue, factoryMethods),
			_ => throw new NotImplementedException()
		};
	}

	static object CreateInstanceUsingAConstructor(this Type type, object parameterValue, ConstructorInfo[] constructors)
	{
		object? constructed = null;
		foreach (var constructor in constructors)
		{
			try	{ constructed = constructor.Invoke(new[] { parameterValue }); }
			catch { }
		}
		return constructed ?? throw new InvalidOperationException($"{type.FullName} instance creation failed using {nameof(Type)}.{nameof(CreateInstanceUsingAConstructor)} on {parameterValue.GetStringRepresentation()}.");
	}
	static object CreateInstanceUsingAFactoryMethod(this Type type, object parameterValue, MethodInfo[] factoryMethods)
	{
		object? constructed = null;
		foreach (var factoryMethod in factoryMethods)
		{
			try { constructed = factoryMethod.Invoke(null, new[] { parameterValue })!; }
			catch { }
		}
		return constructed ?? throw new InvalidOperationException($"{type.FullName} instance creation failed using {nameof(Type)}.{nameof(CreateInstanceUsingAFactoryMethod)} on {parameterValue.GetStringRepresentation()}.");
	}
	public static bool HasCollectionFieldOrPropertyOf(this Type type, Type collectionType)
	{
		var itemType = collectionType.GetCollectionItemType();

		if (type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).Any(f => f.FieldType.ImplementsGenericIEnumerableWithAnArgumentTypeThatVerifies(argType => argType.IsAssignableFrom(itemType) || argType != typeof(object) && argType.IsAssignableTo(itemType))))
		{
			return true;
		}

		if (type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Any(prop => prop.PropertyType.ImplementsGenericIEnumerableWithAnArgumentTypeThatVerifies(argType => argType.IsAssignableFrom(itemType) || argType != typeof(object) && argType.IsAssignableTo(itemType))))
		{
			return true;
		}

		return false;
	}

	internal static bool IsDeclaredAsAGetSetStyleClass(this Type type)
	{
		var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		if (constructors.Length != 1)
		{
			return false;
		}
		var parameterlessConstructor = constructors[0];
		if (parameterlessConstructor.GetParameters().Length != 0)
		{
			return false;
		}

		var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			.Where(prop => prop.PropertyType != typeof(Type))
			.ToArray();

		if (properties.Any(prop => prop.IsInitOnly() || prop.GetSetMethod() == null))
		{
			return false;
		}

		return true;
	}

	static readonly Type[] TypesThatGetDeserializedFromString = new[]
	{
		typeof(string),
		typeof(char),
		typeof(DateTime),
		typeof(DateTimeOffset),
		typeof(TimeSpan),
		typeof(Guid),
		typeof(Uri),
		typeof(Version),
		typeof(DateOnly),
		typeof(TimeOnly),
	};
	public static bool IsSerializedAsString(this Type type)
	=> TypesThatGetDeserializedFromString.Contains(type);
}
