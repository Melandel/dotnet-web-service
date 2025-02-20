using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.TestCases;

class ObjectTextualRenderingTestCases : IEnumerable
{
	public IEnumerator GetEnumerator()
	{
		foreach (var kvp in AllCases)
		{
			yield return new object[] { kvp.Key, kvp.Value };
		}
	}

	static IEnumerable<KeyValuePair<object, string>> AllCases
	=> EdgeCases
		.Concat(String_LowerCaseEnglishAlphabeticLetters)
		.Concat(String_UpperCaseEnglishAlphabeticLetters)
		.Concat(String_LowerCaseLettersWithAccent)
		.Concat(String_UpperCaseLettersWithAccent)
		.Concat(String_Numbers)
		.Concat(String_Punctuation)
		.Concat(Integer_PositiveNumbers)
		.Concat(Integer_NegativeNumbers)
		.Concat(DateTimes)
		.Concat(Guids)
		.Concat(NativeSimpleTypesCollections)
		.Concat(SimpleValueObjects)
		.Concat(SimpleValueObjectsCollections)
		.Concat(ObjectsFeaturingSimpleValueObjectsAsProperties)
		.Concat(ObjectsFeaturingSimpleValueObjectsAsPropertiesCollections)
		.Concat(ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty)
		.Concat(ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyCollections)
		.Concat(FirstClassCollectionsWithoutPublicProperties)
		.Concat(FirstClassCollectionsWithoutPublicPropertiesCollections)
		.Concat(FirstClassCollectionsWithPublicProperties)
		.Concat(FirstClassCollectionsWithPublicPropertiesCollections);

