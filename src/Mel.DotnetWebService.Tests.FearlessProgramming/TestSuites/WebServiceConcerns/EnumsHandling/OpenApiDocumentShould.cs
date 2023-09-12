using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.EnumTypes;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ControllerTestDoubles;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.EnumsHandling;

class OpenApiDocumentShould : TestSuiteUsingTestServer
{
	[Test]
	public void Document_Enum_Types_From_Body_Without_Mentioning_TechnicalDefaultEnumValue()
	{
		// Arrange
		var routeDocumentation = TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
			nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInRequestBody),
			HttpMethod.Post);

		var bodyTypeSchema = TestServer.OpenApiDocument.GetSchemaReferencedBy(routeDocumentation.RequestBody.Content.First().Value.Schema);
		var enumTypeSchema = TestServer.OpenApiDocument.GetSchemaReferencedBy(bodyTypeSchema.Properties.First().Value);

		// Act
		var documentedEnumValues = enumTypeSchema.GetEnumStringValues();

		// Assert
		Assert.That(documentedEnumValues, Does.Not.Contain(EnumTypesNamingConvention.TechnicalDefaultEnumValue));
	}

	[Test]
	public void Document_Enum_Types_From_QueryString_Without_Mentioning_TechnicalDefaultEnumValue()
	{
		// Arrange
		var routeDocumentation = TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
			nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInQueryString),
			HttpMethod.Get);

		var enumTypeSchema = TestServer.OpenApiDocument.GetSchemaReferencedBy(routeDocumentation.Parameters.First().Schema);

		// Act
		var documentedEnumValues = enumTypeSchema.GetEnumStringValues();

		// Assert
		Assert.That(documentedEnumValues, Does.Not.Contain(EnumTypesNamingConvention.TechnicalDefaultEnumValue));
	}

	[Test]
	public void Document_Enum_Types_From_Route_Without_Mentioning_TechnicalDefaultEnumValue()
	{
		// Arrange
		var routeDocumentation = TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
			$"{nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInRoute)}",
			"/{cityName}",
			HttpMethod.Get);

		var enumTypeSchema = TestServer.OpenApiDocument.GetSchemaReferencedBy(routeDocumentation.Parameters.First().Schema);

		// Act
		var documentedEnumValues = enumTypeSchema.GetEnumStringValues();

		// Assert
		Assert.That(enumTypeSchema.Enum, Does.Not.Contain(EnumTypesNamingConvention.TechnicalDefaultEnumValue));
	}

	[Test]
	public void Document_Enum_Types_From_Response_Without_Mentioning_TechnicalDefaultEnumValue()
	{
		// Arrange
		var routeDocumentation = TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
			nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInQueryString),
			HttpMethod.Get);

		var responseTypeSchema = TestServer.OpenApiDocument.GetSchemaReferencedBy(routeDocumentation.Responses.First().Value.Content.First().Value.Schema);
		var enumTypeSchema = TestServer.OpenApiDocument.GetSchemaReferencedBy(responseTypeSchema.Properties.First().Value);

		// Act
		var documentedEnumValues = enumTypeSchema.GetEnumStringValues();

		// Assert
		Assert.That(documentedEnumValues, Does.Not.Contain(EnumTypesNamingConvention.TechnicalDefaultEnumValue));
	}
}
