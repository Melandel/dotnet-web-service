using System.Text.Json;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

class SerializationShould
{
	[Test]
	public void Deserialize_NullableReferenceTypes_NullableConstrainedTypes_And_NullableValueTypes()
	{
		// Arrange
		var serialized = "{\"String\":\"foo\",\"PositiveInt\":1,\"NullableInt\":2}";
		// Act
		var deserialized = JsonSerializer.Deserialize<ClassArchetype.ContainingNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		// Assert
		Assert.That(deserialized.GetStringRepresentation(), Is.EqualTo(serialized));
	}

	[Test]
	public void Deserialize_Missing_NullableReferenceType_Members_As_Null()
	{
		// Arrange
		var serialized = "{\"PositiveInt\":1,\"NullableInt\":2}";
		// Act
		var deserialized = JsonSerializer.Deserialize<ClassArchetype.ContainingNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		// Assert
		Assert.That(
			deserialized.GetStringRepresentation(),
			Is.EqualTo("{\"String\":null,\"PositiveInt\":1,\"NullableInt\":2}"));

		Action action = () => JsonSerializer.Deserialize<ClassArchetype.ContainingNonNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		Assert.That(
			action,
			Throws.Exception);
	}

	[Test]
	public void Deserialize_Missing_NullableConstrainedType_Members_As_Null()
	{
		// Arrange
		var serialized = "{\"String\":\"foo\",\"NullableInt\":2}";
		// Act
		var deserialized = JsonSerializer.Deserialize<ClassArchetype.ContainingNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		// Assert
		Assert.That(
			deserialized.GetStringRepresentation(),
			Is.EqualTo("{\"String\":\"foo\",\"PositiveInt\":null,\"NullableInt\":2}"));

		Action action = () => JsonSerializer.Deserialize<ClassArchetype.ContainingNonNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		Assert.That(
			action,
			Throws.Exception);
	}

	[Test]
	public void Deserialize_Missing_NullableValueType_Members_As_Null()
	{
		// Arrange
		var serialized = "{\"String\":\"foo\",\"PositiveInt\":1}";
		// Act
		var deserialized = JsonSerializer.Deserialize<ClassArchetype.ContainingNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		// Assert
		Assert.That(
			deserialized.GetStringRepresentation(),
			Is.EqualTo("{\"String\":\"foo\",\"PositiveInt\":1,\"NullableInt\":null}"));

		Action action = () => JsonSerializer.Deserialize<ClassArchetype.ContainingNonNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		Assert.That(
			action,
			Throws.Nothing);
	}

	[Test]
	public void Deserialize_Null_NullableValueType_Members_As_Null()
	{
		// Arrange
		var serialized = "{\"String\":\"foo\",\"PositiveInt\":1,\"NullableInt\":null}";
		// Act
		var deserialized = JsonSerializer.Deserialize<ClassArchetype.ContainingNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		// Assert
		Assert.That(
			deserialized.GetStringRepresentation(),
			Is.EqualTo(serialized));

		Action action = () => JsonSerializer.Deserialize<ClassArchetype.ContainingNonNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		Assert.That(
			action,
			Throws.Nothing);
	}


	[Test]
	public void Ignore_UnexpectedPropertyNames_During_Deserialization()
	{
		// Arrange
		var serialized = "{\"IDoNotExist\":true,\"String\":\"foo\",\"PositiveInt\":1,\"NullableInt\":null, \"IDoNotExist\":false}";
		// Act
		var deserialized = JsonSerializer.Deserialize<ClassArchetype.ContainingNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		// Assert
		Assert.That(
			deserialized.GetStringRepresentation(),
			Is.EqualTo("{\"String\":\"foo\",\"PositiveInt\":1,\"NullableInt\":null}"));

		Action action = () => JsonSerializer.Deserialize<ClassArchetype.ContainingNonNullableProperties>(serialized, JsonSerializerOptionsArchetype.ThatInvolvesConstrainedTypesHandling);
		Assert.That(
			action,
			Throws.Nothing);
	}
}
