using Microsoft.AspNetCore.Mvc;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ControllerTestDoubles;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CodingStandards;

class EnumTypesMust
{
	static readonly InMemoryTestServer TestServer;
	static EnumTypesMust()
	{
		TestServer = InMemoryTestServer.Create();
	}

	[TestCaseSource(typeof(Types), nameof(Types.AllEnumTypesDefinedByOurOrganization))]
	public void Define_ZeroValued_EnumItem_Named_TechnicalDefaultEnumValue(Type enumType)
	{
		// Arrange
		Dictionary<int, string> enumTypeItems = Enum.GetValues(enumType)
			.Cast<int>()
			.ToDictionary(enumValue => enumValue, enumValue => Enum.GetName(enumType, enumValue));

		// Act

		// Assert
		Assert.That(
			enumTypeItems,
			Contains.Item(KeyValuePairArchetype.ZeroValuedEnumItem_Named_TechnicalDefaultEnumValue));
	}

	[Test]
	public async Task Be_Accepted_As_String_In_Request_Body()
	{
		// Arrange
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInRequestBody);
		var payload = $@"{{ ""CityName"": ""{CityNameArchetype.SomeValue}"" }}";

		// Act
		var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
			controllerMethod,
			payload.ToJsonContent());

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Be_Rejected_As_Int_In_Request_Body()
	{
		// Arrange
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInRequestBody);
		var enumValueAsInt = (int)CityNameArchetype.SomeValue;
		var payload = $@"{{ ""CityName"": {enumValueAsInt} }}";

		// Act
		var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
			controllerMethod,
			payload.ToJsonContent());
		var response = await httpResponse.ToResponseObject<ValidationProblemDetails>();

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		Assert.That(response, Is.Not.Null);
		Assert.That(response.Errors, Does.ContainKey("$.CityName"));
		Assert.That(response.Errors["$.CityName"], Is.Not.Empty);
		Assert.That(
			response.Errors["$.CityName"].First(),
			Does.Contain($"The JSON value could not be converted to {typeof(ControllerTestDoubles.StubbedEndpointsSpecificallyCreatedForTests.RequestWithCityNameProperty).FullName}"));
	}

	[Test]
	public async Task Be_Accepted_As_String_In_Request_QueryString()
	{
		// Arrange
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInQueryString);
		var enumValueAsString = CityNameArchetype.SomeValue.ToString();

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod, $"?cityName={enumValueAsString}");

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Be_Rejected_As_Int_In_Request_QueryString()
	{
		// Arrange
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInQueryString);
		var enumValueAsInt = (int)CityNameArchetype.SomeValue;
		var parameterName = "cityName";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod, $"?{parameterName}={enumValueAsInt}");
		var response = await httpResponse.ToResponseObject<ProblemDetails>();

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		Assert.That(response.Extensions, Is.Not.Null);
		Assert.That(response.Extensions, Does.ContainKey("IntegerValue").WithValue(enumValueAsInt));
		Assert.That(response.Extensions, Does.ContainKey("EnumParameterName").WithValue(parameterName));
		Assert.That(response.Extensions, Does.ContainKey("SupportedEnumValues"));
		Assert.That(response.Extensions["SupportedEnumValues"], Is.EquivalentTo(new[] { "Toulouse", "Bordeaux", "Paris" }));
	}

	[Test]
	public async Task Be_Accepted_As_String_In_Route()
	{
		// Arrange
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInRoute);
		var enumValueAsString = CityNameArchetype.SomeValue.ToString();

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod, $"/{enumValueAsString}");

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Be_Rejected_As_Int_In_Route()
	{
		// Arrange
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.WithEnumInOutputFieldAndInRoute);
		var enumValueAsInt = (int)CityNameArchetype.SomeValue;

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod, $"/{enumValueAsInt}");
		var response = await httpResponse.ToResponseObject<ProblemDetails>();

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		Assert.That(response.Extensions, Is.Not.Null);
		Assert.That(response.Extensions, Does.ContainKey("IntegerValue"));
		Assert.That(response.Extensions, Does.ContainKey("EnumParameterName"));
		Assert.That(response.Extensions, Does.ContainKey("SupportedEnumValues"));
	}
}
