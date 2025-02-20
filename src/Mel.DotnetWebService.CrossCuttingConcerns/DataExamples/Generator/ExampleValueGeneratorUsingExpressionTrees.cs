using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;


public static class ExampleValueGenerationExpressionTreeBuilder
{
	public static Expression<Func<object>> Build(
		Type type,
		Dictionary<Type, object> examples)
	{
		ArgumentNullException.ThrowIfNull(type);
		ArgumentNullException.ThrowIfNull(examples);

		var expression = BuildExpression(
			type,
			examples,
			new HashSet<Type>());

		return Expression.Lambda<Func<object>>(
			Expression.Convert(expression, typeof(object)));
	}

	static Expression BuildExpression(
		Type type,
		Dictionary<Type, object> examples,
		HashSet<Type> recursionStack)
	{
		if (!recursionStack.Add(type))
		{
			throw new InvalidOperationException(
				$"Circular construction dependency detected for '{type}'.");
		}

		try
		{
			// 1. Native C# types -> example from dictionary.
			if (type.IsANativeScalarType())
			{
				return BuildNativeType(type, examples);
			}

			// Nullable<T> is not a native type, but can be constructed from T.
			var nullableUnderlyingType = Nullable.GetUnderlyingType(type);
			if (nullableUnderlyingType != null)
			{
				return Expression.Convert(
					BuildExpression(
					nullableUnderlyingType,
					examples,
					recursionStack),
					type);
			}

			// 2. Enum -> smallest strictly-positive numeric enum value.
			if (type.IsEnum)
			{
				return BuildEnumExpression(type);
			}

			// 3/4. Collection, including collections of KeyValuePair<T,U>.
			if (TryGetCollectionElementType(type, out var elementType))
			{
				return BuildCollectionExpression(
					type,
					elementType,
					examples,
					recursionStack);
			}

			// 5a. Special one-collection-parameter constructor case.
			if (HasSingleConstructorWithSingleCollectionParameter(
				type,
				out var collectionParameterType))
			{
				var collectionMembers =
					GetCollectionFieldsAndProperties(type);

				if (collectionMembers.Count > 0)
				{
		// Prefer a static factory method whose single parameter
		// is compatible with a collection field/property.
		var factory = FindCollectionFactoryMethod(
						type,
						collectionMembers);

		if (factory != null)
		{
			var parameterType =
			factory.GetParameters()[0].ParameterType;

			var member = FindCompatibleCollectionMember(
			collectionMembers,
			parameterType);

			if (member != null)
			{
				var memberCollectionType = GetMemberType(member);

				var collectionExpression =
				BuildExpression(
					memberCollectionType,
					examples,
					recursionStack);

				return Expression.Call(
				factory,
				ConvertIfNecessary(
					collectionExpression,
					parameterType));
			}
		}

		// No suitable factory: use the first constructor whose
		// single parameter is compatible with the collection
		// field/property.
		var compatibleConstructor =
						FindCompatibleSingleParameterConstructor(
						type,
						collectionMembers);

		if (compatibleConstructor != null)
		{
			var parameterType =
			compatibleConstructor.GetParameters()[0].ParameterType;

			var member = FindCompatibleCollectionMember(
			collectionMembers,
			parameterType);

			var memberCollectionType = GetMemberType(member!);

			var collectionExpression =
			BuildExpression(
							memberCollectionType,
							examples,
							recursionStack);

			return Expression.New(
			compatibleConstructor,
			ConvertIfNecessary(
							collectionExpression,
							parameterType));
		}
				}
			}

			// 6. Positional record -> its constructor.
			if (IsDeclaredAsAPositionalRecord(type))
			{
				return BuildPositionalRecordExpression(
					type,
					examples,
					recursionStack);
			}

			// 7. Class with public constructor having parameters ->
			// constructor with most parameters.
			if (IsClassWithPublicConstructorHavingParameters(type))
			{
				var constructor = type
					.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
					.Where(c => c.GetParameters().Length > 0)
					.OrderByDescending(c => c.GetParameters().Length)
					.First();

				return BuildConstructorExpression(
					constructor,
					examples,
					recursionStack);
			}

			// 8. Public parameterless constructor + setters ->
			// create it and set all settable properties.
			if (IsClassWithPublicParameterlessConstructorAndSetters(type))
			{
				return BuildParameterlessConstructorWithSetters(
					type,
					examples,
					recursionStack);
			}

			// 9. Public static factory method.
			if (IsClassWithPublicStaticFactoryMethod(type))
			{
				var method = FindPublicStaticFactoryMethod(
					type,
					examples,
					recursionStack);

				if (method != null)
				{
		return BuildStaticFactoryExpression(
						method,
						examples,
						recursionStack);
				}
			}

			// 10. Singleton via single public static property.
			if (IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(type))
			{
				var property = FindSingletonProperty(type);

				return Expression.Property(null, property);
			}

			// 11. Singleton via single public static field.
			if (IsClassWithSinglePublicAccessToSingletonInstanceThroughField(type))
			{
				var field = FindSingletonField(type);

				return Expression.Field(null, field);
			}

			// 12. Public instances exposed as static readonly fields.
			if (IsClassWithPublicInstancesExposedAsStaticReadonlyFields(type))
			{
				var field = FindCompatibleStaticField(
					type,
					requireReadonly: true);

				return Expression.Field(null, field!);
			}

			// 13. Public instances exposed as static properties.
			if (IsClassWithPublicInstancesExposedAsStaticProperties(type))
			{
				var property = FindCompatibleStaticProperty(type);

				return Expression.Property(null, property!);
			}

			// 14. Public instances exposed as static fields.
			if (IsClassWithPublicInstancesExposedAsStaticFields(type))
			{
				var field = FindCompatibleStaticField(
					type,
					requireReadonly: false);

				return Expression.Field(null, field!);
			}

			throw new InvalidOperationException(
				$"No supported construction strategy exists for '{type}'.");
		}
		finally
		{
			recursionStack.Remove(type);
		}
	}

