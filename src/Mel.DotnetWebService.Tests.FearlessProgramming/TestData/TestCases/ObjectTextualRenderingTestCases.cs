using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Guids;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class ObjectTextualRenderingTestCases
{
	public static IEnumerable<object[]> String_LowerCaseAlphabeticLetters
	{
		get
		{
			yield return new object[] { "a", "\"a\"" };
			yield return new object[] { "b", "\"b\"" };
		}
	}

	public static IEnumerable<object[]> String_UpperCaseAlphabeticLetters
	{
		get
		{
			yield return new object[] { "A", "\"A\"" };
			yield return new object[] { "B", "\"B\"" };
		}
	}

	public static IEnumerable<object[]> String_LowerCaseLettersWithAccent
	{
		get
		{
			yield return new object[] { "é", "\"é\"" };
			yield return new object[] { "ö", "\"ö\"" };
		}
	}

	public static IEnumerable<object[]> String_UpperCaseLettersWithAccent
	{
		get
		{
			yield return new object[] { "É", "\"É\"" };
			yield return new object[] { "Ö", "\"Ö\"" };
		}
	}

	public static IEnumerable<object[]> String_LowerCaseWords
	{
		get
		{
			yield return new object[] { "foo", "\"foo\"" };
			yield return new object[] { "bar", "\"bar\"" };
		}
	}

	public static IEnumerable<object[]> String_UpperCaseWords
	{
		get
		{
			yield return new object[] { "FOO", "\"FOO\"" };
			yield return new object[] { "BAR", "\"BAR\"" };
		}
	}

	public static IEnumerable<object[]> String_MixedCaseWords
	{
		get
		{
			yield return new object[] { "FoO", "\"FoO\"" };
			yield return new object[] { "bAR", "\"bAR\"" };
		}
	}

	public static IEnumerable<object[]> String_LowerCaseWordsWithAccent
	{
		get
		{
			yield return new object[] { "été", "\"été\"" };
			yield return new object[] { "öho", "\"öho\"" };
		}
	}

	public static IEnumerable<object[]> String_UpperCaseWordsWithAccent
	{
		get
		{
			yield return new object[] { "ÉTÉ", "\"ÉTÉ\"" };
			yield return new object[] { "ÖTO", "\"ÖTO\"" };
		}
	}

	public static IEnumerable<object[]> String_MixedCaseWordsWithAccent
	{
		get
		{
			yield return new object[] { "FoÖ", "\"FoÖ\"" };
			yield return new object[] { "bÄR", "\"bÄR\"" };
		}
	}

	public static IEnumerable<object[]> String_Numbers
	{
		get
		{
			yield return new object[] { "1", "\"1\"" };
			yield return new object[] { "2", "\"2\"" };
			yield return new object[] { "12", "\"12\"" };
		}
	}


	public static IEnumerable<object[]> String_Punctuation
	{
		get
		{
			yield return new object[] { "<", "\"<\"" };
			yield return new object[] { "&", "\"&\"" };
		}
	}

	public static IEnumerable<object[]> Integer_PositiveNumbers
	{
		get
		{
			yield return new object[] { 0,  "0" };
			yield return new object[] { 3,  "3" };
			yield return new object[] { 13, "13" };
		}
	}

	public static IEnumerable<object[]> Integer_NegativeNumbers
	{
		get
		{
			yield return new object[] { -2,  "-2" };
			yield return new object[] { -10, "-10" };
		}
	}

	public static IEnumerable<object[]> DateTimes
	{
		get
		{
			yield return new object[] { DateTime.MinValue,                                           "\"0001-01-01T00:00:00\"" };
			yield return new object[] { new DateTime(2025, 01, 02),                                  "\"2025-01-02T00:00:00\"" };
			yield return new object[] { DateTimeOffset.MinValue,                                     "\"0001-01-01T00:00:00+00:00\"" };
			yield return new object[] { new DateTimeOffset(2025, 01, 02, 01, 02, 03, TimeSpan.FromHours(4)), "\"2025-01-02T01:02:03+04:00\"" };
			yield return new object[] { System.Data.SqlTypes.SqlDateTime.MinValue.Value,             "\"1753-01-01T00:00:00\"" };
		}
	}

	public static IEnumerable<object[]> Guids
	{
		get
		{
			yield return new object[] { Guid.Empty,                                         "\"00000000-0000-0000-0000-000000000000\"" };
			yield return new object[] { Guid.Parse("ec6c1eda-a418-4534-9aa1-525784840ae2"), "\"ec6c1eda-a418-4534-9aa1-525784840ae2\"" };
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
			yield return new object[] { NonEmptyGuid.FromString("09f936b7-7375-4a5a-9cad-53740dd17e57"), "\"09f936b7-7375-4a5a-9cad-53740dd17e57\"" };
		}
	}

	public static IEnumerable<object[]> SimpleValueObjectsOnTwoLayersOfInheritedConstraints
	{
		get
		{
			yield return new object[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter1.FromString("19f936b7-7375-4a5a-9cad-53740dd17e57"), "\"19f936b7-7375-4a5a-9cad-53740dd17e57\"" };
		}
	}

	public static IEnumerable<object[]> SimpleValueObjectsCollections
	{
		get
		{
			yield return new object[] {
				new[]
				{
					NonEmptyGuid.FromString("45d9d454-87fa-4d87-ace3-3ee7789216b1"),
					NonEmptyGuid.FromString("5640b0a7-adc1-490d-a9b9-792d54ff2a18")
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
					{ 1, NonEmptyGuid.FromString("45d9d454-87fa-4d87-ace3-3ee7789216b1") },
					{ 2, NonEmptyGuid.FromString("5640b0a7-adc1-490d-a9b9-792d54ff2a18") }
				},
				"{\"1\":\"45d9d454-87fa-4d87-ace3-3ee7789216b1\",\"2\":\"5640b0a7-adc1-490d-a9b9-792d54ff2a18\"}" };

			yield return new object[] {
				new Dictionary<NonEmptyGuid, int>
				{
					{ NonEmptyGuid.FromString("45d9d454-87fa-4d87-ace3-3ee7789216b1"), 1 },
					{ NonEmptyGuid.FromString("5640b0a7-adc1-490d-a9b9-792d54ff2a18"), 2 }
				},
				"{\"45d9d454-87fa-4d87-ace3-3ee7789216b1\":1,\"5640b0a7-adc1-490d-a9b9-792d54ff2a18\":2}" };
		}
	}

	public static IEnumerable<object[]> ObjectsFeaturingSimpleValueObjectsAsProperties
	{
		get
		{
			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
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
					new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
					new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("22155dc8-8ab2-4668-8450-5327195268b8")),
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
					{ 1, new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("e03205b5-e9ea-4788-b41a-8f2dce13398c")) },
					{ 2, new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("22155dc8-8ab2-4668-8450-5327195268b8")) },
				},
				"{\"1\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"},\"2\":{\"FirstProperty\":\"bar\",\"SecondProperty\":\"22155dc8-8ab2-4668-8450-5327195268b8\"}}" };

			yield return new object[] {
				new Dictionary<ClassArchetype.PositionalRecordContainingANonEmptyGuid, int>
				{
					{ new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("e03205b5-e9ea-4788-b41a-8f2dce13398c")), 1 },
					{ new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("22155dc8-8ab2-4668-8450-5327195268b8")), 2 },
				},
				"{\"{\\\"FirstProperty\\\":\\\"foo\\\",\\\"SecondProperty\\\":\\\"e03205b5-e9ea-4788-b41a-8f2dce13398c\\\"}\":1,\"{\\\"FirstProperty\\\":\\\"bar\\\",\\\"SecondProperty\\\":\\\"22155dc8-8ab2-4668-8450-5327195268b8\\\"}\":2}" };
		}
	}

	public static IEnumerable<object[]> ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty
	{
		get
		{
			yield return new object[] {
				ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
					new[] { "3e8c722a-e7c9-4f47-ac04-37b153f436d4", "b2520961-2504-4bce-84e3-f971436751c5" },
					new[]
					{
						new[] { "08715282-86b4-44db-ba4b-e654117e3ba9", "70dd566e-ee61-43db-bb2d-3e6621694057" },
						new[] { "2661c3f7-73d9-42d6-9dd3-7a43f23daf21", "2a336afe-bd50-4bb3-a3fc-db5edb747440" },
					}),
				string.Join("", new[]
				{
					"{\"_nonEmptyGuids\":[\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\",\"b2520961-2504-4bce-84e3-f971436751c5\"]",
					",\"_nonEmptyGuidss\":[[\"08715282-86b4-44db-ba4b-e654117e3ba9\",\"70dd566e-ee61-43db-bb2d-3e6621694057\"],[\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\",\"2a336afe-bd50-4bb3-a3fc-db5edb747440\"]]}"
				}) };
		}
	}

	public static IEnumerable<object[]> ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyCollections
	{
		get
		{
			yield return new object[] {
				new[]
				{
					ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
					new[] { "3e8c722a-e7c9-4f47-ac04-37b153f436d4", "b2520961-2504-4bce-84e3-f971436751c5" },
					new[]
					{
						new[] { "08715282-86b4-44db-ba4b-e654117e3ba9", "70dd566e-ee61-43db-bb2d-3e6621694057" },
						new[] { "2661c3f7-73d9-42d6-9dd3-7a43f23daf21", "2a336afe-bd50-4bb3-a3fc-db5edb747440" },
					}),
					ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
					new[] { "38a3078a-529c-482c-86e2-c55c24912c43", "636091f6-0e7b-48a8-bdf8-48cafa69eb97" },
					new[]
					{
						new[] { "056c8a63-a80e-4fe4-a548-3eb766fd6f1e", "b7b20f44-c064-48f3-899e-5d2d4061b2e8" },
						new[] { "c13b9d61-31fd-407a-9563-1c010c4af718", "728109ee-c0eb-4f76-95a6-c1ad6ece1c14" },
					})
				},
				string.Join("", new[]
				{
				"[",
					"{",
						"\"_nonEmptyGuids\":[\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\",\"b2520961-2504-4bce-84e3-f971436751c5\"],",
						"\"_nonEmptyGuidss\":[[\"08715282-86b4-44db-ba4b-e654117e3ba9\",\"70dd566e-ee61-43db-bb2d-3e6621694057\"],[\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\",\"2a336afe-bd50-4bb3-a3fc-db5edb747440\"]]",
					"},",
					"{",
						"\"_nonEmptyGuids\":[\"38a3078a-529c-482c-86e2-c55c24912c43\",\"636091f6-0e7b-48a8-bdf8-48cafa69eb97\"],",
						"\"_nonEmptyGuidss\":[[\"056c8a63-a80e-4fe4-a548-3eb766fd6f1e\",\"b7b20f44-c064-48f3-899e-5d2d4061b2e8\"],[\"c13b9d61-31fd-407a-9563-1c010c4af718\",\"728109ee-c0eb-4f76-95a6-c1ad6ece1c14\"]]",
					"}",
				"]"
				}) };
		}
	}

	public static IEnumerable<object[]> ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyDictionaries
	{
		get
		{
			yield return new object[] {
				new Dictionary<int, ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects>
				{
					{
						1,
						ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
						new[] { "3e8c722a-e7c9-4f47-ac04-37b153f436d4", "b2520961-2504-4bce-84e3-f971436751c5" },
						new[]
						{
							new[] { "08715282-86b4-44db-ba4b-e654117e3ba9", "70dd566e-ee61-43db-bb2d-3e6621694057" },
							new[] { "2661c3f7-73d9-42d6-9dd3-7a43f23daf21", "2a336afe-bd50-4bb3-a3fc-db5edb747440" },
						})
					},
					{
						2,
						ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
						new[] { "38a3078a-529c-482c-86e2-c55c24912c43", "636091f6-0e7b-48a8-bdf8-48cafa69eb97" },
						new[]
						{
							new[] { "056c8a63-a80e-4fe4-a548-3eb766fd6f1e", "b7b20f44-c064-48f3-899e-5d2d4061b2e8" },
							new[] { "c13b9d61-31fd-407a-9563-1c010c4af718", "728109ee-c0eb-4f76-95a6-c1ad6ece1c14" },
						})
					}
				},
				string.Join("", new[]
				{
				"{",
						"\"1\":",
					"{",
						"\"_nonEmptyGuids\":[\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\",\"b2520961-2504-4bce-84e3-f971436751c5\"],",
						"\"_nonEmptyGuidss\":[[\"08715282-86b4-44db-ba4b-e654117e3ba9\",\"70dd566e-ee61-43db-bb2d-3e6621694057\"],[\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\",\"2a336afe-bd50-4bb3-a3fc-db5edb747440\"]]",
					"},",
					"\"2\":",
					"{",
						"\"_nonEmptyGuids\":[\"38a3078a-529c-482c-86e2-c55c24912c43\",\"636091f6-0e7b-48a8-bdf8-48cafa69eb97\"],",
						"\"_nonEmptyGuidss\":[[\"056c8a63-a80e-4fe4-a548-3eb766fd6f1e\",\"b7b20f44-c064-48f3-899e-5d2d4061b2e8\"],[\"c13b9d61-31fd-407a-9563-1c010c4af718\",\"728109ee-c0eb-4f76-95a6-c1ad6ece1c14\"]]",
					"}",
				"}"
				}) };
			yield return new object[] {
				new Dictionary<ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects, int>
				{
					{
						ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
						new[] { "3e8c722a-e7c9-4f47-ac04-37b153f436d4", "b2520961-2504-4bce-84e3-f971436751c5" },
						new[]
						{
							new[] { "08715282-86b4-44db-ba4b-e654117e3ba9", "70dd566e-ee61-43db-bb2d-3e6621694057" },
							new[] { "2661c3f7-73d9-42d6-9dd3-7a43f23daf21", "2a336afe-bd50-4bb3-a3fc-db5edb747440" },
						}),
						1
					},
					{
						ClassArchetype.ValueObjectFeaturingNonEmptyGuidCollects.From(
						new[] { "38a3078a-529c-482c-86e2-c55c24912c43", "636091f6-0e7b-48a8-bdf8-48cafa69eb97" },
						new[]
						{
							new[] { "056c8a63-a80e-4fe4-a548-3eb766fd6f1e", "b7b20f44-c064-48f3-899e-5d2d4061b2e8" },
							new[] { "c13b9d61-31fd-407a-9563-1c010c4af718", "728109ee-c0eb-4f76-95a6-c1ad6ece1c14" },
						}),
						2
					}
				},
				string.Join("", new[]
				{
				"{",
					"\"{",
						"\\\"_nonEmptyGuids\\\":[\\\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\\\",\\\"b2520961-2504-4bce-84e3-f971436751c5\\\"],",
						"\\\"_nonEmptyGuidss\\\":[[\\\"08715282-86b4-44db-ba4b-e654117e3ba9\\\",\\\"70dd566e-ee61-43db-bb2d-3e6621694057\\\"],[\\\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\\\",\\\"2a336afe-bd50-4bb3-a3fc-db5edb747440\\\"]]",
					"}\":",
						"1,",
					"\"{",
						"\\\"_nonEmptyGuids\\\":[\\\"38a3078a-529c-482c-86e2-c55c24912c43\\\",\\\"636091f6-0e7b-48a8-bdf8-48cafa69eb97\\\"],",
						"\\\"_nonEmptyGuidss\\\":[[\\\"056c8a63-a80e-4fe4-a548-3eb766fd6f1e\\\",\\\"b7b20f44-c064-48f3-899e-5d2d4061b2e8\\\"],[\\\"c13b9d61-31fd-407a-9563-1c010c4af718\\\",\\\"728109ee-c0eb-4f76-95a6-c1ad6ece1c14\\\"]]",
					"}\":",
					"2",
				"}"
				}) };
		}
	}

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
					NonEmptyGuid.FromString("e03205b5-e9ea-4788-b41a-8f2dce13398c"));

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

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(
					StringArchetype.Foo,
					ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("203205b5-e9ea-4788-b41a-8f2dce13398c"));
			yield return new object[] {
				positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":\"203205b5-e9ea-4788-b41a-8f2dce13398c\"}" };

			var positionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(
				StringArchetype.Bar,
				positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2);
			yield return new object[] {
				positionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2,
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingANonEmptyGuidStartingWithTheCharacter2\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"203205b5-e9ea-4788-b41a-8f2dce13398c\"}}" };

			var positionalRecordContainingPositionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAPositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(
				StringArchetype.Baz,
				positionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2);
			yield return new object[] {
				positionalRecordContainingPositionalRecordContainingPositionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter2,
				"{\"FirstProperty\":\"baz\",\"SecondProperty\":{\"FirstProperty\":\"bar\",\"SecondPropertyContainingANonEmptyGuidStartingWithTheCharacter2\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"203205b5-e9ea-4788-b41a-8f2dce13398c\"}}}" };
		}
	}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingANonEmptyGuidStartingWithTheCharacter3 = new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(
					StringArchetype.Foo,
					ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("303205b5-e9ea-4788-b41a-8f2dce13398c"));
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
						NonEmptyGuid.FromString("57cba1ed-add2-4d30-9ac3-2e941af888cd"),
						NonEmptyGuid.FromString("f20f1385-2679-49b1-8e64-94183d4a6f13"),
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
						NonEmptyGuid.FromString("67cba1ed-add2-4d30-9ac3-2e941af888cd"),
						NonEmptyGuid.FromString("f30f1385-2679-49b1-8e64-94183d4a6f13"),
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
						NonEmptyGuid.FromString("77cba1ed-add2-4d30-9ac3-2e941af888cd"),
						NonEmptyGuid.FromString("f40f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"77cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f40f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Quux, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids),
				"{\"FirstProperty\":\"quux\",\"SecondPropertyContainingAnIEnumerableOfNonEmptyGuids\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"77cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f40f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };
		}
	}
	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(
					StringArchetype.Foo,
					new[]
					{
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("27cba1ed-add2-4d30-9ac3-2e941af888cd"),
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("220f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"27cba1ed-add2-4d30-9ac3-2e941af888cd\",\"220f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Bar, positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"27cba1ed-add2-4d30-9ac3-2e941af888cd\",\"220f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };



			var positionalRecordDirectlyContainingAListOfNonEmptyGuidsStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(
					StringArchetype.Baz,
					new List<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>
					{
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("28cba1ed-add2-4d30-9ac3-2e941af888cd"),
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("240f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAListOfNonEmptyGuidsStartingWithTheCharacter2,
				"{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"28cba1ed-add2-4d30-9ac3-2e941af888cd\",\"240f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Foobar, positionalRecordDirectlyContainingAListOfNonEmptyGuidsStartingWithTheCharacter2),
				"{\"FirstProperty\":\"foobar\",\"SecondPropertyContainingAListOfNonEmptyGuidsStartingWithTheCharacter2\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"28cba1ed-add2-4d30-9ac3-2e941af888cd\",\"240f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };



			var positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2 = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(
					StringArchetype.Qux,
					new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>
					{
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("28dba1ed-add2-4d30-9ac3-2e941af888cd"),
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("250f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"28dba1ed-add2-4d30-9ac3-2e941af888cd\",\"250f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Quux, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2),
				"{\"FirstProperty\":\"quux\",\"SecondPropertyContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"28dba1ed-add2-4d30-9ac3-2e941af888cd\",\"250f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };
		}
	}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass2
	{
		get
		{
			var positionalRecordDirectlyContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3 = new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(
					StringArchetype.Foo,
					new[]
					{
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("37cba1ed-add2-4d30-9ac3-2e941af888cd"),
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("320f1385-2679-49b1-8e64-94183d4a6f13"),
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
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("38cba1ed-add2-4d30-9ac3-2e941af888cd"),
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("340f1385-2679-49b1-8e64-94183d4a6f13"),
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
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("38dba1ed-add2-4d30-9ac3-2e941af888cd"),
						ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("350f1385-2679-49b1-8e64-94183d4a6f13"),
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
						{  StringArchetype.Foo, NonEmptyGuid.FromString("7b3c6a10-b042-4d54-bdc9-e418082d7677") },
						{  StringArchetype.Bar, NonEmptyGuid.FromString("7c3c6a10-b042-4d54-bdc9-e418082d7677") },
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
						{  NonEmptyGuid.FromString("36dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Baz },
						{  NonEmptyGuid.FromString("46dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Foobar },
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
						{  NonEmptyGuid.FromString("d0b01da5-8ec1-4d20-8145-8be0ce451a4e"), NonEmptyGuid.FromString("e0b01da5-8ec1-4d20-8145-8be0ce451a4e") },
						{  NonEmptyGuid.FromString("d1b01da5-8ec1-4d20-8145-8be0ce451a4e"), NonEmptyGuid.FromString("e1b01da5-8ec1-4d20-8145-8be0ce451a4e") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeysAndValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"d0b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e0b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"d1b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e1b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidKeysAndValues(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeysAndValues),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidKeysAndValues\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"d0b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e0b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"d1b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e1b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}}" };
		}
	}
	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values(
					StringArchetype.Foo,
					new Dictionary<string, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>
					{
						{  StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2b3c6a10-b042-4d54-bdc9-e418082d7677") },
						{  StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2c3c6a10-b042-4d54-bdc9-e418082d7677") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"2b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"2c3c6a10-b042-4d54-bdc9-e418082d7677\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Values\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"foo\":\"2b3c6a10-b042-4d54-bdc9-e418082d7677\",\"bar\":\"2c3c6a10-b042-4d54-bdc9-e418082d7677\"}}}" };

			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys(
					StringArchetype.Foobar,
					new Dictionary<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2, string>
					{
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("26dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Baz },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("27dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Foobar },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys,
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"26dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"27dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys(StringArchetype.Baz, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys),
				"{\"FirstProperty\":\"baz\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2Keys\":{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"26dbea44-f2e7-4877-8842-6afc7f664da9\":\"baz\",\"27dbea44-f2e7-4877-8842-6afc7f664da9\":\"foobar\"}}}" };

			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues(
					StringArchetype.Foo,
					new Dictionary<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>
					{
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("20b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("21b01da5-8ec1-4d20-8145-8be0ce451a4e") },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("21b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("22b01da5-8ec1-4d20-8145-8be0ce451a4e") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"20b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"21b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"21b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"22b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter2KeysAndValues\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"20b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"21b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"21b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"22b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}}" };
		}
	}
	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values = new ClassArchetype.PositionalRecordContainingADictionaryWithNonEmptyGuidStartingWithTheCharacter3Values(
					StringArchetype.Foo,
					new Dictionary<string, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>
					{
						{  StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3b3c6a10-b042-4d54-bdc9-e418082d7677") },
						{  StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3c3c6a10-b042-4d54-bdc9-e418082d7677") },
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
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("36dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Baz },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("37dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Foobar },
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
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("30b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("31b01da5-8ec1-4d20-8145-8be0ce451a4e") },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("31b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("32b01da5-8ec1-4d20-8145-8be0ce451a4e") },
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
						{  StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3b3c6a10-b042-4d54-bdc9-e418082d7677") },
						{  StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3c3c6a10-b042-4d54-bdc9-e418082d7677") },
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
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("36dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Baz },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("37dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Foobar },
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
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("30b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("31b01da5-8ec1-4d20-8145-8be0ce451a4e") },
						{  ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("31b01da5-8ec1-4d20-8145-8be0ce451a4e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("32b01da5-8ec1-4d20-8145-8be0ce451a4e") },
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
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("15199727-a673-4f67-a221-91500ded8618")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Baz, NonEmptyGuid.FromString("3caa6d26-fa7b-4892-b6b0-b254d2453780"))
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
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Baz, NonEmptyGuid.FromString("bfcc8a2c-f8af-4441-ba59-bdd57592aeb7")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foobar, NonEmptyGuid.FromString("17d76e6b-227b-4a97-aa02-78c0ddb4e69c"))
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
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Quux, NonEmptyGuid.FromString("cbccbf7d-6650-4524-b945-e3a84c0a20f8")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("731ae06d-0f46-4250-b528-31ca0a3357da")),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"cbccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"731ae06d-0f46-4250-b528-31ca0a3357da\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuid(StringArchetype.Bar, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids),
				"{\"FirstProperty\":\"bar\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"cbccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"731ae06d-0f46-4250-b528-31ca0a3357da\"}]}}" };
		}
	}
	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2s = new ClassArchetype.PositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(
					StringArchetype.Foo,
					new[]
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("25199727-a673-4f67-a221-91500ded8618")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Baz, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2caa6d26-fa7b-4892-b6b0-b254d2453780"))
					});
			yield return new object[] {
				positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2s,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":\"25199727-a673-4f67-a221-91500ded8618\"},{\"FirstProperty\":\"baz\",\"SecondProperty\":\"2caa6d26-fa7b-4892-b6b0-b254d2453780\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Foobar, positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2s),
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":\"25199727-a673-4f67-a221-91500ded8618\"},{\"FirstProperty\":\"baz\",\"SecondProperty\":\"2caa6d26-fa7b-4892-b6b0-b254d2453780\"}]}}" };



			var positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuidStartingWithTheCharacter2s = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(
					StringArchetype.Bar,
					new List<ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2>
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Baz, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2fcc8a2c-f8af-4441-ba59-bdd57592aeb7")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Foobar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("27d76e6b-227b-4a97-aa02-78c0ddb4e69c"))
					});
			yield return new object[] {
				positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuidStartingWithTheCharacter2s,
				"{\"FirstProperty\":\"bar\",\"SecondProperty\":[{\"FirstProperty\":\"baz\",\"SecondProperty\":\"2fcc8a2c-f8af-4441-ba59-bdd57592aeb7\"},{\"FirstProperty\":\"foobar\",\"SecondProperty\":\"27d76e6b-227b-4a97-aa02-78c0ddb4e69c\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Qux, positionalRecordContainingPositionalRecordDirectlyContainingAListOfNonEmptyGuidStartingWithTheCharacter2s),
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":{\"FirstProperty\":\"bar\",\"SecondProperty\":[{\"FirstProperty\":\"baz\",\"SecondProperty\":\"2fcc8a2c-f8af-4441-ba59-bdd57592aeb7\"},{\"FirstProperty\":\"foobar\",\"SecondProperty\":\"27d76e6b-227b-4a97-aa02-78c0ddb4e69c\"}]}}" };


			var positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2s = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(
					StringArchetype.Qux,
					new HashSet<ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2>
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Quux, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2bccbf7d-6650-4524-b945-e3a84c0a20f8")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("231ae06d-0f46-4250-b528-31ca0a3357da")),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2s,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"2bccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"231ae06d-0f46-4250-b528-31ca0a3357da\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter2(StringArchetype.Bar, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2s),
				"{\"FirstProperty\":\"bar\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"2bccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"231ae06d-0f46-4250-b528-31ca0a3357da\"}]}}" };
		}
	}
	public static IEnumerable<object> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordContainingPositionalRecordDirectlyContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3s = new ClassArchetype.PositionalRecordContainingAnArrayOfPositionalRecordsContainingANonEmptyGuidStartingWithTheCharacter3(
					StringArchetype.Foo,
					new[]
					{
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Bar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("35199727-a673-4f67-a221-91500ded8618")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Baz, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3caa6d26-fa7b-4892-b6b0-b254d2453780"))
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
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Baz, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3fcc8a2c-f8af-4441-ba59-bdd57592aeb7")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Foobar, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("37d76e6b-227b-4a97-aa02-78c0ddb4e69c"))
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
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Quux, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3bccbf7d-6650-4524-b945-e3a84c0a20f8")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuidStartingWithTheCharacter3(StringArchetype.Foo, ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("331ae06d-0f46-4250-b528-31ca0a3357da")),
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.FromString("fffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.FromString("ba75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.FromString("ff8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.FromString("336aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.FromString("0ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.FromString("ca75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.FromString("0f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.FromString("436aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.FromString("1ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.FromString("da75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.FromString("1f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.FromString("536aecda-25b0-45c3-91d5-39af24adc71e") })
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
			var positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Foo,
				new[]
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.FromString("2a75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.FromString("236aecda-25b0-45c3-91d5-39af24adc71e") })
				});
			yield return new object[] {
				positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"2ffd1abd-2713-4af3-a990-93ef802ec1af\",\"2a75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"2f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"236aecda-25b0-45c3-91d5-39af24adc71e\"]}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(StringArchetype.Foobar, positionalRecordContainingAnArrayOfPositionalRecordsContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"foobar\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[{\"FirstProperty\":\"bar\",\"SecondProperty\":[\"2ffd1abd-2713-4af3-a990-93ef802ec1af\",\"2a75bf97-e78c-4bf5-af2b-cdc8cbf28341\"]},{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"2f8c5ac3-3ffe-455f-adfa-1cd37676ef80\",\"236aecda-25b0-45c3-91d5-39af24adc71e\"]}]}}" };



			var positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Qux,
				new List<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.FromString("0ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.FromString("ca75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.FromString("0f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.FromString("436aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.FromString("1ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.FromString("da75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.FromString("1f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.FromString("536aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.FromString("3a75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.FromString("336aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.FromString("0ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.FromString("ca75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.FromString("0f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.FromString("436aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.FromString("1ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.FromString("da75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.FromString("1f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.FromString("536aecda-25b0-45c3-91d5-39af24adc71e") })
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
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.FromString("084d2aee-99dd-4494-a702-ec7a098e8a88"), NonEmptyGuid.FromString("1bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Qux, new[] { NonEmptyGuid.FromString("e6895f58-2535-4d52-90eb-4c572e80c3c6"), NonEmptyGuid.FromString("cc739b95-3614-4d3d-898d-a8c15c29370e") }) },
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
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Baz, new() { NonEmptyGuid.FromString("084d2aee-99dd-4494-a702-ec7a098e8a88"), NonEmptyGuid.FromString("1bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Qux, new() { NonEmptyGuid.FromString("e6895f58-2535-4d52-90eb-4c572e80c3c6"), NonEmptyGuid.FromString("cc739b95-3614-4d3d-898d-a8c15c29370e") }) },
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
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Baz, new HashSet<NonEmptyGuid>() { NonEmptyGuid.FromString("feffc8ae-d14f-4fca-9f9a-c9d001478d3d"), NonEmptyGuid.FromString("e1046b1f-53a6-4a4e-9be9-cc7718b6ca93") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Qux, new HashSet<NonEmptyGuid>() { NonEmptyGuid.FromString("dd13b555-9a4e-4c8d-b64b-cfd00a7a897c"), NonEmptyGuid.FromString("3d1f7bb3-f06e-4181-9f26-f0635e3d3983") }) },
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
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.FromString("04b0ae6c-2b98-41f5-b2e8-16a158037157"), NonEmptyGuid.FromString("3d925b20-9951-4bae-b65c-8a01e8d3e493") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Qux, new[] { NonEmptyGuid.FromString("65276338-a7fd-4647-8262-df8c4f6de190"), NonEmptyGuid.FromString("ac963cd5-7b84-417a-87e4-a907ca895642") }), StringArchetype.Bar },
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
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Baz, new() { NonEmptyGuid.FromString("90efae4f-20c4-47cd-93d5-8acd846a937c"), NonEmptyGuid.FromString("b4c22756-abf5-4dc1-ab43-896d06e01d1a") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Qux, new() { NonEmptyGuid.FromString("72415979-9256-418d-bead-3103b19dcb6b"), NonEmptyGuid.FromString("123b2fb8-d90e-4ff5-ab96-704c05ad06bd") }), StringArchetype.Bar },
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
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Baz, new HashSet<NonEmptyGuid>() { NonEmptyGuid.FromString("6fd6010c-98df-454d-be6b-14e21f76610e"), NonEmptyGuid.FromString("49371516-633f-4564-a5dc-e0da5524c7d2") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Qux, new HashSet<NonEmptyGuid>() { NonEmptyGuid.FromString("605a273c-8426-4c45-a7bf-de797259ee17"), NonEmptyGuid.FromString("c10e386f-53c8-4b25-81d0-875b2e1d0cf0") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"6fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"49371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"605a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"c10e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"6fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"49371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"605a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"c10e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}}" };
		}
	}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("284d2aee-99dd-4494-a702-ec7a098e8a88"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("26895f58-2535-4d52-90eb-4c572e80c3c6"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2c739b95-3614-4d3d-898d-a8c15c29370e") }) },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"284d2aee-99dd-4494-a702-ec7a098e8a88\",\"2bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"26895f58-2535-4d52-90eb-4c572e80c3c6\",\"2c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Values),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"284d2aee-99dd-4494-a702-ec7a098e8a88\",\"2bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"26895f58-2535-4d52-90eb-4c572e80c3c6\",\"2c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}}" };

			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("284d2aee-99dd-4494-a702-ec7a098e8a88"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("26895f58-2535-4d52-90eb-4c572e80c3c6"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2c739b95-3614-4d3d-898d-a8c15c29370e") }) },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"284d2aee-99dd-4494-a702-ec7a098e8a88\",\"2bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"26895f58-2535-4d52-90eb-4c572e80c3c6\",\"2c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Values),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"284d2aee-99dd-4494-a702-ec7a098e8a88\",\"2bb19a21-1067-4e1e-a7f2-86127c3a12c1\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"26895f58-2535-4d52-90eb-4c572e80c3c6\",\"2c739b95-3614-4d3d-898d-a8c15c29370e\"]}}}}" };


			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2Values(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2effc8ae-d14f-4fca-9f9a-c9d001478d3d"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("21046b1f-53a6-4a4e-9be9-cc7718b6ca93") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2d13b555-9a4e-4c8d-b64b-cfd00a7a897c"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2d1f7bb3-f06e-4181-9f26-f0635e3d3983") }) },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"2effc8ae-d14f-4fca-9f9a-c9d001478d3d\",\"21046b1f-53a6-4a4e-9be9-cc7718b6ca93\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"2d13b555-9a4e-4c8d-b64b-cfd00a7a897c\",\"2d1f7bb3-f06e-4181-9f26-f0635e3d3983\"]}}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2Values(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidValues),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"bar\":{\"FirstProperty\":\"baz\",\"SecondProperty\":[\"2effc8ae-d14f-4fca-9f9a-c9d001478d3d\",\"21046b1f-53a6-4a4e-9be9-cc7718b6ca93\"]},\"foobar\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"2d13b555-9a4e-4c8d-b64b-cfd00a7a897c\",\"2d1f7bb3-f06e-4181-9f26-f0635e3d3983\"]}}}}" };

			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Keys(
				StringArchetype.Foo,
				new Dictionary<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2, string>
				{
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("24b0ae6c-2b98-41f5-b2e8-16a158037157"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2d925b20-9951-4bae-b65c-8a01e8d3e493") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("25276338-a7fd-4647-8262-df8c4f6de190"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2c963cd5-7b84-417a-87e4-a907ca895642") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"24b0ae6c-2b98-41f5-b2e8-16a158037157\\\",\\\"2d925b20-9951-4bae-b65c-8a01e8d3e493\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"25276338-a7fd-4647-8262-df8c4f6de190\\\",\\\"2c963cd5-7b84-417a-87e4-a907ca895642\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter2Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"24b0ae6c-2b98-41f5-b2e8-16a158037157\\\",\\\"2d925b20-9951-4bae-b65c-8a01e8d3e493\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"25276338-a7fd-4647-8262-df8c4f6de190\\\",\\\"2c963cd5-7b84-417a-87e4-a907ca895642\\\"]}\":\"bar\"}}}" };



			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Keys(
				StringArchetype.Foo,
				new Dictionary<ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2, string>
				{
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("20efae4f-20c4-47cd-93d5-8acd846a937c"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("24c22756-abf5-4dc1-ab43-896d06e01d1a") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("22415979-9256-418d-bead-3103b19dcb6b"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("223b2fb8-d90e-4ff5-ab96-704c05ad06bd") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"20efae4f-20c4-47cd-93d5-8acd846a937c\\\",\\\"24c22756-abf5-4dc1-ab43-896d06e01d1a\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"22415979-9256-418d-bead-3103b19dcb6b\\\",\\\"223b2fb8-d90e-4ff5-ab96-704c05ad06bd\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidStartingWithTheCharacter2Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"20efae4f-20c4-47cd-93d5-8acd846a937c\\\",\\\"24c22756-abf5-4dc1-ab43-896d06e01d1a\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"22415979-9256-418d-bead-3103b19dcb6b\\\",\\\"223b2fb8-d90e-4ff5-ab96-704c05ad06bd\\\"]}\":\"bar\"}}}" };



			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2Keys(
				StringArchetype.Foo,
				new Dictionary<ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2, string>
				{
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Baz, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("2fd6010c-98df-454d-be6b-14e21f76610e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("29371516-633f-4564-a5dc-e0da5524c7d2") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter2(StringArchetype.Qux, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter2>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("205a273c-8426-4c45-a7bf-de797259ee17"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("210e386f-53c8-4b25-81d0-875b2e1d0cf0") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"2fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"29371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"205a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"210e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter2Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"2fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"29371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"205a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"210e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}}" };
		}
	}

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass
	{
		get
		{
			var positionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3Values = new ClassArchetype.PositionalRecordContainingADictionaryWithPositionalRecordsContainingAnArrayOfNonEmptyGuidStartingWithTheCharacter3Values(
				StringArchetype.Foo,
				new Dictionary<string, ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3>
				{
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("384d2aee-99dd-4494-a702-ec7a098e8a88"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("36895f58-2535-4d52-90eb-4c572e80c3c6"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3c739b95-3614-4d3d-898d-a8c15c29370e") }) },
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
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("384d2aee-99dd-4494-a702-ec7a098e8a88"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("36895f58-2535-4d52-90eb-4c572e80c3c6"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3c739b95-3614-4d3d-898d-a8c15c29370e") }) },
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
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3effc8ae-d14f-4fca-9f9a-c9d001478d3d"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("31046b1f-53a6-4a4e-9be9-cc7718b6ca93") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3d13b555-9a4e-4c8d-b64b-cfd00a7a897c"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3d1f7bb3-f06e-4181-9f26-f0635e3d3983") }) },
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
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("34b0ae6c-2b98-41f5-b2e8-16a158037157"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3d925b20-9951-4bae-b65c-8a01e8d3e493") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new[] { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("35276338-a7fd-4647-8262-df8c4f6de190"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3c963cd5-7b84-417a-87e4-a907ca895642") }), StringArchetype.Bar },
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
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("30efae4f-20c4-47cd-93d5-8acd846a937c"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("34c22756-abf5-4dc1-ab43-896d06e01d1a") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("32415979-9256-418d-bead-3103b19dcb6b"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("323b2fb8-d90e-4ff5-ab96-704c05ad06bd") }), StringArchetype.Bar },
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
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Baz, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("3fd6010c-98df-454d-be6b-14e21f76610e"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("39371516-633f-4564-a5dc-e0da5524c7d2") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuidsStartingWithTheCharacter3(StringArchetype.Qux, new HashSet<ClassArchetype.NonEmptyGuidStartingWithTheCharacter3>() { ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("305a273c-8426-4c45-a7bf-de797259ee17"), ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("310e386f-53c8-4b25-81d0-875b2e1d0cf0") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"3fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"39371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"305a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"310e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidStartingWithTheCharacter3Keys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"3fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"39371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"305a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"310e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}}" };
		}
	}
}
