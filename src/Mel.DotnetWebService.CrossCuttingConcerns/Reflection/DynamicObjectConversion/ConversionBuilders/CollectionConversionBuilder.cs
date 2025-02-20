using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectConversion;

abstract class CollectionConversionBuilder
{
	static readonly Type[] NonGenericCollectionInterfaceTypes = [ typeof(IList), typeof(ICollection), typeof(IEnumerable) ];
	protected static readonly IEnumerable<MethodInfo> EnumerableGenericMethods = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.IsGenericMethodDefinition);
	protected static readonly MethodInfo OpenGenericSelectMethod = EnumerableGenericMethods.Single(m => m.Name == nameof(Enumerable.Select)
		&& m.GetGenericArguments().Length == 2
		&& m.GetParameters().Length == 2
		&& m.GetParameters()[1].ParameterType.IsGenericType
		&& m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>));
	protected static MethodInfo SelectMethodClosedWithTheTypeArguments(Type sourceCollectionItemType, Type destinationCollectionItemType)
	=> OpenGenericSelectMethod.MakeGenericMethod(sourceCollectionItemType, destinationCollectionItemType);
	protected abstract MethodInfo GetClosedGenericToDestinationCollectionMethod(Type destinationCollectionType, Type destinationCollectionItemType);
	protected abstract MethodCallExpression CallConversionToDestinationCollectionType(Expression sourceCollectionParameter_OrExpressionBuiltSoFar, MethodInfo closedGenericToDestinationCollectionMethod);
	protected abstract MethodCallExpression CallSelectThenToDestinationCollectionType(Expression sourceCollectionParameter_OrExpressionBuiltSoFar, MethodInfo closedGenericSelectMethod, LambdaExpression selectClauseLambda, MethodInfo closedGenericToDestinationCollectionMethod);

	public static MethodCallExpression Build(Expression sourceCollection, Type destinationType)
	{
		if (destinationType == typeof(IDictionary))
		{
			return sourceCollection.Type.IsOrImplementsGenericIEnumerableOfKeyPairValues(out var keyType, out var valueType)
				? Build(sourceCollection, typeof(Dictionary<,>).MakeGenericType(keyType, valueType))
				: throw new InvalidOperationException();
		}
		if (NonGenericCollectionInterfaceTypes.Contains(destinationType))
		{
			return Build(
				sourceCollection,
				typeof(List<>).MakeGenericType(sourceCollection.Type.GetCollectionItemType()));
		}

		if (IsNonGenericConstrainedCollection(sourceCollection.Type, out var sourceCollectionConstrainedTypeInfo))
		{
			var sourceCollectionAsGenericCollectionCallMethodExpression = sourceCollectionConstrainedTypeInfo.BuildConvertorToNativeRootTypeMethodCallExpression(sourceCollection);
			if (IsNonGenericConstrainedCollection(destinationType, out var destinationCollectionConstrainedTypeInfo))
			{
				var intermediateCollectionType = destinationCollectionConstrainedTypeInfo.FullyNativeRootType;
				var toIntermediatedCollectionType = Build(sourceCollectionAsGenericCollectionCallMethodExpression, intermediateCollectionType);

				return Expression.Call(
					destinationCollectionConstrainedTypeInfo.InvokableInstantiationMethod,
					toIntermediatedCollectionType);
			}

			return Build(sourceCollectionAsGenericCollectionCallMethodExpression, destinationType);
		}


		var collectionConversionCallExpression = ResolveCollectionConversionCallExpressionBuilderFor(destinationType);
		return collectionConversionCallExpression.BuildConversionExpressionFor(sourceCollection, destinationType);
	}

	static CollectionConversionBuilder ResolveCollectionConversionCallExpressionBuilderFor(Type destinationType)
	=> destinationType switch
	{
		var t when t.IsArray => CollectionToGenericArrayConversionBuilder.Instance,
		var t when t.IsInterface => t switch
		{
			var genericItf when genericItf.IsOrImplementsGenericInterface(typeof(IDictionary<,>), out _) => CollectionToGenericDictionaryConversionBuilder.Instance,
			var genericItf when genericItf.IsOrImplementsGenericInterface(typeof(IReadOnlyDictionary<,>), out _) => CollectionToGenericDictionaryConversionBuilder.Instance,
			var genericItf when genericItf.IsOrImplementsGenericInterface(typeof(ISet<>), out _) => CollectionToGenericHashSetConversionBuilder.Instance,
			var genericItf when genericItf.IsOrImplementsGenericInterface(typeof(IReadOnlySet<>), out _) => CollectionToGenericHashSetConversionBuilder.Instance,
			_ => CollectionToGenericListConversionBuilder.Instance,
		},
		var t when !t.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out _) => t switch
		{
			var nonIEnumerableType when nonIEnumerableType.ImplementsGenericInterface(typeof(IConstrainedCollection<,>), out _) => CollectionToGenericConstrainedCollectionConversionBuilder.Instance,
			var nonIEnumerableType when nonIEnumerableType.ImplementsGenericInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>), out _) => CollectionToGenericConstrainedCollectionConversionBuilder.Instance,
			_ => throw new NotImplementedException()
		},
		var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
		{
			var gt when gt == typeof(List<>) => CollectionToGenericListConversionBuilder.Instance,
			var gt when gt == typeof(HashSet<>) => CollectionToGenericHashSetConversionBuilder.Instance,
			var gt when gt == typeof(LinkedList<>) => CollectionToGenericLinkedListConversionBuilder.Instance,
			var gt when gt == typeof(Queue<>) => CollectionToGenericQueueConversionBuilder.Instance,
			var gt when gt == typeof(SortedSet<>) => CollectionToGenericSortedSetConversionBuilder.Instance,
			var gt when gt == typeof(Stack<>) => CollectionToGenericStackConversionBuilder.Instance,
			var gt when gt == typeof(Dictionary<,>) => CollectionToGenericDictionaryConversionBuilder.Instance,
			var gt when gt == typeof(SortedDictionary<,>) => CollectionToGenericSortedDictionaryConversionBuilder.Instance,
			var gt when gt == typeof(SortedList<,>) => CollectionToGenericSortedListConversionBuilder.Instance,
			_ => t switch
			{
				var tNotInSystemCollectionNamespace when ConstrainedTypeInfos.Include(tNotInSystemCollectionNamespace) => CollectionToGenericConstrainedCollectionConversionBuilder.Instance,
				_ => throw new NotImplementedException()
			}
		},
		_ => throw new NotImplementedException()
	};

	static bool IsNonGenericConstrainedCollection(Type sourceCollectionType, out ConstrainedTypeInfo constrainedTypeInfo)
	{
		constrainedTypeInfo = null!;
		return !sourceCollectionType.IsGenericType && ConstrainedTypeInfos.TryGet(sourceCollectionType, out constrainedTypeInfo);
	}

	MethodCallExpression BuildConversionExpressionFor(Expression sourceCollectionParameter_OrExpressionBuiltSoFar, Type destinationCollectionType)
	{
		var sourceCollectionType = sourceCollectionParameter_OrExpressionBuiltSoFar.Type;
		var sourceCollectionItemType = sourceCollectionType.GetCollectionItemType();
		var destinationCollectionItemType = destinationCollectionType.GetCollectionItemType();
		var closedGenericToDestinationCollectionMethod = GetClosedGenericToDestinationCollectionMethod(destinationCollectionType, destinationCollectionItemType);

		if (sourceCollectionItemType == destinationCollectionItemType)
		{
			return CallConversionToDestinationCollectionType(sourceCollectionParameter_OrExpressionBuiltSoFar, closedGenericToDestinationCollectionMethod);
		}

		return CallSelectThenToDestinationCollectionType(
			sourceCollectionParameter_OrExpressionBuiltSoFar,
			BuildSelectMethod(sourceCollectionItemType, destinationCollectionItemType),
			SelectClauseBuilder.Build(sourceCollectionItemType, destinationCollectionItemType),
			closedGenericToDestinationCollectionMethod);
	}

	MethodInfo BuildSelectMethod(Type sourceCollectionItemType, Type destinationCollectionItemType)
	=> OpenGenericSelectMethod.MakeGenericMethod(sourceCollectionItemType, destinationCollectionItemType);
}
