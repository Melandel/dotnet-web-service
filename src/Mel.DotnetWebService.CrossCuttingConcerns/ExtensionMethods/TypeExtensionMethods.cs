﻿using System;
using System.Collections;
using System.Data;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

public static class TypeExtensionMethods
{
	static int i = 0;
	static int indent = 0;
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

	public static bool IsOrInvolvesAConstrainedType(this Type type)
	=> type.IsAConstrainedType()
		|| type.IsGenericWithATypeArgumentBeingOrInvolvingAConstrainedType()
		|| type.HasFieldOrPropertyWithConstrainedType(browseRecursively: true);

	public static bool IsAConstrainedType(this Type type)
	=> type switch
	{
		var _ when !type.IsDefinedByOurOrganization() => false,
		var _ when type.GetUserDefinedConversions().Any() => true,
		var _ when type.HasBaseType(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Constrained<>)) => true,
		_ => false
	};

	public static bool IsAConstrainedType(this Type type, out Type? rootType)
	{
		rootType = null;
		if (!type.IsDefinedByOurOrganization())
		{
			return false;
		}

		var userDefinedConversions = type.GetUserDefinedConversions();
		if (userDefinedConversions.Any())
		{
			rootType = userDefinedConversions.Last().ReturnType;
			return true;
		}

		var currentBaseType = type.BaseType;
		while (currentBaseType != null)
		{
			if (currentBaseType.IsGenericType && currentBaseType.GetGenericTypeDefinition() == typeof(Constrained<>))
			{
				rootType = currentBaseType.GetGenericArguments().First();
				return true;
			}

			currentBaseType = currentBaseType!.BaseType;
		}

		return false;
	}

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

	public static bool ExtendsGenericClassWithGenericArgument(this Type type, Type expectedGenericClass, Func<Type, bool>? expectedGenericArgumentTypePredicate = null)
	{
		var currentBaseType = type.BaseType;
		while (currentBaseType != null)
		{
			if (!currentBaseType.IsGenericType)
			{
				currentBaseType = currentBaseType!.BaseType;
				continue;
			}

			if (currentBaseType.GetGenericTypeDefinition() != expectedGenericClass)
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

			if (expectedGenericArgumentTypePredicate == null)
			{
				return true;
			}
			else
			{
				var currentBaseTypeSingleGenericArg = currentBaseTypeGenericArgs[0];
				if (expectedGenericArgumentTypePredicate.Invoke(currentBaseTypeSingleGenericArg) == true)
				{
					return true;
				}
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

	public static bool HasFieldOrPropertyWithConstrainedType(this Type type, bool browseRecursively = false, List<Type>? alreadyRecursedThrough = null)
	{
		if (type.FullName == "Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes.ClassArchetype+PositionalRecordContainingAnIEnumerableOfNonEmptyGuids")
		{
			var v = 0;
		}
		Console.WriteLine($"{i++,3} {new string(' ', 4 * indent)}{type.FullName}");
		indent++;

		if (type.IsEnum)
		{
			indent--;
			return false;
		}

		if (type.Namespace != null && type.Namespace.StartsWith("System.Collections"))
		{
			if (!type.IsGenericType)
			{
				indent--;
				return false;
			}
			alreadyRecursedThrough ??= new List<Type>();
			alreadyRecursedThrough.Add(type);
			var v = type.GetGenericArguments().Any(fieldType => fieldType.IsAConstrainedType() || fieldType.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) || fieldType.IsAGenericDictionaryType(argType => argType.IsOrInvolvesAConstrainedType()));
			indent--;
			return v;
		}

		if (type.Namespace != null && type.Namespace.StartsWith("System"))
		{
			indent--;
			return false;
		}

		alreadyRecursedThrough ??= new List<Type>();
		alreadyRecursedThrough.Add(type);
		var fs = type
		.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
		.Where(f => !alreadyRecursedThrough.Contains(f.FieldType) && !f.Name.Contains("k__BackingField")).ToArray();

		foreach (var f in fs)
		{
			Console.WriteLine($"{i++,-3} {new string(' ', 4 * indent)}field {f.FieldType.FullName} {f.Name}");
			if (f.FieldType.FullName == "Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids.NonEmptyGuid")
			{
				int x = 0;
			}
		}
		var fieldTypes = fs.Select(field => field.FieldType).Distinct()
		.ToArray();

		if (fieldTypes.Any(fieldType => fieldType.IsAConstrainedType() || fieldType.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) || fieldType.IsAGenericDictionaryType(argType => argType.IsOrInvolvesAConstrainedType())))
		{
			return true;
		}

		var ps = type
		.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
		.Where(prop => !alreadyRecursedThrough.Contains(prop.PropertyType)).ToArray();
		foreach (var p in ps)
		{
			Console.WriteLine($"{i++,3} {new string(' ', 4 * indent)}property {p.PropertyType.FullName} {p.Name}");
			if (p.PropertyType.FullName == "Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids.NonEmptyGuid")
			{
				int x = 0;
			}
		}
		var propertyTypes = ps.Select(property => property.PropertyType).Distinct()
		.ToArray();

		if (type.FullName.Contains("ClassArchetype"))
		{
			var v = 43;
		}
		if (propertyTypes.Any(propertyType => propertyType.IsAConstrainedType() || propertyType.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) || propertyType.IsAGenericDictionaryType(argType => argType.IsOrInvolvesAConstrainedType())))
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
		indent--;
		return false;
	}

	public static bool IsArrayOfConstrainedTypeItems(this Type type)
	=> type.IsArray && type.GetElementType()!.IsAConstrainedType();

	public static bool IsGenericWithATypeArgumentBeingOrInvolvingAConstrainedType(this Type type)
	=> type != typeof(Type) && type.IsGenericType && type.GetGenericArguments().Any(arg => arg.IsOrInvolvesAConstrainedType());

	internal static bool IsAGenericCollectionType(this Type type, Func<Type, bool> argTypePredicate)
	{
		var genericInterfaces = type.GetInterfaces().Where(itf => itf.IsGenericType);
		return genericInterfaces switch
		{
			var itfs when itfs.Any(itf => itf.GetGenericTypeDefinition() == typeof(IEnumerable<>)  && itf.GetGenericArguments().Any(argType => argTypePredicate.Invoke(argType) == true)) => true,
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
}