	static IEnumerable<KeyValuePair<object, string>> EdgeCases
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(null, "null");
		}
	}
	static IEnumerable<KeyValuePair<object, string>> String_LowerCaseEnglishAlphabeticLetters
	{
		get
		{
			yield return KeyValuePair.Create<object, string>("a", "\"a\"");
			yield return KeyValuePair.Create<object, string>("b", "\"b\"");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> String_UpperCaseEnglishAlphabeticLetters
	{
		get
		{
			yield return KeyValuePair.Create<object, string>("A", "\"A\"");
			yield return KeyValuePair.Create<object, string>("B", "\"B\"");
		}
	}
	static IEnumerable<KeyValuePair<object, string>> String_LowerCaseLettersWithAccent
	{
		get
		{
			yield return KeyValuePair.Create<object, string>("é", "\"é\"");
			yield return KeyValuePair.Create<object, string>("ö", "\"ö\"");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> String_UpperCaseLettersWithAccent
	{
		get
		{
			yield return KeyValuePair.Create<object, string>("É", "\"É\"");
			yield return KeyValuePair.Create<object, string>("Ö", "\"Ö\"");
		}
	}


	static IEnumerable<KeyValuePair<object, string>> String_Numbers
	{
		get
		{
			yield return KeyValuePair.Create<object, string>("1", "\"1\"");
			yield return KeyValuePair.Create<object, string>("2", "\"2\"");
		}
	}


	static IEnumerable<KeyValuePair<object, string>> String_Punctuation
	{
		get
		{
			yield return KeyValuePair.Create<object, string>("<", "\"<\"");
			yield return KeyValuePair.Create<object, string>("&", "\"&\"");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> Integer_PositiveNumbers
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(0,  "0");
			yield return KeyValuePair.Create<object, string>(3,  "3");
			yield return KeyValuePair.Create<object, string>(13, "13");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> Integer_NegativeNumbers
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(-2,  "-2");
			yield return KeyValuePair.Create<object, string>(-10, "-10");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> DateTimes
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(DateTime.MinValue,                               "\"0001-01-01T00:00:00\"");
			yield return KeyValuePair.Create<object, string>(System.Data.SqlTypes.SqlDateTime.MinValue.Value, "\"1753-01-01T00:00:00\"");
			yield return KeyValuePair.Create<object, string>(new DateTime(2025, 01, 02),                      "\"2025-01-02T00:00:00\"");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> Guids
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(Guid.Empty,                                         "\"00000000-0000-0000-0000-000000000000\"");
			yield return KeyValuePair.Create<object, string>(Guid.Parse("ec6c1eda-a418-4534-9aa1-525784840ae2"), "\"ec6c1eda-a418-4534-9aa1-525784840ae2\"");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> NativeSimpleTypesCollections
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(Array.Empty<object>(),   "[]");
			yield return KeyValuePair.Create<object, string>(new[] { 1, 2, 3 },       "[1,2,3]");
			yield return KeyValuePair.Create<object, string>(new[] { "1", "2", "3" }, "[\"1\",\"2\",\"3\"]");
			yield return KeyValuePair.Create<object, string>(
				new[] { new DateTime(2025, 01, 02), new DateTime(2025, 02, 03) },
				"[\"2025-01-02T00:00:00\",\"2025-02-03T00:00:00\"]");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> SimpleValueObjects
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(NonEmptyGuid.FromString("09f936b7-7375-4a5a-9cad-53740dd17e57"), "\"09f936b7-7375-4a5a-9cad-53740dd17e57\"");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> SimpleValueObjectsCollections
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(
				new[]
				{
					NonEmptyGuid.FromString("45d9d454-87fa-4d87-ace3-3ee7789216b1"),
					NonEmptyGuid.FromString("5640b0a7-adc1-490d-a9b9-792d54ff2a18")
				},
				"[\"45d9d454-87fa-4d87-ace3-3ee7789216b1\",\"5640b0a7-adc1-490d-a9b9-792d54ff2a18\"]");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> ObjectsFeaturingSimpleValueObjectsAsProperties
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(
				new SimplePositionalRecord(
					StringArchetype.Foo,
					NonEmptyGuid.FromString("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
				"{\"firstProperty\":\"foo\",\"secondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"}");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> ObjectsFeaturingSimpleValueObjectsAsPropertiesCollections
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(
				new[]
				{
					new SimplePositionalRecord(
						StringArchetype.Foo,
						NonEmptyGuid.FromString("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
					new SimplePositionalRecord(
						StringArchetype.Bar,
						NonEmptyGuid.FromString("22155dc8-8ab2-4668-8450-5327195268b8")),
				},
				"[{\"firstProperty\":\"foo\",\"secondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"},{\"firstProperty\":\"bar\",\"secondProperty\":\"22155dc8-8ab2-4668-8450-5327195268b8\"}]");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(
				ValueObjectFeaturingNonEmptyGuidCollects.From(
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
				}));
		}
	}

	static IEnumerable<KeyValuePair<object, string>> ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyCollections
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(
				new[]
				{
					ValueObjectFeaturingNonEmptyGuidCollects.From(
					new[] { "3e8c722a-e7c9-4f47-ac04-37b153f436d4", "b2520961-2504-4bce-84e3-f971436751c5" },
					new[]
					{
						new[] { "08715282-86b4-44db-ba4b-e654117e3ba9", "70dd566e-ee61-43db-bb2d-3e6621694057" },
						new[] { "2661c3f7-73d9-42d6-9dd3-7a43f23daf21", "2a336afe-bd50-4bb3-a3fc-db5edb747440" },
					}),
					ValueObjectFeaturingNonEmptyGuidCollects.From(
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
				}));
		}
	}

	static IEnumerable<KeyValuePair<object, string>> FirstClassCollectionsWithoutPublicProperties
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(
				NonEmptyGuids.FromStrings(
					"6537d663-4a04-419f-b1f5-524a87e6a228",
					"f2e543a5-51e6-49bc-ba90-b3f21027d5a2"),
				"[\"6537d663-4a04-419f-b1f5-524a87e6a228\",\"f2e543a5-51e6-49bc-ba90-b3f21027d5a2\"]");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> FirstClassCollectionsWithoutPublicPropertiesCollections
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(
				new[]
				{
					NonEmptyGuids.FromStrings("af433453-1770-44c6-a2bf-d3933c38780e", "25a16d4c-6489-45b1-aed8-e08784646b54"),
					NonEmptyGuids.FromStrings("41302a2f-c586-4fb5-a4f2-89ee6f0a1380", "987ca9f7-4e36-468e-adbc-412bc664b2f9"),
				},
				"[[\"af433453-1770-44c6-a2bf-d3933c38780e\",\"25a16d4c-6489-45b1-aed8-e08784646b54\"],[\"41302a2f-c586-4fb5-a4f2-89ee6f0a1380\",\"987ca9f7-4e36-468e-adbc-412bc664b2f9\"]]");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> FirstClassCollectionsWithPublicProperties
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(
				NonEmptyGuidsWithPublicProperty.FromStrings(
					"6537d663-4a04-419f-b1f5-524a87e6a228",
					"f2e543a5-51e6-49bc-ba90-b3f21027d5a2"),
				"{\"_encapsulated_count\":2,\"_encapsulated\":[\"6537d663-4a04-419f-b1f5-524a87e6a228\",\"f2e543a5-51e6-49bc-ba90-b3f21027d5a2\"],\"PublicProperty\":2}");
		}
	}

	static IEnumerable<KeyValuePair<object, string>> FirstClassCollectionsWithPublicPropertiesCollections
	{
		get
		{
			yield return KeyValuePair.Create<object, string>(
				new[]
				{
					NonEmptyGuidsWithPublicProperty.FromStrings("6537d663-4a04-419f-b1f5-524a87e6a228", "f2e543a5-51e6-49bc-ba90-b3f21027d5a2"),
					NonEmptyGuidsWithPublicProperty.FromStrings("47e09057-8b02-45f1-b6b4-c756e473084d", "d04427f6-4e59-4d2c-b7db-36a1bdc8bb16")
				},
				"[{\"_encapsulated_count\":2,\"_encapsulated\":[\"6537d663-4a04-419f-b1f5-524a87e6a228\",\"f2e543a5-51e6-49bc-ba90-b3f21027d5a2\"],\"PublicProperty\":2},{\"_encapsulated_count\":2,\"_encapsulated\":[\"47e09057-8b02-45f1-b6b4-c756e473084d\",\"d04427f6-4e59-4d2c-b7db-36a1bdc8bb16\"],\"PublicProperty\":2}]");
		}
	}

	record SimplePositionalRecord(string FirstProperty, NonEmptyGuid SecondProperty);
	class NonEmptyGuids
	{
		readonly NonEmptyGuid[] _encapsulated;
		NonEmptyGuids(NonEmptyGuid[] encapsulated)
		{
			_encapsulated = encapsulated;
		}

		public static NonEmptyGuids FromStrings(params string[] strings)
		{
			try
			{
				return new(strings.Select(str => NonEmptyGuid.FromGuid(Guid.Parse(str))).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichWithInformationAbout<NonEmptyGuids>(strings);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuids>(developerMistake, strings);
			}
		}
	}

	class ValueObjectFeaturingNonEmptyGuidCollects
	{
		readonly NonEmptyGuids _nonEmptyGuids;
		readonly NonEmptyGuids[] _nonEmptyGuidss;
		ValueObjectFeaturingNonEmptyGuidCollects(NonEmptyGuids nonEmptyGuids, NonEmptyGuids[] nonEmptyGuidss)
		{
			_nonEmptyGuids = nonEmptyGuids;
			_nonEmptyGuidss = nonEmptyGuidss;
		}

		public static ValueObjectFeaturingNonEmptyGuidCollects From(string[] nonEmptyGuids, string[][] multipleSetsOfNonEmptyGuids)
		{
			try
			{
				return new(
					NonEmptyGuids.FromStrings(nonEmptyGuids),
					multipleSetsOfNonEmptyGuids.Select(array => NonEmptyGuids.FromStrings(array)).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichWithInformationAbout<ValueObjectFeaturingNonEmptyGuidCollects>(nonEmptyGuids, multipleSetsOfNonEmptyGuids);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<ValueObjectFeaturingNonEmptyGuidCollects>(developerMistake, nonEmptyGuids, multipleSetsOfNonEmptyGuids);
			}
		}
	}

	class NonEmptyGuidsWithPublicProperty
	{
		readonly NonEmptyGuid[] _encapsulated;
		public int PublicProperty => _encapsulated.Length;
		NonEmptyGuidsWithPublicProperty(NonEmptyGuid[] encapsulated)
		{
			_encapsulated = encapsulated;
		}

		public static NonEmptyGuidsWithPublicProperty FromStrings(params string[] strings)
		{
			try
			{
				return new(strings.Select(guid => NonEmptyGuid.FromGuid(Guid.Parse(guid))).ToArray());;
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichWithInformationAbout<NonEmptyGuidsWithPublicProperty>(strings);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuidsWithPublicProperty>(developerMistake, strings);
			}
		}
	}
}
