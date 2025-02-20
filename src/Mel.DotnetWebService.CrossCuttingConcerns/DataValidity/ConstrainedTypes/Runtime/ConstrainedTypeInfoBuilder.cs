using System.Collections;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Core;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

public class ConstrainedTypeInfoBuilder
{
	readonly Type _type;
	readonly ConstrainedTypeCategory _typeCategory;
	Type? _rootType;
	IEnumerable<dynamic>? _validScalarExamples;
	Dictionary<dynamic, string>? _invalidScalarExamples;
	MethodInfo[]? _successionOfImplicitConversionsEndingInRootType;
	MethodInfo? _instanciationMethod; // Func<TRoot, TConstrained>
	ConstrainedTypeInfoBuilder(Type type)
	{
		_type = type switch
		{
			var t when !t.ImplementsInterface(typeof(IConstrainedType)) => throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>($"{nameof(_type)} must implement {typeof(IConstrainedType).GetName()}.", this, type),
			_ => type
		};

		_typeCategory = type switch
		{
			var t when t.ImplementsGenericInterface(typeof(IConstrainedValue<,>), out _) => ConstrainedTypeCategory.Value,
			var t when t.ImplementsGenericInterface(typeof(IConstrainedCollection<,>), out _) => ConstrainedTypeCategory.GenericCollection,
			_ => throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>($"{nameof(_type)} must implement either {typeof(IConstrainedValue<,>).GetName()}, either {typeof(IConstrainedCollection<,>).GetName()}.", this, type)
		};
	}

	public static ConstrainedTypeInfoBuilder For(Type type)
		=> new(type);

	public ConstrainedTypeInfo Build()
	{
		try
		{
			return _typeCategory switch
			{
				ConstrainedTypeCategory.Value => BuildConstrainedValueTypeInfo(),
				ConstrainedTypeCategory.GenericCollection => BuildConstrainedGenericCollectionTypeInfo(),
				_ => throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>($"{nameof(_type)} must implement either {typeof(IConstrainedValue<,>).GetName()}, either {typeof(IConstrainedCollection<,>).GetName()}.", this)
			};
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureViaAnotherClassContextWith<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(this); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(defect,   this); }
	}

	ConstrainedTypeInfo BuildConstrainedValueTypeInfo()
	{
		_rootType = GetRootType(_type);
		_successionOfImplicitConversionsEndingInRootType = GetSuccessionOfImplicitConversionsEndingInRootType(_type, _rootType);

		_validScalarExamples = GetValidScalarExamples(_type, _rootType);
		_invalidScalarExamples = GetInvalidScalarExamples(_type, _rootType);
		_type.ImplementsGenericInterface(typeof(IConstrainedValue<,>), out var constrainedValueArgTypes);
		_instanciationMethod = _type
			.GetInterfaceMap(typeof(IConstrainedValue<,>).MakeGenericType(constrainedValueArgTypes))
			.InterfaceMethods
			.Single(m => m.Name == nameof(IConstrainedValue<Guid, NonEmptyGuid>.ApplyConstraintsTo));

		return ConstrainedTypeInfo.CreateForValueType(
			_type,
			_rootType,
			_validScalarExamples,
			_invalidScalarExamples,
			_successionOfImplicitConversionsEndingInRootType,
			_instanciationMethod);
	}

	ConstrainedTypeInfo BuildConstrainedGenericCollectionTypeInfo()
	{
		_rootType = GetRootType(_type);
		_successionOfImplicitConversionsEndingInRootType = GetSuccessionOfImplicitConversionsEndingInRootType(_type, _rootType);

		_type.ImplementsGenericInterface(typeof(IConstrainedCollection<,>), out var constrainedCollectionArgTypes);
		_instanciationMethod = _type
					.GetInterfaceMap(typeof(IConstrainedCollection<,>).MakeGenericType(constrainedCollectionArgTypes))
					.InterfaceMethods
					.Single(m => m.Name == nameof(IConstrainedCollection<Guid, NonEmptyGuid>.ApplyConstraintsTo));

		return ConstrainedTypeInfo.CreateForGenericCollectionType(
			_type,
			_rootType,
			_validScalarExamples,
			_invalidScalarExamples);
	}

