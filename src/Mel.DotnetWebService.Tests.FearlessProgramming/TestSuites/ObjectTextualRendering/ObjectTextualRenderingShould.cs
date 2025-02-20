namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.ObjectTextualRendering;

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
	public void Return_String_BetweenDoubleQuotes_When_Object_Is_String(string str, string expected)
	=> Assert.That(str.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Integer_PositiveNumbers))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Integer_NegativeNumbers))]
	public void Return_StringifiedNumber_When_Object_Is_Number(object number, string expected)
	=> Assert.That(number.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.DateTimes))]
	public void Return_StringifiedDate_When_Object_Is_Date(object datetime, string expected)
	=> Assert.That(datetime.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.Guids))]
	public void Return_String_When_Object_Is_NativeGuid(object guid, string expected)
	=> Assert.That(guid.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.SimpleValueObjects))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.SimpleValueObjectsOnTwoLayersOfInheritedConstraints))]
	public void Return_StringifiedEncapsulatedValue_When_Object_Is_SimpleValueObject(object valueObject, string expected)
	=> Assert.That(valueObject.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ObjectsFeaturingSimpleValueObjectsAsProperties))]
	public void Return_WithStringifiedEncapsulatedPropertyValues_When_Object_Has_SimpleValueObjects_As_Properties(object withValueObjectProperties, string expected)
	=> Assert.That(withValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsProperty))]
	public void Return_WithStringifiedCollectionOfEncapsulatedPropertyValues_Whenhen_Object_Is_ValueObject_With_CollectionOfSimpleValueObjects_As_Properties(object withCollectionOfValueObjectProperties, string expected)
	=> Assert.That(withCollectionOfValueObjectProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithoutPublicProperties))]
	public void Return_StringifiedEncapsulatedCollection_When_Object_Is_FirstClassCollectionWithoutPublicProperties(object firstClassCollectionWithoutProperties, string expected)
	=> Assert.That(firstClassCollectionWithoutProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithPublicProperties))]
	public void Return_StringifiedEncapsulatedCollection_When_Object_Is_FirstClassCollectionWithPublicProperties(object firstClassCollectionWithProperties, string expected)
	=> Assert.That(firstClassCollectionWithProperties.GetStringRepresentation(), Is.EqualTo(expected));

	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.NativeSimpleTypesCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.SimpleValueObjectsCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ObjectsFeaturingSimpleValueObjectsAsPropertiesCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.ValueObjectsFeaturingSimpleValueObjectsCollectionAsPropertyCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithoutPublicPropertiesCollections))]
	[TestCaseSource(typeof(ObjectTextualRenderingTestCases), nameof(ObjectTextualRenderingTestCases.FirstClassCollectionsWithPublicPropertiesCollections))]
	public void Return_StringifiedCollection_When_Object_Is_Collection(object obj, string expected)
	=> Assert.That(obj.GetStringRepresentation(), Is.EqualTo(expected));
}
