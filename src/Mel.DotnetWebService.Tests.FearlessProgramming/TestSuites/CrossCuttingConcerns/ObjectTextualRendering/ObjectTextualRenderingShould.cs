using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Guids;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.ObjectTextualRendering;

class ObjectRenderingShould
{
	[Test]
	public void Return_Null_When_Object_Is_Null()
	=> Assert.That(default(object).GetStringRepresentation(), Is.EqualTo("null"));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_LowerCaseAlphabeticLetters))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_UpperCaseAlphabeticLetters))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_LowerCaseLettersWithAccent))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_UpperCaseLettersWithAccent))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_LowerCaseWords))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_UpperCaseWords))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_MixedCaseWords))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_LowerCaseWordsWithAccent))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_UpperCaseWordsWithAccent))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_MixedCaseWordsWithAccent))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_Numbers))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.String_Punctuation))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Integer_PositiveNumbers))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Integer_NegativeNumbers))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.DateTimes))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Guids))]
	public void Render_Objects_That_Are_Not_ConstrainedTypes_LikeDefaultJsonConverterImplementation(object obj, string expected)
	=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expected));

	public class RenderTheEncapsulatedTypeOnTypesThatInheritTheConstrainedClass
	{
		public class WhenRenderingAnObject
		{
			[Test]
			public void That_DirectlyInheritsTheConstrainedClass()
			{
				var obj = NonEmptyGuid.FromString("63de52ef-2944-40a5-8183-b656eaf2574f");

				Assert.That(
					obj.GetStringRepresentation(),
					Is.EqualTo("\"63de52ef-2944-40a5-8183-b656eaf2574f\""));
			}

			[Test]
			public void That_Inherits_AnObjectThatInherits_TheConstrainedClass()
			{
				var obj = ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.FromString("23de52ef-2944-40a5-8183-b656eaf2574f");

				Assert.That(
					obj.GetStringRepresentation(),
					Is.EqualTo("\"23de52ef-2944-40a5-8183-b656eaf2574f\""));
			}

			[Test]
			public void That_Inherits_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
				var obj = ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.FromString("33de52ef-2944-40a5-8183-b656eaf2574f");

				Assert.That(
					obj.GetStringRepresentation(),
					Is.EqualTo("\"33de52ef-2944-40a5-8183-b656eaf2574f\""));
			}

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass2))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));


			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			// TODO: collections of collections, dictionary of collections

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
		}

		public class WhenRenderingACollectionOf
		{
			[Test]
			public void Objects_That_DirectlyInheritsTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_Inherits_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_Inherits_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}


			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}


			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}


			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
		}

		public class WhenRenderingADictionaryOf
		{
			[Test]
			public void Objects_That_DirectlyInheritsTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_Inherits_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_Inherits_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}


			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}


			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}


			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
			}
		}

		public class WhenRenderingAnObjectThatContainsAsAPublicFieldOrProperty
		{
			public class AnObject
			{
				[Test]
				public void That_DirectlyInheritsTheConstrainedClass()
				{
				}

				[Test]
				public void That_Inherits_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_Inherits_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}


				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
			}

			public class ACollectionOf
			{
				[Test]
				public void Objects_That_DirectlyInheritsTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_Inherits_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_Inherits_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}


				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}


				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}


				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
			}

			public class ADictionaryOf
			{
				[Test]
				public void Objects_That_DirectlyInheritsTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_Inherits_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_Inherits_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}


				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}


				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}


				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyInheritTheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
				{
				}
			}
		}
	}

	public class RenderConstructorParametersAndPublicPropertiesOnTypesDirectlyImplementingTheIConstrainedInterface
	{
		public class WhenRenderingAnObject
		{
			[Test]
			public void That_DirectlyImplementsTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_DirectlyImplementsAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}


			[Test]
			public void That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}


			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
		}

		public class WhenRenderingACollectionOf
		{
			[Test]
			public void Objects_That_DirectlyImplementsTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_Implements_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_DirectlyImplementsAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}


			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}


			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}


			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
		}

		public class WhenRenderingADictionaryOf
		{
			[Test]
			public void Objects_That_DirectlyImplementsTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_Implements_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_DirectlyImplementsAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}


			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}


			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}


			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
			{
			}

			[Test]
			public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
			{
			}
		}

		public class WhenRenderingAnObjectThatContainsAsAPublicFieldOrProperty
		{
			public class AnObject
			{
				[Test]
				public void That_DirectlyImplementsTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_Implements_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_DirectlyImplementsAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}


				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
			}

			public class ACollectionOf
			{
				[Test]
				public void Objects_That_DirectlyImplementsTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_Implements_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_DirectlyImplementsAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}


				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}


				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}


				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
			}

			public class ADictionaryOf
			{
				[Test]
				public void Objects_That_DirectlyImplementsTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_Implements_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_DirectlyImplementsAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}


				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void Objects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}


				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void CollectionsOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}


				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementTheIConstrainedInterface()
				{
				}

				[Test]
				public void DictionariesOfObjects_That_InvolveFieldsOrProperties_That_AreObjectsContainingDictionaryOfObjects_That_InvolvesFieldsOrProperties_That_AreObjectsContainingCollectionOfObjects_That_DirectlyImplementAnIConstrainedInterface_WhoseGenericTypeIs_AnObjectThatImplements_TheIConstrainedInterface()
				{
				}
			}
		}
	}

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void RenderTheEncapsulatedTypeOnTypesInheritingConstrainedClass_When_ObjectInvolvesFieldsOrPropertiesInheritingConstrainedClass(object withValueObjectProperties, string expected) // Todo: one giant object with all cases of ConstrainedObjects
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty))]
	public void RenderConstructorParametersAndPublicPropertiesOnTypesDirectlyImplementingIConstrainedInterface_When_ObjectInvolvesFieldsOrPropertiesDirectlyImplementingIConstrainedInterface(object withCollectionOfValueObjectProperties, string expected)
	=> Assert.That(withCollectionOfValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void RenderTheEncapsulatedType_When_Object_That_Involves_Fields_Or_Properties_InheritingConstrainedClass_DirectlyInheringConstrainedClass(object withValueObjectProperties, string expected) // Todo: one giant object with all cases of ConstrainedObjects
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void Render_Object_IndirectlyInheringConstrainedClass(object withValueObjectProperties, string expected) // Todo: one giant object with all cases of ConstrainedObjects
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty))]
	public void Render_ContainerObjects_That_Involve_Fields_Or_Properties_DirectlyImplementingIConstrainedInterface(object withCollectionOfValueObjectProperties, string expected)
	=> Assert.That(withCollectionOfValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.SimpleValueObjects))]
	public void By_Direct_Inheritance_To_Constrained_Class(object contrainedType, string expected)
	=> Assert.That(contrainedType.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.SimpleValueObjectsOnTwoLayersOfInheritedConstraints))]
	public void By_Inheriting_Another_Class_That_Inherits_Constrained_Class(object contrainedType, string expected)
	=> Assert.That(contrainedType.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.NativeSimpleTypesCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.SimpleValueObjectsCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ObjectsFeaturingSimpleValueObjectsAsPropertiesCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithoutPublicPropertiesCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithPublicPropertiesCollections))]
	public void A_Collection_Of_ConstrainedTypes(object obj, string expected)
	=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.NativeSimpleTypesDictionaries))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.SimpleValueObjectsDictionaries))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ObjectsFeaturingSimpleValueObjectsAsPropertiesDictionaries))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyDictionaries))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithoutPublicPropertiesDictionaries))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithPublicPropertiesDictionaries))]
	public void A_Dictionary_Of_ConstrainedTypes(object obj, string expected)
	=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void ConstrainedTypes_As_Properties(object withValueObjectProperties, string expected)
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty))]
	public void ConstrainedTypeCollections_As_Properties(object withCollectionOfValueObjectProperties, string expected)
	=> Assert.That(withCollectionOfValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithoutPublicProperties))]
	public void A_FirstClassCollection_Without_PublicProperties(object firstClassCollectionWithoutProperties, string expected)
	=> Assert.That(firstClassCollectionWithoutProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithPublicProperties))]
	public void A_FirstClassCollection_With_PublicProperties2(object firstClassCollectionWithProperties, string expected)
	=> Assert.That(firstClassCollectionWithProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void ConstrainedTypes_As_Properties2(object withValueObjectProperties, string expected)
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty))]
	public void ConstrainedTypeCollections_As_Properties2(object withCollectionOfValueObjectProperties, string expected)
	=> Assert.That(withCollectionOfValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

}
