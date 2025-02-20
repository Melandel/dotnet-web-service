using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;

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

	public static IEnumerable<object[]> ObjectsFeaturingSimpleValueObjectsAsProperties
	{
		get
		{
			yield return new object[] {
				new ClassArchetype.SimplePositionalRecord(StringArchetype.Foo, NonEmptyGuid.FromString("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
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
					new ClassArchetype.SimplePositionalRecord(StringArchetype.Foo, NonEmptyGuid.FromString("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
					new ClassArchetype.SimplePositionalRecord(StringArchetype.Bar, NonEmptyGuid.FromString("22155dc8-8ab2-4668-8450-5327195268b8")),
				},
				"[{\"FirstProperty\":\"foo\",\"SecondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"},{\"FirstProperty\":\"bar\",\"SecondProperty\":\"22155dc8-8ab2-4668-8450-5327195268b8\"}]" };
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
					"{\"_nonEmptyGuids_count\":2,\"_nonEmptyGuids\":[\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\",\"b2520961-2504-4bce-84e3-f971436751c5\"]",
					",\"_nonEmptyGuidss_count\":2,\"_nonEmptyGuidss\":[[\"08715282-86b4-44db-ba4b-e654117e3ba9\",\"70dd566e-ee61-43db-bb2d-3e6621694057\"],[\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\",\"2a336afe-bd50-4bb3-a3fc-db5edb747440\"]]}"
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
						"\"_nonEmptyGuids_count\":2,\"_nonEmptyGuids\":[\"3e8c722a-e7c9-4f47-ac04-37b153f436d4\",\"b2520961-2504-4bce-84e3-f971436751c5\"],",
						"\"_nonEmptyGuidss_count\":2,\"_nonEmptyGuidss\":[[\"08715282-86b4-44db-ba4b-e654117e3ba9\",\"70dd566e-ee61-43db-bb2d-3e6621694057\"],[\"2661c3f7-73d9-42d6-9dd3-7a43f23daf21\",\"2a336afe-bd50-4bb3-a3fc-db5edb747440\"]]",
					"},",
					"{",
						"\"_nonEmptyGuids_count\":2,\"_nonEmptyGuids\":[\"38a3078a-529c-482c-86e2-c55c24912c43\",\"636091f6-0e7b-48a8-bdf8-48cafa69eb97\"],",
						"\"_nonEmptyGuidss_count\":2,\"_nonEmptyGuidss\":[[\"056c8a63-a80e-4fe4-a548-3eb766fd6f1e\",\"b7b20f44-c064-48f3-899e-5d2d4061b2e8\"],[\"c13b9d61-31fd-407a-9563-1c010c4af718\",\"728109ee-c0eb-4f76-95a6-c1ad6ece1c14\"]]",
					"}",
				"]"
				}) };
		}
	}

	public static IEnumerable<object[]> FirstClassCollectionsWithoutPublicProperties
	{
		get
		{
			yield return new object[] {
				ClassArchetype.NonEmptyGuids.FromStrings(
					"6537d663-4a04-419f-b1f5-524a87e6a228",
					"f2e543a5-51e6-49bc-ba90-b3f21027d5a2"),
				"[\"6537d663-4a04-419f-b1f5-524a87e6a228\",\"f2e543a5-51e6-49bc-ba90-b3f21027d5a2\"]" };
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

	public static IEnumerable<object[]> FirstClassCollectionsWithPublicProperties
	{
		get
		{
			yield return new object[] {
				ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings(
					"6537d663-4a04-419f-b1f5-524a87e6a228",
					"f2e543a5-51e6-49bc-ba90-b3f21027d5a2"),
				"{\"Value_count\":2,\"Value\":[\"6537d663-4a04-419f-b1f5-524a87e6a228\",\"f2e543a5-51e6-49bc-ba90-b3f21027d5a2\"],\"PublicProperty\":2}" };
		}
	}

	public static IEnumerable<object[]> FirstClassCollectionsWithPublicPropertiesCollections
	{
		get
		{
			yield return new object[] {
				new[]
				{
					ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings("6537d663-4a04-419f-b1f5-524a87e6a228", "f2e543a5-51e6-49bc-ba90-b3f21027d5a2"),
					ClassArchetype.NonEmptyGuidsWithPublicProperty.FromStrings("47e09057-8b02-45f1-b6b4-c756e473084d", "d04427f6-4e59-4d2c-b7db-36a1bdc8bb16")
				},
				"[{\"Value_count\":2,\"Value\":[\"6537d663-4a04-419f-b1f5-524a87e6a228\",\"f2e543a5-51e6-49bc-ba90-b3f21027d5a2\"],\"PublicProperty\":2},{\"Value_count\":2,\"Value\":[\"47e09057-8b02-45f1-b6b4-c756e473084d\",\"d04427f6-4e59-4d2c-b7db-36a1bdc8bb16\"],\"PublicProperty\":2}]" };
		}
	}
}