	// ---------------------------------------------------------------------
	// Native types
	// ---------------------------------------------------------------------

	static Expression BuildNativeType(
		Type type,
		Dictionary<Type, object> examples)
	{
		if (!examples.TryGetValue(type, out var value))
		{
			throw new InvalidOperationException(
				$"No example value was supplied for native type '{type}'.");
		}

		if (value == null)
		{
			if (type.IsValueType)
			{
				throw new InvalidOperationException(
					$"Example value for '{type}' cannot be null.");
			}

			return Expression.Constant(null, type);
		}

		if (!type.IsInstanceOfType(value))
		{
			throw new InvalidOperationException(
				$"Example value for '{type}' is of type '{value.GetType()}'.");
		}

		return Expression.Constant(value, type);
	}

	// ---------------------------------------------------------------------
	// Enum
	// ---------------------------------------------------------------------

	static Expression BuildEnumExpression(Type type)
	{
		var values = Enum.GetValues(type);

		var candidates = values
			.Cast<object>()
			.Select(v => new
			{
				Value = v,
				NumericValue = Convert.ToDecimal(v)
			})
			.Where(x => x.NumericValue > 0)
			.OrderBy(x => x.NumericValue)
			.ToList();

		if (candidates.Count == 0)
		{
			throw new InvalidOperationException(
				$"Enum '{type}' has no strictly-positive-valued member.");
		}

		return Expression.Constant(
			candidates[0].Value,
			type);
	}

	// ---------------------------------------------------------------------
	// Collections
	// ---------------------------------------------------------------------