	Type GetRootType(Type type)
	{
		try
		{
			var currentBaseType = type.BaseType;
			var constrainedTypesToImplement = new[]
			{
				typeof(ConstrainedValue<>),
				typeof(ConstrainedCollection<>)
			};
			while (currentBaseType != null)
			{
				if (!currentBaseType.IsGenericType)
				{
					currentBaseType = currentBaseType!.BaseType;
					continue;
				}

				var genericTypeDefinition = currentBaseType.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(ConstrainedFurthermore<>))
				{
					return GetRootType(currentBaseType.GetGenericArguments().First());
				}

				if (!constrainedTypesToImplement.Contains(genericTypeDefinition))
				{
					currentBaseType = currentBaseType!.BaseType;
					continue;
				}

				return currentBaseType.GetGenericArguments().First();
			}

			throw new InvalidOperationException($"Parent type {typeof(ConstrainedValue<>).GetName()} not found in type {type.FullName}. Does it extend {typeof(ConstrainedValue<>).FullName}?");
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureViaAnotherClassContextWith<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(this, type); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(defect,   this, type); }
	}

	IEnumerable<dynamic> GetValidScalarExamples(Type type, Type rootType)
	{
		try
		{
			var examplesProperty = type.GetProperty(nameof(IConstrainedValue<int, ConstrainedInt>.Examples), BindingFlags.Static | BindingFlags.Public)!;
			var examples = examplesProperty.GetValue(null, null);
			var validValuesProperty = typeof(ExampleValues<>).MakeGenericType(rootType).GetProperty(nameof(ExampleValues<object>.ValidValues));
			var validValues = validValuesProperty!.GetValue(examples)!;
			var validValuesAsEnumerable = validValues as IEnumerable;

			var list = new List<dynamic>();
			foreach(var value in validValuesAsEnumerable!)
			{
				list.Add(value);
			}

			return list;
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureViaAnotherClassContextWith<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(this, type); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(defect,   this, type); }
	}

	Dictionary<dynamic, string> GetInvalidScalarExamples(Type type, Type rootType)
	{
		try
		{
			var examplesProperty = type.GetProperty(nameof(IConstrainedValue<int, ConstrainedInt>.Examples), BindingFlags.Static | BindingFlags.Public)!;
			var examples = examplesProperty.GetValue(null, null);
			var validValuesProperty = typeof(ExampleValues<>).MakeGenericType(rootType).GetProperty(nameof(ExampleValues<object>.ErrorMessagesByInvalidValue));
			var invalidValues = validValuesProperty!.GetValue(examples)!;
			var invalidValuesAsDictionary = invalidValues as IDictionary;

			var dictionary = new Dictionary<dynamic, string>();
			foreach (DictionaryEntry kvp in invalidValuesAsDictionary!)
			{
				dictionary.Add(kvp.Key, kvp.Value!.ToString()!);
			}
			return dictionary;
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureViaAnotherClassContextWith<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(this, type); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(defect, this, type); }
	}

	MethodInfo[] GetSuccessionOfImplicitConversionsEndingInRootType(Type type, Type rootType)
	{
		try
		{
			var conversions = type.GetUserDefinedConversions(browseParentTypes: true);
			var currentImplicitConversionIndex = 0;
			MethodInfo? currentImplicitConversionMethod;
			Type? currentSourceType;
			Type? currentDestinationType = null;
			Type? lastDestinationType = null;
			var successionOfImplicitConversions = new List<MethodInfo>();
			while (currentImplicitConversionIndex < conversions.Length || currentDestinationType == null || currentDestinationType != rootType)
			{
				currentImplicitConversionMethod = conversions[currentImplicitConversionIndex];
				currentSourceType = currentImplicitConversionMethod.GetParameters()[0].ParameterType;
				currentDestinationType = currentImplicitConversionMethod.ReturnType;

				if (lastDestinationType != null && lastDestinationType.IsAssignableTo(currentSourceType))
				{
					successionOfImplicitConversions.Add(currentImplicitConversionMethod);
					lastDestinationType = currentDestinationType;
					currentImplicitConversionIndex++;
				}
				else
				{
					currentImplicitConversionIndex++;
					continue;
				}
			}

			return conversions;
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureViaAnotherClassContextWith<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(this, type, rootType); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(defect, this, type, rootType); }
	}
}
