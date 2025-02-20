using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class EnumExampleInstantiationBuilder : ExampleInstantiationBuilder
{
	public static readonly EnumExampleInstantiationBuilder Instance = new();
	static readonly MethodInfo OpenGenericPickEnumValueExample = typeof(EnumExampleInstantiationBuilder)
		.GetMethod(nameof(EnumExampleInstantiationBuilder.PickEnumValueExample), BindingFlags.NonPublic | BindingFlags.Static)!;

	public override Expression BuildInstantiationExpressionFor(Type type, Expression currentSalt, HashSet<Type> parentObjectsAlreadyRecursedThrough)
	=> Expression.Call(
		OpenGenericPickEnumValueExample.MakeGenericMethod(type),
		currentSalt);

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
