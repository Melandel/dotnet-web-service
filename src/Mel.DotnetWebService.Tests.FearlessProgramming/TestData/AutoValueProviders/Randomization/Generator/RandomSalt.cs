using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

static class RandomSalt
{
	public static Type Type = typeof(int);
	static readonly MethodInfo RandomNextMethod = typeof(Random).GetMethod(nameof(Random.Next), [typeof(int), typeof(int)]);
	static readonly MemberExpression RandomSharedProperty = Expression.Property(null, typeof(Random), nameof(Random.Shared));
	static readonly ConstantExpression ConstantZero = Expression.Constant(0);
	static readonly ConstantExpression ConstantOneHundred = Expression.Constant(100);
	static readonly ConstantExpression ConstantOne = Expression.Constant(1);
	static readonly ConstantExpression ConstantTen = Expression.Constant(10);

	public static readonly Expression Select = Expression.Call(RandomSharedProperty, RandomNextMethod, ConstantZero, ConstantOneHundred);
	public static readonly Expression SelectBetweenOneAndTen = Expression.Call(RandomSharedProperty, RandomNextMethod, ConstantOne, ConstantTen);
}
