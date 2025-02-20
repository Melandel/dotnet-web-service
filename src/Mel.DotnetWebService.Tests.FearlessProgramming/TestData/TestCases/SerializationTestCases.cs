using System.Collections;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;
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

	public class That_Involve_Constrained_Types : IEnumerable
	{
		public IEnumerator GetEnumerator()
		{
			foreach (var constrainedType in Types.AllConcreteConstrainedTypes)
			{
				if (ConstrainedTypeInfos.TryGet(constrainedType, out var constrainedTypeInfo))
				{
					if (constrainedTypeInfo.RootType == typeof(byte))
					{
						// 👇 Contstrained types built around the "byte" type are not covered by (de)serialization tests
						// Justification: byte[] is conventionally used as a binary format, hence serialized into string, and not into a collection of numbers
						//   It is a special case, where constrained types can behave as a collection of numbers (and be serialized as a collection of numbers)
						//   Despite the root type (byte[]) behaves as a binary format (and serializes as a base64 string)
						continue;
						// TODO examples dictionary, Make.SomeValue<Collection|Dictionary>
					}

					var exampleValue = constrainedTypeInfo.ValidValueExamples[0];
					var example = constrainedTypeInfo.InvokeStaticFactoryMethod(exampleValue);
					foreach (var testCase in GenerateDataStructuresInvolving(new[] { example }, DataStructureScope.Unlimited))
					{
						yield return new object[] { testCase.StringRepresentation, testCase.DeserializationType };
					}
				}
			}
		}
	}
	public static IEnumerable<object[]> NativeSimpleTypesCollections
	{
		get
		{
			yield return new object[] { Array.Empty<object>(), "[]" };
			yield return new object[] { new[] { 1, 2, 3 }, "[1,2,3]" };
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
			yield return new object[] { new Dictionary<string, int>(), "{}" };
			yield return new object[] { new Dictionary<string, int>() { { "1", 1 }, { "2", 2 }, { "3", 3 } }, "{\"1\":1,\"2\":2,\"3\":3}" };

			yield return new object[] { new Dictionary<int, string>(), "{}" };
			yield return new object[] { new Dictionary<int, string>() { { 1, "1" }, { 2, "2" }, { 3, "3" } }, "{\"1\":\"1\",\"2\":\"2\",\"3\":\"3\"}" };

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
			yield return new object[] { NonEmptyGuid.ApplyConstraintsTo("09f936b7-7375-4a5a-9cad-53740dd17e57"), "\"09f936b7-7375-4a5a-9cad-53740dd17e57\"" };
		}
	}

	public static IEnumerable<object[]> SimpleValueObjectsCollections
	{
		get
		{
			yield return new object[] {
				new[]
				{
					NonEmptyGuid.ApplyConstraintsTo("45d9d454-87fa-4d87-ace3-3ee7789216b1"),
					NonEmptyGuid.ApplyConstraintsTo("5640b0a7-adc1-490d-a9b9-792d54ff2a18")
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
					{ 1, NonEmptyGuid.ApplyConstraintsTo("45d9d454-87fa-4d87-ace3-3ee7789216b1") },
					{ 2, NonEmptyGuid.ApplyConstraintsTo("5640b0a7-adc1-490d-a9b9-792d54ff2a18") }
				},
				"{\"1\":\"45d9d454-87fa-4d87-ace3-3ee7789216b1\",\"2\":\"5640b0a7-adc1-490d-a9b9-792d54ff2a18\"}" };

			yield return new object[] {
				new Dictionary<NonEmptyGuid, int>
				{
					{ NonEmptyGuid.ApplyConstraintsTo("45d9d454-87fa-4d87-ace3-3ee7789216b1"), 1 },
					{ NonEmptyGuid.ApplyConstraintsTo("5640b0a7-adc1-490d-a9b9-792d54ff2a18"), 2 }
				},
				"{\"45d9d454-87fa-4d87-ace3-3ee7789216b1\":1,\"5640b0a7-adc1-490d-a9b9-792d54ff2a18\":2}" };
		}
	}

	public static IEnumerable<object[]> ObjectsFeaturingSimpleValueObjectsAsProperties
	{
		get
		{
			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.ApplyConstraintsTo("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
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
					new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.ApplyConstraintsTo("e03205b5-e9ea-4788-b41a-8f2dce13398c")),
					new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.ApplyConstraintsTo("22155dc8-8ab2-4668-8450-5327195268b8")),
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
					{ 1, new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.ApplyConstraintsTo("e03205b5-e9ea-4788-b41a-8f2dce13398c")) },
					{ 2, new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.ApplyConstraintsTo("22155dc8-8ab2-4668-8450-5327195268b8")) },
				},
				"{\"1\":{\"FirstProperty\":\"foo\",\"SecondProperty\":\"e03205b5-e9ea-4788-b41a-8f2dce13398c\"},\"2\":{\"FirstProperty\":\"bar\",\"SecondProperty\":\"22155dc8-8ab2-4668-8450-5327195268b8\"}}" };

			yield return new object[] {
				new Dictionary<ClassArchetype.PositionalRecordContainingANonEmptyGuid, int>
				{
					{ new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.ApplyConstraintsTo("e03205b5-e9ea-4788-b41a-8f2dce13398c")), 1 },
					{ new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.ApplyConstraintsTo("22155dc8-8ab2-4668-8450-5327195268b8")), 2 },
				},
				"{\"{\\\"FirstProperty\\\":\\\"foo\\\",\\\"SecondProperty\\\":\\\"e03205b5-e9ea-4788-b41a-8f2dce13398c\\\"}\":1,\"{\\\"FirstProperty\\\":\\\"bar\\\",\\\"SecondProperty\\\":\\\"22155dc8-8ab2-4668-8450-5327195268b8\\\"}\":2}" };
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
					NonEmptyGuid.ApplyConstraintsTo("e03205b5-e9ea-4788-b41a-8f2dce13398c"));

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

	public static IEnumerable<object[]> Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass
	{
		get
		{
			var positionalRecordDirectlyContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(
					StringArchetype.Foo,
					new[]
					{
						NonEmptyGuid.ApplyConstraintsTo("57cba1ed-add2-4d30-9ac3-2e941af888cd"),
						NonEmptyGuid.ApplyConstraintsTo("f20f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnArrayOfNonEmptyGuids,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"57cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f20f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, positionalRecordDirectlyContainingAnArrayOfNonEmptyGuids),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingAnArrayOfNonEmptyGuids\":{\"FirstProperty\":\"foo\",\"SecondProperty\":[\"57cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f20f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };



			var positionalRecordDirectlyContainingAListOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(
					StringArchetype.Baz,
					new List<NonEmptyGuid>
					{
						NonEmptyGuid.ApplyConstraintsTo("67cba1ed-add2-4d30-9ac3-2e941af888cd"),
						NonEmptyGuid.ApplyConstraintsTo("f30f1385-2679-49b1-8e64-94183d4a6f13"),
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
						NonEmptyGuid.ApplyConstraintsTo("77cba1ed-add2-4d30-9ac3-2e941af888cd"),
						NonEmptyGuid.ApplyConstraintsTo("f40f1385-2679-49b1-8e64-94183d4a6f13"),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"77cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f40f1385-2679-49b1-8e64-94183d4a6f13\"]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Quux, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids),
				"{\"FirstProperty\":\"quux\",\"SecondPropertyContainingAnIEnumerableOfNonEmptyGuids\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[\"77cba1ed-add2-4d30-9ac3-2e941af888cd\",\"f40f1385-2679-49b1-8e64-94183d4a6f13\"]}}" };
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
						{  StringArchetype.Foo, NonEmptyGuid.ApplyConstraintsTo("7b3c6a10-b042-4d54-bdc9-e418082d7677") },
						{  StringArchetype.Bar, NonEmptyGuid.ApplyConstraintsTo("7c3c6a10-b042-4d54-bdc9-e418082d7677") },
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
						{  NonEmptyGuid.ApplyConstraintsTo("36dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Baz },
						{  NonEmptyGuid.ApplyConstraintsTo("46dbea44-f2e7-4877-8842-6afc7f664da9"), StringArchetype.Foobar },
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
						{  NonEmptyGuid.ApplyConstraintsTo("d0b01da5-8ec1-4d20-8145-8be0ce451a4e"), NonEmptyGuid.ApplyConstraintsTo("e0b01da5-8ec1-4d20-8145-8be0ce451a4e") },
						{  NonEmptyGuid.ApplyConstraintsTo("d1b01da5-8ec1-4d20-8145-8be0ce451a4e"), NonEmptyGuid.ApplyConstraintsTo("e1b01da5-8ec1-4d20-8145-8be0ce451a4e") },
					});
			yield return new object[] {
				positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeysAndValues,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"d0b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e0b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"d1b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e1b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingADictionaryWithNonEmptyGuidKeysAndValues(StringArchetype.Bar, positionalRecordDirectlyContainingADictionaryWithNonEmptyGuidKeysAndValues),
				"{\"FirstProperty\":\"bar\",\"SecondPropertyContainingADictionaryWithNonEmptyGuidKeysAndValues\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"d0b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e0b01da5-8ec1-4d20-8145-8be0ce451a4e\",\"d1b01da5-8ec1-4d20-8145-8be0ce451a4e\":\"e1b01da5-8ec1-4d20-8145-8be0ce451a4e\"}}}" };
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
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.ApplyConstraintsTo("15199727-a673-4f67-a221-91500ded8618")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Baz, NonEmptyGuid.ApplyConstraintsTo("3caa6d26-fa7b-4892-b6b0-b254d2453780"))
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
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Baz, NonEmptyGuid.ApplyConstraintsTo("bfcc8a2c-f8af-4441-ba59-bdd57592aeb7")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foobar, NonEmptyGuid.ApplyConstraintsTo("17d76e6b-227b-4a97-aa02-78c0ddb4e69c"))
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
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Quux, NonEmptyGuid.ApplyConstraintsTo("cbccbf7d-6650-4524-b945-e3a84c0a20f8")),
						new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.ApplyConstraintsTo("731ae06d-0f46-4250-b528-31ca0a3357da")),
					});
			yield return new object[] {
				positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids,
				"{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"cbccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"731ae06d-0f46-4250-b528-31ca0a3357da\"}]}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnIEnumerableOfPositionalRecordsContainingANonEmptyGuid(StringArchetype.Bar, positionalRecordDirectlyContainingAnIEnumerableOfNonEmptyGuids),
				"{\"FirstProperty\":\"bar\",\"SecondProperty\":{\"FirstProperty\":\"qux\",\"SecondProperty\":[{\"FirstProperty\":\"quux\",\"SecondProperty\":\"cbccbf7d-6650-4524-b945-e3a84c0a20f8\"},{\"FirstProperty\":\"foo\",\"SecondProperty\":\"731ae06d-0f46-4250-b528-31ca0a3357da\"}]}}" };
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.ApplyConstraintsTo("fffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.ApplyConstraintsTo("ba75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.ApplyConstraintsTo("ff8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.ApplyConstraintsTo("336aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.ApplyConstraintsTo("0ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.ApplyConstraintsTo("ca75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.ApplyConstraintsTo("0f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.ApplyConstraintsTo("436aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.ApplyConstraintsTo("1ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.ApplyConstraintsTo("da75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.ApplyConstraintsTo("1f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.ApplyConstraintsTo("536aecda-25b0-45c3-91d5-39af24adc71e") })
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
			var positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Qux,
				new List<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.ApplyConstraintsTo("0ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.ApplyConstraintsTo("ca75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.ApplyConstraintsTo("0f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.ApplyConstraintsTo("436aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.ApplyConstraintsTo("1ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.ApplyConstraintsTo("da75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.ApplyConstraintsTo("1f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.ApplyConstraintsTo("536aecda-25b0-45c3-91d5-39af24adc71e") })
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
			var positionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids = new ClassArchetype.PositionalRecordContainingAListOfPositionalRecordsContainingAnArrayOfNonEmptyGuids(
				StringArchetype.Qux,
				new List<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>
				{
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.ApplyConstraintsTo("0ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.ApplyConstraintsTo("ca75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.ApplyConstraintsTo("0f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.ApplyConstraintsTo("436aecda-25b0-45c3-91d5-39af24adc71e") })
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
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Bar, new[] { NonEmptyGuid.ApplyConstraintsTo("1ffd1abd-2713-4af3-a990-93ef802ec1af"), NonEmptyGuid.ApplyConstraintsTo("da75bf97-e78c-4bf5-af2b-cdc8cbf28341") }),
					new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.ApplyConstraintsTo("1f8c5ac3-3ffe-455f-adfa-1cd37676ef80"), NonEmptyGuid.ApplyConstraintsTo("536aecda-25b0-45c3-91d5-39af24adc71e") })
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
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.ApplyConstraintsTo("084d2aee-99dd-4494-a702-ec7a098e8a88"), NonEmptyGuid.ApplyConstraintsTo("1bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Qux, new[] { NonEmptyGuid.ApplyConstraintsTo("e6895f58-2535-4d52-90eb-4c572e80c3c6"), NonEmptyGuid.ApplyConstraintsTo("cc739b95-3614-4d3d-898d-a8c15c29370e") }) },
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
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Baz, new() { NonEmptyGuid.ApplyConstraintsTo("084d2aee-99dd-4494-a702-ec7a098e8a88"), NonEmptyGuid.ApplyConstraintsTo("1bb19a21-1067-4e1e-a7f2-86127c3a12c1") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Qux, new() { NonEmptyGuid.ApplyConstraintsTo("e6895f58-2535-4d52-90eb-4c572e80c3c6"), NonEmptyGuid.ApplyConstraintsTo("cc739b95-3614-4d3d-898d-a8c15c29370e") }) },
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
					{ StringArchetype.Bar,    new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Baz, new HashSet<NonEmptyGuid>() { NonEmptyGuid.ApplyConstraintsTo("feffc8ae-d14f-4fca-9f9a-c9d001478d3d"), NonEmptyGuid.ApplyConstraintsTo("e1046b1f-53a6-4a4e-9be9-cc7718b6ca93") }) },
					{ StringArchetype.Foobar, new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Qux, new HashSet<NonEmptyGuid>() { NonEmptyGuid.ApplyConstraintsTo("dd13b555-9a4e-4c8d-b64b-cfd00a7a897c"), NonEmptyGuid.ApplyConstraintsTo("3d1f7bb3-f06e-4181-9f26-f0635e3d3983") }) },
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
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Baz, new[] { NonEmptyGuid.ApplyConstraintsTo("04b0ae6c-2b98-41f5-b2e8-16a158037157"), NonEmptyGuid.ApplyConstraintsTo("3d925b20-9951-4bae-b65c-8a01e8d3e493") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(StringArchetype.Qux, new[] { NonEmptyGuid.ApplyConstraintsTo("65276338-a7fd-4647-8262-df8c4f6de190"), NonEmptyGuid.ApplyConstraintsTo("ac963cd5-7b84-417a-87e4-a907ca895642") }), StringArchetype.Bar },
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
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Baz, new() { NonEmptyGuid.ApplyConstraintsTo("90efae4f-20c4-47cd-93d5-8acd846a937c"), NonEmptyGuid.ApplyConstraintsTo("b4c22756-abf5-4dc1-ab43-896d06e01d1a") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(StringArchetype.Qux, new() { NonEmptyGuid.ApplyConstraintsTo("72415979-9256-418d-bead-3103b19dcb6b"), NonEmptyGuid.ApplyConstraintsTo("123b2fb8-d90e-4ff5-ab96-704c05ad06bd") }), StringArchetype.Bar },
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
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Baz, new HashSet<NonEmptyGuid>() { NonEmptyGuid.ApplyConstraintsTo("6fd6010c-98df-454d-be6b-14e21f76610e"), NonEmptyGuid.ApplyConstraintsTo("49371516-633f-4564-a5dc-e0da5524c7d2") }), StringArchetype.Foo },
					{ new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(StringArchetype.Qux, new HashSet<NonEmptyGuid>() { NonEmptyGuid.ApplyConstraintsTo("605a273c-8426-4c45-a7bf-de797259ee17"), NonEmptyGuid.ApplyConstraintsTo("c10e386f-53c8-4b25-81d0-875b2e1d0cf0") }), StringArchetype.Bar },
				});
			yield return new object[] {
				positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys,
				"{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"6fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"49371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"605a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"c10e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}" };

			yield return new object[] {
				new ClassArchetype.PositionalRecordContainingPositionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys(StringArchetype.Quux, positionalRecordContainingADictionaryWithPositionalRecordsContainingAnIEnumerableOfNonEmptyGuidKeys),
				"{\"FirstProperty\":\"quux\",\"SecondProperty\":{\"FirstProperty\":\"foo\",\"SecondProperty\":{\"{\\\"FirstProperty\\\":\\\"baz\\\",\\\"SecondProperty\\\":[\\\"6fd6010c-98df-454d-be6b-14e21f76610e\\\",\\\"49371516-633f-4564-a5dc-e0da5524c7d2\\\"]}\":\"foo\",\"{\\\"FirstProperty\\\":\\\"qux\\\",\\\"SecondProperty\\\":[\\\"605a273c-8426-4c45-a7bf-de797259ee17\\\",\\\"c10e386f-53c8-4b25-81d0-875b2e1d0cf0\\\"]}\":\"bar\"}}}" };
		}
	}

	public enum DataStructureScope
	{
		TechnicalDefaultEnumValue = 0,
		SupportedByDefaultJsonConverter = 1,
		Unlimited = 2
	}
	public static IEnumerable<TestCase> GenerateDataStructuresInvolving<T>(T[] values, DataStructureScope dataStructureScope)
	{
		foreach (var value in values)
		{
			foreach (var testCase in GenerateDataStructuresInvolving(value, dataStructureScope))
			{
				yield return testCase;
			}
		}
	}

	const string SerializedEmptyCollection = "[]";
	const string SerializedEmptyDictionary = "{}";
	public static IEnumerable<TestCase> GenerateDataStructuresInvolving<T>(T data, DataStructureScope dataStructureScope)
	{
		string dataAsJson = "";
		try
		{
			dataAsJson = data.GetType() switch
			{
				var t when t == typeof(ClassArchetype.NonEmptyGuids) && data is ClassArchetype.NonEmptyGuids nonEmptyGuids => JsonSerializer.Serialize(((NonEmptyGuid[])nonEmptyGuids).Select(x => (Guid)x).ToArray(), new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }),
				var t when t == typeof(ClassArchetype.NonEmptyGuidsWithPublicProperty) && data is ClassArchetype.NonEmptyGuidsWithPublicProperty nonEmptyGuidsWithPublicProperty => JsonSerializer.Serialize(((NonEmptyGuid[])nonEmptyGuidsWithPublicProperty).Select(x => (Guid)x).ToArray(), new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }),
				var t when ConstrainedTypeInfos.TryGet(t, out var constrainedTypeInfo) => JsonSerializer.Serialize(constrainedTypeInfo.InvokeImplicitConversionToFullyNativeType(data), constrainedTypeInfo.FullyNativeRootType, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping }),
				_ => JsonSerializer.Serialize(data, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping })
			};
		}
		catch
		{
			Console.WriteLine($"Failed creating dataAsJson for type {data.GetType().GetName()}");
		}

		var dataType = data switch
		{
			not null when typeof(T) == typeof(object) => data.GetType(),
			_ => typeof(T)
		};
		yield return new(dataAsJson, dataType);

		var TArray = /* T[] */ dataType.MakeArrayType();
		var TList = /* List<T> */ typeof(List<>).MakeGenericType(dataType);
		var TIEnumerable = /* IEnumerable<T> */ typeof(IEnumerable<>).MakeGenericType(dataType);
		var TNonEmptyArray = /* NonEmptyArray<T> */ typeof(NonEmptyArray<>).MakeGenericType(dataType);
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
			if (dataStructureScope == DataStructureScope.Unlimited && !serialized.Contains(SerializedEmptyCollection))
			{
				yield return new(serialized, TNonEmptyArray);
			}
		}

		var arrayOfTArrays = /* T[][] */                dataType.MakeArrayType().MakeArrayType();
		var arrayOfTLists = /* List<T>[] */            typeof(List<>).MakeGenericType(dataType).MakeArrayType();
		var arrayOfTIEnumerables = /* IEnumerable<T>[] */     typeof(IEnumerable<>).MakeGenericType(dataType).MakeArrayType();
		var arrayOfTNonEmptyArrays = /* NonEmptyArray<T>[] */ typeof(NonEmptyArray<>).MakeGenericType(dataType).MakeArrayType();
		var listOfTArrays = /* List<T[]> */                typeof(List<>).MakeGenericType(dataType.MakeArrayType());
		var listOfTLists = /* List<List<T>> */            typeof(List<>).MakeGenericType(typeof(List<>).MakeGenericType(dataType));
		var listOfTIEnumerables = /* List<IEnumerable<T>> */     typeof(List<>).MakeGenericType(typeof(IEnumerable<>).MakeGenericType(dataType));
		var listOfTNonEmptyArrays = /* List<NonEmptyArray<T>> */ typeof(List<>).MakeGenericType(typeof(NonEmptyArray<>).MakeGenericType(dataType));
		var iEnumerableOfTArrays = /* IEnumerable<T[]> */                typeof(IEnumerable<>).MakeGenericType(dataType.MakeArrayType());
		var iEnumerableOfTLists = /* IEnumerable<List<T>> */            typeof(IEnumerable<>).MakeGenericType(typeof(List<>).MakeGenericType(dataType));
		var iEnumerableOfTIEnumerables = /* IEnumerable<IEnumerable<T>> */     typeof(IEnumerable<>).MakeGenericType(typeof(IEnumerable<>).MakeGenericType(dataType));
		var iEnumerableOfTNonEmptyArrays = /* IEnumerable<NonEmptyArray<T>> */ typeof(IEnumerable<>).MakeGenericType(typeof(NonEmptyArray<>).MakeGenericType(dataType));
		var nonEmptyArrayOfTArrays = /* NonEmptyArray<T[]> */                typeof(NonEmptyArray<>).MakeGenericType(dataType.MakeArrayType());
		var nonEmptyArrayOfTLists = /* NonEmptyArray<List<T>> */            typeof(NonEmptyArray<>).MakeGenericType(typeof(List<>).MakeGenericType(dataType));
		var nonEmptyArrayOfTIEnumerables = /* NonEmptyArray<IEnumerable<T>> */     typeof(NonEmptyArray<>).MakeGenericType(typeof(IEnumerable<>).MakeGenericType(dataType));
		var nonEmptyArrayOfTNonEmptyArrays = /* NonEmptyArray<NonEmptyArray<T>> */ typeof(NonEmptyArray<>).MakeGenericType(typeof(NonEmptyArray<>).MakeGenericType(dataType));
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

			if (dataStructureScope == DataStructureScope.Unlimited && !serialized.Contains(SerializedEmptyCollection))
			{
				yield return new(serialized, arrayOfTNonEmptyArrays);
				yield return new(serialized, listOfTNonEmptyArrays);
				yield return new(serialized, iEnumerableOfTNonEmptyArrays);

				yield return new(serialized, nonEmptyArrayOfTNonEmptyArrays);

				yield return new(serialized, nonEmptyArrayOfTArrays);
				yield return new(serialized, nonEmptyArrayOfTLists);
				yield return new(serialized, nonEmptyArrayOfTIEnumerables);
			}
		}

		var dictionaryWithTValues =  /* Dictionary<int,T> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType);
		var idictionaryWithTValues =  /* IDictionary<int,T> */ typeof(IDictionary<,>).MakeGenericType(typeof(int), dataType);
		var nonEmptyDictionaryWithTValues =  /* NonEmptyDictionary<int,T> */ typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), dataType);
		foreach (var serialized in new[]
		{
			"{}",
			$"{{\"10\":{dataAsJson}}}",
			$"{{\"11\":{dataAsJson},\"12\":{dataAsJson}}}"
		})
		{
			yield return new(serialized, dictionaryWithTValues);
			yield return new(serialized, idictionaryWithTValues);
			if (dataStructureScope == DataStructureScope.Unlimited && !serialized.Contains(SerializedEmptyDictionary))
			{
				yield return new(serialized, nonEmptyDictionaryWithTValues);
			}
		}

		var dictionaryWithTArrayValues = /* Dictionary<int,T[]> */                typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType.MakeArrayType());
		var dictionaryWithTListValues = /* Dictionary<int,List<T>> */            typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(List<>).MakeGenericType(dataType));
		var dictionaryWithTIEnumerableValues = /* Dictionary<int,IEnumerable<T>> */     typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(IEnumerable<>).MakeGenericType(dataType));
		var dictionaryWithTNonEmptyArrayValues = /* Dictionary<int,NonEmptyArray<T>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyArray<>).MakeGenericType(dataType));
		var idictionaryWithTArrayValues = /* IDictionary<int,T[]> */                typeof(IDictionary<,>).MakeGenericType(typeof(int), dataType.MakeArrayType());
		var idictionaryWithTListValues = /* IDictionary<int,List<T>> */            typeof(IDictionary<,>).MakeGenericType(typeof(int), typeof(List<>).MakeGenericType(dataType));
		var idictionaryWithTIEnumerableValues = /* IDictionary<int,IEnumerable<T>> */     typeof(IDictionary<,>).MakeGenericType(typeof(int), typeof(IEnumerable<>).MakeGenericType(dataType));
		var idictionaryWithTNonEmptyArrayValues = /* IDictionary<int,NonEmptyArray<T>> */ typeof(IDictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyArray<>).MakeGenericType(dataType));
		var nonEmptyDictionaryWithTArrayValues = /* NonEmptyDictionary<int,T[]> */                typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), dataType.MakeArrayType());
		var nonEmptyDictionaryWithTListValues = /* NonEmptyDictionary<int,List<T>> */            typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), typeof(List<>).MakeGenericType(dataType));
		var nonEmptyDictionaryWithTIEnumerableValues = /* NonEmptyDictionary<int,IEnumerable<T>> */     typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), typeof(IEnumerable<>).MakeGenericType(dataType));
		var nonEmptyDictionaryWithTNonEmptyArrayValues = /* NonEmptyDictionary<int,NonEmptyArray<T>> */ typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyArray<>).MakeGenericType(dataType));
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
			yield return new(serialized, idictionaryWithTArrayValues);
			yield return new(serialized, idictionaryWithTListValues);
			yield return new(serialized, idictionaryWithTIEnumerableValues);
			if (dataStructureScope == DataStructureScope.Unlimited)
			{
				if (!serialized.Contains(SerializedEmptyCollection))
				{
					yield return new(serialized, dictionaryWithTNonEmptyArrayValues);
					yield return new(serialized, idictionaryWithTNonEmptyArrayValues);
				}

				if (serialized != SerializedEmptyDictionary)
				{
					yield return new(serialized, nonEmptyDictionaryWithTArrayValues);
					yield return new(serialized, nonEmptyDictionaryWithTListValues);
					yield return new(serialized, nonEmptyDictionaryWithTIEnumerableValues);
				}
			}
		}

		var dictionaryWithDictionaryWithTValuesValues = /* Dictionary<int,Dictionary<string, T>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(Dictionary<,>).MakeGenericType(typeof(string), dataType));
		var idictionaryWithDictionaryWithTValuesValues = /* IDictionary<int,Dictionary<string, T>> */ typeof(IDictionary<,>).MakeGenericType(typeof(int), typeof(Dictionary<,>).MakeGenericType(typeof(string), dataType));
		var nonEmptyDictionaryWithDictionaryWithTValuesValues = /* NonEmptyDictionary<int,Dictionary<string, T>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(Dictionary<,>).MakeGenericType(typeof(string), dataType));
		foreach (var serialized in new[]
		{
			"{}",
			"{\"30\":{}}",
			$"{{\"30\":{{\"foo\":{dataAsJson}}}}}",
			$"{{\"30\":{{\"foo\":{dataAsJson},\"bar:\":{dataAsJson}}}}}"
		})
		{
			yield return new(serialized, dictionaryWithDictionaryWithTValuesValues);
			yield return new(serialized, idictionaryWithDictionaryWithTValuesValues);

			if (serialized != SerializedEmptyDictionary)
			{
				yield return new(serialized, nonEmptyDictionaryWithDictionaryWithTValuesValues);
			}
		}

		var dictionaryWithArrayOfTArraysValues = /* Dictionary<int,T[][]> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType.MakeArrayType().MakeArrayType());
		var dictionaryWithNonEmptyArrayOfTNonEmptyArraysValues = /* Dictionary<int,NonEmptyArray<NonEmptyArray<T>>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyArray<>).MakeGenericType(typeof(NonEmptyArray<>).MakeGenericType(dataType)));
		var idictionaryWithArrayOfTArraysValues = /* IDictionary<int,T[][]> */ typeof(IDictionary<,>).MakeGenericType(typeof(int), dataType.MakeArrayType().MakeArrayType());
		var idictionaryWithNonEmptyArrayOfTNonEmptyArraysValues = /* IDictionary<int,NonEmptyArray<NonEmptyArray<T>>> */ typeof(IDictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyArray<>).MakeGenericType(typeof(NonEmptyArray<>).MakeGenericType(dataType)));
		var nonEmptyDictionaryWithArrayOfTArraysValues = /* NonEmptyDictionary<int,T[][]> */ typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), dataType.MakeArrayType().MakeArrayType());
		var nonEmptyDictionaryWithNonEmptyArrayOfTNonEmptyArraysValues = /* NonEmptyDictionary<int,NonEmptyArray<NonEmptyArray<T>>> */ typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyArray<>).MakeGenericType(typeof(NonEmptyArray<>).MakeGenericType(dataType)));
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
			yield return new(serialized, idictionaryWithArrayOfTArraysValues);
			if (dataStructureScope == DataStructureScope.Unlimited)
			{
				if (!serialized.Contains(SerializedEmptyCollection))
				{
					yield return new(serialized, dictionaryWithNonEmptyArrayOfTNonEmptyArraysValues);
					yield return new(serialized, idictionaryWithNonEmptyArrayOfTNonEmptyArraysValues);
				}

				if (serialized != SerializedEmptyDictionary)
				{
					yield return new(serialized, nonEmptyDictionaryWithArrayOfTArraysValues);

					if (!serialized.Contains(SerializedEmptyCollection))
					{
						yield return new(serialized, nonEmptyDictionaryWithNonEmptyArrayOfTNonEmptyArraysValues);
					}
				}
			}
		}

		var dataAsPropertyName = dataAsJson.Trim('"');
		if (dataAsPropertyName.Contains('"'))
		{
			dataAsPropertyName = dataAsPropertyName.Replace("\"", "\\\"");
		}
		if (!String.IsNullOrEmpty(dataAsPropertyName))
		{
			var dictionaryWithTKeys = /* Dictionary<T, int> */ typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int));
			var idictionaryWithTKeys = /* IDictionary<T, int> */ typeof(IDictionary<,>).MakeGenericType(dataType, typeof(int));
			var nonEmptyDictionaryWithTKeys = /* NonEmptyDictionary<T, int> */ typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, typeof(int));
			foreach (var serialized in new[]
			{
				"{}",
				$"{{\"{dataAsPropertyName}\":50}}",
			})
			{
				yield return new(serialized, dictionaryWithTKeys);
				yield return new(serialized, idictionaryWithTKeys);
				if (dataStructureScope == DataStructureScope.Unlimited)
				{
					if (serialized != SerializedEmptyDictionary)
					{
						yield return new(serialized, nonEmptyDictionaryWithTKeys);
					}
				}
			}

			var dictionaryWithTKeysAndValues = /* Dictionary<T, T> */ typeof(Dictionary<,>).MakeGenericType(dataType, dataType);
			var idictionaryWithTKeysAndValues = /* IDictionary<T, T> */ typeof(IDictionary<,>).MakeGenericType(dataType, dataType);
			var nonEmptyDictionaryWithTKeysAndValues = /* NonEmptyDictionary<T, T> */ typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, dataType);
			foreach (var serialized in new[]
			{
				"{}",
				$"{{\"{dataAsPropertyName}\":{dataAsJson}}}"
			})
			{
				yield return new(serialized, dictionaryWithTKeysAndValues);
				yield return new(serialized, idictionaryWithTKeysAndValues);
				if (dataStructureScope == DataStructureScope.Unlimited)
				{
					if (serialized != SerializedEmptyDictionary)
					{
						yield return new(serialized, nonEmptyDictionaryWithTKeysAndValues);
					}
				}
			}

			var dictionaryWithDictionaryWithTKeysKeys = /* Dictionary<int,Dictionary<T, string>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(Dictionary<,>).MakeGenericType(dataType, typeof(string)));
			var dictionaryWithIDictionaryWithTKeysKeys = /* Dictionary<int,IDictionary<T, string>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(IDictionary<,>).MakeGenericType(dataType, typeof(string)));
			var dictionaryWithNonEmptyDictionaryWithTKeysKeys = /* Dictionary<int,NonEmptyDictionary<T, string>> */ typeof(Dictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, typeof(string)));
			var idictionaryWithDictionaryWithTKeysKeys = /* Dictionary<int,Dictionary<T, string>> */ typeof(IDictionary<,>).MakeGenericType(typeof(int), typeof(Dictionary<,>).MakeGenericType(dataType, typeof(string)));
			var idictionaryWithIDictionaryWithTKeysKeys = /* Dictionary<int,IDictionary<T, string>> */ typeof(IDictionary<,>).MakeGenericType(typeof(int), typeof(IDictionary<,>).MakeGenericType(dataType, typeof(string)));
			var idictionaryWithNonEmptyDictionaryWithTKeysKeys = /* Dictionary<int,NonEmptyDictionary<T, string>> */ typeof(IDictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, typeof(string)));
			var nonEmptyDictionaryWithDictionaryWithTKeysKeys = /* Dictionary<int,Dictionary<T, string>> */ typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), typeof(Dictionary<,>).MakeGenericType(dataType, typeof(string)));
			var nonEmptyDictionaryWithIDictionaryWithTKeysKeys = /* Dictionary<int,IDictionary<T, string>> */ typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), typeof(IDictionary<,>).MakeGenericType(dataType, typeof(string)));
			var nonEmptyDictionaryWithNonEmptyDictionaryWithTKeysKeys = /* Dictionary<int,NonEmptyDictionary<T, string>> */ typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, typeof(string)));

			foreach (var serialized in new[]
			{
				"{}",
				"{\"60\":{}}",
				$"{{\"61\":{{\"{dataAsPropertyName}\":\"bar\"}}}}"
			})
			{
				yield return new(serialized, dictionaryWithDictionaryWithTKeysKeys);
				yield return new(serialized, dictionaryWithIDictionaryWithTKeysKeys);
				yield return new(serialized, idictionaryWithIDictionaryWithTKeysKeys);
				yield return new(serialized, idictionaryWithIDictionaryWithTKeysKeys);

				if (dataStructureScope == DataStructureScope.Unlimited)
				{
					if (!serialized.Contains(SerializedEmptyDictionary))
					{
						yield return new(serialized, nonEmptyDictionaryWithDictionaryWithTKeysKeys);
						yield return new(serialized, nonEmptyDictionaryWithIDictionaryWithTKeysKeys);
						yield return new(serialized, dictionaryWithNonEmptyDictionaryWithTKeysKeys);
						if (!serialized.Contains(SerializedEmptyCollection))
						{
							yield return new(serialized, idictionaryWithNonEmptyDictionaryWithTKeysKeys);
							yield return new(serialized, nonEmptyDictionaryWithNonEmptyDictionaryWithTKeysKeys);
						}
					}
				}
			}
		}

		var arrayOfDictionariesWithTValues = /* Dictionary<int,T>[] */                typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType).MakeArrayType();
		var listOfDictionariesWithTValues = /* List<Dictionary<int,T>> */            typeof(List<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType));
		var iEnumerableOfDictionariesWithTValues = /* IEnumerable<Dictionary<int,T>> */     typeof(IEnumerable<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType));
		var nonEmptyArrayOfDictionariesWithTValues = /* NonEmptyArray<Dictionary<int,T>> */ typeof(NonEmptyArray<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(typeof(int), dataType));
		var arrayOfIDictionariesWithTValues = /* IDictionary<int,T>[] */                typeof(IDictionary<,>).MakeGenericType(typeof(int), dataType).MakeArrayType();
		var listOfIDictionariesWithTValues = /* List<IDictionary<int,T>> */            typeof(List<>).MakeGenericType(typeof(IDictionary<,>).MakeGenericType(typeof(int), dataType));
		var iEnumerableOfIDictionariesWithTValues = /* IEnumerable<IDictionary<int,T>> */     typeof(IEnumerable<>).MakeGenericType(typeof(IDictionary<,>).MakeGenericType(typeof(int), dataType));
		var nonEmptyArrayOfIDictionariesWithTValues = /* NonEmptyArray<IDictionary<int,T>> */ typeof(NonEmptyArray<>).MakeGenericType(typeof(IDictionary<,>).MakeGenericType(typeof(int), dataType));
		var arrayOfNonEmptyDictionariesWithTValues = /* NonEmptyDictionary<int,T>[] */                typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), dataType).MakeArrayType();
		var listOfNonEmptyDictionariesWithTValues = /* List<NonEmptyDictionary<int,T>> */            typeof(List<>).MakeGenericType(typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), dataType));
		var iEnumerableOfNonEmptyDictionariesWithTValues = /* IEnumerable<NonEmptyDictionary<int,T>> */     typeof(IEnumerable<>).MakeGenericType(typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), dataType));
		var nonEmptyArrayOfNonEmptyDictionariesWithTValues = /* NonEmptyArray<NonEmptyDictionary<int,T>> */ typeof(NonEmptyArray<>).MakeGenericType(typeof(NonEmptyDictionary<,>).MakeGenericType(typeof(int), dataType));
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
			yield return new(serialized, arrayOfIDictionariesWithTValues);
			yield return new(serialized, listOfIDictionariesWithTValues);
			yield return new(serialized, iEnumerableOfIDictionariesWithTValues);

			if (dataStructureScope == DataStructureScope.Unlimited)
			{
				if (!serialized.Contains(SerializedEmptyCollection))
				{
					yield return new(serialized, nonEmptyArrayOfDictionariesWithTValues);
				}

				if (!serialized.Contains(SerializedEmptyDictionary))
				{
					yield return new(serialized, arrayOfNonEmptyDictionariesWithTValues);
					yield return new(serialized, listOfNonEmptyDictionariesWithTValues);
					yield return new(serialized, iEnumerableOfNonEmptyDictionariesWithTValues);
				}
			}
		}

		if (!String.IsNullOrEmpty(dataAsPropertyName))
		{
			var arrayOfDictionariesWithTKeys = /* Dictionary<T, int>[] */                typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int)).MakeArrayType();
			var listOfDictionariesWithTKeys = /* List<Dictionary<T, int>> */            typeof(List<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int)));
			var iEnumerableOfDictionariesWithTKeys = /* IEnumerable<Dictionary<T, int>> */     typeof(IEnumerable<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int)));
			var nonEmptyArrayOfDictionariesWithTKeys = /* NonEmptyArray<Dictionary<T, int>> */ typeof(NonEmptyArray<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, typeof(int)));
			var arrayOfIDictionariesWithTKeys = /* IDictionary<T, int>[] */                typeof(IDictionary<,>).MakeGenericType(dataType, typeof(int)).MakeArrayType();
			var listOfIDictionariesWithTKeys = /* List<IDictionary<T, int>> */            typeof(List<>).MakeGenericType(typeof(IDictionary<,>).MakeGenericType(dataType, typeof(int)));
			var iEnumerableOfIDictionariesWithTKeys = /* IEnumerable<IDictionary<T, int>> */     typeof(IEnumerable<>).MakeGenericType(typeof(IDictionary<,>).MakeGenericType(dataType, typeof(int)));
			var nonEmptyArrayOfIDictionariesWithTKeys = /* NonEmptyArray<IDictionary<T, int>> */ typeof(NonEmptyArray<>).MakeGenericType(typeof(IDictionary<,>).MakeGenericType(dataType, typeof(int)));
			var arrayOfNonEmptyDictionariesWithTKeys = /* NonEmptyDictionary<T, int>[] */                typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, typeof(int)).MakeArrayType();
			var listOfNonEmptyDictionariesWithTKeys = /* List<NonEmptyDictionary<T, int>> */            typeof(List<>).MakeGenericType(typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, typeof(int)));
			var iEnumerableOfNonEmptyDictionariesWithTKeys = /* IEnumerable<NonEmptyDictionary<T, int>> */     typeof(IEnumerable<>).MakeGenericType(typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, typeof(int)));
			var nonEmptyArrayOfNonEmptyDictionariesWithTKeys = /* NonEmptyArray<NonEmptyDictionary<T, int>> */ typeof(NonEmptyArray<>).MakeGenericType(typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, typeof(int)));
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
				yield return new(serialized, arrayOfIDictionariesWithTKeys);
				yield return new(serialized, listOfIDictionariesWithTKeys);
				yield return new(serialized, iEnumerableOfIDictionariesWithTKeys);
				if (dataStructureScope == DataStructureScope.Unlimited)
				{
					if (!serialized.Contains(SerializedEmptyCollection))
					{
						yield return new(serialized, nonEmptyArrayOfDictionariesWithTKeys);
						yield return new(serialized, nonEmptyArrayOfIDictionariesWithTKeys);
					}

					if (!serialized.Contains(SerializedEmptyDictionary))
					{
						yield return new(serialized, arrayOfNonEmptyDictionariesWithTKeys);
						yield return new(serialized, listOfNonEmptyDictionariesWithTKeys);
						yield return new(serialized, iEnumerableOfNonEmptyDictionariesWithTKeys);
						if (!serialized.Contains(SerializedEmptyCollection))
						{
							yield return new(serialized, nonEmptyArrayOfNonEmptyDictionariesWithTKeys);
						}
					}
				}
			}

			var arrayOfDictionariesWithTKeysAndValues = /* Dictionary<T, T>[] */                                                                               typeof(Dictionary<,>).MakeGenericType(dataType, dataType).MakeArrayType();
			var listOfDictionariesWithTKeysAndValues = /* List<Dictionary<T, T>> */                                            typeof(List<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, dataType));
			var iEnumerableOfDictionariesWithTKeysAndValues = /* IEnumerable<Dictionary<T, T>> */                              typeof(IEnumerable<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, dataType));
			var nonEmptyArrayOfDictionariesWithTKeysAndValues = /* NonEmptyArray<Dictionary<T, T>> */                         typeof(NonEmptyArray<>).MakeGenericType(typeof(Dictionary<,>).MakeGenericType(dataType, dataType));
			var arrayOfIDictionariesWithTKeysAndValues = /* IDictionary<T, T>[] */                                                                           typeof(IDictionary<,>).MakeGenericType(dataType, dataType).MakeArrayType();
			var listOfIDictionariesWithTKeysAndValues = /* List<IDictionary<T, T>> */                                        typeof(List<>).MakeGenericType(typeof(IDictionary<,>).MakeGenericType(dataType, dataType));
			var iEnumerableOfIDictionariesWithTKeysAndValues = /* IEnumerable<IDictionary<T, T>> */                          typeof(IEnumerable<>).MakeGenericType(typeof(IDictionary<,>).MakeGenericType(dataType, dataType));
			var nonEmptyArrayOfIDictionariesWithTKeysAndValues = /* NonEmptyArray<IDictionary<T, T>> */                      typeof(NonEmptyArray<>).MakeGenericType(typeof(IDictionary<,>).MakeGenericType(dataType, dataType));
			var arrayOfNonEmptyDictionariesWithTKeysAndValues = /* NonEmptyDictionary<T, T>[] */                                                      typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, dataType).MakeArrayType();
			var listOfNonEmptyDictionariesWithTKeysAndValues = /* List<NonEmptyDictionary<T, T>> */                   typeof(List<>).MakeGenericType(typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, dataType));
			var iEnumerableOfNonEmptyDictionariesWithTKeysAndValues = /* IEnumerable<NonEmptyDictionary<T, T>> */     typeof(IEnumerable<>).MakeGenericType(typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, dataType));
			var nonEmptyArrayOfNonEmptyDictionariesWithTKeysAndValues = /* NonEmptyArray<NonEmptyDictionary<T, T>> */ typeof(NonEmptyArray<>).MakeGenericType(typeof(NonEmptyDictionary<,>).MakeGenericType(dataType, dataType));
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
				yield return new(serialized, arrayOfIDictionariesWithTKeysAndValues);
				yield return new(serialized, listOfIDictionariesWithTKeysAndValues);
				yield return new(serialized, iEnumerableOfIDictionariesWithTKeysAndValues);
				if (dataStructureScope == DataStructureScope.Unlimited)
				{
					if (!serialized.Contains(SerializedEmptyCollection))
					{
						yield return new(serialized, nonEmptyArrayOfDictionariesWithTKeysAndValues);
						yield return new(serialized, nonEmptyArrayOfIDictionariesWithTKeysAndValues);
					}
					if (!serialized.Contains(SerializedEmptyDictionary))
					{
						yield return new(serialized, arrayOfNonEmptyDictionariesWithTKeysAndValues);
						yield return new(serialized, listOfNonEmptyDictionariesWithTKeysAndValues);
						yield return new(serialized, iEnumerableOfNonEmptyDictionariesWithTKeysAndValues);
						if (!serialized.Contains(SerializedEmptyCollection))
						{
							yield return new(serialized, nonEmptyArrayOfNonEmptyDictionariesWithTKeysAndValues);
						}
					}
				}
			}
		}
	}
}
