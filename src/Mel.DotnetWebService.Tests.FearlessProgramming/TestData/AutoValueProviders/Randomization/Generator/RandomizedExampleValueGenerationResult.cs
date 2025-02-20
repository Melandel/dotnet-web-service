using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

static class RandomizedExampleValueGenerationResult
{
	static readonly Type OpenGenericType = typeof(RandomizedExampleValueGenerationResult<>);
	static ConstructorInfo GetClosedGenericQueueConstructor(Type type)
	=> OpenGenericType.MakeGenericType(type).GetConstructors().First();
	public static Expression Create(Type type, ParameterExpression exampleValue, ParameterExpression generativeSalts)
	=> Expression.Block(
			[exampleValue, generativeSalts],
			Expression.New(
				GetClosedGenericQueueConstructor(type),
				exampleValue,
				generativeSalts));
}
