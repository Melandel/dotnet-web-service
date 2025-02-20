using System.Collections;
using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
using NUnit.Framework.Constraints;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.NUnitCustomizations.Constraints;

public class CollectionEquivalentConstraint : NUnit.Framework.Constraints.CollectionEquivalentConstraint
{
	public CollectionEquivalentConstraint(IEnumerable expected) : base(expected)
	{
	}

	public override ConstraintResult ApplyTo<TActual>(TActual actual)
	{
		var nativeConstraintResult = base.ApplyTo(actual);
		if (nativeConstraintResult.Status == ConstraintStatus.Failure)
		{
			var expected = (IEnumerable) Arguments[0];
			if (actual.GetType().IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var actualItemTypes) && expected.GetType().IsOrImplementsGenericInterface(typeof(IEnumerable<>), out var expectedItemTypes))
			{
				var actualItemType = actualItemTypes[0];
				var expectedItemType = expectedItemTypes[0];

				if (actualItemType.IsKeyValuePairType(out var keyType1, out var valueType1) && expectedItemType.IsKeyValuePairType(out var keyType2, out var valueType2))
				{
					var actualKeys = GetKeys(actual);
					var expectedKeys = GetKeys(expected);
					ConstraintResult constraintResultForKeys = new CollectionEquivalentConstraint(actualKeys).ApplyTo(expectedKeys);
					if (!constraintResultForKeys.IsSuccess)
					{
						return constraintResultForKeys;
					}

					var actualValues = GetValues(actual);
					var expectedValues = GetValues(expected);
					ConstraintResult constraintResultForValues = new CollectionEquivalentConstraint(actualValues).ApplyTo(expectedValues);
					if (!constraintResultForValues.IsSuccess)
					{
						return constraintResultForValues;
					}

					return new ConstraintResult(this, actual, isSuccess: true);
				}

				if (actualItemType.IsEnum && expectedItemType == typeof(string))
				{
					var expectedAsStrings = expected;
					var actualAsStrings = BuildCollectionOfStrings(actual);
					return new NUnit.Framework.Constraints.CollectionEquivalentConstraint(expectedAsStrings).ApplyTo(actualAsStrings);
				}

				if (actualItemType == typeof(string) && expectedItemType.IsEnum)
				{
					var actualAsStrings = actual;
					var expectedAsStrings = BuildCollectionOfStrings(expected);
					return new NUnit.Framework.Constraints.CollectionEquivalentConstraint(expectedAsStrings).ApplyTo(actualAsStrings);
				}

				if (actualItemType.IsEnum && expectedItemType == typeof(int))
				{
					var expectedAsInts = expected;
					var actualAsInts = BuildCollectionOfInts(actual);
					return new NUnit.Framework.Constraints.CollectionEquivalentConstraint(expectedAsInts).ApplyTo(actualAsInts);
				}

				if (actualItemType == typeof(int) && expectedItemType.IsEnum)
				{
					var actualAsInts = actual;
					var expectedAsInts = BuildCollectionOfInts(expected);
					return new NUnit.Framework.Constraints.CollectionEquivalentConstraint(expectedAsInts).ApplyTo(actualAsInts);
				}

				if (actualItemType.IsEnum && ConstrainedTypeInfos.TryGet(expectedItemType, out var constrainedExpectedItemTypeInfo))
				{
					if (constrainedExpectedItemTypeInfo.RootType == typeof(string))
					{
						var expectedAsContrainedStrings = expected;
						var actualAsConstrainedStrings = BuildCollectionOfStrings(actual).Select(x => constrainedExpectedItemTypeInfo.InvokeStaticFactoryMethod(x));
						return new NUnit.Framework.Constraints.CollectionEquivalentConstraint(expectedAsContrainedStrings).ApplyTo(actualAsConstrainedStrings);
					}
					if (constrainedExpectedItemTypeInfo.RootType == typeof(int))
					{
						var expectedAsContrainedInts = expected;
						var actualAsConstrainedInts = BuildCollectionOfStrings(actual).Select(x => constrainedExpectedItemTypeInfo.InvokeStaticFactoryMethod(x));
						return new NUnit.Framework.Constraints.CollectionEquivalentConstraint(expectedAsContrainedInts).ApplyTo(actualAsConstrainedInts);
					}
				}

				if (ConstrainedTypeInfos.TryGet(actualItemType, out var constrainedActualItemTypeInfo) && expectedItemType.IsEnum)
				{
					if (constrainedActualItemTypeInfo.RootType == typeof(string))
					{
						var actualAsConstrainedStrings = actual;
						var expectedAsContrainedStrings = BuildCollectionOfStrings(expected).Select(x => constrainedActualItemTypeInfo.InvokeStaticFactoryMethod(x));
						return new NUnit.Framework.Constraints.CollectionEquivalentConstraint(expectedAsContrainedStrings).ApplyTo(actualAsConstrainedStrings);
					}
					if (constrainedActualItemTypeInfo.RootType == typeof(int))
					{
						var actualAsConstrainedInts = actual;
						var expectedAsContrainedInts = BuildCollectionOfInts(expected).Select(x => constrainedActualItemTypeInfo.InvokeStaticFactoryMethod(x));
						return new NUnit.Framework.Constraints.CollectionEquivalentConstraint(expectedAsContrainedInts).ApplyTo(actualAsConstrainedInts);
					}
				}
			}
		}