	static bool TryGetCollectionElementType(
		Type type,
		out Type elementType)
	{
		// Array
		if (type.IsArray)
		{
			elementType = type.GetElementType()!;
			return true;
		}

		// Avoid treating string as IEnumerable<char>.
		if (type == typeof(string))
		{
			elementType = null!;
			return false;
		}

		// ICollection<T>, IEnumerable<T>, IList<T>, etc.
		if (type.IsGenericType)
		{
			var genericDefinition = type.GetGenericTypeDefinition();

			if (genericDefinition == typeof(IEnumerable<>) ||
				genericDefinition == typeof(ICollection<>) ||
				genericDefinition == typeof(IList<>) ||
				genericDefinition == typeof(ISet<>) ||
				genericDefinition == typeof(IReadOnlyCollection<>) ||
				genericDefinition == typeof(IReadOnlyList<>) ||
				genericDefinition == typeof(IReadOnlySet<>))
			{
				elementType = type.GetGenericArguments()[0];
				return true;
			}
		}

		var enumerableInterface = type
			.GetInterfaces()
			.FirstOrDefault(i =>
			i.IsGenericType &&
			i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

		if (enumerableInterface != null)
		{
			elementType = enumerableInterface.GetGenericArguments()[0];
			return true;
		}

		elementType = null!;
		return false;
	}

	static Expression BuildCollectionExpression(
		Type requestedType,
		Type elementType,
		Dictionary<Type, object> examples,
		HashSet<Type> recursionStack)
	{
		// Array<T>
		if (requestedType.IsArray)
		{
			var element1 = BuildExpression(
				elementType,
				examples,
				recursionStack);

			var element2 = BuildExpression(
				elementType,
				examples,
				recursionStack);

			return Expression.NewArrayInit(
				elementType,
				element1,
				element2);
		}

		// Build a concrete List<T> containing exactly two items.
		var listType = typeof(List<>).MakeGenericType(elementType);

		var item1 = BuildExpression(
			elementType,
			examples,
			recursionStack);

		var item2 = BuildExpression(
			elementType,
			examples,
			recursionStack);

		var listExpression = Expression.ListInit(
			Expression.New(listType),
			Expression.ElementInit(
			listType.GetMethod("Add")!,
			item1),
			Expression.ElementInit(
			listType.GetMethod("Add")!,
			item2));

		// If List<T> itself is assignable to requestedType,
		// return the List<T>.
		if (requestedType.IsAssignableFrom(listType))
		{
			return listExpression;
		}

		// Common concrete collections with a constructor taking
		// IEnumerable<T>, ICollection<T>, or similar.
		var compatibleConstructor =
			requestedType
			.GetConstructors(
							BindingFlags.Public | BindingFlags.Instance)
			.Select(c => new
			{
				Constructor = c,
				Parameters = c.GetParameters()
			})
			.Where(x => x.Parameters.Length == 1)
			.Select(x => new
			{
				x.Constructor,
				Parameter = x.Parameters[0]
			})
			.FirstOrDefault(x =>
							x.Parameter.ParameterType.IsAssignableFrom(listType) ||
							x.Parameter.ParameterType.IsAssignableFrom(
								typeof(IEnumerable<>).MakeGenericType(elementType)));

		if (compatibleConstructor != null)
		{
			return Expression.New(
				compatibleConstructor.Constructor,
				ConvertIfNecessary(
				listExpression,
				compatibleConstructor.Parameter.ParameterType));
		}

		// ICollection<T>-style concrete type with parameterless ctor + Add.
		var parameterlessConstructor =
			requestedType.GetConstructor(Type.EmptyTypes);

		var addMethod = requestedType
			.GetMethods(BindingFlags.Public | BindingFlags.Instance)
			.FirstOrDefault(m =>
			m.Name == "Add" &&
			m.GetParameters().Length == 1 &&
			m.GetParameters()[0].ParameterType.IsAssignableFrom(elementType));

		if (parameterlessConstructor != null && addMethod != null)
		{
			var variable = Expression.Variable(
				requestedType,
				"collection");

			var assign = Expression.Assign(
				variable,
				Expression.New(parameterlessConstructor));

			var add1 = Expression.Call(
				variable,
				addMethod,
				BuildExpression(
				elementType,
				examples,
				recursionStack));

			var add2 = Expression.Call(
				variable,
				addMethod,
				BuildExpression(
				elementType,
				examples,
				recursionStack));

			return Expression.Block(
				new[] { variable },
				assign,
				add1,
				add2,
				variable);
		}

		throw new InvalidOperationException(
			$"Unable to create a two-item collection of type '{requestedType}'.");
	}

	// ---------------------------------------------------------------------
	// One-collection-parameter special case
	// ---------------------------------------------------------------------

	static bool HasSingleConstructorWithSingleCollectionParameter(
		Type type,
		out Type collectionParameterType)
	{
		var constructors = type.GetConstructors(
			BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.Instance);

		if (constructors.Length != 1)
		{
			collectionParameterType = null!;
			return false;
		}

		var parameters = constructors[0].GetParameters();

		if (parameters.Length != 1)
		{
			collectionParameterType = null!;
			return false;
		}

		if (!TryGetCollectionElementType(
			parameters[0].ParameterType,
			out _))
		{
			collectionParameterType = null!;
			return false;
		}

		collectionParameterType = parameters[0].ParameterType;
		return true;
	}

	static List<MemberInfo> GetCollectionFieldsAndProperties(Type type)
	{
		var members = new List<MemberInfo>();

		members.AddRange(
			type.GetProperties(
							BindingFlags.Public |
							BindingFlags.NonPublic |
							BindingFlags.Instance)
			.Where(p =>
							p.GetIndexParameters().Length == 0 &&
							TryGetCollectionElementType(p.PropertyType, out _)));

		members.AddRange(
			type.GetFields(
							BindingFlags.Public |
							BindingFlags.NonPublic |
							BindingFlags.Instance)
			.Where(f =>
							TryGetCollectionElementType(f.FieldType, out _)));

		return members;
	}

	static MethodInfo? FindCollectionFactoryMethod(
		Type type,
		List<MemberInfo> collectionMembers)
	{
		var methods = type.GetMethods(
			BindingFlags.Public |
			BindingFlags.Static)
			.Where(m =>
			!m.IsSpecialName &&
			m.ReturnType != typeof(void) &&
			type.IsAssignableFrom(m.ReturnType) &&
			m.GetParameters().Length == 1)
			.ToList();

		foreach (var method in methods)
		{
			var parameterType = method.GetParameters()[0].ParameterType;

			if (collectionMembers.Any(m =>
				parameterType.IsAssignableFrom(GetMemberType(m))))
			{
				return method;
			}
		}

		return null;
	}

	static ConstructorInfo? FindCompatibleSingleParameterConstructor(
		Type type,
		List<MemberInfo> collectionMembers)
	{
		var constructors = type.GetConstructors(
			BindingFlags.Public |
			BindingFlags.NonPublic |
			BindingFlags.Instance)
			.Where(c => c.GetParameters().Length == 1)
			.ToList();

		foreach (var constructor in constructors)
		{
			var parameterType =
				constructor.GetParameters()[0].ParameterType;

			if (collectionMembers.Any(m =>
				parameterType.IsAssignableFrom(GetMemberType(m))))
			{
				return constructor;
			}
		}

		return null;
	}

	static MemberInfo? FindCompatibleCollectionMember(
		List<MemberInfo> collectionMembers,
		Type parameterType)
	{
		return collectionMembers.FirstOrDefault(m =>
			parameterType.IsAssignableFrom(GetMemberType(m)));
	}

	static Type GetMemberType(MemberInfo member)
	{
		return member switch
		{
			PropertyInfo p => p.PropertyType,
			FieldInfo f => f.FieldType,
			_ => throw new InvalidOperationException()
		};
	}

	// ---------------------------------------------------------------------
	// Positional records
	// ---------------------------------------------------------------------

	static bool IsDeclaredAsAPositionalRecord(Type type)
	{
		if (!type.IsClass)
			return false;

		// A positional record has the compiler-generated EqualityContract
		// property. The constructor is then used below.
		var equalityContract = type.GetProperty(
			"EqualityContract",
			BindingFlags.Instance |
			BindingFlags.NonPublic |
			BindingFlags.Public);

		if (equalityContract == null)
			return false;

		var getter = equalityContract.GetMethod;

		return getter != null &&
						getter.IsVirtual &&
						getter.IsFamily;
	}

	static Expression BuildPositionalRecordExpression(
		Type type,
		Dictionary<Type, object> examples,
		HashSet<Type> recursionStack)
	{
		var constructor = type
			.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
			.OrderByDescending(c => c.GetParameters().Length)
			.FirstOrDefault();

		if (constructor == null)
		{
			throw new InvalidOperationException(
				$"Positional record '{type}' has no public constructor.");
		}

		return BuildConstructorExpression(
			constructor,
			examples,
			recursionStack);
	}

	// ---------------------------------------------------------------------
	// Constructor-based classes
	// ---------------------------------------------------------------------

	static bool IsClassWithPublicConstructorHavingParameters(
		Type type)
	{
		return type.IsClass &&
						!type.IsAbstract &&
						type.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
						.Any(c => c.GetParameters().Length > 0);
	}

	static Expression BuildConstructorExpression(
		ConstructorInfo constructor,
		Dictionary<Type, object> examples,
		HashSet<Type> recursionStack)
	{
		var arguments = constructor
			.GetParameters()
			.Select(p =>
			BuildExpression(
							p.ParameterType,
							examples,
							recursionStack))
			.ToArray();

		return Expression.New(constructor, arguments);
	}

	// ---------------------------------------------------------------------
	// Parameterless constructor + setters
	// ---------------------------------------------------------------------

	static bool IsClassWithPublicParameterlessConstructorAndSetters(
		Type type)
	{
		if (!type.IsClass || type.IsAbstract)
			return false;

		var parameterless =
			type.GetConstructor(
			BindingFlags.Public | BindingFlags.Instance,
			binder: null,
			types: Type.EmptyTypes,
			modifiers: null);

		if (parameterless == null)
			return false;

		return type.GetProperties(
			BindingFlags.Public | BindingFlags.Instance)
			.Any(p =>
			p.CanWrite &&
			p.SetMethod != null &&
			p.SetMethod.IsPublic &&
			p.GetIndexParameters().Length == 0);
	}

	static Expression BuildParameterlessConstructorWithSetters(
		Type type,
		Dictionary<Type, object> examples,
		HashSet<Type> recursionStack)
	{
		var constructor =
			type.GetConstructor(Type.EmptyTypes)!;

		var newExpression = Expression.New(constructor);

		var bindings = new List<MemberBinding>();

		foreach (var property in type.GetProperties(
					BindingFlags.Public | BindingFlags.Instance)
					.Where(p =>
									p.CanWrite &&
									p.SetMethod != null &&
									p.SetMethod.IsPublic &&
									p.GetIndexParameters().Length == 0))
		{
			var value = BuildExpression(
				property.PropertyType,
				examples,
				recursionStack);

			bindings.Add(
				Expression.Bind(
				property,
				ConvertIfNecessary(value, property.PropertyType)));
		}

		return Expression.MemberInit(
			newExpression,
			bindings);
	}

	// ---------------------------------------------------------------------
	// Static factory methods
	// ---------------------------------------------------------------------

	static bool IsClassWithPublicStaticFactoryMethod(Type type)
	{
		return type.IsClass &&
						type.GetMethods(
						BindingFlags.Public | BindingFlags.Static)
						.Any(IsPotentialFactoryMethod);
	}

	static MethodInfo? FindPublicStaticFactoryMethod(
		Type type,
		Dictionary<Type, object> examples,
		HashSet<Type> recursionStack)
	{
		foreach (var method in type
					.GetMethods(
									BindingFlags.Public | BindingFlags.Static)
					.Where(IsPotentialFactoryMethod))
		{
			try
			{
				foreach (var parameter in method.GetParameters())
				{
		_ = BuildExpression(
						parameter.ParameterType,
						examples,
						new HashSet<Type>(recursionStack));
				}

				return method;
			}
			catch
			{
				// This method is not constructible from the supplied
				// examples; try the next public static factory method.
			}
		}

		return null;
	}

	static bool IsPotentialFactoryMethod(MethodInfo method)
	{
		return !method.IsSpecialName &&
						method.ReturnType != typeof(void) &&
						method.IsStatic &&
						method.DeclaringType != null;
	}

	static Expression BuildStaticFactoryExpression(
		MethodInfo method,
		Dictionary<Type, object> examples,
		HashSet<Type> recursionStack)
	{
		var arguments = method
			.GetParameters()
			.Select(p =>
			ConvertIfNecessary(
							BuildExpression(
								p.ParameterType,
								examples,
								recursionStack),
							p.ParameterType))
			.ToArray();

		return Expression.Call(method, arguments);
	}

	// ---------------------------------------------------------------------
	// Singleton / static instance exposure
	// ---------------------------------------------------------------------

	static bool IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(
		Type type)
	{
		return type.IsClass &&
						GetCompatibleStaticProperties(type).Count == 1;
	}

	static PropertyInfo FindSingletonProperty(Type type)
	{
		return GetCompatibleStaticProperties(type)[0];
	}

	static bool IsClassWithSinglePublicAccessToSingletonInstanceThroughField(
		Type type)
	{
		return type.IsClass &&
						GetCompatibleStaticFields(type).Count == 1;
	}

	static FieldInfo FindSingletonField(Type type)
	{
		return GetCompatibleStaticFields(type)[0];
	}

	static bool IsClassWithPublicInstancesExposedAsStaticReadonlyFields(
		Type type)
	{
		return GetCompatibleStaticFields(
			type,
			requireReadonly: true).Count > 0;
	}

	static bool IsClassWithPublicInstancesExposedAsStaticProperties(
		Type type)
	{
		return GetCompatibleStaticProperties(type).Count > 0;
	}

	static bool IsClassWithPublicInstancesExposedAsStaticFields(
		Type type)
	{
		return GetCompatibleStaticFields(
			type,
			requireReadonly: false).Count > 0;
	}

	static List<PropertyInfo> GetCompatibleStaticProperties(Type type)
	{
		return type
			.GetProperties(
			BindingFlags.Public |
			BindingFlags.Static)
			.Where(p =>
			p.GetMethod != null &&
			p.GetMethod.IsPublic &&
			p.GetIndexParameters().Length == 0 &&
			type.IsAssignableFrom(p.PropertyType))
			.ToList();
	}

	static List<FieldInfo> GetCompatibleStaticFields(
		Type type,
		bool? requireReadonly = null)
	{
		return type
			.GetFields(
			BindingFlags.Public |
			BindingFlags.Static)
			.Where(f =>
			type.IsAssignableFrom(f.FieldType) &&
			(!requireReadonly.HasValue ||
				f.IsInitOnly == requireReadonly.Value))
			.ToList();
	}

	static PropertyInfo? FindCompatibleStaticProperty(Type type)
		=> GetCompatibleStaticProperties(type).FirstOrDefault();

	static FieldInfo? FindCompatibleStaticField(
		Type type,
		bool requireReadonly)
		=> GetCompatibleStaticFields(type, requireReadonly)
						.FirstOrDefault();

	// ---------------------------------------------------------------------
	// Conversion helpers
	// ---------------------------------------------------------------------

	static Expression ConvertIfNecessary(
		Expression expression,
		Type targetType)
	{
		if (expression.Type == targetType)
			return expression;

		if (targetType.IsAssignableFrom(expression.Type))
			return expression;

		return Expression.Convert(expression, targetType);
	}
}

