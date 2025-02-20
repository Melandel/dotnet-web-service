using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

class EnumExampleInstanciationBuilder : RandomizedExampleInstanciationBuilder
{
	public static readonly EnumExampleInstanciationBuilder Instance = new();
	static readonly MethodInfo OpenGenericGetEnumValueCandidates = typeof(EnumExampleInstanciationBuilder)
		.GetMethod(nameof(EnumExampleInstanciationBuilder.GetEnumValueCandidates), BindingFlags.NonPublic | BindingFlags.Static);

	public override Expression BuildInstanciationExpressionFor(Type type, Expression currentSalt, Expression generativeSalts)
	{
		var nonZeroEnumValues = Expression.Variable(type,        "nonZeroEnumValues");
		var index             = Expression.Variable(typeof(int), "index");

		return Expression.Block(
			[nonZeroEnumValues, index],

			Expression.Assign(nonZeroEnumValues, Expression.Call(OpenGenericGetEnumValueCandidates.MakeGenericMethod(type))),
			Expression.Assign(index,             Expression.Modulo(currentSalt, Expression.ArrayLength(nonZeroEnumValues))),

			GenerativeSalts.Enqueue(generativeSalts, index),
			Expression.ArrayIndex(nonZeroEnumValues, index));
	}

	static TEnum[] GetEnumValueCandidates<TEnum>() where TEnum : Enum
	=> Enum.GetValues(typeof(TEnum))
		.Cast<object>()
		.Where(e => (int) e != 0)
		.OrderBy(e => (int) e)
		.ThenBy(e => e.ToString())
		.Cast<TEnum>()
		.ToArray();
}
