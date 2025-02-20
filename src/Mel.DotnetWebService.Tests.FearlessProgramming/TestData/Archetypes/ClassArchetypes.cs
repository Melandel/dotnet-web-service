using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;
public static partial class ClassArchetype
{
	public record PositionalRecordContainingAGuid(string FirstProperty, Guid SecondProperty);
	public record PositionalRecordContainingANonEmptyGuid(string FirstProperty, NonEmptyGuid SecondProperty);
	public class GetSetStyleClassContainingANonEmptyGuid { public string FirstProperty { get; set; } public NonEmptyGuid SecondProperty { get; set; } };
	public record PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(string FirstProperty, PositionalRecordContainingANonEmptyGuid SecondPropertyContainingANonEmptyGuid);
	public record PositionalRecordContainingAnArrayOfNonEmptyGuids(string FirstProperty, NonEmptyGuid[] SecondProperty);
	public record PositionalRecordContainingAnArrayOfNonEmptyGuidsAndAnArrayOfInts(string FirstProperty, NonEmptyGuid[] SecondProperty, int[] ThirdProperty);
	public record PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuids(string FirstProperty, PositionalRecordContainingAnArrayOfNonEmptyGuids SecondPropertyContainingAnArrayOfNonEmptyGuids);
	public record PositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(string FirstProperty, PositionalRecordContainingAnArrayOfNonEmptyGuids[] SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(string FirstProperty, PositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids SecondProperty);
	public record PositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(string FirstProperty, List<PositionalRecordContainingAnArrayOfNonEmptyGuids> SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(string FirstProperty, PositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids SecondProperty);
	public record PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(string FirstProperty, IEnumerable<PositionalRecordContainingAnArrayOfNonEmptyGuids> SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(string FirstProperty, PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids SecondProperty);
	public record PositionalRecordContainingAListOfNonEmptyGuids(string FirstProperty, List<NonEmptyGuid> SecondProperty);
	public record PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuids(string FirstProperty, PositionalRecordContainingAListOfNonEmptyGuids SecondPropertyContainingAListOfNonEmptyGuids);
	public record PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(string FirstProperty, IEnumerable<NonEmptyGuid> SecondProperty);
	public record PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfNonEmptyGuids(string FirstProperty, PositionalRecordContainingAnIEnumerableOfNonEmptyGuids SecondPropertyContainingAnIEnumerableOfNonEmptyGuids);
	public record PositionalRecordContainingADictionaryWithNonEmptyGuidValues(string FirstProperty, Dictionary<string, NonEmptyGuid> SecondProperty);
	public record PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidValues(string FirstProperty, PositionalRecordContainingADictionaryWithNonEmptyGuidValues SecondPropertyContainingADictionaryWithNonEmptyGuidValues);
	public record PositionalRecordContainingADictionaryWithNonEmptyGuidKeys(string FirstProperty, Dictionary<NonEmptyGuid, string> SecondProperty);
	public record PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidKeys(string FirstProperty, PositionalRecordContainingADictionaryWithNonEmptyGuidKeys SecondPropertyContainingADictionaryWithNonEmptyGuidKeys);
	public record PositionalRecordContainingADictionaryWithNonEmptyGuidKeysAndValues(string FirstProperty, Dictionary<NonEmptyGuid, NonEmptyGuid> SecondProperty);
	public record PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidKeysAndValues(string FirstProperty, PositionalRecordContainingADictionaryWithNonEmptyGuidKeysAndValues SecondPropertyContainingADictionaryWithNonEmptyGuidKeysAndValues);

	public record PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidValues(string FirstProperty, Dictionary<string, PositionalRecordContainingAnArrayOfNonEmptyGuids> SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidValues(string FirstProperty, PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidValues SecondProperty);
	public record PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidValues(string FirstProperty, Dictionary<string, PositionalRecordContainingAListOfNonEmptyGuids> SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidValues(string FirstProperty, PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidValues SecondProperty);
	public record PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues(string FirstProperty, Dictionary<string, PositionalRecordContainingAnIEnumerableOfNonEmptyGuids> SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues(string FirstProperty, PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues SecondProperty);

	public record PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys(string FirstProperty, Dictionary<PositionalRecordContainingAnArrayOfNonEmptyGuids, string> SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys(string FirstProperty, PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys SecondProperty);
	public record PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys(string FirstProperty, Dictionary<PositionalRecordContainingAListOfNonEmptyGuids, string> SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys(string FirstProperty, PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys SecondProperty);
	public record PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys(string FirstProperty, Dictionary<PositionalRecordContainingAnIEnumerableOfNonEmptyGuids, string> SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys(string FirstProperty, PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys SecondProperty);
	public record PositionalRecordContainingPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys(string FirstProperty, PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys SecondProperty);

	public record PositionalRecordContainingAPositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(string FirstProperty, PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid SecondProperty);
	public record PositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuid(string FirstProperty, PositionalRecordContainingANonEmptyGuid[] SecondProperty);
	public record PositionalRecordContainingAPositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuid(string FirstProperty, PositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuid SecondProperty);
	public record PositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuid(string FirstProperty, List<PositionalRecordContainingANonEmptyGuid> SecondProperty);
	public record PositionalRecordContainingAPositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuid(string FirstProperty, PositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuid SecondProperty);
	public record PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuid(string FirstProperty, IEnumerable<PositionalRecordContainingANonEmptyGuid> SecondProperty);
	public record PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuid(string FirstProperty, PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuid SecondProperty);

	public sealed class NonEmptyGuids : ConstrainedCollection<NonEmptyGuid[]>, IConstrainedCollection<Guid, NonEmptyGuids>
	{
	public static ExampleValues<IEnumerable<Guid>> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues:
		[
			[ Some.Value<NonEmptyGuid>() ],
			[ Some.Value<NonEmptyGuid>(), Another.Value<NonEmptyGuid>() ],
		],
		constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid>>([ Guid.Empty ], "Value must not be empty") ]);

		NonEmptyGuids(NonEmptyGuid[] value) : base(value)
		{ }
		public static NonEmptyGuids FromStrings(params string[] strings)
		{
			try
			{
				return ApplyConstraintsTo(strings.Select(str => Guid.Parse(str)));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuids>(strings);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuids>(developerMistake, strings);
			}
		}

		public static NonEmptyGuids ApplyConstraintsTo(IEnumerable<Guid> collection)
		{
			try
			{
				return new(collection.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuids>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuids>(developerMistake, collection);
			}
		}
	}
	public sealed class NonEmptyGuidsWithPublicProperty : ConstrainedCollection<NonEmptyGuid[]>, IConstrainedCollection<Guid, NonEmptyGuidsWithPublicProperty>
	{
		public int PublicProperty => Collection.Length;

		public static ExampleValues<IEnumerable<Guid>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ Some.Value<NonEmptyGuid>() ],
				[ Some.Value<NonEmptyGuid>(), Another.Value<NonEmptyGuid>() ],
			],
		constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid>>([ Guid.Empty ], "Value must not be empty") ]);

		NonEmptyGuidsWithPublicProperty(NonEmptyGuid[] value) : base(value)
		{
		}

		public static NonEmptyGuidsWithPublicProperty FromStrings(params string[] strings)
		{
			try
			{
				return ApplyConstraintsTo(strings.Select(guid => Guid.Parse(guid)));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuidsWithPublicProperty>(strings);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuidsWithPublicProperty>(developerMistake, strings);
			}
		}

		public static NonEmptyGuidsWithPublicProperty ApplyConstraintsTo(IEnumerable<Guid> collection)
		{
			try
			{
				return new(collection.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuidsWithPublicProperty>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuidsWithPublicProperty>(developerMistake, collection);
			}
		}
	}

	public sealed class EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter : ConstrainedCollection<NonEmptyGuid[]>, IConstrainedCollection<Guid, EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter>
	{
		EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter(NonEmptyGuid[] collection) : base(collection)
		{
		}

		public static ExampleValues<IEnumerable<Guid>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ Some.Value<NonEmptyGuid>() ],
				[ Some.Value<NonEmptyGuid>(), Another.Value<NonEmptyGuid>() ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid>>([ Guid.Empty ], "Value must not be empty") ]);

		public static EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter ApplyConstraintsTo(IEnumerable<Guid> collection)
		{
			try
			{
				return new(collection.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid)).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuids>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuids>(developerMistake, collection);
			}
		}
	}

	public sealed class EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter : ConstrainedCollection<HashSet<NonEmptyGuid>>, IConstrainedCollection<Guid, EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter>
	{
		EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter(HashSet<NonEmptyGuid> collection) : base(collection)
		{
		}

		public static ExampleValues<IEnumerable<Guid>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ Some.Value<NonEmptyGuid>() ],
				[ Some.Value<NonEmptyGuid>(), Another.Value<NonEmptyGuid>() ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid>>([ Guid.Empty ], "Value must not be empty") ]);

		public static EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter ApplyConstraintsTo(IEnumerable<Guid> collection)
		{
			try
			{
				return new(new HashSet<NonEmptyGuid>(collection.Select(guid => NonEmptyGuid.ApplyConstraintsTo(guid))));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuids>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuids>(developerMistake, collection);
			}
		}
	}
	public sealed class EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter : ConstrainedCollection<HashSet<NonEmptyGuid>>, IConstrainedCollection<Guid, EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter>
	{
		EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter(HashSet<NonEmptyGuid> collection) : base(collection)
		{
		}

