using System.Linq.Expressions;
using System.Reflection;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders.Randomization.Generator;

static class RandomSalt
{
	public static Type Type = typeof(int);
	static readonly MethodInfo RandomNextMethod = typeof(Random).GetMethod(nameof(Random.Next), [typeof(int), typeof(int)]);
	static readonly MemberExpression RandomSharedProperty = Expression.Property(null, typeof(Random), nameof(Random.Shared));
	public static readonly ConstantExpression ConstantLowestPossibleValue = Expression.Constant(0);

	public const int NumberThatConfidentlyExceedsTheHighestNumberOfExamplesProvidedByAnyType = 100;
	static readonly ConstantExpression ConstantNumberThatConfidentlyExceedsTheNumberOfExamplesProvidedByAnyType
		= Expression.Constant(
			GetZeroBasedIndex(
				NumberThatConfidentlyExceedsTheHighestNumberOfExamplesProvidedByAnyType));
	public static readonly Expression Select = Expression.Call(
		RandomSharedProperty,
		RandomNextMethod,
		ConstantLowestPossibleValue,
		ConstantNumberThatConfidentlyExceedsTheNumberOfExamplesProvidedByAnyType);

	public const int MeanRecurrenceIntervalForEmptyCollectionGeneration = 10;
	static readonly ConstantExpression ConstantMeanRecurrenceIntervalForEmptyCollectionGeneration
		= Expression.Constant(
			GetZeroBasedIndex(
				MeanRecurrenceIntervalForEmptyCollectionGeneration));
	public static readonly Expression SelectBetweenOneAndMeanRecurrenceIntervalForEmptyCollectionGeneration = Expression.Call(
		RandomSharedProperty,
		RandomNextMethod,
		ConstantLowestPossibleValue,
		ConstantMeanRecurrenceIntervalForEmptyCollectionGeneration);

	static int GetZeroBasedIndex(int number) => number - 1;
}
