using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

abstract class CollectionConversionCallExpressionBuilder
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

	public static MethodCallExpression Build(Expression sourceCollectionParameter_OrExpressionBuiltSoFar, Type destinationType)
	{
		if (destinationType == typeof(IDictionary))
		{
			return sourceCollectionParameter_OrExpressionBuiltSoFar.Type.IsOrImplementsGenericIEnumerableOfKeyPairValues(out var keyType, out var valueType)
				? Build(sourceCollectionParameter_OrExpressionBuiltSoFar, typeof(Dictionary<,>).MakeGenericType(keyType, valueType))
				: throw new InvalidOperationException();
		}
		if (NonGenericCollectionInterfaceTypes.Contains(destinationType))
		{
			return Build(
				sourceCollectionParameter_OrExpressionBuiltSoFar,
				typeof(List<>).MakeGenericType(sourceCollectionParameter_OrExpressionBuiltSoFar.Type.GetCollectionItemType()));
		}

		if (IsNonGenericConstrainedCollection(sourceCollectionParameter_OrExpressionBuiltSoFar.Type, out var sourceCollectionConstrainedTypeInfo))
		{
			var sourceCollectionAsGenericCollectionCallMethodExpression = sourceCollectionConstrainedTypeInfo.BuildConvertorToNativeRootTypeMethodCallExpression(sourceCollectionParameter_OrExpressionBuiltSoFar);
			if (IsNonGenericConstrainedCollection(destinationType, out var destinationCollectionConstrainedTypeInfo))
			{
				var intermediateCollectionType = destinationCollectionConstrainedTypeInfo.FullyNativeRootType;
				var toIntermediatedCollectionType = Build(sourceCollectionAsGenericCollectionCallMethodExpression, intermediateCollectionType);

				return Expression.Call(
					destinationCollectionConstrainedTypeInfo.InvokableInstanciationMethod,
					toIntermediatedCollectionType);
			}

			return Build(sourceCollectionAsGenericCollectionCallMethodExpression, destinationType);
		}


		var collectionConversionCallExpression = ResolveCollectionConversionCallExpressionBuilderFor(destinationType);
		return collectionConversionCallExpression.BuildConversionCallExpression(sourceCollectionParameter_OrExpressionBuiltSoFar, destinationType);
	}

	static CollectionConversionCallExpressionBuilder ResolveCollectionConversionCallExpressionBuilderFor(Type destinationType)
	=> destinationType switch
	{
		var t when t.IsArray => CollectionToGenericArrayConverter.Instance,
		var t when t.IsInterface => t switch
		{
			var genericItf when genericItf.IsOrImplementsGenericInterface(typeof(IDictionary<,>), out _) => CollectionToGenericDictionaryConverter.Instance,
			var genericItf when genericItf.IsOrImplementsGenericInterface(typeof(IReadOnlyDictionary<,>), out _) => CollectionToGenericDictionaryConverter.Instance,
			var genericItf when genericItf.IsOrImplementsGenericInterface(typeof(ISet<>), out _) => CollectionToGenericHashSetConverter.Instance,
			var genericItf when genericItf.IsOrImplementsGenericInterface(typeof(IReadOnlySet<>), out _) => CollectionToGenericHashSetConverter.Instance,
			_ => CollectionToGenericListConverter.Instance,
		},
		var t when !t.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out _) => t switch
		{
			var nonIEnumerableType when nonIEnumerableType.ImplementsGenericInterface(typeof(IConstrainedCollection<,>), out _) => CollectionToGenericConstrainedCollectionConversionCallExpressionBuilder.Instance,
			var nonIEnumerableType when nonIEnumerableType.ImplementsGenericInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>), out _) => CollectionToGenericConstrainedCollectionConversionCallExpressionBuilder.Instance,
			_ => throw new NotSupportedException()
		},
		var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
		{
			var gt when gt == typeof(List<>) => CollectionToGenericListConverter.Instance,
			var gt when gt == typeof(HashSet<>) => CollectionToGenericHashSetConverter.Instance,
			var gt when gt == typeof(LinkedList<>) => CollectionToGenericLinkedListConverter.Instance,
			var gt when gt == typeof(Queue<>) => CollectionToGenericQueueConverter.Instance,
			var gt when gt == typeof(SortedSet<>) => CollectionToGenericSortedSetConverter.Instance,
			var gt when gt == typeof(Stack<>) => CollectionToGenericStackConverter.Instance,
			var gt when gt == typeof(Dictionary<,>) => CollectionToGenericDictionaryConverter.Instance,
			var gt when gt == typeof(SortedDictionary<,>) => CollectionToGenericSortedDictionaryConverter.Instance,
			var gt when gt == typeof(SortedList<,>) => CollectionToGenericSortedListConverter.Instance,
			_ => t switch
			{
				var tNotInSystemCollectionNamespace when ConstrainedTypeInfos.Include(tNotInSystemCollectionNamespace) => CollectionToGenericConstrainedCollectionConversionCallExpressionBuilder.Instance,
				_ => DefaultConversionCallExpressionBuilder.Instance
			}
		},
		_ => DefaultConversionCallExpressionBuilder.Instance
	};

	static bool IsNonGenericConstrainedCollection(Type sourceCollectionType, out ConstrainedTypeInfo constrainedTypeInfo)
	{
		constrainedTypeInfo = null!;
		return !sourceCollectionType.IsGenericType && ConstrainedTypeInfos.TryGet(sourceCollectionType, out constrainedTypeInfo);
	}

	MethodCallExpression BuildConversionCallExpression(Expression sourceCollectionParameter_OrExpressionBuiltSoFar, Type destinationCollectionType)
	{
		var sourceCollectionType = sourceCollectionParameter_OrExpressionBuiltSoFar.Type;
		var sourceCollectionItemType = sourceCollectionType.GetCollectionItemType();
		var destinationCollectionItemType = destinationCollectionType.GetCollectionItemType();
		var closedGenericToDestinationCollectionMethod = GetClosedGenericToDestinationCollectionMethod(destinationCollectionType, destinationCollectionItemType);

		if (sourceCollectionItemType == destinationCollectionItemType)
		{
			return CallConversionToDestinationCollectionType(sourceCollectionParameter_OrExpressionBuiltSoFar, closedGenericToDestinationCollectionMethod);
		}

		var closedGenericSelectMethod = OpenGenericSelectMethod.MakeGenericMethod(sourceCollectionItemType, destinationCollectionItemType);
		var selectClauseLambda = SelectClauseLambdaExpressionBuilder.Build(sourceCollectionItemType, destinationCollectionItemType);
		return CallSelectThenToDestinationCollectionType(
			sourceCollectionParameter_OrExpressionBuiltSoFar,
			closedGenericSelectMethod,
			selectClauseLambda,
			closedGenericToDestinationCollectionMethod);
	}
}
