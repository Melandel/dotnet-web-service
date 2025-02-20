using System.Linq.Expressions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public abstract class InstanciationExpressionBuilder
{
	static readonly Dictionary<Type, Dictionary<int, Expression>> CachedInstanciationExpressionsBySaltByType = [];
	protected abstract Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0);

	public static Expression BuildFor(Type type, int salt = 0)
	{
		if (IsInstanciationExpressionAlreadyCached(type, salt, out var instanciationExpression))
		{
			return instanciationExpression!;
		}

		var newRecursionStack = new HashSet<Type>();
		instanciationExpression = BuildFor(type, newRecursionStack, salt);
		return instanciationExpression;
	}

	protected static Expression BuildFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		if (IsInstanciationExpressionAlreadyCached(type, salt, out var instanciationExpression))
		{
			return instanciationExpression!;
		}

		instanciationExpression = Build(type, recursionStack, salt);
		Cache(instanciationExpression, type, salt);
		return instanciationExpression;
	}

	static bool IsInstanciationExpressionAlreadyCached(Type type, int salt, out Expression? instanciationExpression)
	{
		if (CachedInstanciationExpressionsBySaltByType.TryGetValue(type, out var instanciationExpressionsBySalt)
			&& instanciationExpressionsBySalt.TryGetValue(salt, out instanciationExpression))
		{
			return true;
		};
		instanciationExpression = null;
		return false;
	}

	static Expression Build(Type type, HashSet<Type> recursionStack, int salt)
	{
		if (!recursionStack.Add(type))
		{
			throw new InvalidOperationException($"Circular construction dependency detected for '{type.GetName()}'.");
		}

		var typeCategory = TypeCategoryResolver.Resolve(type);
		var builder = InstanciationExpressionBuilderResolver.Resolve(typeCategory, type);
		var instanciationExpression = builder.BuildInstanciationExpressionFor(type, recursionStack, salt);
		recursionStack.Remove(type);

		return instanciationExpression;
	}

	static void Cache(Expression instanciationExpression, Type type, int salt)
	{
		if (CachedInstanciationExpressionsBySaltByType.TryGetValue(type, out var instanciationExpressionsBySalt))
		{
			instanciationExpressionsBySalt.Add(salt, instanciationExpression);
		}
		else
		{
			instanciationExpressionsBySalt = new Dictionary<int, Expression>() { { salt, instanciationExpression } };
			CachedInstanciationExpressionsBySaltByType.Add(type, instanciationExpressionsBySalt);
		}
	}
}

class InstanciationExpressionBuilderResolver
{
	public static InstanciationExpressionBuilder Resolve(TypeCategory typeCategory, Type type)
	=> typeCategory switch
	{
		//TypeCategory.NativeValueType => NativeValueExampleGenerator.InstanceSuitedFor(type),
		TypeCategory.NativeValueType => NativeValueInstanciationExpressionBuilder.InstanceSuitedFor(type),
		TypeCategory.EnumType => EnumInstanciationBuilder.Instance,
		TypeCategory.CollectionType => CollectionInstanciationExpressionBuilder.Instance,
		TypeCategory.UnconstrainedCollectionOfKeyValuePairsType => UnconstrainedCollectionOfKeyValuePairsInstanciationExpressionBuilder.Instance,
		TypeCategory.ParentObjectType => ParentObjectInstanciationExpressionBuilder.InstanceSuitedFor(type),
		_ => throw new NotImplementedException()
	};
}

class UnconstrainedCollectionOfKeyValuePairsInstanciationExpressionBuilder : InstanciationExpressionBuilder
{
	public static UnconstrainedCollectionOfKeyValuePairsInstanciationExpressionBuilder Instance = new();
	UnconstrainedCollectionOfKeyValuePairsInstanciationExpressionBuilder()
	{}

	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
	}
}

class EnumInstanciationBuilder : InstanciationExpressionBuilder
{
	public static EnumInstanciationBuilder Instance = new();
	EnumInstanciationBuilder()
	{}

	protected override Expression BuildInstanciationExpressionFor(Type type, HashSet<Type> recursionStack, int salt = 0)
	{
		throw new NotImplementedException();
	}
}
