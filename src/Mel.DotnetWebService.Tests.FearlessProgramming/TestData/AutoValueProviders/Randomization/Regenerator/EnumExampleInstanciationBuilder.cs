using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Regenerator;

class EnumExampleInstanciationBuilder : ExampleRegenerationBuilder
{
	public static readonly EnumExampleInstanciationBuilder Instance = new();
	static readonly MethodInfo OpenGenericPickEnumValueExample = typeof(EnumExampleInstanciationBuilder)
		.GetMethod(nameof(EnumExampleInstanciationBuilder.PickEnumValueExample), BindingFlags.NonPublic | BindingFlags.Static)!;

	public override Expression BuildInstanciationExpressionFor(Type type, ParameterExpression generativeSalts)
	=> Expression.Call(
		OpenGenericPickEnumValueExample.MakeGenericMethod(type),
		GenerativeSalts.Dequeue(generativeSalts));

	static TEnum PickEnumValueExample<TEnum>(int salt) where TEnum : Enum
	{
		var enumValueCandidates = GetEnumValueCandidates<TEnum>();
		return enumValueCandidates[salt % enumValueCandidates.Length];
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
