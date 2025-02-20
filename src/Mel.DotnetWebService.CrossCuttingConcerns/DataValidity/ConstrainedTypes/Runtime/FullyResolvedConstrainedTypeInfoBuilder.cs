using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

public class ConstrainedTypeInfoBuilder
{
	readonly Type _type;
	readonly ConstrainedTypeCategory _typeCategory;
	ConstrainedTypeInfoBuilder(Type type)
	{
		_type = type switch
		{
			var t when !t.ImplementsInterface(typeof(IConstrainedType)) => throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>($"{nameof(_type)} must implement {typeof(IConstrainedType).GetName()}.", this, type),
			_ => type
		};

		_typeCategory = type switch
		{
			var t when t.ContainsGenericParameters => throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>($"{nameof(_type)} must be either non-generic, either fully resolved.", this, type),
			var t when t.ImplementsGenericInterface(typeof(IConstrainedValue<,>), out _)                      => ConstrainedTypeCategory.Value,
			var t when t.ImplementsGenericInterface(typeof(IConstrainedCollection<,>), out _)                 => ConstrainedTypeCategory.NonGenericCollection,
			var t when t.ImplementsGenericInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>), out _) => ConstrainedTypeCategory.NonGenericCollectionOfKeyValuePairs,
			_ => throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>($"{nameof(_type)} must implement either {typeof(IConstrainedValue<,>).GetName()}, either {typeof(IConstrainedCollection<,>).GetName()}, either {typeof(IConstrainedCollectionOfKeyValuePairs<,,>).GetName()}.", this, type)
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
				ConstrainedTypeCategory.Value                               => BuildFullyResolvedConstrainedValueTypeInfo(),
				ConstrainedTypeCategory.NonGenericCollection                => BuildFullyResolvedConstrainedCollectionTypeInfo(),
				ConstrainedTypeCategory.NonGenericCollectionOfKeyValuePairs => BuildFullyResolvedConstrainedCollectionOfKeyValuePairsTypeInfo(),
				_ => throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>($"{nameof(_type)} must implement either {typeof(IConstrainedValue<,>).GetName()}, either {typeof(IConstrainedCollection<,>).GetName()}.", this)
			};
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureViaAnotherClassContextWith<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(this); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(defect,   this); }
	}

	FullyResolvedConstrainedValueTypeInfo BuildFullyResolvedConstrainedValueTypeInfo()
	{
		var rootTypes = GetRootTypes(_type);
		var fullyNativeRootType = rootTypes.Last();
		_type.ImplementsGenericInterface(typeof(IConstrainedValue<,>), out var constrainedValueArgTypes);

		var instanciationMethod = _type
			.GetInterfaceMap(typeof(IConstrainedValue<,>).MakeGenericType(constrainedValueArgTypes))
			.InterfaceMethods
			.Single(m => m.Name == nameof(IConstrainedValue<Guid, NonEmptyGuid>.ApplyConstraintsTo));
		var instanciationMethodParameterTypes = instanciationMethod.GetParameters().Select(p => p.ParameterType).ToArray();
		var instanciator = new Lazy<CompiledInvokable>(
			() => CompiledInvokableBuilder
				.CreateForCompiledInvokableHavingParameterTypes(instanciationMethodParameterTypes)
				.AddCallTo(instanciationMethod)
				.Build());
		var convertorToNativeRootType = new Lazy<CompiledInvokable>(
			() => CompiledInvokableBuilder
				.CreateForCompiledInvokableHavingParameterTypes([_type])
				.AddCallTo(GetSuccessionOfImplicitConversionsEndingInRootType(_type, fullyNativeRootType))
				.Build());

		return new FullyResolvedConstrainedValueTypeInfo(
			_type,
			rootTypes,
			instanciator,
			convertorToNativeRootType);
	}

	FullyResolvedConstrainedCollectionTypeInfo BuildFullyResolvedConstrainedCollectionTypeInfo()
	{
		var rootTypes = GetRootTypes(_type);
		var rootmostType = rootTypes.Last();
		var convertorToNativeRootType = new Lazy<CompiledInvokable>(
			() => CompiledInvokableBuilder
				.CreateForCompiledInvokableHavingParameterTypes([_type])
				.AddCallTo(GetSuccessionOfImplicitConversionsEndingInRootType(_type, rootmostType))
				.Build());

		_type.ImplementsGenericInterface(typeof(IConstrainedCollection<,>), out var constrainedCollectionArgTypes);
		var instanciationMethod = _type
			.GetInterfaceMap(typeof(IConstrainedCollection<,>).MakeGenericType(constrainedCollectionArgTypes))
			.InterfaceMethods
			.Single(m => m.Name == nameof(IConstrainedCollection<Guid, NonEmptyArray<Guid>>.ApplyConstraintsTo));
		var instanciationMethodParameterTypes = instanciationMethod.GetParameters().Select(p => p.ParameterType).ToArray();
		var instanciator = new Lazy<CompiledInvokable>(
			() => CompiledInvokableBuilder
				.CreateForCompiledInvokableHavingParameterTypes(instanciationMethodParameterTypes)
				.AddCallTo(instanciationMethod)
				.Build());
		return new FullyResolvedConstrainedCollectionTypeInfo(
			_type,
			rootTypes,
			instanciator,
			convertorToNativeRootType);
	}

	FullyResolvedConstrainedCollectionOfKeyValuePairsTypeInfo BuildFullyResolvedConstrainedCollectionOfKeyValuePairsTypeInfo()
	{
		var rootTypes = GetRootTypes(_type);
		var rootmostType = rootTypes.Last();
		var convertorToNativeRootType = new Lazy<CompiledInvokable>(
			() => CompiledInvokableBuilder
				.CreateForCompiledInvokableHavingParameterTypes([_type])
				.AddCallTo(GetSuccessionOfImplicitConversionsEndingInRootType(_type, rootmostType))
				.Build());

		_type.ImplementsGenericInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>), out var constrainedCollectionArgTypes);
		var instanciationMethod = _type
			.GetInterfaceMap(typeof(IConstrainedCollectionOfKeyValuePairs<,,>).MakeGenericType(constrainedCollectionArgTypes))
			.InterfaceMethods
			.Single(m => m.Name == nameof(IConstrainedCollectionOfKeyValuePairs<Guid, Guid, NonEmptyDictionary<Guid, Guid>>.ApplyConstraintsTo));
		var instanciationMethodParameterTypes = instanciationMethod.GetParameters().Select(p => p.ParameterType).ToArray();
		var instanciator = new Lazy<CompiledInvokable>(
			() => CompiledInvokableBuilder
				.CreateForCompiledInvokableHavingParameterTypes(instanciationMethodParameterTypes)
				.AddCallTo(instanciationMethod)
				.Build());

		return new FullyResolvedConstrainedCollectionOfKeyValuePairsTypeInfo(
			_type,
			rootTypes,
			instanciator,
			convertorToNativeRootType);
	}

	NonEmptyArray<Type> GetRootTypes(Type type)
	{
		var rootTypesRecipient = new List<Type>();
		return NonEmptyArray<Type>.ApplyConstraintsTo(GetRootTypes(type, rootTypesRecipient, type));
	}

	List<Type> GetRootTypes(Type type, List<Type> rootTypes, Type firstIterationType)
	{
		if (type != firstIterationType)
		{
			rootTypes.Add(type);
		}

		try
		{
			var currentBaseType = type.BaseType;
			while (currentBaseType != null)
			{
				if (!currentBaseType.IsGenericType)
				{
					currentBaseType = currentBaseType.BaseType;
					continue;
				}

				var genericTypeDefinition = currentBaseType.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(ConstrainedFurthermore<>))
				{
					var furtheredConstrainedType = currentBaseType.GetGenericArguments().First();
					return GetRootTypes(furtheredConstrainedType, rootTypes, firstIterationType);
				}

				if (genericTypeDefinition != typeof(ConstrainedValue<>)
					&& genericTypeDefinition != typeof(ConstrainedCollection<>)
					&& genericTypeDefinition != typeof(ConstrainedCollectionOfKeyValuePairs<>))
				{
					currentBaseType = currentBaseType.BaseType;
					continue;
				}

				var fullyNativeType = currentBaseType.GetGenericArguments().First();
				rootTypes.Add(fullyNativeType);
				return rootTypes;
			}

			throw new InvalidOperationException($"Parent type {typeof(ConstrainedValue<>).GetName()} not found in type {type.FullName}. Does it extend {typeof(ConstrainedValue<>).FullName}?");
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureViaAnotherClassContextWith<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(this, type); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(defect,   this, type); }
	}

	NonEmptyArray<MethodInfo> GetSuccessionOfImplicitConversionsEndingInRootType(Type type, Type rootType)
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

			return NonEmptyArray.ApplyConstraintsTo(conversions); // TODO successionOfImplicitConversions.ToArray();
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureViaAnotherClassContextWith<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(this, type, rootType); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceViaAnotherClass<ConstrainedTypeInfo, ConstrainedTypeInfoBuilder>(defect, this, type, rootType); }
	}

	// Type BuildFullyNativeTypeForEnumerable(Type enumerableType)
	// {
	// 	if (!enumerableType.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var argumentTypes))
	// 	{
	// 		throw new InvalidOperationException();
	// 	}

	// 	// Donc:
	// 	//   1) Entre AbstractClass<TCollection> et IStaticFactoryMethodDefiner<IEnumerable<TElement>>, besoin de:
	// 	//     a] trouver IEnumerable à partir de TCollection (chaîne supplémentaire) pour créer Instanciation Func<IEnumerable<TElement>, TConstrained>
	// 	//       => FAUX, on pourrait aller directement trouver le paramètre dans l'interface
	// 	//     b] convertir List<Guid> -> IEnumerable<Guid> pour la désérialisation
	// 	//       => FAUX, on a choisi de gérer ça nativement
	// 	//     c] il reste convertir List<NonEmptyGuid> en IEnumerable<Guid>, par exemple dans FirstClassCollection : ConstrainedCollection<List<NonEmptyGuid>> qu'on veut sérialiser
	// 	//       => FAUX, la sérialisation marche
	// 	// to IEnumerable<KeyValuePair<TNative,UNative>> (list)
	// 	// ====================================================
	// 	// !collectionType.IsGenericType
	// 	//   IDictionary containing KeyValuePairs or DictionaryEntry
	// 	//   ICollection containing KeyValuePairs
	// 	// collectionType.IsGenericType
	// 	//   IDictionary<T, U>
	// 	//   ICollectionOfKeyValuePairs<T, U>
	// 	//   IEnumerable<KeyValuePair<T,U>>
	// 	//   IFurthermore<TFurthered> --> Recursion on TFurthered
	// 	//
	// 	// to IEnumerable<TNative>
	// 	// =======================
	// 	// -> Recursion on TNative

	// 	if (enumerableType != typeof(string) && enumerableType.ImplementsGenericInterface(typeof(IEnumerable<>), out var argumentTypes))
	// 	{
	// 		var collectionItemType = enumerableType.GetCollectionItemType();
	// 		if (collectionItemType.IsGenericType && collectionItemType.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
	// 		{
	// 			var kvpTypes = collectionItemType.GetGenericArguments();
	// 			var keyType = kvpTypes.First();
	// 			var valueType = kvpTypes.Last();
	// 			var fullyNativeKeyType = BuildFullyNativeTypeForEnumerable(keyType);
	// 			var fullyNativeValueType = BuildFullyNativeTypeForEnumerable(valueType);

	// 			return typeof(KeyValuePair<,>).MakeGenericType([fullyNativeKeyType, fullyNativeValueType]).MakeArrayType();
	// 		}
	// 		return BuildFullyNativeTypeForEnumerable(collectionItemType).MakeArrayType();
	// 	}

	// 	if (ConstrainedTypeInfos.TryGet(enumerableType, out var constrainedTypeInfo))
	// 	{
	// 		return BuildFullyNativeTypeForEnumerable(constrainedTypeInfo.RootType);
	// 	}

	// 	return enumerableType;
	// }
}