		return nativeConstraintResult;
	}

	List<string> BuildCollectionOfStrings(object obj)
	{
		if (obj is IEnumerable enumerable)
		{
			var collectionOfStrings = new List<string>();
			foreach (var item in enumerable)
			{
				collectionOfStrings.Add(item.ToString());
			}
			return collectionOfStrings;
		}

		throw new InvalidOperationException();
	}

	List<int> BuildCollectionOfInts(object obj)
	{
		if (obj is IEnumerable enumerable)
		{
			var collectionOfInts = new List<int>();
			foreach (var item in enumerable)
			{
				collectionOfInts.Add((int)item);
			}
			return collectionOfInts;
		}

		throw new InvalidOperationException();
	}
	static IEnumerable GetKeys(object v)
	{
		var enumerableType = v.GetType()
			.GetInterfaces()
			.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

		var kvpType = enumerableType.GetGenericArguments()[0];

		var keyType = kvpType.GetGenericArguments()[0];
		var valueType = kvpType.GetGenericArguments()[1];

		var method = typeof(CollectionEquivalentConstraint)
			.GetMethod(nameof(GetKeysGeneric), BindingFlags.Static | BindingFlags.NonPublic)
			.MakeGenericMethod(keyType, valueType);

		return (IEnumerable)method.Invoke(null, [v]);
	}

	static IEnumerable<TKey> GetKeysGeneric<TKey, TValue>(
		IEnumerable<KeyValuePair<TKey, TValue>> source)
	{
		foreach (var kvp in source)
			yield return kvp.Key;
	}
	static IEnumerable GetValues(object v)
	{
		var enumerableType = v.GetType()
			.GetInterfaces()
			.First(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

		var kvpType = enumerableType.GetGenericArguments()[0];

		var keyType = kvpType.GetGenericArguments()[0];
		var valueType = kvpType.GetGenericArguments()[1];

		var method = typeof(CollectionEquivalentConstraint)
			.GetMethod(nameof(GetValuesGeneric), BindingFlags.Static | BindingFlags.NonPublic)
			.MakeGenericMethod(keyType, valueType);

		return (IEnumerable)method.Invoke(null, [v]);
	}

	static IEnumerable<TValue> GetValuesGeneric<TKey, TValue>(
			IEnumerable<KeyValuePair<TKey, TValue>> source)
	{
		foreach (var kvp in source)
			yield return kvp.Value;
	}
}
