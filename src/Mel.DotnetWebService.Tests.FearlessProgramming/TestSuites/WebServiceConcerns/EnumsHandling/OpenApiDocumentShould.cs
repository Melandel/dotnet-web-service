using Mel.DotnetWebService.CrossCuttingConcerns.EnumTypesHandling;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ControllerTestDoubles;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.EnumsHandling;

class OpenApiDocumentShould : TestSuiteUsingTestServer
{
	OpenApiSchema GetSchemaReferencedBy(OpenApiSchema schemaConstainingReference)
	=> TestServer.OpenApiDocument.Components.Schemas[schemaConstainingReference.Reference.Id];

	[Test]
	public void Document_Enum_Types_From_Body_Without_Mentioning_TechnicalDefaultEnumValue()
	{
		// Arrange
		var routeDocumentation = TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
			nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInRequestBody),
			HttpMethod.Post);

		var bodyTypeSchema = GetSchemaReferencedBy(routeDocumentation.RequestBody.Content.First().Value.Schema);
		var enumTypeSchema = GetSchemaReferencedBy(bodyTypeSchema.Properties.First().Value);

		// Act
		var documentedEnumValues = enumTypeSchema.Enum.Select(e => (e as OpenApiString).Value).ToArray();

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

		var enumTypeSchema = GetSchemaReferencedBy(routeDocumentation.Parameters.First().Schema);

		// Act
		var documentedEnumValues = enumTypeSchema.Enum.Select(e => (e as OpenApiString).Value).ToArray();

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

		var enumTypeSchema = GetSchemaReferencedBy(routeDocumentation.Parameters.First().Schema);

		// Act
		var documentedEnumValues = enumTypeSchema.Enum.Select(e => (e as OpenApiString).Value).ToArray();

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

		var responseTypeSchema = GetSchemaReferencedBy(routeDocumentation.Responses.First().Value.Content.First().Value.Schema);
		var enumTypeSchema = GetSchemaReferencedBy(responseTypeSchema.Properties.First().Value);

		// Act
		var documentedEnumValues = enumTypeSchema.Enum.Select(e => (e as OpenApiString).Value).ToArray();

		// Assert
		Assert.That(documentedEnumValues, Does.Not.Contain(EnumTypesNamingConvention.TechnicalDefaultEnumValue));
	}
}
