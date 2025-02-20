using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
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
	protected abstract MethodCallExpression CallConversionToDestinationCollectionType(ParameterExpression sourceCollectionParameter, MethodInfo closedGenericToDestinationCollectionMethod);
	protected abstract MethodCallExpression CallSelectThenToDestinationCollectionType(ParameterExpression sourceCollectionParameter, MethodInfo closedGenericSelectMethod, LambdaExpression selectClauseContent, MethodInfo closedGenericToDestinationCollectionMethod);

	public static MethodCallExpression Build(ParameterExpression sourceCollectionParameter, Type destinationType)
	{
		if (destinationType == typeof(IDictionary))
		{
			if (!sourceCollectionParameter.Type.ImplementsGenericIEnumerableOfKeyPairValues(out var keyType, out var valueType))
			{
				throw new InvalidOperationException();
			}
			var dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
			return Build(sourceCollectionParameter, dictionaryType);
		}
		if (NonGenericCollectionInterfaceTypes.Contains(destinationType))
		{
			var listType = typeof(List<>).MakeGenericType(sourceCollectionParameter.Type.GetCollectionItemType());
			return Build(sourceCollectionParameter, listType);
		}
		CollectionConversionCallExpressionBuilder collectionConversionCallExpression = destinationType switch
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
			var t when !t.IsOrImplementsGenericInterface(typeof(IEnumerable<>), out _) => throw new InvalidOperationException(),
			var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
			{
				var gt when gt == typeof(List<>)        => CollectionToGenericListConverter.Instance,
				var gt when gt == typeof(HashSet<>)     => CollectionToGenericHashSetConverter.Instance,
				var gt when gt == typeof(LinkedList<>)  => CollectionToGenericLinkedListConverter.Instance,
				var gt when gt == typeof(Queue<>)       => CollectionToGenericQueueConverter.Instance,
				var gt when gt == typeof(SortedSet<>)   => CollectionToGenericSortedSetConverter.Instance,
				var gt when gt == typeof(Stack<>)       => CollectionToGenericStackConverter.Instance,
				var gt when gt == typeof(Dictionary<,>)       => CollectionToGenericDictionaryConverter.Instance,
				var gt when gt == typeof(SortedDictionary<,>) => CollectionToGenericSortedDictionaryConverter.Instance,
				var gt when gt == typeof(SortedList<,>)       => CollectionToGenericSortedListConverter.Instance,
				_ => t switch
				{
					var tNotInSystemCollectionNamespace when ConstrainedTypeInfos.Include(tNotInSystemCollectionNamespace) => CollectionToGenericConstrainedCollectionConversionCallExpressionBuilder.Instance,
					_ => DefaultConversionCallExpressionBuilder.Instance
				}
			},
			_ => DefaultConversionCallExpressionBuilder.Instance
	//		var t when t.ImplementsGenericInterface(typeof(IConstrainedCollectionOfKeyValuePairs<,,>), out _) => CollectionToConstrainedKeyValuePairsConverter.Instance,
	//		var t when t.ImplementsGenericInterface(typeof(IConstrainedCollection<,>), out _) => CollectionToConstrainedCollectionConverter.Instance,
	//		var t when t.IsInterface => t switch
	//		{
	//			var itf when itf.IsGenericType => itf.GetGenericTypeDefinition() switch
	//			{
	//				var igd when igd == typeof(IList<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(IEnumerable<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(ICollection<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(IReadOnlyCollection<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(ISet<>) => CollectionToListConverter.Instance,
	//				var igd when igd == typeof(IReadOnlySet<>) => CollectionToListConverter.Instance,
	//				_ => NotImplementedCollectionConverter.Instance
	//			},
	//			var itf when itf == typeof(System.Collections.IEnumerable) => NotImplementedCollectionConverter.Instance,
	//			var itf when itf == typeof(System.Collections.ICollection) => NotImplementedCollectionConverter.Instance,
	//			var itf when itf == typeof(System.Collections.IDictionary) => NotImplementedCollectionConverter.Instance,
	//			_ => NotImplementedCollectionConverter.Instance
	//		},
	//		var t when t.IsGenericType => t.GetGenericTypeDefinition() switch
	//		{
	//			var gt when gt == typeof(HashSet<>) => CollectionToHashSetConverter.Instance,
	//			var gt when gt == typeof(LinkedList<>) => CollectionToLinkedListConverter.Instance,
	//			var gt when gt == typeof(List<>) => CollectionToListConverter.Instance,
	//			var gt when gt == typeof(Queue<>) => CollectionToQueueConverter.Instance,
	//			var gt when gt == typeof(SortedSet<>) => CollectionToSortedSetConverter.Instance,
	//			var gt when gt == typeof(Stack<>) => CollectionToStackConverter.Instance,
	//			var gt when gt == typeof(Dictionary<,>) => CollectionToDictionaryConverter.Instance,
	//			var gt when gt == typeof(SortedDictionary<,>) => CollectionToSortedDictionaryConverter.Instance,
	//			var gt when gt == typeof(SortedList<,>) => CollectionToSortedListConverter.Instance,
	//			_ => NotImplementedCollectionConverter.Instance
	//		},
	//		_ => CollectionToClassWithMatchingSingleInstanciatorParameterConverter.Instance
		};
		return collectionConversionCallExpression.BuildConversionCallExpression(sourceCollectionParameter, destinationType);
	}

	MethodCallExpression BuildConversionCallExpression(ParameterExpression sourceCollectionParameter, Type destinationCollectionType)
	{
		var sourceCollectionType = sourceCollectionParameter.Type;
		var sourceCollectionItemType = sourceCollectionType.GetCollectionItemType();
		var destinationCollectionItemType = destinationCollectionType.GetCollectionItemType();
		var closedGenericToDestinationCollectionMethod = GetClosedGenericToDestinationCollectionMethod(destinationCollectionType, destinationCollectionItemType);

		if (sourceCollectionItemType == destinationCollectionItemType)
		{
			return CallConversionToDestinationCollectionType(sourceCollectionParameter, closedGenericToDestinationCollectionMethod);
		}

		var closedGenericSelectMethod = OpenGenericSelectMethod.MakeGenericMethod(sourceCollectionItemType, destinationCollectionItemType);
		var selectClause = ValueConversionLambdaExpressionBuilder.Build(sourceCollectionItemType, destinationCollectionItemType);
		return CallSelectThenToDestinationCollectionType(
			sourceCollectionParameter,
			closedGenericSelectMethod,
			selectClause,
			closedGenericToDestinationCollectionMethod);
	}
}