		public static ExampleValues<IEnumerable<Guid>> Examples
		=> ExampleValues.ValidAndInvalid(
			validValues:
			[
				[ Some.Value<NonEmptyGuid>() ],
				[ Some.Value<NonEmptyGuid>(), Another.Value<NonEmptyGuid>() ],
			],
			constraintViolationExamples: [ ConstraintViolationExample.Document<IEnumerable<Guid>>([ Guid.Empty ], "Value must not be empty") ]);

		public static EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter ApplyConstraintsTo(IEnumerable<Guid> collection)
		{
			try
			{
				return new(new HashSet<NonEmptyGuid>(collection.Select(g => NonEmptyGuid.ApplyConstraintsTo(g))));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuids>(collection);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuids>(developerMistake, collection);
			}
		}
	}

	public sealed class EncapsulationOfADictionaryWithNonEmptyGuidKeysAndDistinctStringValues: ConstrainedCollectionOfKeyValuePairs<Dictionary<NonEmptyGuid, string>>, IConstrainedCollectionOfKeyValuePairs<Guid, string, EncapsulationOfADictionaryWithNonEmptyGuidKeysAndDistinctStringValues>
	{
		EncapsulationOfADictionaryWithNonEmptyGuidKeysAndDistinctStringValues(Dictionary<NonEmptyGuid, string> collectionOfKeyValuePairs) : base(collectionOfKeyValuePairs)
		{
			if (collectionOfKeyValuePairs.Values.Distinct().Count() != collectionOfKeyValuePairs.Count)
			{
				throw ObjectConstructionException.WhenConstructingAMemberFor<EncapsulationOfADictionaryWithNonEmptyGuidKeysAndDistinctStringValues>(nameof(CollectionOfKeyValuePairs), CollectionOfKeyValuePairs, $"@member should not have duplicate values, but instead there are {string.Join(',', collectionOfKeyValuePairs.GroupBy(kvp => kvp.Value).Where(g => g.Count() > 1).OrderByDescending(g => g.Count()).Select(g => $"{g.Count()} keys associated with the value {g.First().Value.GetStringRepresentation()} : {g.Select(kvp => kvp.Key).GetStringRepresentation()}"))}");
			}
		}

