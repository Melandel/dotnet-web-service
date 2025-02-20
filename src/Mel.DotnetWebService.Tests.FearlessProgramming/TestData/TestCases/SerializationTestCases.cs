using System.Collections;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes.DotnetPrimitiveTypes;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class SerializationTestCases
{
	public record TestCase(string StringRepresentation, Type DeserializationType);
	public class That_Do_Not_Involve_Constrained_Types : IEnumerable
	{
		public IEnumerator GetEnumerator()
		{
			foreach (var testCase in GenerateDataStructuresInvolving(new[]
			{
				string.Empty,
				"a", "b", "A", "B",
				"foo", "bar", "FOO", "BAR", "FoO", "bAR",
				"1", "2", "12",
				"<", "&"
			}, DataStructureScope.SupportedByDefaultJsonConverter))
			yield return new object[] { testCase.StringRepresentation, testCase.DeserializationType };

			foreach (var testCase in GenerateDataStructuresInvolving(new[]
			{
				0,
				int.MinValue, int.MaxValue,
				3, 13,
				-2, -10
			}, DataStructureScope.SupportedByDefaultJsonConverter))
			yield return new object[] { testCase.StringRepresentation, testCase.DeserializationType };

			foreach (var testCase in GenerateDataStructuresInvolving(new[]
			{
				new DateTime(),
				DateTime.MinValue, DateTime.MaxValue,
				DateTime.Now
			}, DataStructureScope.SupportedByDefaultJsonConverter))
			yield return new object[] { testCase.StringRepresentation, testCase.DeserializationType };

			foreach (var testCase in GenerateDataStructuresInvolving(new[]
			{
				new DateTimeOffset(),
				DateTimeOffset.MinValue, DateTimeOffset.MaxValue,
				new DateTimeOffset(DateTime.Now)
			}, DataStructureScope.SupportedByDefaultJsonConverter))
			yield return new object[] { testCase.StringRepresentation, testCase.DeserializationType };

			foreach (var testCase in GenerateDataStructuresInvolving(new[]
			{
				Guid.Empty,
				Guid.Parse("6a27094d-8386-4dca-87af-fbe85eaba2eb")
			}, DataStructureScope.SupportedByDefaultJsonConverter))
			yield return new object[] { testCase.StringRepresentation, testCase.DeserializationType };
		}
	}

	public static IEnumerable<object[]> NativeSimpleTypesCollections
	{
		get
		{
			yield return new object[] { Array.Empty<object>(),   "[]" };
			yield return new object[] { new[] { 1, 2, 3 },       "[1,2,3]" };
			yield return new object[] { new[] { "1", "2", "3" }, "[\"1\",\"2\",\"3\"]" };
			yield return new object[] {
				new[] { new DateTime(2025, 01, 02), new DateTime(2025, 02, 03) },
				"[\"2025-01-02T00:00:00\",\"2025-02-03T00:00:00\"]" };
		}
	}

	public static IEnumerable<object[]> NativeSimpleTypesDictionaries
	{
		get
		{
			yield return new object[] { new Dictionary<string, int>(),   "{}" };
			yield return new object[] { new Dictionary<string, int>() { { "1", 1 }, { "2", 2 }, { "3", 3 } },       "{\"1\":1,\"2\":2,\"3\":3}" };

			yield return new object[] { new Dictionary<int, string>(),   "{}" };
			yield return new object[] { new Dictionary<int, string>() { { 1, "1" }, { 2, "2" }, { 3, "3" } },       "{\"1\":\"1\",\"2\":\"2\",\"3\":\"3\"}" };

			yield return new object[] {
				new Dictionary<DateTime, int> { { new DateTime(2025, 01, 02), 1 }, { new DateTime(2025, 02, 03), 2 } },
				"{\"2025-01-02T00:00:00\":1,\"2025-02-03T00:00:00\":2}" };

			yield return new object[] {
				new Dictionary<int, DateTime> { { 1, new DateTime(2025, 01, 02) }, { 2, new DateTime(2025, 02, 03) } },
				"{\"1\":\"2025-01-02T00:00:00\",\"2\":\"2025-02-03T00:00:00\"}" };
		}
	}

	public static IEnumerable<object[]> SimpleValueObjects
	{
		get
		{
			yield return new object[] { NonEmptyGuid.Constraining("09f936b7-7375-4a5a-9cad-53740dd17e57"), "\"09f936b7-7375-4a5a-9cad-53740dd17e57\"" };
		}
	}

	//public static IEnumerable<object[]> SimpleValueObjectsOnTwoLayersOfInheritedConstraints
	//{
	//	get
	//	{
	//		yield return new object[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter1.Constraining("19f936b7-7375-4a5a-9cad-53740dd17e57"), "\"19f936b7-7375-4a5a-9cad-53740dd17e57\"" };
	//	}
	//}

	public static IEnumerable<object[]> SimpleValueObjectsCollections
	{
		get
		{
			yield return new object[] {
				new[]
				{
					NonEmptyGuid.Constraining("45d9d454-87fa-4d87-ace3-3ee7789216b1"),
					NonEmptyGuid.Constraining("5640b0a7-adc1-490d-a9b9-792d54ff2a18")
				},
				"[\"45d9d454-87fa-4d87-ace3-3ee7789216b1\",\"5640b0a7-adc1-490d-a9b9-792d54ff2a18\"]" };
		}
	}

	public static IEnumerable<object[]> SimpleValueObjectsDictionaries
	{
		get
		{
			yield return new object[] {
				new Dictionary<int, NonEmptyGuid>
				{
					{ 1, NonEmptyGuid.Constraining("45d9d454-87fa-4d87-ace3-3ee7789216b1") },
					{ 2, NonEmptyGuid.Constraining("5640b0a7-adc1-490d-a9b9-792d54ff2a18") }
				},
				"{\"1\":\"45d9d454-87fa-4d87-ace3-3ee7789216b1\",\"2\":\"5640b0a7-adc1-490d-a9b9-792d54ff2a18\"}" };

			yield return new object[] {
				new Dictionary<NonEmptyGuid, int>
				{
					{ NonEmptyGuid.Constraining("45d9d454-87fa-4d87-ace3-3ee7789216b1"), 1 },
					{ NonEmptyGuid.Constraining("5640b0a7-adc1-490d-a9b9-792d54ff2a18"), 2 }
				},
				"{\"45d9d454-87fa-4d87-ace3-3ee7789216b1\":1,\"5640b0a7-adc1-490d-a9b9-792d54ff2a18\":2}" };
		}
	}

	public static IEnumerable<object[]> ObjectsFeaturingSimpleValueObjectsAsProperties
	{
		get
		{
			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.Constraining("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"}" };
		}
	}

	public static IEnumerable<object[]> ObjectsFeaturingSimpleValueObjectsAsPropertiesCollections
	{
		get
		{
			yield return new object[] {
				new[]
				{
					new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.Constraining("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
					new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.Constraining("22155dc8-8ab2-4668-8450-5327195268b8")),
				},
				"[{\"FirstProperty\":\"foo\",\"SecondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"},{\"FirstProperty\":\"bar\",\"SecondProperty\":\"22155dc8-8ab2-4668-8450-5327195268b8\"}]" };
		}
	}

	public static IEnumerable<object[]> ObjectsFeaturingSimpleValueObjectsAsPropertiesDictionaries
	{
		get
		{
			yield return new object[] {
				new Dictionary<int, ClassArchetype.PositionalRecordContainingANonEmptyGuid>
				{
					{ 1, new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.Constraining("e03205b5-e9ea-4788-b41a-8f2dce13398c")) },
					{ 2, new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.Constraining("22155dc8-8ab2-4668-8450-5327195268b8")) },
				},
				"{\"1\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"},\"2\":{\"FirstProperty\":\"bar\",\"SecondProperty\":\"22155dc8-8ab2-4668-8450-5327195268b8\"}}" };

			yield return new object[] {
				new Dictionary<ClassArchetype.PositionalRecordContainingANonEmptyGuid, int>
				{
					{ new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.Constraining("e03205b5-e9ea-4788-b41a-8f2dce13398c")), 1 },
					{ new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.Constraining("22155dc8-8ab2-4668-8450-5327195268b8")), 2 },
				},
				"{\"{\\\"FirstProperty\\\":\\\"foo\\\",\\\"SecondProperty\\\":\\\"e03205b5-e9ea-4788-b41a-8f2dce13398c\\\"}\":1,\"{\\\"FirstProperty\\\":\\\"bar\\\",\\\"SecondProperty\\\":\\\"22155dc8-8ab2-4668-8450-5327195268b8\\\"}\":2}" };
		}
	}

	//public static IEnumerable<object[]> ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty
	//{
	//	get
	//	{
	//		yield return new object[] {
	//			ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
	//				new[] { "3e8c722a-e7c9-4f47-ac04-37b153f436d4", "b2520961-2504-4bce-84e3-f971436751c5" },
	//				new[]
	//				{
	//					new[] { "08715282-86b4-44db-ba4b-e654117e3ba9", "70dd566e-ee61-43db-bb2d-3e6621694057" },
	//					new[] { "2661c3f7-73d9-42d6-9dd3-7a43f23daf21", "2a336afe-bd50-4bb3-a3fc-db5edb747440" },
	//				}),
	//			string.Join("", new[]
	//			{
	//				"{\"_nonEmptyGuids\":[\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\",\"b2520961-2504-4bce-84e3-f971436751c5\"]",
	//				",\"_nonEmptyGuidss\":[[\"08715282-86b4-44db-ba4b-e654117e3ba9\",\"70dd566e-ee61-43db-bb2d-3e6621694057\"],[\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\",\"2a336afe-bd50-4bb3-a3fc-db5edb747440\"]]}"
	//			}) };
	//	}
	//}

	//public static IEnumerable<object[]> ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyCollections
	//{
	//	get
	//	{
	//		yield return new object[] {
	//			new[]
	//			{
	//				ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
	//				new[] { "3e8c722a-e7c9-4f47-ac04-37b153f436d4", "b2520961-2504-4bce-84e3-f971436751c5" },
	//				new[]
	//				{
	//					new[] { "08715282-86b4-44db-ba4b-e654117e3ba9", "70dd566e-ee61-43db-bb2d-3e6621694057" },
	//					new[] { "2661c3f7-73d9-42d6-9dd3-7a43f23daf21", "2a336afe-bd50-4bb3-a3fc-db5edb747440" },
	//				}),
	//				ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
	//				new[] { "38a3078a-529c-482c-86e2-c55c24912c43", "636091f6-0e7b-48a8-bdf8-48cafa69eb97" },
	//				new[]
	//				{
	//					new[] { "056c8a63-a80e-4fe4-a548-3eb766fd6f1e", "b7b20f44-c064-48f3-899e-5d2d4061b2e8" },
	//					new[] { "c13b9d61-31fd-407a-9563-1c010c4af718", "728109ee-c0eb-4f76-95a6-c1ad6ece1c14" },
	//				})
	//			},
	//			string.Join("", new[]
	//			{
	//			"[",
	//				"{",
	//					"\"_nonEmptyGuids\":[\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\",\"b2520961-2504-4bce-84e3-f971436751c5\"],",
	//					"\"_nonEmptyGuidss\":[[\"08715282-86b4-44db-ba4b-e654117e3ba9\",\"70dd566e-ee61-43db-bb2d-3e6621694057\"],[\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\",\"2a336afe-bd50-4bb3-a3fc-db5edb747440\"]]",
	//				"},",
	//				"{",
	//					"\"_nonEmptyGuids\":[\"38a3078a-529c-482c-86e2-c55c24912c43\",\"636091f6-0e7b-48a8-bdf8-48cafa69eb97\"],",
	//					"\"_nonEmptyGuidss\":[[\"056c8a63-a80e-4fe4-a548-3eb766fd6f1e\",\"b7b20f44-c064-48f3-899e-5d2d4061b2e8\"],[\"c13b9d61-31fd-407a-9563-1c010c4af718\",\"728109ee-c0eb-4f76-95a6-c1ad6ece1c14\"]]",
	//				"}",
	//			"]"
	//			}) };
	//	}
	//}

	//public static IEnumerable<object[]> ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyDictionaries
	//{
	//	get
	//	{
	//		yield return new object[] {
	//			new Dictionary<int, ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects>
	//			{
	//				{
	//					1,
	//					ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
	//					new[] { "3e8c722a-e7c9-4f47-ac04-37b153f436d4", "b2520961-2504-4bce-84e3-f971436751c5" },
	//					new[]
	//					{
	//						new[] { "08715282-86b4-44db-ba4b-e654117e3ba9", "70dd566e-ee61-43db-bb2d-3e6621694057" },
	//						new[] { "2661c3f7-73d9-42d6-9dd3-7a43f23daf21", "2a336afe-bd50-4bb3-a3fc-db5edb747440" },
	//					})
	//				},
	//				{
	//					2,
	//					ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
	//					new[] { "38a3078a-529c-482c-86e2-c55c24912c43", "636091f6-0e7b-48a8-bdf8-48cafa69eb97" },
	//					new[]
	//					{
	//						new[] { "056c8a63-a80e-4fe4-a548-3eb766fd6f1e", "b7b20f44-c064-48f3-899e-5d2d4061b2e8" },
	//						new[] { "c13b9d61-31fd-407a-9563-1c010c4af718", "728109ee-c0eb-4f76-95a6-c1ad6ece1c14" },
	//					})
	//				}
	//			},
	//			string.Join("", new[]
	//			{
	//			"{",
	//					"\"1\":",
	//				"{",
	//					"\"_nonEmptyGuids\":[\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\",\"b2520961-2504-4bce-84e3-f971436751c5\"],",
	//					"\"_nonEmptyGuidss\":[[\"08715282-86b4-44db-ba4b-e654117e3ba9\",\"70dd566e-ee61-43db-bb2d-3e6621694057\"],[\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\",\"2a336afe-bd50-4bb3-a3fc-db5edb747440\"]]",
	//				"},",
	//				"\"2\":",
	//				"{",
	//					"\"_nonEmptyGuids\":[\"38a3078a-529c-482c-86e2-c55c24912c43\",\"636091f6-0e7b-48a8-bdf8-48cafa69eb97\"],",
	//					"\"_nonEmptyGuidss\":[[\"056c8a63-a80e-4fe4-a548-3eb766fd6f1e\",\"b7b20f44-c064-48f3-899e-5d2d4061b2e8\"],[\"c13b9d61-31fd-407a-9563-1c010c4af718\",\"728109ee-c0eb-4f76-95a6-c1ad6ece1c14\"]]",
	//				"}",
	//			"}"
	//			}) };
	//		yield return new object[] {
	//			new Dictionary<ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects, int>
	//			{
	//				{
	//					ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
	//					new[] { "3e8c722a-e7c9-4f47-ac04-37b153f436d4", "b2520961-2504-4bce-84e3-f971436751c5" },
	//					new[]
	//					{
	//						new[] { "08715282-86b4-44db-ba4b-e654117e3ba9", "70dd566e-ee61-43db-bb2d-3e6621694057" },
	//						new[] { "2661c3f7-73d9-42d6-9dd3-7a43f23daf21", "2a336afe-bd50-4bb3-a3fc-db5edb747440" },
	//					}),
	//					1
	//				},
	//				{
	//					ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
	//					new[] { "38a3078a-529c-482c-86e2-c55c24912c43", "636091f6-0e7b-48a8-bdf8-48cafa69eb97" },
	//					new[]
	//					{
	//						new[] { "056c8a63-a80e-4fe4-a548-3eb766fd6f1e", "b7b20f44-c064-48f3-899e-5d2d4061b2e8" },
	//						new[] { "c13b9d61-31fd-407a-9563-1c010c4af718", "728109ee-c0eb-4f76-95a6-c1ad6ece1c14" },
	//					}),
	//					2
	//				}
	//			},
	//			string.Join("", new[]
	//			{
	//			"{",
	//				"\"{",
	//					"\\\"_nonEmptyGuids\\\":[\\\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\\\",\\\"b2520961-2504-4bce-84e3-f971436751c5\\\"],",
	//					"\\\"_nonEmptyGuidss\\\":[[\\\"08715282-86b4-44db-ba4b-e654117e3ba9\\\",\\\"70dd566e-ee61-43db-bb2d-3e6621694057\\\"],[\\\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\\\",\\\"2a336afe-bd50-4bb3-a3fc-db5edb747440\\\"]]",
	//				"}\":",
	//					"1,",
	//				"\"{",
	//					"\\\"_nonEmptyGuids\\\":[\\\"38a3078a-529c-482c-86e2-c55c24912c43\\\",\\\"636091f6-0e7b-48a8-bdf8-48cafa69eb97\\\"],",
	//					"\\\"_nonEmptyGuidss\\\":[[\\\"056c8a63-a80e-4fe4-a548-3eb766fd6f1e\\\",\\\"b7b20f44-c064-48f3-899e-5d2d4061b2e8\\\"],[\\\"c13b9d61-31fd-407a-9563-1c010c4af718\\\",\\\"728109ee-c0eb-4f76-95a6-c1ad6ece1c14\\\"]]",
	//				"}\":",
	//				"2",
	//			"}"
	//			}) };
	//	}
	//}

	public static IEnumerable<object[]> FirstClassCollectionsWithoutPublicProperties
	{
		get
		{
			yield return new object[] {
				ClassArchetype.NonEmptyGuids.FromStrings(
					"dca969db-2ac9-48e8-b30c-7a5b3b00bfc9",
					"e64f4596-2b61-4bbe-887b-19dbe0dfeeeb"),
				"[\"dca969db-2ac9-48e8-b30c-7a5b3b00bfc9\",\"e64f4596-2b61-4bbe-887b-19dbe0dfeeeb\"]" };
		}
	}

	public static IEnumerable<object[]> FirstClassCollectionsWithoutPublicPropertiesCollections
	{
		get
		{
			yield return new object[] {
				new[]
				{
					ClassArchetype.NonEmptyGuids.FromStrings("af433453-1770-44c6-a2bf-d3933c38780e", "25a16d4c-6489-45b1-aed8-e08784646b54"),
					ClassArchetype.NonEmptyGuids.FromStrings("41302a2f-c586-4fb5-a4f2-89ee6f0a1380", "987ca9f7-4e36-468e-adbc-412bc664b2f9"),
				},
				"[[\"af433453-1770-44c6-a2bf-d3933c38780e\",\"25a16d4c-6489-45b1-aed8-e08784646b54\"],[\"41302a2f-c586-4fb5-a4f2-89ee6f0a1380\",\"987ca9f7-4e36-468e-adbc-412bc664b2f9\"]]" };
		}
	}

	public static IEnumerable<object[]> FirstClassCollectionsWithoutPublicPropertiesDictionaries
	{
		get
		{
			yield return new object[] {
				new Dictionary<int, ClassArchetype.NonEmptyGuids>
				{
					{ 1, ClassArchetype.NonEmptyGuids.FromStrings("af433453-1770-44c6-a2bf-d3933c38780e", "25a16d4c-6489-45b1-aed8-e08784646b54") },
					{ 2, ClassArchetype.NonEmptyGuids.FromStrings("41302a2f-c586-4fb5-a4f2-89ee6f0a1380", "987ca9f7-4e36-468e-adbc-412bc664b2f9") },
				},
				"{\"1\":[\"af433453-1770-44c6-a2bf-d3933c38780e\",\"25a16d4c-6489-45b1-aed8-e08784646b54\"],\"2\":[\"41302a2f-c586-4fb5-a4f2-89ee6f0a1380\",\"987ca9f7-4e36-468e-adbc-412bc664b2f9\"]}" };

			yield return new object[] {
				new Dictionary<ClassArchetype.NonEmptyGuids, int>
				{
					{ ClassArchetype.NonEmptyGuids.FromStrings("af433453-1770-44c6-a2bf-d3933c38780e", "25a16d4c-6489-45b1-aed8-e08784646b54"), 1 },
					{ ClassArchetype.NonEmptyGuids.FromStrings("41302a2f-c586-4fb5-a4f2-89ee6f0a1380", "987ca9f7-4e36-468e-adbc-412bc664b2f9"), 2 },
				},
				"{\"[\\\"af433453-1770-44c6-a2bf-d3933c38780e\\\",\\\"25a16d4c-6489-45b1-aed8-e08784646b54\\\"]\":1,\"[\\\"41302a2f-c586-4fb5-a4f2-89ee6f0a1380\\\",\\\"987ca9f7-4e36-468e-adbc-412bc664b2f9\\\"]\":2}" };
		}
	}

	public static IEnumerable<object[]> FirstClassCollectionsWithPublicProperties
	{
		get
		{
			yield return new object[] {
				ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings(
					"e6e8cbd3-3dae-4d54-aee2-388f46875bfd",
					"8e1fb903-e4bc-4f83-a512-6e1722e94fb6"),
				"[\"e6e8cbd3-3dae-4d54-aee2-388f46875bfd\",\"8e1fb903-e4bc-4f83-a512-6e1722e94fb6\"]" };
		}
	}

	public static IEnumerable<object[]> FirstClassCollectionsWithPublicPropertiesCollections
	{
		get
		{
			yield return new object[] {
				new[]
				{
					ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings("25dd708c-e9c7-4529-8152-967effc73b7d", "ba5d5bd9-43e2-476d-8fec-e6d6c53be863"),
					ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings("faa014f0-29e9-4b4d-82dc-605869239ac5", "d04427f6-4e59-4d2c-b7db-36a1bdc8bb16")
				},
				"[[\"25dd708c-e9c7-4529-8152-967effc73b7d\",\"ba5d5bd9-43e2-476d-8fec-e6d6c53be863\"],[\"faa014f0-29e9-4b4d-82dc-605869239ac5\",\"d04427f6-4e59-4d2c-b7db-36a1bdc8bb16\"]]" };
		}
	}

	public static IEnumerable<object[]> FirstClassCollectionsWithPublicPropertiesDictionaries
	{
		get
		{
			yield return new object[] {
				new Dictionary<int, ClassArchetype.NonEmptyGuidsWithPublicProperty>
				{
					{ 1, ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings("34081b26-83d3-49ab-a373-e14b3ec44e3a", "34081b26-83d3-49ab-a373-e14b3ec44e3a") },
					{ 2, ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings("e1f932c1-e4e9-4721-9eb7-df599ca2564f", "1aa503ac-dc44-4c93-ac76-6026786d5068") }
				},
				"{\"1\":[\"34081b26-83d3-49ab-a373-e14b3ec44e3a\",\"34081b26-83d3-49ab-a373-e14b3ec44e3a\"],\"2\":[\"e1f932c1-e4e9-4721-9eb7-df599ca2564f\",\"1aa503ac-dc44-4c93-ac76-6026786d5068\"]}" };

			yield return new object[] {
				new Dictionary<ClassArchetype.NonEmptyGuidsWithPublicProperty, int>
				{
					{ ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings("a63b0f64-6a87-4c50-a850-248935903ea8", "7ba0764e-60c9-4f78-99e5-ba47337bb3d0"), 1 },
					{ ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings("9af12f7f-0663-4b11-b772-e495186521ca", "cb64bfb5-0c80-4a3e-acb8-53fc365f9a7f"), 2 }
				},
				"{\"[\\\"a63b0f64-6a87-4c50-a850-248935903ea8\\\",\\\"7ba0764e-60c9-4f78-99e5-ba47337bb3d0\\\"]\":1,\"[\\\"9af12f7f-0663-4b11-b772-e495186521ca\\\",\\\"cb64bfb5-0c80-4a3e-acb8-53fc365f9a7f\\\"]\":2}" };
		}
	}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingANonEmptyGuid = new ClassArchetype.PositionalRecordContainingANonEmptyGuid(
					StringArchetype.Foo,
					NonEmptyGuid.Constraining("e03205b5-e9ea-4788-b41a-8f2dce13398c"));

			yield return new object[] {
				positionalRecordDirectlyContainingANonEmptyGuid,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"}" };

			var positionalRecordContainingARecordContainingANonEmptyGuid = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
				StringArchetype.Bar,
				positionalRecordDirectlyContainingANonEmptyGuid);
			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
					StringArchetype.Bar,
					positionalRecordDirectlyContainingANonEmptyGuid),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingANonEmptyGuid\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"}}" };

			var positionalRecordContainingARecordContainingARecordContainingANonEmptyGuid = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
				StringArchetype.Baz,
				positionalRecordContainingARecordContainingANonEmptyGuid);
			yield return new object[] {
				positionalRecordContainingARecordContainingARecordContainingANonEmptyGuid,
				"{\"FirstProperty\":\"baz\",\"SecondProperty\":{\"FirstProperty\":\"bar\",\"SecondPropertyContainingANonEmptyGuid\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"}}}" };
		}
	}

	//public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	//{
	//	get
	//	{
			//var positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(
			//		StringArchetype.Foo,
			//		ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("203205b5-e9ea-4788-b41a-8f2dce13398c"));
			//yield return new object[] {
			//	positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2,
			//	"{\"FirstProperty\":\"foo\",\"SecondProperty\":\"203205b5-e9ea-4788-b41a-8f2dce13398c\"}" };

			//var positionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(
			//	StringArchetype.Bar,
			//	positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2);
			//yield return new object[] {
			//	positionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2,
			//	"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingANonEmptyGuidStartingWithTheCharacter2\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"203205b5-e9ea-4788-b41a-8f2dce13398c\"}}" };

			//var positionalRecordContainingPositionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAPositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(
			//	StringArchetype.Baz,
			//	positionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2);
			//yield return new object[] {
			//	positionalRecordContainingPositionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2,
			//	"{\"FirstProperty\":\"baz\",\"SecondProperty\":{\"FirstProperty\":\"bar\",\"SecondPropertyContainingANonEmptyGuidStartingWithTheCharacter2\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"203205b5-e9ea-4788-b41a-8f2dce13398c\"}}}" };
	//	}
	//}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter3 = new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(
					StringArchetype.Foo,
					ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("303205b5-e9ea-4788-b41a-8f2dce13398c"));
			yield return new object[] {
				positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter3,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":\"303205b5-e9ea-4788-b41a-8f2dce13398c\"}" };

			var positionalRecordContainingAPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter3 = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(
				StringArchetype.Bar,
				positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter3);
			yield return new object[] {
				positionalRecordContainingAPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter3,
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingANonEmptyGuidStartingWithTheCharacter3\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"303205b5-e9ea-4788-b41a-8f2dce13398c\"}}" };

			var positionalRecordContainingAPositionalRecordContainingAPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter3 = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAPositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(
				StringArchetype.Baz,
				positionalRecordContainingAPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter3);
			yield return new object[] {
				positionalRecordContainingAPositionalRecordContainingAPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter3,
				"{\"FirstProperty\":\"baz\",\"SecondProperty\":{\"FirstProperty\":\"bar\",\"SecondPropertyContainingANonEmptyGuidStartingWithTheCharacter3\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"303205b5-e9ea-4788-b41a-8f2dce13398c\"}}}" };
		}
	}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(
					StringArchetype.Foo,
					new[]
					{
						NonEmptyGuid.Constraining("57cba1ed-add2-4d30-9ac3-2e941af888cd"),
						NonEmptyGuid.Constraining("f20f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"57cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f20f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar,	positionalRecordDirectlyContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingAnArrayOfNonEmptyGuids\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"57cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f20f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };



			var positionalRecordDirectlyContainingAListOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(
					StringArchetype.Baz,
					new List<NonEmptyGuid>
					{
						NonEmptyGuid.Constraining("67cba1ed-add2-4d30-9ac3-2e941af888cd"),
						NonEmptyGuid.Constraining("f30f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAListOfNonEmptyGuids,
				"{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"67cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f30f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Foobar, positionalRecordDirectlyContainingAListOfNonEmptyGuids),
				"{\"FirstProperty\":\"foobar\",\"SecondPropertyContainingAListOfNonEmptyGuids\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"67cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f30f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };



			var positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(
					StringArchetype.Qux,
					new HashSet<NonEmptyGuid>
					{
						NonEmptyGuid.Constraining("77cba1ed-add2-4d30-9ac3-2e941af888cd"),
						NonEmptyGuid.Constraining("f40f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"77cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f40f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Quux, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids),
				"{\"FirstProperty\":\"quux\",\"SecondPropertyContainingAnIEnumerableOfNonEmptyGuids\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"77cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f40f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };
		}
	}
	//public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	//{
	//	get
	//	{
			//var positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(
			//		StringArchetype.Foo,
			//		new[]
			//		{
			//			ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("27cba1ed-add2-4d30-9ac3-2e941af888cd"),
			//			ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("220f1385-2679-49b1-8e64-94183d4a6f13"),
			//		});
			//yield return new object[] {
			//	positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2,
			//	"{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"27cba1ed-add2-4d30-9ac3-2e941af888cd\",\"220f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			//yield return new object[] {
			//	new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Bar, positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2),
			//	"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"27cba1ed-add2-4d30-9ac3-2e941af888cd\",\"220f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };



			//var positionalRecordDirectlyContainingAListOfNonEmptyGuidsStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(
			//		StringArchetype.Baz,
			//		new List<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>
			//		{
			//			ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("28cba1ed-add2-4d30-9ac3-2e941af888cd"),
			//			ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("240f1385-2679-49b1-8e64-94183d4a6f13"),
			//		});
			//yield return new object[] {
			//	positionalRecordDirectlyContainingAListOfNonEmptyGuidsStartingWithTheCharacter2,
			//	"{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"28cba1ed-add2-4d30-9ac3-2e941af888cd\",\"240f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			//yield return new object[] {
			//	new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Foobar, positionalRecordDirectlyContainingAListOfNonEmptyGuidsStartingWithTheCharacter2),
			//	"{\"FirstProperty\":\"foobar\",\"SecondPropertyContainingAListOfNonEmptyGuidsStartingWithTheCharacter2\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"28cba1ed-add2-4d30-9ac3-2e941af888cd\",\"240f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };



			//var positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(
			//		StringArchetype.Qux,
			//		new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>
			//		{
			//			ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("28dba1ed-add2-4d30-9ac3-2e941af888cd"),
			//			ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("250f1385-2679-49b1-8e64-94183d4a6f13"),
			//		});
			//yield return new object[] {
			//	positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2,
			//	"{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"28dba1ed-add2-4d30-9ac3-2e941af888cd\",\"250f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			//yield return new object[] {
			//	new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Quux, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2),
			//	"{\"FirstProperty\":\"quux\",\"SecondPropertyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"28dba1ed-add2-4d30-9ac3-2e941af888cd\",\"250f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };
	//	}
	//}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass2
	{
		get
		{
			var positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3 = new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(
					StringArchetype.Foo,
					new[]
					{
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("37cba1ed-add2-4d30-9ac3-2e941af888cd"),
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("320f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"37cba1ed-add2-4d30-9ac3-2e941af888cd\",\"320f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Bar, positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"37cba1ed-add2-4d30-9ac3-2e941af888cd\",\"320f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };



			var positionalRecordDirectlyContainingAListOfNonEmptyGuidsStartingWithTheCharacter3 = new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(
					StringArchetype.Baz,
					new List<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>
					{
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("38cba1ed-add2-4d30-9ac3-2e941af888cd"),
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("340f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAListOfNonEmptyGuidsStartingWithTheCharacter3,
				"{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"38cba1ed-add2-4d30-9ac3-2e941af888cd\",\"340f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Foobar, positionalRecordDirectlyContainingAListOfNonEmptyGuidsStartingWithTheCharacter3),
				"{\"FirstProperty\":\"foobar\",\"SecondPropertyContainingAListOfNonEmptyGuidsStartingWithTheCharacter3\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"38cba1ed-add2-4d30-9ac3-2e941af888cd\",\"340f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };



			var positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3 = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(
					StringArchetype.Qux,
					new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>
					{
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("38dba1ed-add2-4d30-9ac3-2e941af888cd"),
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("350f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"38dba1ed-add2-4d30-9ac3-2e941af888cd\",\"350f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Quux, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3),
				"{\"FirstProperty\":\"quux\",\"SecondPropertyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"38dba1ed-add2-4d30-9ac3-2e941af888cd\",\"350f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };
		}
	}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidValues = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidValues(
					StringArchetype.Foo,
					new Dictionary<string, NonEmptyGuid>
					{
						{  StringArchetype.Foo, NonEmptyGuid.Constraining("7b3c6a10-b042-4d54-bdc9-e418082d7677") },
						{  StringArchetype.Bar, NonEmptyGuid.Constraining("7c3c6a10-b042-4d54-bdc9-e418082d7677") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"7b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"7c3c6a10-b042-4d54-bdc9-e418082d7677\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidValues(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidValues),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidValues\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"7b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"7c3c6a10-b042-4d54-bdc9-e418082d7677\"}}}" };

			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidKeys(
					StringArchetype.Foobar,
					new Dictionary<NonEmptyGuid, string>
					{
						{  NonEmptyGuid.Constraining("36dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Baz },
						{  NonEmptyGuid.Constraining("46dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Foobar },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"36dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"46dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidKeys(StringArchetype.Baz, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeys),
				"{\"FirstProperty\":\"baz\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidKeys\":{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"36dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"46dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}}" };

			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeysAndValues = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidKeysAndValues(
					StringArchetype.Foo,
					new Dictionary<NonEmptyGuid, NonEmptyGuid>
					{
						{  NonEmptyGuid.Constraining("d0b01da5-8ec1-4d20-8145-8be0ce451a4e"), NonEmptyGuid.Constraining("e0b01da5-8ec1-4d20-8145-8be0ce451a4e") },
						{  NonEmptyGuid.Constraining("d1b01da5-8ec1-4d20-8145-8be0ce451a4e"), NonEmptyGuid.Constraining("e1b01da5-8ec1-4d20-8145-8be0ce451a4e") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeysAndValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"d0b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e0b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"d1b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e1b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidKeysAndValues(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeysAndValues),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidKeysAndValues\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"d0b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e0b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"d1b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e1b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}}" };
		}
	}
	//public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	//{
	//	get
	//	{
	//		var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values(
	//				StringArchetype.Foo,
	//				new Dictionary<string, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>
	//				{
	//					{  StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2b3c6a10-b042-4d54-bdc9-e418082d7677") },
	//					{  StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2c3c6a10-b042-4d54-bdc9-e418082d7677") },
	//				});
	//		yield return new object[] {
	//			positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values,
	//			"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"2b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"2c3c6a10-b042-4d54-bdc9-e418082d7677\"}}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values),
	//			"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"2b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"2c3c6a10-b042-4d54-bdc9-e418082d7677\"}}}" };

	//		var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys(
	//				StringArchetype.Foobar,
	//				new Dictionary<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2, string>
	//				{
	//					{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("26dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Baz },
	//					{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("27dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Foobar },
	//				});
	//		yield return new object[] {
	//			positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys,
	//			"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"26dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"27dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys(StringArchetype.Baz, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys),
	//			"{\"FirstProperty\":\"baz\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys\":{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"26dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"27dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}}" };

	//		var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues(
	//				StringArchetype.Foo,
	//				new Dictionary<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>
	//				{
	//					{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("20b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("21b01da5-8ec1-4d20-8145-8be0ce451a4e") },
	//					{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("21b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("22b01da5-8ec1-4d20-8145-8be0ce451a4e") },
	//				});
	//		yield return new object[] {
	//			positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues,
	//			"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"20b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"21b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"21b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"22b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues),
	//			"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"20b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"21b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"21b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"22b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}}" };
	//	}
	//}
	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values(
					StringArchetype.Foo,
					new Dictionary<string, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>
					{
						{  StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3b3c6a10-b042-4d54-bdc9-e418082d7677") },
						{  StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3c3c6a10-b042-4d54-bdc9-e418082d7677") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"3b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"3c3c6a10-b042-4d54-bdc9-e418082d7677\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"3b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"3c3c6a10-b042-4d54-bdc9-e418082d7677\"}}}" };

			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys(
					StringArchetype.Foobar,
					new Dictionary<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3, string>
					{
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("36dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Baz },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("37dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Foobar },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys,
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"36dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"37dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys(StringArchetype.Baz, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys),
				"{\"FirstProperty\":\"baz\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys\":{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"36dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"37dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}}" };

			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues(
					StringArchetype.Foo,
					new Dictionary<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>
					{
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("30b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("31b01da5-8ec1-4d20-8145-8be0ce451a4e") },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("31b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("32b01da5-8ec1-4d20-8145-8be0ce451a4e") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"30b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"31b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"31b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"32b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"30b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"31b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"31b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"32b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}}" };
		}
	}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values(
					StringArchetype.Foo,
					new Dictionary<string, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>
					{
						{  StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3b3c6a10-b042-4d54-bdc9-e418082d7677") },
						{  StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3c3c6a10-b042-4d54-bdc9-e418082d7677") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"3b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"3c3c6a10-b042-4d54-bdc9-e418082d7677\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"3b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"3c3c6a10-b042-4d54-bdc9-e418082d7677\"}}}" };

			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys(
					StringArchetype.Foobar,
					new Dictionary<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3, string>
					{
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("36dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Baz },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("37dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Foobar },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys,
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"36dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"37dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys(StringArchetype.Baz, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys),
				"{\"FirstProperty\":\"baz\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Keys\":{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"36dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"37dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}}" };

			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues(
					StringArchetype.Foo,
					new Dictionary<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>
					{
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("30b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("31b01da5-8ec1-4d20-8145-8be0ce451a4e") },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("31b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("32b01da5-8ec1-4d20-8145-8be0ce451a4e") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"30b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"31b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"31b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"32b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3KeysAndValues\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"30b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"31b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"31b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"32b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}}" };
		}
	}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass
	{
		get
		{
			var positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuid(
					StringArchetype.Foo,
					new[]
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.Constraining("15199727-a673-4f67-a221-91500ded8618")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Baz, NonEmptyGuid.Constraining("3caa6d26-fa7b-4892-b6b0-b254d2453780"))
					});
			yield return new object[] {
				positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":\"15199727-a673-4f67-a221-91500ded8618\"},{\"FirstProperty\":\"baz\",\"SecondProperty\":\"3caa6d26-fa7b-4892-b6b0-b254d2453780\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuid(StringArchetype.Foobar, positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":\"15199727-a673-4f67-a221-91500ded8618\"},{\"FirstProperty\":\"baz\",\"SecondProperty\":\"3caa6d26-fa7b-4892-b6b0-b254d2453780\"}]}}" };



			var positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuid(
					StringArchetype.Bar,
					new List<ClassArchetype.PositionalRecordContainingANonEmptyGuid>
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Baz, NonEmptyGuid.Constraining("bfcc8a2c-f8af-4441-ba59-bdd57592aeb7")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foobar, NonEmptyGuid.Constraining("17d76e6b-227b-4a97-aa02-78c0ddb4e69c"))
					});
			yield return new object[] {
				positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuids,
				"{\"FirstProperty\":\"bar\",\"SecondProperty\":[{\"FirstProperty\":\"baz\",\"SecondProperty\":\"bfcc8a2c-f8af-4441-ba59-bdd57592aeb7\"},{\"FirstProperty\":\"foobar\",\"SecondProperty\":\"17d76e6b-227b-4a97-aa02-78c0ddb4e69c\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuid(StringArchetype.Qux, positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuids),
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":{\"FirstProperty\":\"bar\",\"SecondProperty\":[{\"FirstProperty\":\"baz\",\"SecondProperty\":\"bfcc8a2c-f8af-4441-ba59-bdd57592aeb7\"},{\"FirstProperty\":\"foobar\",\"SecondProperty\":\"17d76e6b-227b-4a97-aa02-78c0ddb4e69c\"}]}}" };


			var positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuid(
					StringArchetype.Qux,
					new HashSet<ClassArchetype.PositionalRecordContainingANonEmptyGuid>
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Quux, NonEmptyGuid.Constraining("cbccbf7d-6650-4524-b945-e3a84c0a20f8")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.Constraining("731ae06d-0f46-4250-b528-31ca0a3357da")),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"cbccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"731ae06d-0f46-4250-b528-31ca0a3357da\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuid(StringArchetype.Bar, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids),
				"{\"FirstProperty\":\"bar\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"cbccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"731ae06d-0f46-4250-b528-31ca0a3357da\"}]}}" };
		}
	}
	//public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	//{
	//	get
	//	{
	//		var positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2s = new ClassArchetype.PositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(
	//				StringArchetype.Foo,
	//				new[]
	//				{
	//					new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("25199727-a673-4f67-a221-91500ded8618")),
	//					new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Baz, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2caa6d26-fa7b-4892-b6b0-b254d2453780"))
	//				});
	//		yield return new object[] {
	//			positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2s,
	//			"{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":\"25199727-a673-4f67-a221-91500ded8618\"},{\"FirstProperty\":\"baz\",\"SecondProperty\":\"2caa6d26-fa7b-4892-b6b0-b254d2453780\"}]}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Foobar, positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2s),
	//			"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":\"25199727-a673-4f67-a221-91500ded8618\"},{\"FirstProperty\":\"baz\",\"SecondProperty\":\"2caa6d26-fa7b-4892-b6b0-b254d2453780\"}]}}" };



	//		var positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuidStartingWithTheCharacter2s = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(
	//				StringArchetype.Bar,
	//				new List<ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2>
	//				{
	//					new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Baz, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2fcc8a2c-f8af-4441-ba59-bdd57592aeb7")),
	//					new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Foobar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("27d76e6b-227b-4a97-aa02-78c0ddb4e69c"))
	//				});
	//		yield return new object[] {
	//			positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuidStartingWithTheCharacter2s,
	//			"{\"FirstProperty\":\"bar\",\"SecondProperty\":[{\"FirstProperty\":\"baz\",\"SecondProperty\":\"2fcc8a2c-f8af-4441-ba59-bdd57592aeb7\"},{\"FirstProperty\":\"foobar\",\"SecondProperty\":\"27d76e6b-227b-4a97-aa02-78c0ddb4e69c\"}]}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Qux, positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuidStartingWithTheCharacter2s),
	//			"{\"FirstProperty\":\"qux\",\"SecondProperty\":{\"FirstProperty\":\"bar\",\"SecondProperty\":[{\"FirstProperty\":\"baz\",\"SecondProperty\":\"2fcc8a2c-f8af-4441-ba59-bdd57592aeb7\"},{\"FirstProperty\":\"foobar\",\"SecondProperty\":\"27d76e6b-227b-4a97-aa02-78c0ddb4e69c\"}]}}" };


	//		var positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2s = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(
	//				StringArchetype.Qux,
	//				new HashSet<ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2>
	//				{
	//					new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Quux, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2bccbf7d-6650-4524-b945-e3a84c0a20f8")),
	//					new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("231ae06d-0f46-4250-b528-31ca0a3357da")),
	//				});
	//		yield return new object[] {
	//			positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2s,
	//			"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"2bccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"231ae06d-0f46-4250-b528-31ca0a3357da\"}]}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Bar, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2s),
	//			"{\"FirstProperty\":\"bar\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"2bccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"231ae06d-0f46-4250-b528-31ca0a3357da\"}]}}" };
	//	}
	//}
	public static IEnumerable<object> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3s = new ClassArchetype.PositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter3(
					StringArchetype.Foo,
					new[]
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("35199727-a673-4f67-a221-91500ded8618")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Baz, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3caa6d26-fa7b-4892-b6b0-b254d2453780"))
					});
			yield return new object[] {
				positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3s,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":\"35199727-a673-4f67-a221-91500ded8618\"},{\"FirstProperty\":\"baz\",\"SecondProperty\":\"3caa6d26-fa7b-4892-b6b0-b254d2453780\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Foobar, positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3s),
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":\"35199727-a673-4f67-a221-91500ded8618\"},{\"FirstProperty\":\"baz\",\"SecondProperty\":\"3caa6d26-fa7b-4892-b6b0-b254d2453780\"}]}}" };



			var positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuidStartingWithTheCharacter3s = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter3(
					StringArchetype.Bar,
					new List<ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3>
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Baz, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3fcc8a2c-f8af-4441-ba59-bdd57592aeb7")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Foobar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("37d76e6b-227b-4a97-aa02-78c0ddb4e69c"))
					});
			yield return new object[] {
				positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuidStartingWithTheCharacter3s,
				"{\"FirstProperty\":\"bar\",\"SecondProperty\":[{\"FirstProperty\":\"baz\",\"SecondProperty\":\"3fcc8a2c-f8af-4441-ba59-bdd57592aeb7\"},{\"FirstProperty\":\"foobar\",\"SecondProperty\":\"37d76e6b-227b-4a97-aa02-78c0ddb4e69c\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Qux, positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuidStartingWithTheCharacter3s),
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":{\"FirstProperty\":\"bar\",\"SecondProperty\":[{\"FirstProperty\":\"baz\",\"SecondProperty\":\"3fcc8a2c-f8af-4441-ba59-bdd57592aeb7\"},{\"FirstProperty\":\"foobar\",\"SecondProperty\":\"37d76e6b-227b-4a97-aa02-78c0ddb4e69c\"}]}}" };


			var positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter3s = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter3(
					StringArchetype.Qux,
					new HashSet<ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3>
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Quux, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3bccbf7d-6650-4524-b945-e3a84c0a20f8")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("331ae06d-0f46-4250-b528-31ca0a3357da")),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter3s,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"3bccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"331ae06d-0f46-4250-b528-31ca0a3357da\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Bar, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter3s),
				"{\"FirstProperty\":\"bar\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"3bccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"331ae06d-0f46-4250-b528-31ca0a3357da\"}]}}" };
		}
	}
	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass
	{
		get
		{
			var positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Foo,
				new[]
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.Constraining("fffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.Constraining("ba75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.Constraining("ff8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.Constraining("336aecda-25b0-45c3-91d5-39af24adc71e") })
				});
			yield return new object[] {
				positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"fffd1abd-2713-4af3-a990-93ef802ec1af\",\"ba75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"ff8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"336aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Foobar, positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"fffd1abd-2713-4af3-a990-93ef802ec1af\",\"ba75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"ff8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"336aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };



			var positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Qux,
				new List<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.Constraining("0ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.Constraining("ca75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.Constraining("0f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.Constraining("436aecda-25b0-45c3-91d5-39af24adc71e") })
				});
			yield return new object[] {
				positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"0ffd1abd-2713-4af3-a990-93ef802ec1af\",\"ca75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"0f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"436aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Quux, positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"0ffd1abd-2713-4af3-a990-93ef802ec1af\",\"ca75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"0f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"436aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };



			var positionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Qux,
				new HashSet<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.Constraining("1ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.Constraining("da75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.Constraining("1f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.Constraining("536aecda-25b0-45c3-91d5-39af24adc71e") })
				});
			yield return new object[] {
				positionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"1ffd1abd-2713-4af3-a990-93ef802ec1af\",\"da75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"1f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"536aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Quux, positionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"1ffd1abd-2713-4af3-a990-93ef802ec1af\",\"da75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"1f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"536aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };
		}
	}
	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			//var positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
			//	StringArchetype.Foo,
			//	new[]
			//	{
			//		new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.Constraining("2a75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
			//		new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.Constraining("236aecda-25b0-45c3-91d5-39af24adc71e") })
			//	});
			//yield return new object[] {
			//	positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
			//	"{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"2ffd1abd-2713-4af3-a990-93ef802ec1af\",\"2a75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"2f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"236aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			//yield return new object[] {
			//	new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Foobar, positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
			//	"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"2ffd1abd-2713-4af3-a990-93ef802ec1af\",\"2a75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"2f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"236aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };



			var positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Qux,
				new List<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.Constraining("0ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.Constraining("ca75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.Constraining("0f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.Constraining("436aecda-25b0-45c3-91d5-39af24adc71e") })
				});
			yield return new object[] {
				positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"0ffd1abd-2713-4af3-a990-93ef802ec1af\",\"ca75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"0f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"436aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Quux, positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"0ffd1abd-2713-4af3-a990-93ef802ec1af\",\"ca75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"0f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"436aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };



			var positionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Qux,
				new HashSet<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.Constraining("1ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.Constraining("da75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.Constraining("1f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.Constraining("536aecda-25b0-45c3-91d5-39af24adc71e") })
				});
			yield return new object[] {
				positionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"1ffd1abd-2713-4af3-a990-93ef802ec1af\",\"da75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"1f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"536aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Quux, positionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"1ffd1abd-2713-4af3-a990-93ef802ec1af\",\"da75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"1f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"536aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };
		}
	}
	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Foo,
				new[]
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.Constraining("3a75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.Constraining("336aecda-25b0-45c3-91d5-39af24adc71e") })
				});
			yield return new object[] {
				positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"3ffd1abd-2713-4af3-a990-93ef802ec1af\",\"3a75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"3f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"336aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Foobar, positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"3ffd1abd-2713-4af3-a990-93ef802ec1af\",\"3a75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"3f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"336aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };



			var positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Qux,
				new List<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.Constraining("0ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.Constraining("ca75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.Constraining("0f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.Constraining("436aecda-25b0-45c3-91d5-39af24adc71e") })
				});
			yield return new object[] {
				positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"0ffd1abd-2713-4af3-a990-93ef802ec1af\",\"ca75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"0f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"436aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Quux, positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"0ffd1abd-2713-4af3-a990-93ef802ec1af\",\"ca75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"0f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"436aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };



			var positionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Qux,
				new HashSet<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.Constraining("1ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.Constraining("da75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.Constraining("1f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.Constraining("536aecda-25b0-45c3-91d5-39af24adc71e") })
				});
			yield return new object[] {
				positionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"1ffd1abd-2713-4af3-a990-93ef802ec1af\",\"da75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"1f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"536aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Quux, positionalRecordContainingAnIEnumerableOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"1ffd1abd-2713-4af3-a990-93ef802ec1af\",\"da75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"1f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"536aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };
		}
	}
	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass
	{
		get
		{
			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidValues = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidValues(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.Constraining("084d2aee-99dd-4494-a702-ec7a098e8a88"), NonEmptyGuid.Constraining("1bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Qux, new[] { NonEmptyGuid.Constraining("e6895f58-2535-4d52-90eb-4c572e80c3c6"), NonEmptyGuid.Constraining("cc739b95-3614-4d3d-898d-a8c15c29370e") }) },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"084d2aee-99dd-4494-a702-ec7a098e8a88\",\"1bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"e6895f58-2535-4d52-90eb-4c572e80c3c6\",\"cc739b95-3614-4d3d-898d-a8c15c29370e\"]}}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidValues(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidValues),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"084d2aee-99dd-4494-a702-ec7a098e8a88\",\"1bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"e6895f58-2535-4d52-90eb-4c572e80c3c6\",\"cc739b95-3614-4d3d-898d-a8c15c29370e\"]}}}}" };

			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidValues = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidValues(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Baz, new() { NonEmptyGuid.Constraining("084d2aee-99dd-4494-a702-ec7a098e8a88"), NonEmptyGuid.Constraining("1bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Qux, new() { NonEmptyGuid.Constraining("e6895f58-2535-4d52-90eb-4c572e80c3c6"), NonEmptyGuid.Constraining("cc739b95-3614-4d3d-898d-a8c15c29370e") }) },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"084d2aee-99dd-4494-a702-ec7a098e8a88\",\"1bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"e6895f58-2535-4d52-90eb-4c572e80c3c6\",\"cc739b95-3614-4d3d-898d-a8c15c29370e\"]}}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidValues(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidValues),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"084d2aee-99dd-4494-a702-ec7a098e8a88\",\"1bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"e6895f58-2535-4d52-90eb-4c572e80c3c6\",\"cc739b95-3614-4d3d-898d-a8c15c29370e\"]}}}}" };


			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Baz, new HashSet<NonEmptyGuid>() { NonEmptyGuid.Constraining("feffc8ae-d14f-4fca-9f9a-c9d001478d3d"), NonEmptyGuid.Constraining("e1046b1f-53a6-4a4e-9be9-cc7718b6ca93") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Qux, new HashSet<NonEmptyGuid>() { NonEmptyGuid.Constraining("dd13b555-9a4e-4c8d-b64b-cfd00a7a897c"), NonEmptyGuid.Constraining("3d1f7bb3-f06e-4181-9f26-f0635e3d3983") }) },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"feffc8ae-d14f-4fca-9f9a-c9d001478d3d\",\"e1046b1f-53a6-4a4e-9be9-cc7718b6ca93\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"dd13b555-9a4e-4c8d-b64b-cfd00a7a897c\",\"3d1f7bb3-f06e-4181-9f26-f0635e3d3983\"]}}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"feffc8ae-d14f-4fca-9f9a-c9d001478d3d\",\"e1046b1f-53a6-4a4e-9be9-cc7718b6ca93\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"dd13b555-9a4e-4c8d-b64b-cfd00a7a897c\",\"3d1f7bb3-f06e-4181-9f26-f0635e3d3983\"]}}}}" };

			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys(
				StringArchetype.Foo,
				new Dictionary<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids, string>
				{
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.Constraining("04b0ae6c-2b98-41f5-b2e8-16a158037157"), NonEmptyGuid.Constraining("3d925b20-9951-4bae-b65c-8a01e8d3e493") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Qux, new[] { NonEmptyGuid.Constraining("65276338-a7fd-4647-8262-df8c4f6de190"), NonEmptyGuid.Constraining("ac963cd5-7b84-417a-87e4-a907ca895642") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"04b0ae6c-2b98-41f5-b2e8-16a158037157\\\",\\\"3d925b20-9951-4bae-b65c-8a01e8d3e493\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"65276338-a7fd-4647-8262-df8c4f6de190\\\",\\\"ac963cd5-7b84-417a-87e4-a907ca895642\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"04b0ae6c-2b98-41f5-b2e8-16a158037157\\\",\\\"3d925b20-9951-4bae-b65c-8a01e8d3e493\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"65276338-a7fd-4647-8262-df8c4f6de190\\\",\\\"ac963cd5-7b84-417a-87e4-a907ca895642\\\"]}\":\"bar\"}}}" };



			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys(
				StringArchetype.Foo,
				new Dictionary<ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids, string>
				{
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Baz, new() { NonEmptyGuid.Constraining("90efae4f-20c4-47cd-93d5-8acd846a937c"), NonEmptyGuid.Constraining("b4c22756-abf5-4dc1-ab43-896d06e01d1a") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Qux, new() { NonEmptyGuid.Constraining("72415979-9256-418d-bead-3103b19dcb6b"), NonEmptyGuid.Constraining("123b2fb8-d90e-4ff5-ab96-704c05ad06bd") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"90efae4f-20c4-47cd-93d5-8acd846a937c\\\",\\\"b4c22756-abf5-4dc1-ab43-896d06e01d1a\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"72415979-9256-418d-bead-3103b19dcb6b\\\",\\\"123b2fb8-d90e-4ff5-ab96-704c05ad06bd\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"90efae4f-20c4-47cd-93d5-8acd846a937c\\\",\\\"b4c22756-abf5-4dc1-ab43-896d06e01d1a\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"72415979-9256-418d-bead-3103b19dcb6b\\\",\\\"123b2fb8-d90e-4ff5-ab96-704c05ad06bd\\\"]}\":\"bar\"}}}" };



			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys(
				StringArchetype.Foo,
				new Dictionary<ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids, string>
				{
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Baz, new HashSet<NonEmptyGuid>() { NonEmptyGuid.Constraining("6fd6010c-98df-454d-be6b-14e21f76610e"), NonEmptyGuid.Constraining("49371516-633f-4564-a5dc-e0da5524c7d2") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Qux, new HashSet<NonEmptyGuid>() { NonEmptyGuid.Constraining("605a273c-8426-4c45-a7bf-de797259ee17"), NonEmptyGuid.Constraining("c10e386f-53c8-4b25-81d0-875b2e1d0cf0") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"6fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"49371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"605a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"c10e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"6fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"49371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"605a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"c10e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}}" };
		}
	}

	//public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	//{
	//	get
	//	{
	//		var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values(
	//			StringArchetype.Foo,
	//			new Dictionary<string, ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2>
	//			{
	//				{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("284d2aee-99dd-4494-a702-ec7a098e8a88"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
	//				{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("26895f58-2535-4d52-90eb-4c572e80c3c6"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2c739b95-3614-4d3d-898d-a8c15c29370e") }) },
	//			});
	//		yield return new object[] {
	//			positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values,
	//			"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"284d2aee-99dd-4494-a702-ec7a098e8a88\",\"2bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"26895f58-2535-4d52-90eb-4c572e80c3c6\",\"2c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values),
	//			"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"284d2aee-99dd-4494-a702-ec7a098e8a88\",\"2bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"26895f58-2535-4d52-90eb-4c572e80c3c6\",\"2c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}}" };

	//		var positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values(
	//			StringArchetype.Foo,
	//			new Dictionary<string, ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2>
	//			{
	//				{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("284d2aee-99dd-4494-a702-ec7a098e8a88"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
	//				{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("26895f58-2535-4d52-90eb-4c572e80c3c6"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2c739b95-3614-4d3d-898d-a8c15c29370e") }) },
	//			});
	//		yield return new object[] {
	//			positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values,
	//			"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"284d2aee-99dd-4494-a702-ec7a098e8a88\",\"2bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"26895f58-2535-4d52-90eb-4c572e80c3c6\",\"2c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values),
	//			"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"284d2aee-99dd-4494-a702-ec7a098e8a88\",\"2bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"26895f58-2535-4d52-90eb-4c572e80c3c6\",\"2c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}}" };


	//		var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2Values(
	//			StringArchetype.Foo,
	//			new Dictionary<string, ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2>
	//			{
	//				{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2effc8ae-d14f-4fca-9f9a-c9d001478d3d"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("21046b1f-53a6-4a4e-9be9-cc7718b6ca93") }) },
	//				{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2d13b555-9a4e-4c8d-b64b-cfd00a7a897c"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2d1f7bb3-f06e-4181-9f26-f0635e3d3983") }) },
	//			});
	//		yield return new object[] {
	//			positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues,
	//			"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"2effc8ae-d14f-4fca-9f9a-c9d001478d3d\",\"21046b1f-53a6-4a4e-9be9-cc7718b6ca93\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"2d13b555-9a4e-4c8d-b64b-cfd00a7a897c\",\"2d1f7bb3-f06e-4181-9f26-f0635e3d3983\"]}}}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2Values(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues),
	//			"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"2effc8ae-d14f-4fca-9f9a-c9d001478d3d\",\"21046b1f-53a6-4a4e-9be9-cc7718b6ca93\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"2d13b555-9a4e-4c8d-b64b-cfd00a7a897c\",\"2d1f7bb3-f06e-4181-9f26-f0635e3d3983\"]}}}}" };

	//		var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Keys(
	//			StringArchetype.Foo,
	//			new Dictionary<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2, string>
	//			{
	//				{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("24b0ae6c-2b98-41f5-b2e8-16a158037157"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2d925b20-9951-4bae-b65c-8a01e8d3e493") }), StringArchetype.Foo },
	//				{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("25276338-a7fd-4647-8262-df8c4f6de190"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2c963cd5-7b84-417a-87e4-a907ca895642") }), StringArchetype.Bar },
	//			});
	//		yield return new object[] {
	//			positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys,
	//			"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"24b0ae6c-2b98-41f5-b2e8-16a158037157\\\",\\\"2d925b20-9951-4bae-b65c-8a01e8d3e493\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"25276338-a7fd-4647-8262-df8c4f6de190\\\",\\\"2c963cd5-7b84-417a-87e4-a907ca895642\\\"]}\":\"bar\"}}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys),
	//			"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"24b0ae6c-2b98-41f5-b2e8-16a158037157\\\",\\\"2d925b20-9951-4bae-b65c-8a01e8d3e493\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"25276338-a7fd-4647-8262-df8c4f6de190\\\",\\\"2c963cd5-7b84-417a-87e4-a907ca895642\\\"]}\":\"bar\"}}}" };



	//		var positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Keys(
	//			StringArchetype.Foo,
	//			new Dictionary<ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2, string>
	//			{
	//				{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("20efae4f-20c4-47cd-93d5-8acd846a937c"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("24c22756-abf5-4dc1-ab43-896d06e01d1a") }), StringArchetype.Foo },
	//				{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("22415979-9256-418d-bead-3103b19dcb6b"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("223b2fb8-d90e-4ff5-ab96-704c05ad06bd") }), StringArchetype.Bar },
	//			});
	//		yield return new object[] {
	//			positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys,
	//			"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"20efae4f-20c4-47cd-93d5-8acd846a937c\\\",\\\"24c22756-abf5-4dc1-ab43-896d06e01d1a\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"22415979-9256-418d-bead-3103b19dcb6b\\\",\\\"223b2fb8-d90e-4ff5-ab96-704c05ad06bd\\\"]}\":\"bar\"}}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys),
	//			"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"20efae4f-20c4-47cd-93d5-8acd846a937c\\\",\\\"24c22756-abf5-4dc1-ab43-896d06e01d1a\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"22415979-9256-418d-bead-3103b19dcb6b\\\",\\\"223b2fb8-d90e-4ff5-ab96-704c05ad06bd\\\"]}\":\"bar\"}}}" };



	//		var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2Keys(
	//			StringArchetype.Foo,
	//			new Dictionary<ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2, string>
	//			{
	//				{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("2fd6010c-98df-454d-be6b-14e21f76610e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("29371516-633f-4564-a5dc-e0da5524c7d2") }), StringArchetype.Foo },
	//				{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("205a273c-8426-4c45-a7bf-de797259ee17"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("210e386f-53c8-4b25-81d0-875b2e1d0cf0") }), StringArchetype.Bar },
	//			});
	//		yield return new object[] {
	//			positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys,
	//			"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"2fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"29371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"205a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"210e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}" };

	//		yield return new object[] {
	//			new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys),
	//			"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"2fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"29371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"205a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"210e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}}" };
	//	}
	//}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3Values = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3Values(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("384d2aee-99dd-4494-a702-ec7a098e8a88"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("36895f58-2535-4d52-90eb-4c572e80c3c6"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3c739b95-3614-4d3d-898d-a8c15c29370e") }) },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3Values,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"384d2aee-99dd-4494-a702-ec7a098e8a88\",\"3bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"36895f58-2535-4d52-90eb-4c572e80c3c6\",\"3c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3Values(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3Values),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"384d2aee-99dd-4494-a702-ec7a098e8a88\",\"3bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"36895f58-2535-4d52-90eb-4c572e80c3c6\",\"3c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}}" };

			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter3Values = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter3Values(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("384d2aee-99dd-4494-a702-ec7a098e8a88"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("36895f58-2535-4d52-90eb-4c572e80c3c6"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3c739b95-3614-4d3d-898d-a8c15c29370e") }) },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter3Values,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"384d2aee-99dd-4494-a702-ec7a098e8a88\",\"3bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"36895f58-2535-4d52-90eb-4c572e80c3c6\",\"3c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter3Values(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter3Values),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"384d2aee-99dd-4494-a702-ec7a098e8a88\",\"3bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"36895f58-2535-4d52-90eb-4c572e80c3c6\",\"3c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}}" };


			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter3Values(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3effc8ae-d14f-4fca-9f9a-c9d001478d3d"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("31046b1f-53a6-4a4e-9be9-cc7718b6ca93") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3d13b555-9a4e-4c8d-b64b-cfd00a7a897c"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3d1f7bb3-f06e-4181-9f26-f0635e3d3983") }) },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"3effc8ae-d14f-4fca-9f9a-c9d001478d3d\",\"31046b1f-53a6-4a4e-9be9-cc7718b6ca93\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"3d13b555-9a4e-4c8d-b64b-cfd00a7a897c\",\"3d1f7bb3-f06e-4181-9f26-f0635e3d3983\"]}}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter3Values(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"3effc8ae-d14f-4fca-9f9a-c9d001478d3d\",\"31046b1f-53a6-4a4e-9be9-cc7718b6ca93\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"3d13b555-9a4e-4c8d-b64b-cfd00a7a897c\",\"3d1f7bb3-f06e-4181-9f26-f0635e3d3983\"]}}}}" };

			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3Keys(
				StringArchetype.Foo,
				new Dictionary<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3, string>
				{
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("34b0ae6c-2b98-41f5-b2e8-16a158037157"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3d925b20-9951-4bae-b65c-8a01e8d3e493") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("35276338-a7fd-4647-8262-df8c4f6de190"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3c963cd5-7b84-417a-87e4-a907ca895642") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"34b0ae6c-2b98-41f5-b2e8-16a158037157\\\",\\\"3d925b20-9951-4bae-b65c-8a01e8d3e493\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"35276338-a7fd-4647-8262-df8c4f6de190\\\",\\\"3c963cd5-7b84-417a-87e4-a907ca895642\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"34b0ae6c-2b98-41f5-b2e8-16a158037157\\\",\\\"3d925b20-9951-4bae-b65c-8a01e8d3e493\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"35276338-a7fd-4647-8262-df8c4f6de190\\\",\\\"3c963cd5-7b84-417a-87e4-a907ca895642\\\"]}\":\"bar\"}}}" };



			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter3Keys(
				StringArchetype.Foo,
				new Dictionary<ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3, string>
				{
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("30efae4f-20c4-47cd-93d5-8acd846a937c"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("34c22756-abf5-4dc1-ab43-896d06e01d1a") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("32415979-9256-418d-bead-3103b19dcb6b"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("323b2fb8-d90e-4ff5-ab96-704c05ad06bd") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"30efae4f-20c4-47cd-93d5-8acd846a937c\\\",\\\"34c22756-abf5-4dc1-ab43-896d06e01d1a\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"32415979-9256-418d-bead-3103b19dcb6b\\\",\\\"323b2fb8-d90e-4ff5-ab96-704c05ad06bd\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter3Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"30efae4f-20c4-47cd-93d5-8acd846a937c\\\",\\\"34c22756-abf5-4dc1-ab43-896d06e01d1a\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"32415979-9256-418d-bead-3103b19dcb6b\\\",\\\"323b2fb8-d90e-4ff5-ab96-704c05ad06bd\\\"]}\":\"bar\"}}}" };



			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter3Keys(
				StringArchetype.Foo,
				new Dictionary<ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3, string>
				{
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("3fd6010c-98df-454d-be6b-14e21f76610e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("39371516-633f-4564-a5dc-e0da5524c7d2") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("305a273c-8426-4c45-a7bf-de797259ee17"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("310e386f-53c8-4b25-81d0-875b2e1d0cf0") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"3fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"39371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"305a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"310e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter3Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"3fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"39371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"305a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"310e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}}" };
		}
	}

	record GrandParent<T>(
		Parent<T> Child,
		Parent<T>[] ArrayOfChildren,
		List<Parent<T>> ListOfChildren,
		NonEmptyHashSet<Parent<T>> NonEmptyHashSetOfChildren,
		IEnumerable<Parent<T>> IEnumerableOfChildren,
		Dictionary<Parent<T>, int> DictionaryWithChildrenKeys,
		Dictionary<int, Parent<T>> DictionaryWithChildrenValues,
		Dictionary<int, Parent<T>> DictionaryWithChildrenKeysAndValues);

	record Parent<T>(
		T Value,
		T[] Array,
		List<T> List,
		NonEmptyHashSet<T> NonEmptyHashSet,
		IEnumerable<T> IEnumerable,

		Parent<T> ValueParent,
		Parent<T>[] ArrayOfParents,
		List<Parent<T>> ListOfParents,
		NonEmptyHashSet<Parent<T>> NonEmptyHashSetOfParents,
		IEnumerable<Parent<T>> IEnumerableOfParents,

		GrandParent<T> ValueGrandParent,
		GrandParent<T>[] ArrayOfGrandParents,
		List<GrandParent<T>> ListOfGrandParents,
		NonEmptyHashSet<GrandParent<T>> NonEmptyHashSetOfGrandParents,
		IEnumerable<GrandParent<T>> IEnumerableOfGrandParents,

		Dictionary<T,                                                  int> DictionaryWithTKeys,
		Dictionary<T[],                                                int> DictionaryWithArrayOfTKeys,
		Dictionary<List<T>,                                            int> DictionaryWithListOfTKeys,
		Dictionary<NonEmptyHashSet<T>,                                 int> DictionaryWithNonEmptyHashSetOfTKeys,
		Dictionary<IEnumerable<T>,                                     int> DictionaryWithIEnumerableOfTKeys,
		Dictionary<Dictionary<T,                                 int>, int> DictionaryWithTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                                 T>, int> DictionaryWithTValueDictionaryKeys,
		Dictionary<Dictionary<T[],                               int>, int> DictionaryWithArrayOfTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                               T[]>, int> DictionaryWithArrayOfTValueDictionaryKeys,
		Dictionary<Dictionary<List<T>,                           int>, int> DictionaryWithListOfTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                           List<T>>, int> DictionaryWithListOfTValueDictionaryKeys,
		Dictionary<Dictionary<NonEmptyHashSet<T>,                int>, int> DictionaryWithNonEmptyHashSetOfTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                NonEmptyHashSet<T>>, int> DictionaryWithNonEmptyHashSetOfTValueDictionaryKeys,
		Dictionary<Dictionary<IEnumerable<T>,                    int>, int> DictionaryWithIEnumerableOfTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                     IEnumerable<T>>,int> DictionaryWithIEnumerableOfTValueDictionaryKeys,

		Dictionary<Parent<T>,                                                  int> DictionaryWithParentKeys,
		Dictionary<Parent<T>[],                                                int> DictionaryWithArrayOfParentKeys,
		Dictionary<List<Parent<T>>,                                            int> DictionaryWithListOfParentKeys,
		Dictionary<NonEmptyHashSet<Parent<T>>,                                 int> DictionaryWithNonEmptyHashSetOfParentKeys,
		Dictionary<IEnumerable<Parent<T>>,                                     int> DictionaryWithIEnumerableOfParentKeys,
		Dictionary<Dictionary<Parent<T>,                                 int>, int> DictionaryWithParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                                 Parent<T>>, int> DictionaryWithParentValueDictionaryKeys,
		Dictionary<Dictionary<Parent<T>[],                               int>, int> DictionaryWithArrayOfParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                               Parent<T>[]>, int> DictionaryWithArrayOfParentValueDictionaryKeys,
		Dictionary<Dictionary<List<Parent<T>>,                           int>, int> DictionaryWithListOfParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                           List<Parent<T>>>, int> DictionaryWithListOfParentValueDictionaryKeys,
		Dictionary<Dictionary<NonEmptyHashSet<Parent<T>>,                int>, int> DictionaryWithNonEmptyHashSetOfParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                NonEmptyHashSet<Parent<T>>>, int> DictionaryWithNonEmptyHashSetOfParentValueDictionaryKeys,
		Dictionary<Dictionary<IEnumerable<Parent<T>>,                    int>, int> DictionaryWithIEnumerableOfParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                     IEnumerable<Parent<T>>>,int> DictionaryWithIEnumerableOfParentValueDictionaryKeys,

		Dictionary<GrandParent<T>,                                                  int> DictionaryWithGrandParentKeys,
		Dictionary<GrandParent<T>[],                                                int> DictionaryWithArrayOfGrandParentKeys,
		Dictionary<List<GrandParent<T>>,                                            int> DictionaryWithListOfGrandParentKeys,
		Dictionary<NonEmptyHashSet<GrandParent<T>>,                                 int> DictionaryWithNonEmptyHashSetOfGrandParentKeys,
		Dictionary<IEnumerable<GrandParent<T>>,                                     int> DictionaryWithIEnumerableOfGrandParentKeys,
		Dictionary<Dictionary<GrandParent<T>,                                 int>, int> DictionaryWithGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                                 GrandParent<T>>, int> DictionaryWithGrandParentValueDictionaryKeys,
		Dictionary<Dictionary<GrandParent<T>[],                               int>, int> DictionaryWithArrayOfGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                               GrandParent<T>[]>, int> DictionaryWithArrayOfGrandParentValueDictionaryKeys,
		Dictionary<Dictionary<List<GrandParent<T>>,                           int>, int> DictionaryWithListOfGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                           List<GrandParent<T>>>, int> DictionaryWithListOfGrandParentValueDictionaryKeys,
		Dictionary<Dictionary<NonEmptyHashSet<GrandParent<T>>,                int>, int> DictionaryWithNonEmptyHashSetOfGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                NonEmptyHashSet<GrandParent<T>>>, int> DictionaryWithNonEmptyHashSetOfGrandParentValueDictionaryKeys,
		Dictionary<Dictionary<IEnumerable<GrandParent<T>>,                    int>, int> DictionaryWithIEnumerableOfGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                     IEnumerable<GrandParent<T>>>,int> DictionaryWithIEnumerableOfGrandParentValueDictionaryKeys,

		Dictionary<T, T> DictionaryWithTKeysAndValues,
		Dictionary<Parent<T>, Parent<T>> DictionaryWithParentKeysAndValues,
		Dictionary<GrandParent<T>, GrandParent<T>> DictionaryWithGrandParentKeysAndValues,

		T[][] ArrayOfArrays,
		List<T>[] ArrayOfLists,
		NonEmptyHashSet<T>[] ArrayOfNonEmptyHashSets,
		IEnumerable<T>[] ArrayOfIEnumerables,
		List<T[]> ListOfArrays,
		List<List<T>> ListOfLists,
		List<NonEmptyHashSet<T>> ListOfNonEmptyHashSets,
		List<IEnumerable<T>> ListOfIEnumerables,
		NonEmptyHashSet<T[]> NonEmptyHashSetOfArrays,
		NonEmptyHashSet<List<T>> NonEmptyHashSetOfLists,
		NonEmptyHashSet<NonEmptyHashSet<T>> NonEmptyHashSetOfNonEmptyHashSets,
		NonEmptyHashSet<IEnumerable<T>> NonEmptyHashSetOfIEnumerables,
		IEnumerable<T[]> IEnumerableOfArrays,
		IEnumerable<List<T>> IEnumerableOfLists,
		IEnumerable<NonEmptyHashSet<T>> IEnumerableOfNonEmptyHashSets,
		IEnumerable<IEnumerable<T>> IEnumerableOfIEnumerables,

		Parent<T>[][] ArrayOfArrayOfParents,
		List<Parent<T>>[] ArrayOfListOfParents,
		NonEmptyHashSet<Parent<T>>[] ArrayOfNonEmptyHashSetOfParents,
		IEnumerable<Parent<T>>[] ArrayOfIEnumerableOfParents,

		List<Parent<T>[]> ListOfArrayOfParents,
		List<List<Parent<T>>> ListOfListOfParents,
		List<NonEmptyHashSet<Parent<T>>> ListOfNonEmptyHashSetOfParents,
		List<IEnumerable<Parent<T>>> ListOfIEnumerableOfParents,

		NonEmptyHashSet<Parent<T>[]> NonEmptyHashSetOfArrayOfParents,
		NonEmptyHashSet<List<Parent<T>>> NonEmptyHashSetOfListOfParents,
		NonEmptyHashSet<NonEmptyHashSet<Parent<T>>> NonEmptyHashSetOfNonEmptyHashSetOfParents,
		NonEmptyHashSet<IEnumerable<Parent<T>>> NonEmptyHashSetOfIEnumerableOfParents,

		IEnumerable<Parent<T>[]>IEnumerableOfArrayOfParents,
		IEnumerable<List<Parent<T>>>IEnumerableOfListOfParents,
		IEnumerable<NonEmptyHashSet<Parent<T>>>IEnumerableOfNonEmptyHashSetOfParents,
		IEnumerable<IEnumerable<Parent<T>>>IEnumerableOfIEnumerableOfParents,

		GrandParent<T>[][] ArrayOfArrayOfGrandParents,
		List<GrandParent<T>>[] ArrayOfListOfGrandParents,
		NonEmptyHashSet<GrandParent<T>>[] ArrayOfNonEmptyHashSetOfGrandParents,
		IEnumerable<GrandParent<T>>[] ArrayOfIEnumerableOfGrandParents,

		List<GrandParent<T>[]> ListOfArrayOfGrandParents,
		List<List<GrandParent<T>>> ListOfListOfGrandParents,
		List<NonEmptyHashSet<GrandParent<T>>> ListOfNonEmptyHashSetOfGrandParents,
		List<IEnumerable<GrandParent<T>>> ListOfIEnumerableOfGrandParents,

		NonEmptyHashSet<GrandParent<T>[]> NonEmptyHashSetOfArrayOfGrandParents,
		NonEmptyHashSet<List<GrandParent<T>>> NonEmptyHashSetOfListOfGrandParents,
		NonEmptyHashSet<NonEmptyHashSet<GrandParent<T>>> NonEmptyHashSetOfNonEmptyHashSetOfGrandParents,
		NonEmptyHashSet<IEnumerable<GrandParent<T>>> INonEmptyHashSetOfEnumerableOfGrandParents,

		IEnumerable<GrandParent<T>[]> IEnumerableOfArrayOfGrandParents,
		IEnumerable<List<GrandParent<T>>> IEnumerableOfListOfGrandParents,
		IEnumerable<NonEmptyHashSet<GrandParent<T>>> IEnumerableOfNonEmptyHashSetOfGrandParents,
		IEnumerable<IEnumerable<GrandParent<T>>> IEnumerableOfIEnumerableOfGrandParents,

// Dictionaries
		Dictionary<T,                                                  int>[] ArrayOfDictionariesWithTKeys,
		Dictionary<T[],                                                int>[] ArrayOfDictionariesWithArrayOfTKeys,
		Dictionary<List<T>,                                            int>[] ArrayOfDictionariesWithListOfTKeys,
		Dictionary<NonEmptyHashSet<T>,                                 int>[] ArrayOfDictionariesWithNonEmptyHashSetOfTKeys,
		Dictionary<IEnumerable<T>,                                     int>[] ArrayOfDictionariesWithIEnumerableOfTKeys,
		Dictionary<Dictionary<T,                                 int>, int>[] ArrayOfDictionariesWithTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                                 T>, int>[] ArrayOfDictionariesWithTValueDictionaryKeys,
		Dictionary<Dictionary<T[],                               int>, int>[] ArrayOfDictionariesWithArrayOfTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                               T[]>, int>[] ArrayOfDictionariesWithArrayOfTValueDictionaryKeys,
		Dictionary<Dictionary<List<T>,                           int>, int>[] ArrayOfDictionariesWithListOfTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                           List<T>>, int>[] ArrayOfDictionariesWithListOfTValueDictionaryKeys,
		Dictionary<Dictionary<NonEmptyHashSet<T>,                int>, int>[] ArrayOfDictionariesWithNonEmptyHashSetOfTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                NonEmptyHashSet<T>>, int>[] ArrayOfDictionariesWithNonEmptyHashSetOfTValueDictionaryKeys,
		Dictionary<Dictionary<IEnumerable<T>,                    int>, int>[] ArrayOfDictionariesWithIEnumerableOfTKeyDictionaryKeys,
		Dictionary<Dictionary<int,                     IEnumerable<T>>,int>[] ArrayOfDictionariesWithIEnumerableOfTValueDictionaryKeys,
		Dictionary<Parent<T>,                                                  int>[] ArrayOfDictionariesWithParentKeys,
		Dictionary<Parent<T>[],                                                int>[] ArrayOfDictionariesWithArrayOfParentKeys,
		Dictionary<List<Parent<T>>,                                            int>[] ArrayOfDictionariesWithListOfParentKeys,
		Dictionary<NonEmptyHashSet<Parent<T>>,                                 int>[] ArrayOfDictionariesWithNonEmptyHashSetOfParentKeys,
		Dictionary<IEnumerable<Parent<T>>,                                     int>[] ArrayOfDictionariesWithIEnumerableOfParentKeys,
		Dictionary<Dictionary<Parent<T>,                                 int>, int>[] ArrayOfDictionariesWithParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                                 Parent<T>>, int>[] ArrayOfDictionariesWithParentValueDictionaryKeys,
		Dictionary<Dictionary<Parent<T>[],                               int>, int>[] ArrayOfDictionariesWithArrayOfParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                               Parent<T>[]>, int>[] ArrayOfDictionariesWithArrayOfParentValueDictionaryKeys,
		Dictionary<Dictionary<List<Parent<T>>,                           int>, int>[] ArrayOfDictionariesWithListOfParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                           List<Parent<T>>>, int>[] ArrayOfDictionariesWithListOfParentValueDictionaryKeys,
		Dictionary<Dictionary<NonEmptyHashSet<Parent<T>>,                int>, int>[] ArrayOfDictionariesWithNonEmptyHashSetOfParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                NonEmptyHashSet<Parent<T>>>, int>[] ArrayOfDictionariesWithNonEmptyHashSetOfParentValueDictionaryKeys,
		Dictionary<Dictionary<IEnumerable<Parent<T>>,                    int>, int>[] ArrayOfDictionariesWithIEnumerableOfParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                     IEnumerable<Parent<T>>>,int>[] ArrayOfDictionariesWithIEnumerableOfParentValueDictionaryKeys,
		Dictionary<GrandParent<T>,                                                  int>[] ArrayOfDictionariesWithGrandParentKeys,
		Dictionary<GrandParent<T>[],                                                int>[] ArrayOfDictionariesWithArrayOfGrandParentKeys,
		Dictionary<List<GrandParent<T>>,                                            int>[] ArrayOfDictionariesWithListOfGrandParentKeys,
		Dictionary<NonEmptyHashSet<GrandParent<T>>,                                 int>[] ArrayOfDictionariesWithNonEmptyHashSetOfGrandParentKeys,
		Dictionary<IEnumerable<GrandParent<T>>,                                     int>[] ArrayOfDictionariesWithIEnumerableOfGrandParentKeys,
		Dictionary<Dictionary<GrandParent<T>,                                 int>, int>[] ArrayOfDictionariesWithGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                                 GrandParent<T>>, int>[] ArrayOfDictionariesWithGrandParentValueDictionaryKeys,
		Dictionary<Dictionary<GrandParent<T>[],                               int>, int>[] ArrayOfDictionariesWithArrayOfGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                               GrandParent<T>[]>, int>[] ArrayOfDictionariesWithArrayOfGrandParentValueDictionaryKeys,
		Dictionary<Dictionary<List<GrandParent<T>>,                           int>, int>[] ArrayOfDictionariesWithListOfGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                           List<GrandParent<T>>>, int>[] ArrayOfDictionariesWithListOfGrandParentValueDictionaryKeys,
		Dictionary<Dictionary<NonEmptyHashSet<GrandParent<T>>,                int>, int>[] ArrayOfDictionariesWithNonEmptyHashSetOfGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                NonEmptyHashSet<GrandParent<T>>>, int>[] ArrayOfDictionariesWithNonEmptyHashSetOfGrandParentValueDictionaryKeys,
		Dictionary<Dictionary<IEnumerable<GrandParent<T>>,                    int>, int>[] ArrayOfDictionariesWithIEnumerableOfGrandParentKeyDictionaryKeys,
		Dictionary<Dictionary<int,                     IEnumerable<GrandParent<T>>>,int>[] ArrayOfDictionariesWithIEnumerableOfGrandParentValueDictionaryKeys,

		List<Dictionary<T,                                                  int>> ListOfDictionariesWithTKeys,
		List<Dictionary<T[],                                                int>> ListOfDictionariesWithArrayOfTKeys,
		List<Dictionary<List<T>,                                            int>> ListOfDictionariesWithListOfTKeys,
		List<Dictionary<NonEmptyHashSet<T>,                                 int>> ListOfDictionariesWithNonEmptyHashSetOfTKeys,
		List<Dictionary<IEnumerable<T>,                                     int>> ListOfDictionariesWithIEnumerableOfTKeys,
		List<Dictionary<Dictionary<T,                                 int>, int>> ListOfDictionariesWithTKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                                 T>, int>> ListOfDictionariesWithTValueDictionaryKeys,
		List<Dictionary<Dictionary<T[],                               int>, int>> ListOfDictionariesWithArrayOfTKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                               T[]>, int>> ListOfDictionariesWithArrayOfTValueDictionaryKeys,
		List<Dictionary<Dictionary<List<T>,                           int>, int>> ListOfDictionariesWithListOfTKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                           List<T>>, int>> ListOfDictionariesWithListOfTValueDictionaryKeys,
		List<Dictionary<Dictionary<NonEmptyHashSet<T>,                int>, int>> ListOfDictionariesWithNonEmptyHashSetOfTKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                NonEmptyHashSet<T>>, int>> ListOfDictionariesWithNonEmptyHashSetOfTValueDictionaryKeys,
		List<Dictionary<Dictionary<IEnumerable<T>,                    int>, int>> ListOfDictionariesWithIEnumerableOfTKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                     IEnumerable<T>>,int>> ListOfDictionariesWithIEnumerableOfTValueDictionaryKeys,
		List<Dictionary<Parent<T>,                                                  int>> ListOfDictionariesWithParentKeys,
		List<Dictionary<Parent<T>[],                                                int>> ListOfDictionariesWithArrayOfParentKeys,
		List<Dictionary<List<Parent<T>>,                                            int>> ListOfDictionariesWithListOfParentKeys,
		List<Dictionary<NonEmptyHashSet<Parent<T>>,                                 int>> ListOfDictionariesWithNonEmptyHashSetOfParentKeys,
		List<Dictionary<IEnumerable<Parent<T>>,                                     int>> ListOfDictionariesWithIEnumerableOfParentKeys,
		List<Dictionary<Dictionary<Parent<T>,                                 int>, int>> ListOfDictionariesWithParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                                 Parent<T>>, int>> ListOfDictionariesWithParentValueDictionaryKeys,
		List<Dictionary<Dictionary<Parent<T>[],                               int>, int>> ListOfDictionariesWithArrayOfParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                               Parent<T>[]>, int>> ListOfDictionariesWithArrayOfParentValueDictionaryKeys,
		List<Dictionary<Dictionary<List<Parent<T>>,                           int>, int>> ListOfDictionariesWithListOfParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                           List<Parent<T>>>, int>> ListOfDictionariesWithListOfParentValueDictionaryKeys,
		List<Dictionary<Dictionary<NonEmptyHashSet<Parent<T>>,                int>, int>> ListOfDictionariesWithNonEmptyHashSetOfParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                NonEmptyHashSet<Parent<T>>>, int>> ListOfDictionariesWithNonEmptyHashSetOfParentValueDictionaryKeys,
		List<Dictionary<Dictionary<IEnumerable<Parent<T>>,                    int>, int>> ListOfDictionariesWithIEnumerableOfParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                     IEnumerable<Parent<T>>>,int>> ListOfDictionariesWithIEnumerableOfParentValueDictionaryKeys,
		List<Dictionary<GrandParent<T>,                                                  int>> ListOfDictionariesWithGrandParentKeys,
		List<Dictionary<GrandParent<T>[],                                                int>> ListOfDictionariesWithArrayOfGrandParentKeys,
		List<Dictionary<List<GrandParent<T>>,                                            int>> ListOfDictionariesWithListOfGrandParentKeys,
		List<Dictionary<NonEmptyHashSet<GrandParent<T>>,                                 int>> ListOfDictionariesWithNonEmptyHashSetOfGrandParentKeys,
		List<Dictionary<IEnumerable<GrandParent<T>>,                                     int>> ListOfDictionariesWithIEnumerableOfGrandParentKeys,
		List<Dictionary<Dictionary<GrandParent<T>,                                 int>, int>> ListOfDictionariesWithGrandParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                                 GrandParent<T>>, int>> ListOfDictionariesWithGrandParentValueDictionaryKeys,
		List<Dictionary<Dictionary<GrandParent<T>[],                               int>, int>> ListOfDictionariesWithArrayOfGrandParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                               GrandParent<T>[]>, int>> ListOfDictionariesWithArrayOfGrandParentValueDictionaryKeys,
		List<Dictionary<Dictionary<List<GrandParent<T>>,                           int>, int>> ListOfDictionariesWithListOfGrandParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                           List<GrandParent<T>>>, int>> ListOfDictionariesWithListOfGrandParentValueDictionaryKeys,
		List<Dictionary<Dictionary<NonEmptyHashSet<GrandParent<T>>,                int>, int>> ListOfDictionariesWithNonEmptyHashSetOfGrandParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                NonEmptyHashSet<GrandParent<T>>>, int>> ListOfDictionariesWithNonEmptyHashSetOfGrandParentValueDictionaryKeys,
		List<Dictionary<Dictionary<IEnumerable<GrandParent<T>>,                    int>, int>> ListOfDictionariesWithIEnumerableOfGrandParentKeyDictionaryKeys,
		List<Dictionary<Dictionary<int,                     IEnumerable<GrandParent<T>>>,int>> ListOfDictionariesWithIEnumerableOfGrandParentValueDictionaryKeys,

		NonEmptyHashSet<Dictionary<T,                                                  int>> NonEmptyHashSetOfDictionariesWithTKeys,
		NonEmptyHashSet<Dictionary<T[],                                                int>> NonEmptyHashSetOfDictionariesWithArrayOfTKeys,
		NonEmptyHashSet<Dictionary<List<T>,                                            int>> NonEmptyHashSetOfDictionariesWithListOfTKeys,
		NonEmptyHashSet<Dictionary<NonEmptyHashSet<T>,                                 int>> NonEmptyHashSetOfDictionariesWithNonEmptyHashSetOfTKeys,
		NonEmptyHashSet<Dictionary<IEnumerable<T>,                                     int>> NonEmptyHashSetOfDictionariesWithIEnumerableOfTKeys,
		NonEmptyHashSet<Dictionary<Dictionary<T,                                 int>, int>> NonEmptyHashSetOfDictionariesWithTKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                                 T>, int>> NonEmptyHashSetOfDictionariesWithTValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<T[],                               int>, int>> NonEmptyHashSetOfDictionariesWithArrayOfTKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                               T[]>, int>> NonEmptyHashSetOfDictionariesWithArrayOfTValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<List<T>,                           int>, int>> NonEmptyHashSetOfDictionariesWithListOfTKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                           List<T>>, int>> NonEmptyHashSetOfDictionariesWithListOfTValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<NonEmptyHashSet<T>,                int>, int>> NonEmptyHashSetOfDictionariesWithNonEmptyHashSetOfTKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                NonEmptyHashSet<T>>, int>> NonEmptyHashSetOfDictionariesWithNonEmptyHashSetOfTValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<IEnumerable<T>,                    int>, int>> NonEmptyHashSetOfDictionariesWithIEnumerableOfTKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                     IEnumerable<T>>,int>> NonEmptyHashSetOfDictionariesWithIEnumerableOfTValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Parent<T>,                                                  int>> NonEmptyHashSetOfDictionariesWithParentKeys,
		NonEmptyHashSet<Dictionary<Parent<T>[],                                                int>> NonEmptyHashSetOfDictionariesWithArrayOfParentKeys,
		NonEmptyHashSet<Dictionary<List<Parent<T>>,                                            int>> NonEmptyHashSetOfDictionariesWithListOfParentKeys,
		NonEmptyHashSet<Dictionary<NonEmptyHashSet<Parent<T>>,                                 int>> NonEmptyHashSetOfDictionariesWithNonEmptyHashSetOfParentKeys,
		NonEmptyHashSet<Dictionary<IEnumerable<Parent<T>>,                                     int>> NonEmptyHashSetOfDictionariesWithIEnumerableOfParentKeys,
		NonEmptyHashSet<Dictionary<Dictionary<Parent<T>,                                 int>, int>> NonEmptyHashSetOfDictionariesWithParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                                 Parent<T>>, int>> NonEmptyHashSetOfDictionariesWithParentValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<Parent<T>[],                               int>, int>> NonEmptyHashSetOfDictionariesWithArrayOfParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                               Parent<T>[]>, int>> NonEmptyHashSetOfDictionariesWithArrayOfParentValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<List<Parent<T>>,                           int>, int>> NonEmptyHashSetOfDictionariesWithListOfParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                           List<Parent<T>>>, int>> NonEmptyHashSetOfDictionariesWithListOfParentValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<NonEmptyHashSet<Parent<T>>,                int>, int>> NonEmptyHashSetOfDictionariesWithNonEmptyHashSetOfParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                NonEmptyHashSet<Parent<T>>>, int>> NonEmptyHashSetOfDictionariesWithNonEmptyHashSetOfParentValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<IEnumerable<Parent<T>>,                    int>, int>> NonEmptyHashSetOfDictionariesWithIEnumerableOfParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                     IEnumerable<Parent<T>>>,int>> NonEmptyHashSetOfDictionariesWithIEnumerableOfParentValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<GrandParent<T>,                                                  int>> NonEmptyHashSetOfDictionariesWithGrandParentKeys,
		NonEmptyHashSet<Dictionary<GrandParent<T>[],                                                int>> NonEmptyHashSetOfDictionariesWithArrayOfGrandParentKeys,
		NonEmptyHashSet<Dictionary<List<GrandParent<T>>,                                            int>> NonEmptyHashSetOfDictionariesWithListOfGrandParentKeys,
		NonEmptyHashSet<Dictionary<NonEmptyHashSet<GrandParent<T>>,                                 int>> NonEmptyHashSetOfDictionariesWithNonEmptyHashSetOfGrandParentKeys,
		NonEmptyHashSet<Dictionary<IEnumerable<GrandParent<T>>,                                     int>> NonEmptyHashSetOfDictionariesWithIEnumerableOfGrandParentKeys,
		NonEmptyHashSet<Dictionary<Dictionary<GrandParent<T>,                                 int>, int>> NonEmptyHashSetOfDictionariesWithGrandParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                                 GrandParent<T>>, int>> NonEmptyHashSetOfDictionariesWithGrandParentValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<GrandParent<T>[],                               int>, int>> NonEmptyHashSetOfDictionariesWithArrayOfGrandParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                               GrandParent<T>[]>, int>> NonEmptyHashSetOfDictionariesWithArrayOfGrandParentValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<List<GrandParent<T>>,                           int>, int>> NonEmptyHashSetOfDictionariesWithListOfGrandParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                           List<GrandParent<T>>>, int>> NonEmptyHashSetOfDictionariesWithListOfGrandParentValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<NonEmptyHashSet<GrandParent<T>>,                int>, int>> NonEmptyHashSetOfDictionariesWithNonEmptyHashSetOfGrandParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                NonEmptyHashSet<GrandParent<T>>>, int>> NonEmptyHashSetOfDictionariesWithNonEmptyHashSetOfGrandParentValueDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<IEnumerable<GrandParent<T>>,                    int>, int>> NonEmptyHashSetOfDictionariesWithIEnumerableOfGrandParentKeyDictionaryKeys,
		NonEmptyHashSet<Dictionary<Dictionary<int,                     IEnumerable<GrandParent<T>>>,int>> NonEmptyHashSetOfDictionariesWithIEnumerableOfGrandParentValueDictionaryKeys,

		IEnumerable<Dictionary<T,                                                  int>> IEnumerableOfDictionariesWithTKeys,
		IEnumerable<Dictionary<T[],                                                int>> IEnumerableOfDictionariesWithArrayOfTKeys,
		IEnumerable<Dictionary<List<T>,                                            int>> IEnumerableOfDictionariesWithListOfTKeys,
		IEnumerable<Dictionary<NonEmptyHashSet<T>,                                 int>> IEnumerableOfDictionariesWithNonEmptyHashSetOfTKeys,
		IEnumerable<Dictionary<IEnumerable<T>,                                     int>> IEnumerableOfDictionariesWithIEnumerableOfTKeys,
		IEnumerable<Dictionary<Dictionary<T,                                 int>, int>> IEnumerableOfDictionariesWithTKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                                 T>, int>> IEnumerableOfDictionariesWithTValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<T[],                               int>, int>> IEnumerableOfDictionariesWithArrayOfTKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                               T[]>, int>> IEnumerableOfDictionariesWithArrayOfTValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<List<T>,                           int>, int>> IEnumerableOfDictionariesWithListOfTKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                           List<T>>, int>> IEnumerableOfDictionariesWithListOfTValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<NonEmptyHashSet<T>,                int>, int>> IEnumerableOfDictionariesWithNonEmptyHashSetOfTKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                NonEmptyHashSet<T>>, int>> IEnumerableOfDictionariesWithNonEmptyHashSetOfTValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<IEnumerable<T>,                    int>, int>> IEnumerableOfDictionariesWithIEnumerableOfTKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                     IEnumerable<T>>,int>> IEnumerableOfDictionariesWithIEnumerableOfTValueDictionaryKeys,
		IEnumerable<Dictionary<Parent<T>,                                                  int>> IEnumerableOfDictionariesWithParentKeys,
		IEnumerable<Dictionary<Parent<T>[],                                                int>> IEnumerableOfDictionariesWithArrayOfParentKeys,
		IEnumerable<Dictionary<List<Parent<T>>,                                            int>> IEnumerableOfDictionariesWithListOfParentKeys,
		IEnumerable<Dictionary<NonEmptyHashSet<Parent<T>>,                                 int>> IEnumerableOfDictionariesWithNonEmptyHashSetOfParentKeys,
		IEnumerable<Dictionary<IEnumerable<Parent<T>>,                                     int>> IEnumerableOfDictionariesWithIEnumerableOfParentKeys,
		IEnumerable<Dictionary<Dictionary<Parent<T>,                                 int>, int>> IEnumerableOfDictionariesWithParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                                 Parent<T>>, int>> IEnumerableOfDictionariesWithParentValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<Parent<T>[],                               int>, int>> IEnumerableOfDictionariesWithArrayOfParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                               Parent<T>[]>, int>> IEnumerableOfDictionariesWithArrayOfParentValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<List<Parent<T>>,                           int>, int>> IEnumerableOfDictionariesWithListOfParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                           List<Parent<T>>>, int>> IEnumerableOfDictionariesWithListOfParentValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<NonEmptyHashSet<Parent<T>>,                int>, int>> IEnumerableOfDictionariesWithNonEmptyHashSetOfParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                NonEmptyHashSet<Parent<T>>>, int>> IEnumerableOfDictionariesWithNonEmptyHashSetOfParentValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<IEnumerable<Parent<T>>,                    int>, int>> IEnumerableOfDictionariesWithIEnumerableOfParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                     IEnumerable<Parent<T>>>,int>> IEnumerableOfDictionariesWithIEnumerableOfParentValueDictionaryKeys,
		IEnumerable<Dictionary<GrandParent<T>,                                                  int>> IEnumerableOfDictionariesWithGrandParentKeys,
		IEnumerable<Dictionary<GrandParent<T>[],                                                int>> IEnumerableOfDictionariesWithArrayOfGrandParentKeys,
		IEnumerable<Dictionary<List<GrandParent<T>>,                                            int>> IEnumerableOfDictionariesWithListOfGrandParentKeys,
		IEnumerable<Dictionary<NonEmptyHashSet<GrandParent<T>>,                                 int>> IEnumerableOfDictionariesWithNonEmptyHashSetOfGrandParentKeys,
		IEnumerable<Dictionary<IEnumerable<GrandParent<T>>,                                     int>> IEnumerableOfDictionariesWithIEnumerableOfGrandParentKeys,
		IEnumerable<Dictionary<Dictionary<GrandParent<T>,                                 int>, int>> IEnumerableOfDictionariesWithGrandParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                                 GrandParent<T>>, int>> IEnumerableOfDictionariesWithGrandParentValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<GrandParent<T>[],                               int>, int>> IEnumerableOfDictionariesWithArrayOfGrandParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                               GrandParent<T>[]>, int>> IEnumerableOfDictionariesWithArrayOfGrandParentValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<List<GrandParent<T>>,                           int>, int>> IEnumerableOfDictionariesWithListOfGrandParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                           List<GrandParent<T>>>, int>> IEnumerableOfDictionariesWithListOfGrandParentValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<NonEmptyHashSet<GrandParent<T>>,                int>, int>> IEnumerableOfDictionariesWithNonEmptyHashSetOfGrandParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                NonEmptyHashSet<GrandParent<T>>>, int>> IEnumerableOfDictionariesWithNonEmptyHashSetOfGrandParentValueDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<IEnumerable<GrandParent<T>>,                    int>, int>> IEnumerableOfDictionariesWithIEnumerableOfGrandParentKeyDictionaryKeys,
		IEnumerable<Dictionary<Dictionary<int,                     IEnumerable<GrandParent<T>>>,int>> IEnumerableOfDictionariesWithIEnumerableOfGrandParentValueDictionaryKeys
	);

	public enum DataStructureScope
	{
		TechnicalDefaultEnumValue = 0,
		SupportedByDefaultJsonConverter = 1,
		Unlimited = 2
	}
	public static IEnumerable<TestCase> GenerateDataStructuresInvolving<T>(T[] values, DataStructureScope dataStructureScope)
	{
		foreach (var value in values)
			foreach(var testCase in GenerateDataStructuresInvolving(value, dataStructureScope))
				yield return testCase;
	}
	public static IEnumerable<TestCase> GenerateDataStructuresInvolving<T>(T data, DataStructureScope dataStructureScope)
	{
		var dataAsJson = Newtonsoft.Json.JsonConvert.SerializeObject(data);

		var dataType = typeof(T);
		yield return new (dataAsJson, dataType);

		var TArray           = /* T[] */ dataType.MakeArrayType();
		var TList            = /* List<T> */ typeof(List<>).MakeGenericType(dataType);
		var TIEnumerable     = /* IEnumerable<T> */ typeof(IEnumerable<>).MakeGenericType(dataType);
		var TNonEmptyHashSet = /* NonEmptyHashSet<T> */ typeof(NonEmptyHashSet<>).MakeGenericType(dataType);
		foreach (var serialized in new[]
		{
			"[]",
			$"[{dataAsJson}]",
			$"[{dataAsJson},{dataAsJson}]"
		})
		{
			yield return new(serialized, TArray);
			yield return new(serialized, TList);
			yield return new(serialized, TIEnumerable);
			if (dataStructureScope == DataStructureScope.Unlimited)
			{
				yield return new(serialized, TNonEmptyHashSet);
			}
		}

		var arrayOfTArrays           = /* T[][] */                dataType.MakeArrayType().MakeArrayType();
		var arrayOfTLists            = /* List<T>[] */            typeof(List<>).MakeGenericType(dataType).MakeArrayType();
		var arrayOfTIEnumerables     = /* IEnumerable<T>[] */     typeof(IEnumerable<>).MakeGenericType(dataType).MakeArrayType();
		var arrayOfTNonEmptyHashSets = /* NonEmptyHashSet<T>[] */ typeof(NonEmptyHashSet<>).MakeGenericType(dataType).MakeArrayType();
		var listOfTArrays            = /* List<T[]> */                typeof(List<>).MakeGenericType(dataType.MakeArrayType());
		var listOfTLists             = /* List<List<T>> */            typeof(List<>).MakeGenericType(typeof(List<>).MakeGenericType(dataType));
		var listOfTIEnumerables      = /* List<IEnumerable<T>> */     typeof(List<>).MakeGenericType(typeof(IEnumerable<>).MakeGenericType(dataType));
		var listOfTNonEmptyHashSets  = /* List<NonEmptyHashSet<T>> */ typeof(List<>).MakeGenericType(typeof(NonEmptyHashSet<>).MakeGenericType(dataType));
		var iEnumerableOfTArrays            = /* IEnumerable<T[]> */                typeof(IEnumerable<>).MakeGenericType(dataType.MakeArrayType());
		var iEnumerableOfTLists             = /* IEnumerable<List<T>> */            typeof(IEnumerable<>).MakeGenericType(typeof(List<>).MakeGenericType(dataType));
		var iEnumerableOfTIEnumerables      = /* IEnumerable<IEnumerable<T>> */     typeof(IEnumerable<>).MakeGenericType(typeof(IEnumerable<>).MakeGenericType(dataType));
		var iEnumerableOfTNonEmptyHashSets  = /* IEnumerable<NonEmptyHashSet<T>> */ typeof(IEnumerable<>).MakeGenericType(typeof(NonEmptyHashSet<>).MakeGenericType(dataType));
		var nonEmptyHashSetOfTArrays            = /* NonEmptyHashSet<T[]> */                typeof(NonEmptyHashSet<>).MakeGenericType(dataType.MakeArrayType());
		var nonEmptyHashSetOfTLists             = /* NonEmptyHashSet<List<T>> */            typeof(NonEmptyHashSet<>).MakeGenericType(typeof(List<>).MakeGenericType(dataType));
		var nonEmptyHashSetOfTIEnumerables      = /* NonEmptyHashSet<IEnumerable<T>> */     typeof(NonEmptyHashSet<>).MakeGenericType(typeof(IEnumerable<>).MakeGenericType(dataType));
		var nonEmptyHashSetOfTNonEmptyHashSets  = /* NonEmptyHashSet<NonEmptyHashSet<T>> */ typeof(NonEmptyHashSet<>).MakeGenericType(typeof(NonEmptyHashSet<>).MakeGenericType(dataType));
		foreach (var serialized in new[]
		{
			"[[]]",
			$"[[{dataAsJson}]]",
			$"[[{dataAsJson},{dataAsJson}]]",
			$"[[{dataAsJson},{dataAsJson}],[]]",
			$"[[{dataAsJson},{dataAsJson}],[{dataAsJson}]]",
			$"[[{dataAsJson},{dataAsJson}],[{dataAsJson},{dataAsJson}]]"
		})
		{
			yield return new(serialized, arrayOfTArrays);
			yield return new(serialized, arrayOfTLists);
			yield return new(serialized, arrayOfTIEnumerables);

			yield return new(serialized, listOfTArrays);
			yield return new(serialized, listOfTLists);
			yield return new(serialized, listOfTIEnumerables);

			yield return new(serialized, iEnumerableOfTArrays);
			yield return new(serialized, iEnumerableOfTLists);
			yield return new(serialized, iEnumerableOfTIEnumerables);

			if (dataStructureScope == DataStructureScope.Unlimited)
			{
				yield return new(serialized, arrayOfTNonEmptyHashSets);
				yield return new(serialized, listOfTNonEmptyHashSets);
				yield return new(serialized, iEnumerableOfTNonEmptyHashSets);

				yield return new(serialized, nonEmptyHashSetOfTNonEmptyHashSets);

				yield return new(serialized, nonEmptyHashSetOfTArrays);
				yield return new(serialized, nonEmptyHashSetOfTLists);
				yield return new(serialized, nonEmptyHashSetOfTIEnumerables);
			}
		}

		var dictionaryWithTValues =  /* Dictionary<int,T> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType);
		foreach (var serialized in new[]
		{
			"{}",
			$"{{\"10\":{dataAsJson}}}",
			$"{{\"11\":{dataAsJson},\"12\":{dataAsJson}}}"
		})
		{
			yield return new(serialized, dictionaryWithTValues);
		}

		var dictionaryWithTArrayValues           = /* Dictionary<int,T[]> */                typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType.MakeArrayType());
		var dictionaryWithTListValues            = /* Dictionary<int,List<T>> */            typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(List<>).MakeGenericType(dataType));
		var dictionaryWithTIEnumerableValues     = /* Dictionary<int,IEnumerable<T>> */     typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(IEnumerable<>).MakeGenericType(dataType));
		var dictionaryWithTNonEmptyHashSetValues = /* Dictionary<int,NonEmptyHashSet<T>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyHashSet<>).MakeGenericType(dataType));
		foreach (var serialized in new[]
		{
			"{}",
			$"{{\"20\":[]}}",
			$"{{\"21\":[{dataAsJson}]}}",
			$"{{\"22\":[{dataAsJson},{dataAsJson}]}}",
		})
		{
			yield return new(serialized, dictionaryWithTArrayValues);
			yield return new(serialized, dictionaryWithTListValues);
			yield return new(serialized, dictionaryWithTIEnumerableValues);
			if (dataStructureScope == DataStructureScope.Unlimited)
			{
				yield return new(serialized, dictionaryWithTNonEmptyHashSetValues);
			}
		}

		var dictionaryWithDictionaryWithTValuesValues = /* Dictionary<int,Dictionary<string, T>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(Dictionary<,>).MakeGenericType(typeof(string), dataType));
		foreach (var serialized in new[]
		{
			"{}",
			"{\"30\":{}}",
			$"{{\"30\":{{\"foo\":{dataAsJson}}}}}",
			$"{{\"30\":{{\"foo\":{dataAsJson},\"bar:\":{dataAsJson}}}}}"
		})
		{
			yield return new(serialized, dictionaryWithDictionaryWithTValuesValues);
		}

		var dictionaryWithArrayOfTArraysValues                     = /* Dictionary<int,T[][]> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType.MakeArrayType().MakeArrayType());
		var dictionaryWithNonEmptyHashSetOfTNonEmptyHashSetsValues = /* Dictionary<int,NonEmptyHashSet<NonEmptyHashSet<T>>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyHashSet<>).MakeGenericType(typeof(NonEmptyHashSet<>).MakeGenericType(dataType)));
		foreach (var serialized in new[]
		{
		"{}",
		"{\"40\":[[]]}",
			$"{{\"41\":[[{dataAsJson}]]}}",
			$"{{\"42\":[[{dataAsJson},{dataAsJson}]]}}",
			$"{{\"43\":[[{dataAsJson},{dataAsJson}],[]]}}",
			$"{{\"44\":[[{dataAsJson},{dataAsJson}],[{dataAsJson}]]}}",
			$"{{\"45\":[[{dataAsJson},{dataAsJson}],[{dataAsJson},{dataAsJson}]]}}",
		})
		{
			yield return new(serialized, dictionaryWithArrayOfTArraysValues);
			if (dataStructureScope == DataStructureScope.Unlimited)
			{
				yield return new(serialized, dictionaryWithNonEmptyHashSetOfTNonEmptyHashSetsValues);
			}
		}

		var dataAsPropertyName = dataAsJson.Trim('"');
		if (!String.IsNullOrEmpty(dataAsPropertyName))
		{
			var dictionaryWithTKeys = /* Dictionary<T, int> */ typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int));
			foreach (var serialized in new[]
			{
				"{}",
				$"{{\"{dataAsPropertyName}\":50}}",
			})
			{
				yield return new(serialized, dictionaryWithTKeys);
			}

			var dictionaryWithTKeysAndValues = /* Dictionary<T, T> */ typeof(Dictionary<,>).MakeGenericType(dataType, dataType);
			foreach (var serialized in new[]
			{
				"{}",
				$"{{\"{dataAsPropertyName}\":{dataAsJson}}}"
			})
			{
				yield return new(serialized, dictionaryWithTKeysAndValues);
			}

			var dictionaryWithDictionaryWithTKeysKeys = /* Dictionary<int,Dictionary<T, string>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(Dictionary<,>).MakeGenericType(dataType, typeof(string)));
			foreach (var serialized in new[]
			{
				"{}",
				"{\"60\":{}}",
				$"{{\"61\":{{\"{dataAsPropertyName}\":\"bar\"}}}}"
			})
			{
				yield return new(serialized, dictionaryWithDictionaryWithTKeysKeys);
			}
		}

		var arrayOfDictionariesWithTValues           = /* Dictionary<int,T>[] */                typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType).MakeArrayType();
		var listOfDictionariesWithTValues            = /* List<Dictionary<int,T>> */            typeof(List<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType));
		var iEnumerableOfDictionariesWithTValues     = /* IEnumerable<Dictionary<int,T>> */     typeof(IEnumerable<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType));
		var nonEmptyHashSetOfDictionariesWithTValues = /* NonEmptyHashSet<Dictionary<int,T>> */ typeof(NonEmptyHashSet<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType));
		foreach (var serialized in new[]
		{
			"[]",
			"[{}]",
			$"[{{\"70\":{dataAsJson}}}]",
		})
		{
			yield return new(serialized, arrayOfDictionariesWithTValues);
			yield return new(serialized, listOfDictionariesWithTValues);
			yield return new(serialized, iEnumerableOfDictionariesWithTValues);
			if (dataStructureScope == DataStructureScope.Unlimited)
			{
				yield return new(serialized, nonEmptyHashSetOfDictionariesWithTValues);
			}
		}

		if (!String.IsNullOrEmpty(dataAsPropertyName))
		{
			var arrayOfDictionariesWithTKeys           = /* Dictionary<T, int>[] */                typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int)).MakeArrayType();
			var listOfDictionariesWithTKeys            = /* List<Dictionary<T, int>> */            typeof(List<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int)));
			var iEnumerableOfDictionariesWithTKeys     = /* IEnumerable<Dictionary<T, int>> */     typeof(IEnumerable<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int)));
			var nonEmptyHashSetOfDictionariesWithTKeys = /* NonEmptyHashSet<Dictionary<T, int>> */ typeof(NonEmptyHashSet<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int)));
			foreach (var serialized in new[]
			{
				"[]",
				"[{}]",
				$"[{{\"{dataAsPropertyName}\":80}}]",
			})
			{
				yield return new(serialized, arrayOfDictionariesWithTKeys);
				yield return new(serialized, listOfDictionariesWithTKeys);
				yield return new(serialized, iEnumerableOfDictionariesWithTKeys);
				if (dataStructureScope == DataStructureScope.Unlimited)
				{
					yield return new(serialized, nonEmptyHashSetOfDictionariesWithTKeys);
				}
			}

			var arrayOfDictionariesWithTKeysAndValues           = /* Dictionary<T, T>[] */                typeof(Dictionary<,>).MakeGenericType(dataType, dataType).MakeArrayType();
			var listOfDictionariesWithTKeysAndValues            = /* List<Dictionary<T, T>> */            typeof(List<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, dataType));
			var iEnumerableOfDictionariesWithTKeysAndValues     = /* IEnumerable<Dictionary<T, T>> */     typeof(IEnumerable<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, dataType));
			var nonEmptyHashSetOfDictionariesWithTKeysAndValues = /* NonEmptyHashSet<Dictionary<T, T>> */ typeof(NonEmptyHashSet<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, dataType));
			foreach (var serialized in new[]
			{
				"[]",
				"[{}]",
				$"[{{\"{dataAsPropertyName}\":{dataAsJson}}}]",
			})
			{
				yield return new(serialized, arrayOfDictionariesWithTKeysAndValues);
				yield return new(serialized, listOfDictionariesWithTKeysAndValues);
				yield return new(serialized, iEnumerableOfDictionariesWithTKeysAndValues);
				if (dataStructureScope == DataStructureScope.Unlimited)
				{
					yield return new(serialized, nonEmptyHashSetOfDictionariesWithTKeysAndValues);
				}
			}
		}
	}
}
