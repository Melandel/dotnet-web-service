using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.Randomization;

static class GenerativeSalts
{
	public static readonly Type Type = typeof(Queue<int>);
	public static readonly ConstantExpression MarkerForEmptySystemGenericCollections = Expression.Constant(-2);
	public static readonly ConstantExpression MarkerForNullReference = Expression.Constant(-1);

	static readonly ConstructorInfo ClosedGenericQueueConstructor = Type.GetConstructor(Type.EmptyTypes);
	static readonly MethodInfo EnqueueMethod = Type.GetMethod(nameof(Queue<>.Enqueue));
	static readonly MethodInfo DequeueMethod = Type.GetMethod(nameof(Queue<>.Dequeue));
	static readonly MethodInfo PeekMethod = Type.GetMethod(nameof(Queue<>.Peek));

	public static readonly Expression Create = Expression.New(ClosedGenericQueueConstructor);
	public static Expression Enqueue(Expression queueInstance, Expression salt) => Expression.Call(queueInstance, EnqueueMethod, salt);
	public static Expression Dequeue(Expression queueInstance) => Expression.Call(queueInstance, DequeueMethod);
	public static Expression Peek(Expression queueInstance) => Expression.Call(queueInstance, PeekMethod);
}