		public static ExampleValues<IEnumerable<KeyValuePair<Guid, string>>> Examples
		=> ExampleValues.ValidAndInvalid(
		validValues:
		[
			[],
			[
				KeyValuePair.Create<Guid, string>(Some.Value<NonEmptyGuid>(), Some.Value<string>())
			],
			[
				KeyValuePair.Create<Guid, string>(Some.Value<NonEmptyGuid>(), Some.Value<string>()),
				KeyValuePair.Create<Guid, string>(Another.Value<NonEmptyGuid>(), Another.Value<string>())
			],
		],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<IEnumerable<KeyValuePair<Guid, string>>>(
				[
					KeyValuePair.Create<Guid, string>(Some.Value<NonEmptyGuid>(),    Some.Value<string>()),
					KeyValuePair.Create<Guid, string>(Another.Value<NonEmptyGuid>(), Some.Value<string>())
				],
				"CollectionOfKeyValuePairs cannot accept value {\"00000000-0000-0000-0000-000000000001\":\"\",\"00000000-0000-0000-0000-000000000002\":\"\"}: CollectionOfKeyValuePairs should not have duplicate values, but instead there are 2 keys associated with the value \"\" : [\"00000000-0000-0000-0000-000000000001\",\"00000000-0000-0000-0000-000000000002\"]"),
		]);

