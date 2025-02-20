using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.ErrorHandling.ConstrainedTypes;

class ApiControllersShould : TestSuiteUsingTestServer
{
	public class Receive_CorrectlyDeserialized_Input
	{
		public class Inside_Body
		{
			public class When_HttpVerb_IsPost
			{
				public class When_Input_Contains_ConstrainedType
				{
					[Test]
					public async Task As_RootObject()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/non-empty-guid-inside-body/";
						var payload = NonEmptyGuid.FromString("3a6f9351-711b-4244-9124-9fec321a9c3c").GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootArray()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/array-of-non-empty-guids-inside-body/";
						var payload = new[]
						{
							NonEmptyGuid.FromString("ecc1b56d-b71c-47ed-89e5-8181d741638e"),
							NonEmptyGuid.FromString("23f7a299-109b-4962-ac08-5b038186981d")
						}.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootList()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/list-of-non-empty-guids-inside-body/";
						var payload = new List<NonEmptyGuid>
						{
							NonEmptyGuid.FromString("eb44460e-8ddd-436a-b845-99543ba9efad"),
							NonEmptyGuid.FromString("eede98f8-0b98-40ee-a86b-67b4a37a2359")
						}.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootIEnumerable()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/ienumerable-of-non-empty-guids-inside-body/";
						var payload = new HashSet<NonEmptyGuid>
						{
							NonEmptyGuid.FromString("eb44460e-8ddd-436a-b845-99543ba9efad"),
							NonEmptyGuid.FromString("eede98f8-0b98-40ee-a86b-67b4a37a2359")
						}.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_RootObject()
					{
						// Arrange
						var requestUri = "/api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-non-empty-guid-inside-body";
						var payload = new ClassArchetype.PositionalRecordContainingANonEmptyGuid(
							StringArchetype.Foo,
							NonEmptyGuid.FromString("1184cb10-0428-4d35-b6e0-ebe9413bfcbb")).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_Objects_InsideRootArray()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/array-of-positional-records-containing-a-non-empty-guid-inside-body/";
						var payload = new[]
						{
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("e1d9d98e-6752-43dd-af10-d66dbf9b5037")),
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("a3ea4100-32b5-41e3-9a76-f67e8b350e02"))
						}.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_Objects_InsideRootList()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/list-of-positional-records-containing-a-non-empty-guid-inside-body/";
						var payload = new List<ClassArchetype.PositionalRecordContainingANonEmptyGuid>
						{
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("924c9966-2d88-4840-959d-ddb2ecf8af59")),
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("aa708279-389a-46de-92fe-14d3522bcbe0"))
						}.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_Objects_InsideRootIEnumerable()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/ienumerable-of-positional-records-containing-a-non-empty-guid-inside-body/";
						var payload = new HashSet<ClassArchetype.PositionalRecordContainingANonEmptyGuid>
						{
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("ca727cfa-43c0-40de-961f-de244b82c5d0")),
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("7851c746-f86e-4cc1-a290-6b2eabea35c8"))
						}.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AnArrayPropertyOf_RootObject()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-an-array-of-non-empty-guids-inside-body/";
						var payload = new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(
							StringArchetype.Foo,
							new[]
							{
								NonEmptyGuid.FromString("49c27a56-8e4a-46fc-b60a-d74b4bc6692e"),
								NonEmptyGuid.FromString("d8ad6467-eb49-4d3c-bfac-3a6f4aeb11b4"),
							}).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AListPropertyOf_RootObject()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-a-list-of-non-empty-guids-inside-body/";
						var payload = new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(
							StringArchetype.Foo,
							new List<NonEmptyGuid>
							{
								NonEmptyGuid.FromString("49c27a56-8e4a-46fc-b60a-d74b4bc6692e"),
								NonEmptyGuid.FromString("d8ad6467-eb49-4d3c-bfac-3a6f4aeb11b4"),
							}).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AnIEnumerablePropertyOf_RootObject()
					{
						// Arrange
						var requestUri = "api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-an-ienumerable-of-non-empty-guids-inside-body/";
						var payload = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(
							StringArchetype.Foo,
							new HashSet<NonEmptyGuid>
							{
								NonEmptyGuid.FromString("767ee8ed-069c-4f1f-a4f6-23f13940f0ca"),
								NonEmptyGuid.FromString("05f97c30-a54f-4e33-963a-27dce2902960"),
							}).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_ObjectContainedByRootObject()
					{
						// Arrange
						var requestUri = "/api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-positional-record-containing-non-empty-guid-inside-body";
						var payload = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
							StringArchetype.Bar,
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(
								StringArchetype.Foo,
								NonEmptyGuid.FromString("b5fcd95a-7ace-417f-8214-2590fdbd315e")))
							.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_ObjectContainedByObjects_InsideRootArray()
					{
						// Arrange
						var requestUri = "/api/v1/stubbed-endpoints-specifically-created-for-tests/array-of-positional-records-containing-positional-record-containing-non-empty-guid-inside-body";
						var payload = new[]
						{
							new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
								StringArchetype.Foo,
								new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("a943ebd2-8521-4b2e-92c5-f09314aba4f7"))),
							new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
								StringArchetype.Baz,
								new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foobar, NonEmptyGuid.FromString("caef8451-2e13-4e13-bf4c-2861bdd28998")))
						}.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_ObjectContainedByObjects_InsideRootList()
					{
						// Arrange
						var requestUri = "/api/v1/stubbed-endpoints-specifically-created-for-tests/list-of-positional-records-containing-positional-record-containing-non-empty-guid-inside-body";
						var payload = new List<ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid>
						{
							new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
								StringArchetype.Foo,
								new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("ce129a85-0d9e-4c3a-99ec-dae914a2bbdc"))),
							new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
								StringArchetype.Baz,
								new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foobar, NonEmptyGuid.FromString("06dca32f-1166-4390-9b59-bfdca58304c2")))
						}.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_ObjectContainedByObjects_InsideRootIEnumerable()
					{
						// Arrange
						var requestUri = "/api/v1/stubbed-endpoints-specifically-created-for-tests/ienumerable-of-positional-records-containing-positional-record-containing-non-empty-guid-inside-body";
						var payload = new HashSet<ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid>
						{
							new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
								StringArchetype.Foo,
								new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("3cfda204-70bd-43c9-ab8b-6f44d6941d6a"))),
							new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
								StringArchetype.Baz,
								new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foobar, NonEmptyGuid.FromString("4ee015f3-43f1-400f-a685-e26326ba1646")))
						}.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AnArrayPropertyOf_ObjectContainedByRootObject()
					{
						// Arrange
						var requestUri = "/api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-positional-record-containing-an-array-of-non-empty-guids-inside-body";
						var payload = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuids(
							StringArchetype.Foo,
							new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(
								StringArchetype.Bar,
								new[]
								{
									NonEmptyGuid.FromString("6c2aa526-bd51-4436-8a5f-91814501b824"),
									NonEmptyGuid.FromString("18c309d3-5f3c-4f57-befb-904b51ead9e8")
								})).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AListPropertyOf_ObjectContainedByRootObject()
					{
						// Arrange
						var requestUri = "/api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-positional-record-containing-a-list-of-non-empty-guids-inside-body";
						var payload = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuids(
							StringArchetype.Foo,
							new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(
								StringArchetype.Bar,
								new List<NonEmptyGuid>
								{
									NonEmptyGuid.FromString("6c2aa526-bd51-4436-8a5f-91814501b824"),
									NonEmptyGuid.FromString("18c309d3-5f3c-4f57-befb-904b51ead9e8")
								})).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync(
							requestUri,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}
				}
			}
		}

		public class Inside_Route
		{
			public class When_HttpVerb_IsGet
			{
			}
			public class When_HttpVerb_IsPost
			{
			}
		}

		public class Inside_Query
		{
			public class When_HttpVerb_IsGet
			{
			}
			public class When_HttpVerb_IsPost
			{
			}
		}
	}

	public class Return_CorrectlySerialized_Output
	{
		public class When_Output_Contains_ConstrainedType
		{
			public class As_RootObject
			{
			}
			public class As_APropertyOf_RootObject
			{
			}
		}
	}

	[Test]
	public async Task Receive_Guid_When_Guid_Is_Passed_As_HttpPostRequestBody()
	{
		// Arrange
		var validGuid = "bfa352fd-9e00-49a1-acb2-74f923aa578a";
		var requestUri = $"api/v1/stubbed-endpoints-specifically-created-for-tests/guid-inside-body/";
		var payload = $"\"{validGuid}\"";

		// Act
		var httpResponse = await TestServer.HttpClient.PostAsync(
			requestUri,
			payload.ToJsonContent());

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		Assert.That(await httpResponse.ToResponseObject<Guid>(), Is.EqualTo(Guid.Parse(validGuid)));
	}
}
