using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicCodeInvocation;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicObjectInstantiation;

class GenericListBuilder
{
	const int DefaultCapacity = 2;
	readonly dynamic _genericListUnderConstruction;
	readonly Lazy<CompiledInvokable> _addOperation;
	readonly Lazy<CompiledInvokable> _addRangeOperation;
	static readonly ConcurrentDictionary<Type, CompiledInvokable> ListInstantiatorsByItemType = [];
	static readonly ConcurrentDictionary<Type, MethodInfo> ListAddMethodsByItemType = [];
	static readonly ConcurrentDictionary<Type, MethodInfo> ListAddRangeMethodsByItemType = [];
	static readonly MethodInfo OpenGenericListInstantiationMethod = typeof(GenericListBuilder).GetMethod(nameof(CreateList), BindingFlags.Static | BindingFlags.NonPublic)!;
	GenericListBuilder(Type itemType, int capacity)
	{
		_genericListUnderConstruction = GetListInstantiator(itemType).Invoke(capacity);
		var listUnderConstructionParameter = Expression.Constant(_genericListUnderConstruction);

		_addOperation = new Lazy<CompiledInvokable>(() => BuildAddOperation(itemType, listUnderConstructionParameter));
		_addRangeOperation = new Lazy<CompiledInvokable>(() => BuildAddRangeOperation(itemType, listUnderConstructionParameter));
	}

	private static CompiledInvokable GetListInstantiator(Type itemType)
	=> ListInstantiatorsByItemType.GetOrAdd(itemType, _ => CompiledInvokable.FromMethodInfo(OpenGenericListInstantiationMethod.MakeGenericMethod(itemType)));

	record GenericListBuilderAdditionMethods(Type listType, MethodInfo AddMethod, MethodInfo AddRangeMethod);
	public static GenericListBuilder For(Type itemType, int capacity = DefaultCapacity)
	=>  new(itemType, capacity);

	public GenericListBuilder Add(object item)
	{
		_addOperation.Value.Invoke(item);
		return this;
	}

	public GenericListBuilder AddRange(IEnumerable items)
	{
		_addRangeOperation.Value.Invoke(items);
		return this;
	}

	static List<T> CreateList<T>(int capacity) => new List<T>(capacity);

	public IList BuildAsIList()
	=> _genericListUnderConstruction;

	static CompiledInvokable BuildAddOperation(Type itemType, ConstantExpression listUnderConstructionParameter)
	{
		var listAddMethod = GetListAddMethod(itemType);
		var listAddMethodParameter = listAddMethod.GetParameters().First();
		var itemParameter = Expression.Parameter(listAddMethodParameter.ParameterType, listAddMethodParameter.Name);


		return CompiledInvokable.FromExpression(Expression.Lambda(
			Expression.Call(
				listUnderConstructionParameter,
				listAddMethod,
				itemParameter),
			itemParameter));
	}

	static CompiledInvokable BuildAddRangeOperation(Type itemType, ConstantExpression listUnderConstructionParameter)
	{
		var listAddRangeMethod = GetListAddRangeMethod(itemType);
		var listAddRangeMethodParameter = listAddRangeMethod.GetParameters().First();
		var itemParameter = Expression.Parameter(listAddRangeMethodParameter.ParameterType, listAddRangeMethodParameter.Name);

		return CompiledInvokable.FromExpression(
			Expression.Lambda(
				Expression.Call(
					listUnderConstructionParameter,
					listAddRangeMethod,
					itemParameter),
				itemParameter));
	}

	static MethodInfo GetListAddMethod(Type itemType)
	=> ListAddMethodsByItemType.GetOrAdd(
		itemType,
		_ => typeof(List<>).MakeGenericType(itemType).GetMethod(nameof(List<object>.Add), BindingFlags.Public | BindingFlags.Instance)!);

	static MethodInfo GetListAddRangeMethod(Type itemType)
	=> ListAddRangeMethodsByItemType.GetOrAdd(
		itemType,
		_ => typeof(List<>).MakeGenericType(itemType).GetMethod(nameof(List<object>.AddRange), BindingFlags.Public | BindingFlags.Instance)!);
}