		public static EncapsulationOfADictionaryWithNonEmptyGuidKeysAndDistinctStringValues ApplyConstraintsTo(IEnumerable<KeyValuePair<Guid, string>> collectionOfKeyValuePairs)
		{
			try
			{
				return new(collectionOfKeyValuePairs.ToDictionary(
					kvp => NonEmptyGuid.ApplyConstraintsTo(kvp.Key),
					kvp => kvp.Value));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuids>(collectionOfKeyValuePairs);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuids>(developerMistake, collectionOfKeyValuePairs);
			}
		}
	}

	public sealed class EncapsulationOfADictionaryWithGuidKeysAndDistinctStringValues: ConstrainedCollectionOfKeyValuePairs<Dictionary<Guid, string>>, IConstrainedCollectionOfKeyValuePairs<Guid, string, EncapsulationOfADictionaryWithGuidKeysAndDistinctStringValues>
	{
		EncapsulationOfADictionaryWithGuidKeysAndDistinctStringValues(Dictionary<Guid, string> collectionOfKeyValuePairs) : base(collectionOfKeyValuePairs)
		{
			if (collectionOfKeyValuePairs.Values.Distinct().Count() != collectionOfKeyValuePairs.Count)
			{
				throw ObjectConstructionException.WhenConstructingAMemberFor<EncapsulationOfADictionaryWithGuidKeysAndDistinctStringValues>(nameof(CollectionOfKeyValuePairs), CollectionOfKeyValuePairs, $"@member should not have duplicate values, but instead there are {string.Join(',', collectionOfKeyValuePairs.GroupBy(kvp => kvp.Value).Where(g => g.Count() > 1).OrderByDescending(g => g.Count()).Select(g => $"{g.Count()} keys associated with the value {g.First().Value.GetStringRepresentation()} : {g.Select(kvp => kvp.Key).GetStringRepresentation()}"))}");
			}
		}

		public static ExampleValues<IEnumerable<KeyValuePair<Guid, string>>> Examples
		=> ExampleValues.ValidAndInvalid(
		validValues:
		[
			[],
			[
				KeyValuePair.Create(Some.Value<Guid>(), Some.Value<string>())
			],
			[
				KeyValuePair.Create(Some.Value<Guid>(), Some.Value<string>()),
				KeyValuePair.Create(Another.Value<Guid>(), Another.Value<string>())
			],
		],
		constraintViolationExamples:
		[
			ConstraintViolationExample.Document<IEnumerable<KeyValuePair<Guid, string>>>(
				[
					KeyValuePair.Create(Some.Value<Guid>(),    Some.Value<string>()),
					KeyValuePair.Create(Another.Value<Guid>(), Some.Value<string>())
				],
				"CollectionOfKeyValuePairs cannot accept value {\"00000000-0000-0000-0000-000000000000\":\"\",\"00000000-0000-0000-0000-000000000001\":\"\"}: CollectionOfKeyValuePairs should not have duplicate values, but instead there are 2 keys associated with the value \"\" : [\"00000000-0000-0000-0000-000000000000\",\"00000000-0000-0000-0000-000000000001\"]"),
		]);

		public static EncapsulationOfADictionaryWithGuidKeysAndDistinctStringValues ApplyConstraintsTo(IEnumerable<KeyValuePair<Guid, string>> collectionOfKeyValuePairs)
		{
			try
			{
				return new(collectionOfKeyValuePairs.ToDictionary(
					kvp => kvp.Key,
					kvp => kvp.Value));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<EncapsulationOfADictionaryWithGuidKeysAndDistinctStringValues>(collectionOfKeyValuePairs);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<EncapsulationOfADictionaryWithGuidKeysAndDistinctStringValues>(developerMistake, collectionOfKeyValuePairs);
			}
		}
	}
}
