using Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.CodingStandards;

class EnumTypesMust
{
	static readonly TestServerForEnumTypes TestServer;
	static EnumTypesMust()
	{
		TestServer = TestServerForEnumTypes.Create();
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
		var enumValueAsString = CityArchetype.SomeValue.ToString();
		var payload = $@"{{ ""City"": ""{enumValueAsString}"" }}";
		var requestUri = $"api/v3/WeatherForecast/WithEnumOutputInFieldAndInRequestBody";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.PostAsync(
			requestUri,
			payload.ToJsonContent());

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Be_Rejected_As_Int_In_Request_Body()
	{
		// Arrange
		var enumValueAsInt = (int)CityArchetype.SomeValue;
		var payload = $@"{{ ""City"": {enumValueAsInt} }}";
		var requestUri = $"api/v3/WeatherForecast/WithEnumOutputInFieldAndInRequestBody";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.PostAsync(
			requestUri,
			payload.ToJsonContent());

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
	}

	[Test]
	public async Task Be_Accepted_As_String_In_Request_QueryString()
	{
		// Arrange
		var enumValueAsString = CityArchetype.SomeValue.ToString();
		var requestUri = $"api/v3/WeatherForecast/WithEnumInOutputFieldAndInQueryString?city={enumValueAsString}";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Be_Rejected_As_Int_In_Request_QueryString()
	{
		// Arrange
		var enumValueAsInt = (int)CityArchetype.SomeValue;
		var requestUri = $"api/v3/WeatherForecast/WithEnumInOutputFieldAndInQueryString?city={enumValueAsInt}";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
	}

	[Test]
	public async Task Be_Accepted_As_String_In_Route()
	{
		// Arrange
		var enumValueAsString = CityArchetype.SomeValue.ToString();
		var requestUri = $"api/v3/WeatherForecast/WithEnumInOutputFieldAndInRoute/{enumValueAsString}";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
	}

	[Test]
	public async Task Be_Rejected_As_Int_In_Route()
	{
		// Arrange
		var enumValueAsInt = (int)CityArchetype.SomeValue;
		var requestUri = $"api/v3/WeatherForecast/WithEnumInOutputFieldAndInRoute/{enumValueAsInt}";

		// Act
		var httpResponse = await TestServer.TestFriendlyHttpClient.GetAsync(requestUri);

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
	}
}
