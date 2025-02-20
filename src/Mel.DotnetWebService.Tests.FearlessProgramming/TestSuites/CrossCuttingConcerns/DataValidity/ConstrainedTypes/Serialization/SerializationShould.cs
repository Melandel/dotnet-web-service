using System.Text.Encodings.Web;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;

class SerializationShould
{
	public string Serialize(object o) => JsonSerializer.Serialize(o, OptionsInvolvingConstrainedTypesHandling);
	public object Deserialize(string str, Type type) => JsonSerializer.Deserialize(str, type, OptionsInvolvingConstrainedTypesHandling);
	static JsonSerializerOptions OptionsInvolvingConstrainedTypesHandling
	{
		get
		{
			var v = new JsonSerializerOptions()
			{
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};
			v.Converters.Add(new ConstrainedTypeJsonConverter());

			return v;
		}
	}

	[Test]
	public void Return_Null_When_Object_Is_Null()
	=> Assert.That(default(object).GetStringRepresentation(), Is.EqualTo("null"));

	[TestCaseSource(typeof(SerializationTestCases.That_Do_Not_Involve_Constrained_Types))]
	public void Render_Data_That_DontInvolveConstrainedTypes_LikeUnconfiguredNewtonsoftImplementation(string serialized, Type deserializationType)
	{
		// Arrange
		var deserializedByUnconfiguredNewtonSoft = Newtonsoft.Json.JsonConvert.DeserializeObject(serialized, deserializationType);

		// Act
		var deserialized = Deserialize(serialized, deserializationType);
		var reserialized = Serialize(deserialized);

		// Assert
		Assert.That(deserialized, Is.EqualTo(deserializedByUnconfiguredNewtonSoft));
		Assert.That(reserialized, Is.EqualTo(serialized));
	}

	public class RenderTheEncapsulatedTypeOnTypesThatInheritTheConstrainedClass
	{
		public class WhenRenderingAnObject
		{
			[Test]
			public void That_DirectlyInheritsTheConstrainedClass()
			{
				var obj = NonEmptyGuid.Constraining("63de52ef-2944-40a5-8183-b656eaf2574f");

				Assert.That(
					obj.GetStringRepresentation(),
					Is.EqualTo("\"63de52ef-2944-40a5-8183-b656eaf2574f\""));
			}

			//[Test]
			//public void That_Inherits_AnObjectThatInherits_TheConstrainedClass()
			//{
			//	var obj = ClassArchetype.NonEmptyGuidStartingWithTheCharacter2.Constraining("23de52ef-2944-40a5-8183-b656eaf2574f");

			//	Assert.That(
			//		obj.GetStringRepresentation(),
			//		Is.EqualTo("\"23de52ef-2944-40a5-8183-b656eaf2574f\""));
			//}

			[Test]
			public void That_Inherits_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass()
			{
				var obj = ClassArchetype.NonEmptyGuidStartingWithTheCharacter3.ApplyConstraintsTo("33de52ef-2944-40a5-8183-b656eaf2574f");

				Assert.That(
					obj.GetStringRepresentation(),
					Is.EqualTo("\"33de52ef-2944-40a5-8183-b656eaf2574f\""));
			}

