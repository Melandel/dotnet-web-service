using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public class ConstrainedTypeInfo
{
	public ConstrainedTypeCategory Category { get; }
	public Type ConstrainedType { get; }
	public Type RootType { get; }
	public dynamic[] ValidValueExamples { get; }
	public Dictionary<dynamic, string> ErrorMessagesByInvalidValueExample { get; }
	public dynamic InvokeImplicitConversionToRootType(dynamic valueAsConstrainedType)
	{
		if (Category == ConstrainedTypeCategory.Value)
		{
			return _valueImplicitConversionOperation!.Invoke(valueAsConstrainedType);
		}
		else
		{
			Type runtimeConstrainedType = valueAsConstrainedType.GetType();
			if (!_resolvedGenericArgumentCollectionImplicitConversionOperations!.ContainsKey(runtimeConstrainedType))
			{
				runtimeConstrainedType.ImplementsGenericInterface(typeof(IEnumerable<>), out var elementType);
				var conversionParameterTypeWithResolvedGenericArguments = ConstrainedType.MakeGenericType(elementType);
				var conversionReturnTypeWithResolvedGenericArguments = RootType.GetGenericTypeDefinition().MakeGenericType(elementType);
				var successionOfImplicitConversions = GetSuccessionOfImplicitConversionsEndingInRootType(runtimeConstrainedType, conversionReturnTypeWithResolvedGenericArguments);
				_resolvedGenericArgumentCollectionImplicitConversionOperations.Add(
					runtimeConstrainedType,
					Func.CompileSuccessiveCallsToStaticMethods(successionOfImplicitConversions, conversionParameterTypeWithResolvedGenericArguments, conversionReturnTypeWithResolvedGenericArguments));
			}
			return _resolvedGenericArgumentCollectionImplicitConversionOperations[runtimeConstrainedType].Invoke(valueAsConstrainedType);
		}
	}

	public dynamic InvokeStaticFactoryMethod(dynamic valueAsRootType)
	{
		if (Category == ConstrainedTypeCategory.Value)
		{
			return _valueInstanciationOperation!.Invoke(valueAsRootType);
		}
		else
		{
			Type rootType = valueAsRootType.GetType();
			if (!_resolvedGenericArgumentCollectionInstanciationOperations!.ContainsKey(rootType))
			{
				var compiledCallToFactoryMethod = BuildCompiledCallToCollectionFactoryMethod(rootType);
				_resolvedGenericArgumentCollectionInstanciationOperations.Add(rootType, compiledCallToFactoryMethod);
			}
			return _resolvedGenericArgumentCollectionInstanciationOperations[rootType].Invoke(valueAsRootType);
		}
	}

	Func BuildCompiledCallToCollectionFactoryMethod(Type rootType)
	{
		rootType.ImplementsGenericInterface(typeof(IEnumerable<>), out var elementType);
		var runtimeConstrainedType = ConstrainedType.MakeGenericType(elementType);
		var factoryMethod = GetCollectionStaticFactoryMethod(elementType);
		return Func.CompileCallToStaticMethod(factoryMethod!, rootType, runtimeConstrainedType);
	}

	MethodInfo GetCollectionStaticFactoryMethod(Type[] elementType)
	{
		var methodImplementingClass = ConstrainedType.MakeGenericType(elementType);
		var methodDeclaringInterface = typeof(IConstrainedCollection<,>).MakeGenericType(elementType.Append(methodImplementingClass).ToArray());
		var bar = methodImplementingClass
			.GetInterfaceMap(methodDeclaringInterface)
			.InterfaceMethods.First(m => m.Name == nameof(IConstrainedCollection<Guid, NonEmptyGuid>.ApplyConstraintsTo));
		return bar;
	}

	readonly Func? _valueImplicitConversionOperation;
	readonly Func? _valueInstanciationOperation;
	readonly Dictionary<Type, Func>? _resolvedGenericArgumentCollectionImplicitConversionOperations;
	readonly Dictionary<Type, Func>? _resolvedGenericArgumentCollectionInstanciationOperations;

	ConstrainedTypeInfo(
		ConstrainedTypeCategory constrainedTypeCategory,
		Type constrainedType,
		Type rootType,
		dynamic[] validScalarExamples,
		Dictionary<dynamic, string> invalidScalarExamples,
		Func? valueImplicitConversionToRootTypeFriendlyType,
		Func? valueInstanciationOperation)
	{
		Category = constrainedTypeCategory;
		ConstrainedType = constrainedType switch
		{
			var t when !t.IsSealed => throw ObjectConstructionException.WhenConstructingAMemberFor<ConstrainedTypeInfo>(nameof(ConstrainedType), constrainedType, $"@member must be sealed"),
			var t when t.ImplementsGenericInterface(typeof(IConstrainedValue<,>), out var argumentTypes) => t switch
			{
				_ when !Utf8JsonReaderExtensionMethods.NativelySupportedTypes.Contains(argumentTypes.First()) => throw ObjectConstructionException.WhenConstructingAMemberFor<ConstrainedTypeInfo>(nameof(ConstrainedType), constrainedType, $"@member must add constraints to a scalar type ({Utf8JsonReaderExtensionMethods.NativelySupportedTypes.GetStringRepresentation()})"),
				_ when !t.GetUserDefinedConversions(browseParentTypes: true).Any() => throw ObjectConstructionException.WhenConstructingAMemberFor<ConstrainedTypeInfo>(nameof(ConstrainedType), constrainedType, "@member must define an implicit conversion operator"),
				_ when t.GetConstructors().Any(ctor => ctor.IsPublic || ctor.IsAssembly) => throw ObjectConstructionException.WhenConstructingAMemberFor<ConstrainedTypeInfo>(nameof(ConstrainedType), constrainedType, "@member must not define any internal or public constructor"),
				_ when !t.GetStaticFactoryMethods(BindingFlags.Public).Any() => throw ObjectConstructionException.WhenConstructingAMemberFor<ConstrainedTypeInfo>(nameof(ConstrainedType), constrainedType, "@member must define at least one static factory method"),
				_ => constrainedType
			},
			var t when t.ImplementsInterface(typeof(IConstrainedCollection<,>)) => t switch
			{
				_ when !t.GetUserDefinedConversions(browseParentTypes: true).Any() => throw ObjectConstructionException.WhenConstructingAMemberFor<ConstrainedTypeInfo>(nameof(ConstrainedType), constrainedType, "@member must define an implicit conversion operator"),
				_ when t.GetConstructors().Any(ctor => ctor.IsPublic || ctor.IsAssembly) => throw ObjectConstructionException.WhenConstructingAMemberFor<ConstrainedTypeInfo>(nameof(ConstrainedType), constrainedType, "@member must not define any public constructor"),
				_ when !t.GetStaticFactoryMethods(BindingFlags.Public).Any() => throw ObjectConstructionException.WhenConstructingAMemberFor<ConstrainedTypeInfo>(nameof(ConstrainedType), constrainedType, "@member must define at least one static factory method"),
				_ => constrainedType
			},
			_ => throw new Exception($"@member must implement either {typeof(IConstrainedValue<,>).GetName()}, either {typeof(IConstrainedCollection<,>).GetName()}")
		};

		RootType = rootType;
		ValidValueExamples = validScalarExamples;
		ErrorMessagesByInvalidValueExample = invalidScalarExamples;
		_valueImplicitConversionOperation = valueImplicitConversionToRootTypeFriendlyType;
		_valueInstanciationOperation = valueInstanciationOperation;
		if (Category == ConstrainedTypeCategory.GenericCollection)
		{
			_resolvedGenericArgumentCollectionImplicitConversionOperations = new Dictionary<Type, Func>();
			_resolvedGenericArgumentCollectionInstanciationOperations = new Dictionary<Type, Func>();
		}
	}

	public static ConstrainedTypeInfo CreateForValueType(
		Type type,
		Type rootType,
		IEnumerable<dynamic>? validScalarExamples,
		Dictionary<dynamic, string>? invalidScalarExamples,
		MethodInfo[] successionOfImplicitConversionsEndingInRootType,
		MethodInfo instanciationOperationMethod)
	{
		try
		{
			return new(
				ConstrainedTypeCategory.Value,
				type,
				rootType,
				validScalarExamples?.ToArray() ?? Array.Empty<dynamic>(),
				invalidScalarExamples ?? new Dictionary<dynamic, string>(),
				Func.CompileSuccessiveCallsToStaticMethods(successionOfImplicitConversionsEndingInRootType, type, rootType),
				Func.CompileCallToStaticMethod(instanciationOperationMethod, rootType, type));
		}
		catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<ConstrainedTypeInfo>(type, rootType, validScalarExamples, invalidScalarExamples, successionOfImplicitConversionsEndingInRootType); throw; }
		catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<ConstrainedTypeInfo>(defect, type, rootType, validScalarExamples, invalidScalarExamples, successionOfImplicitConversionsEndingInRootType); }
	}

	public static ConstrainedTypeInfo CreateForGenericCollectionType(
		Type type,
		Type rootType,
		IEnumerable<dynamic>? validScalarExamples,
		Dictionary<dynamic, string>? invalidScalarExamples)
	{
		try
		{
			return new(
				ConstrainedTypeCategory.GenericCollection,
				type,
				rootType,
				validScalarExamples?.ToArray() ?? Array.Empty<dynamic>(),
				invalidScalarExamples ?? new Dictionary<dynamic, string>(),
				valueImplicitConversionToRootTypeFriendlyType: null,
				valueInstanciationOperation: null);
		}
		catch (ObjectConstructionException objectConstructionException) { objectConstructionException.EnrichConstructionFailureContextWith<ConstrainedTypeInfo>(type, rootType, validScalarExamples, invalidScalarExamples); throw; }
		catch (Exception defect) { throw ObjectConstructionException.WhenConstructingAnInstanceOf<ConstrainedTypeInfo>(defect, type, rootType, validScalarExamples, invalidScalarExamples); }
	}

	MethodInfo[] GetSuccessionOfImplicitConversionsEndingInRootType(Type type, Type rootType)
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
}
