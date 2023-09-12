using Microsoft.AspNetCore.Mvc;

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
		var enumValueAsString = CityNameArchetype.SomeValue.ToString();
		var payload = $@"{{ ""CityName"": ""{enumValueAsString}"" }}";
		var requestUri = $"api/v3/stubbed-endpoints-specifically-created-for-tests/with-enum-in-output-field-and-in-request-body";

		// Act
		var httpResponse = await TestServer.HttpClient.PostAsync(
			requestUri,
			payload.ToJsonContent());

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Be_Rejected_As_Int_In_Request_Body()
	{
		// Arrange
		var enumValueAsInt = (int)CityNameArchetype.SomeValue;
		var payload = $@"{{ ""CityName"": {enumValueAsInt} }}";
		var requestUri = $"api/v3/stubbed-endpoints-specifically-created-for-tests/with-enum-in-output-field-and-in-request-body";

		// Act
		var httpResponse = await TestServer.HttpClient.PostAsync(
			requestUri,
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
		var enumValueAsString = CityNameArchetype.SomeValue.ToString();
		var requestUri = $"api/v3/stubbed-endpoints-specifically-created-for-tests/with-enum-in-output-field-and-in-query-string?cityName={enumValueAsString}";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Be_Rejected_As_Int_In_Request_QueryString()
	{
		// Arrange
		var enumValueAsInt = (int)CityNameArchetype.SomeValue;
		var parameterName = "cityName";
		var requestUri = $"api/v3/stubbed-endpoints-specifically-created-for-tests/with-enum-in-output-field-and-in-query-string?{parameterName}={enumValueAsInt}";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);
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
		var enumValueAsString = CityNameArchetype.SomeValue.ToString();
		var requestUri = $"api/v3/stubbed-endpoints-specifically-created-for-tests/with-enum-in-output-field-and-in-route/{enumValueAsString}";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Be_Rejected_As_Int_In_Route()
	{
		// Arrange
		var enumValueAsInt = (int)CityNameArchetype.SomeValue;
		var requestUri = $"api/v3/stubbed-endpoints-specifically-created-for-tests/with-enum-in-output-field-and-in-route/{enumValueAsInt}";

		// Act
		var httpResponse = await TestServer.HttpClient.GetAsync(requestUri);
		var response = await httpResponse.ToResponseObject<ProblemDetails>();

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		Assert.That(response.Extensions, Is.Not.Null);
		Assert.That(response.Extensions, Does.ContainKey("IntegerValue"));
		Assert.That(response.Extensions, Does.ContainKey("EnumParameterName"));
		Assert.That(response.Extensions, Does.ContainKey("SupportedEnumValues"));
	}
}