			[Test]
			public void That_Inherits_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_AnObjectThatInherits_TheConstrainedClass()
			{
				var obj = ClassArchetype.NonEmptyGuidStartingAndEndingWithTheCharacter3.ApplyConstraintsTo("33de52ef-2944-40a5-8183-b656eaf25743");

				Assert.That(
					obj.GetStringRepresentation(),
					Is.EqualTo("\"33de52ef-2944-40a5-8183-b656eaf25743\""));
			}

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			//public void That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			//=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			//public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			//=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass2))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));


			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			//public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			//=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			// TODO: collections of collections, dictionary of collections

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			//public void That_InvolvesFieldsOrProperties_That_AreCollectionOfObjects_That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			//=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreCollectionOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
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

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass))]
			public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_DirectlyInheritTheConstrainedClass(object obj, string expectedTextualRepresentation)
			=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass))]
			//public void That_InvolvesFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AnObjectThatInherits_TheConstrainedClass(object obj, string expectedTextualRepresentation)
			//=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expectedTextualRepresentation));

			[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.Objects_That_InvolveFieldsOrProperties_That_AreDictionaryOfObjects_That_Inherit_AConstrainedClass_WhoseGenericTypeIs_AnObjectThatInherits_TheConstrainedClass))]
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

	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void RenderTheEncapsulatedTypeOnTypesInheritingConstrainedClass_When_ObjectInvolvesFieldsOrPropertiesInheritingConstrainedClass(object withValueObjectProperties, string expected) // Todo: one giant object with all cases of ConstrainedObjects
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty))]
	//public void RenderConstructorParametersAndPublicPropertiesOnTypesDirectlyImplementingIConstrainedInterface_When_ObjectInvolvesFieldsOrPropertiesDirectlyImplementingIConstrainedInterface(object withCollectionOfValueObjectProperties, string expected)
	//=> Assert.That(withCollectionOfValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void RenderTheEncapsulatedType_When_Object_That_Involves_Fields_Or_Properties_InheritingConstrainedClass_DirectlyInheringConstrainedClass(object withValueObjectProperties, string expected) // Todo: one giant object with all cases of ConstrainedObjects
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void Render_Object_IndirectlyInheringConstrainedClass(object withValueObjectProperties, string expected) // Todo: one giant object with all cases of ConstrainedObjects
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty))]
	//public void Render_ContainerObjects_That_Involve_Fields_Or_Properties_DirectlyImplementingIConstrainedInterface(object withCollectionOfValueObjectProperties, string expected)
	//=> Assert.That(withCollectionOfValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));
	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.SimpleValueObjects))]
	public void By_Direct_Inheritance_To_Constrained_Class(object contrainedType, string expected)
	=> Assert.That(contrainedType.GetStringRepresentation(), Is.EqualTo(expected));

	//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.SimpleValueObjectsOnTwoLayersOfInheritedConstraints))]
	//public void By_Inheriting_Another_Class_That_Inherits_Constrained_Class(object contrainedType, string expected)
	//=> Assert.That(contrainedType.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.NativeSimpleTypesCollections))]
	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.SimpleValueObjectsCollections))]
	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ObjectsFeaturingSimpleValueObjectsAsPropertiesCollections))]
	//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyCollections))]
	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.FirstClassCollectionsWithoutPublicPropertiesCollections))]
	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.FirstClassCollectionsWithPublicPropertiesCollections))]
	public void A_Collection_Of_ConstrainedTypes(object obj, string expected)
	=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.NativeSimpleTypesDictionaries))]
	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.SimpleValueObjectsDictionaries))]
	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ObjectsFeaturingSimpleValueObjectsAsPropertiesDictionaries))]
	//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyDictionaries))]
	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.FirstClassCollectionsWithoutPublicPropertiesDictionaries))]
	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.FirstClassCollectionsWithPublicPropertiesDictionaries))]
	public void A_Dictionary_Of_ConstrainedTypes(object obj, string expected)
	=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void ConstrainedTypes_As_Properties(object withValueObjectProperties, string expected)
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty))]
	//public void ConstrainedTypeCollections_As_Properties(object withCollectionOfValueObjectProperties, string expected)
	//=> Assert.That(withCollectionOfValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.FirstClassCollectionsWithoutPublicProperties))]
	public void A_FirstClassCollection_Without_PublicProperties(object firstClassCollectionWithoutProperties, string expected)
	=> Assert.That(firstClassCollectionWithoutProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.FirstClassCollectionsWithPublicProperties))]
	public void A_FirstClassCollection_With_PublicProperties2(object firstClassCollectionWithProperties, string expected)
	=> Assert.That(firstClassCollectionWithProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void ConstrainedTypes_As_Properties2(object withValueObjectProperties, string expected)
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	//[TestCaseSource(typeof(SerializationTestCases), nameof(SerializationTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty))]
	//public void ConstrainedTypeCollections_As_Properties2(object withCollectionOfValueObjectProperties, string expected)
	//=> Assert.That(withCollectionOfValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

}
