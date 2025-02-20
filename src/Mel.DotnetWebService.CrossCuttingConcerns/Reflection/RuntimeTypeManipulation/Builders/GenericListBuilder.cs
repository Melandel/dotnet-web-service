using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class GenericListBuilder
{
	readonly dynamic _genericListUnderConstruction;
	readonly Type _itemType;
	static readonly Dictionary<Type, Func<int, dynamic>> InstanciationOperationByItemType = [];
	static readonly Dictionary<Type, Func2> AddOperationByItemType = [];
	static readonly Dictionary<Type, Func2> AddRangeOperationByItemType = [];
	const int DefaultCapacity = 2;
	GenericListBuilder(dynamic genericListInstance, Type itemType)
	{
		_genericListUnderConstruction = genericListInstance;
		_itemType = itemType;
	}
	public static GenericListBuilder For(Type itemType, int capacity = DefaultCapacity)
	{
		if (!InstanciationOperationByItemType.TryGetValue(itemType, out var existingInstanciationOperation))
		{
			existingInstanciationOperation = BuildInstanciationOperation(itemType);
			InstanciationOperationByItemType.Add(itemType, existingInstanciationOperation);
		}
		var genericListInstance = InstanciationOperationByItemType[itemType].Invoke(capacity);

		if (!AddOperationByItemType.TryGetValue(itemType, out var existingAddOperation))
		{
			var addMethod = typeof(GenericListBuilder)
				.GetMethod(nameof(AddItemToList), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(itemType);
			existingAddOperation = Func2.CompileCallToStaticMethod(addMethod);
			AddOperationByItemType.Add(itemType, existingAddOperation);
		}

		if (!AddRangeOperationByItemType.TryGetValue(itemType, out var existingAddRangeOperation))
		{
			var addRangeMethod = typeof(GenericListBuilder)
				.GetMethod(nameof(AddItemsToList), BindingFlags.Static | BindingFlags.NonPublic)!
				.MakeGenericMethod(itemType);
			existingAddRangeOperation = Func2.CompileCallToStaticMethod(addRangeMethod);
			AddRangeOperationByItemType.Add(itemType, existingAddRangeOperation);
		}

		return new(genericListInstance, itemType);
	}

	public GenericListBuilder Add(object item)
	{
		var addOperation = AddOperationByItemType[_itemType];
		addOperation.Invoke(item, _genericListUnderConstruction);
		return this;
	}

	public GenericListBuilder AddRange(IEnumerable items)
	{
		var addRangeOperation = AddRangeOperationByItemType[_itemType];
		addRangeOperation.Invoke(items, _genericListUnderConstruction);
		return this;
	}

	static List<T> AddItemToList<T>(T item, List<T> list) { list.Add(item); return list; }
	static List<T> AddItemsToList<T>(IEnumerable<T> items, List<T> list) { list.AddRange(items); return list; }

	public IList BuildAsIList()
	=> _genericListUnderConstruction;

	public dynamic BuildAsDynamic()
	=> _genericListUnderConstruction;

	static Func<int, dynamic> BuildInstanciationOperation(Type type)
	{
		var listType = typeof(List<>).MakeGenericType(type);
		var ctor = listType.GetConstructor(new[] { typeof(int) })!;

		var capacityParam = Expression.Parameter(typeof(int), "capacity");
		var newExpr = Expression.New(ctor, capacityParam);

		return Expression
			.Lambda<Func<int, dynamic>>(
					newExpr,
					capacityParam)
			.Compile();
	}
}
