using static Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ControllerTestDoubles;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.DataValidity.ConstrainedTypes;

class ApiControllersShould : TestSuiteUsingTestServer
{
	public class Receive_CorrectlyDeserialized_Input
	{
		public class Inside_Body
		{
			public class When_HttpVerb_IsPost
			{
				public class When_Input_Contains_ConstrainedCollection
				{
					[Test]
					public async Task As_RootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostNonEmptyHashsetOfGuidsInsideBody);
						var payload = Some.Valid<NonEmptyHashSet<Guid>>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}
				}

				public class When_Input_Contains_ConstrainedType
				{
					[Test]
					public async Task As_RootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostNonEmptyGuidInsideBody);
						var payload = Some.Valid<NonEmptyGuid>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootArray()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostArrayOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<NonEmptyGuid[]>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootList()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostListOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<List<NonEmptyGuid>>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootHashSet()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostHashsetOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<HashSet<NonEmptyGuid>>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootEncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostEncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIenumerableParameterInsideBody);
						var payload = Some.Valid<ClassArchetype.EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootEncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostEncapsulationOfAHashsetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIenumerableParameterInsideBody);
						var payload = Some.Valid<ClassArchetype.EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootEncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostEncapsulationOfAHashsetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIenumerableOfGuidsParameterInsideBody);
						var payload = Some.Valid<ClassArchetype.EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootIEnumerable()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostIenumerableOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<HashSet<NonEmptyGuid>>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootDictionary()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostDictionaryWithStringKeysAndNonEmptyGuidValuesInsideBody);
						var payload = Some.Valid<Dictionary<string, NonEmptyGuid>>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideAnRootArrayOfArrays()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostArrayOfArraysOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<NonEmptyGuid[][]>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_RootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingNonEmptyGuidInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingANonEmptyGuid>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_RootObject_From_StringWithDifferentPropertyOrder()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingNonEmptyGuidInsideBody);
						var payloadAsObject = new ClassArchetype.PositionalRecordContainingANonEmptyGuid(
							"foo",
							NonEmptyGuid.ApplyConstraintsTo("ffabd9c8-dc4f-482f-9954-d6cd06816cc3"));
						var payloadAsStringWithPropertiesWrittenInDifferentOrder = "{\"SecondProperty\":\"ffabd9c8-dc4f-482f-9954-d6cd06816cc3\",\"FirstProperty\":\"foo\"}";

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payloadAsStringWithPropertiesWrittenInDifferentOrder.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payloadAsObject.GetStringRepresentation()));
					}

					[Test]
					public async Task As_APropertyOf_RootObject_SettersOnly()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostGetSetStyleClassContainingNonEmptyGuidInsideBody);
						var payload = Some.Valid<ClassArchetype.GetSetStyleClassContainingANonEmptyGuid>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_Objects_InsideRootArray()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostArrayOfPositionalRecordsContainingANonEmptyGuidInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingANonEmptyGuid[]>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_Objects_InsideRootList()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostListOfPositionalRecordsContainingANonEmptyGuidInsideBody);
						var payload = Some.Valid<List<ClassArchetype.PositionalRecordContainingANonEmptyGuid>>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_Objects_InsideRootIEnumerable()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostIenumerableOfPositionalRecordsContainingANonEmptyGuidInsideBody);
						var payload = Some.Valid<HashSet<ClassArchetype.PositionalRecordContainingANonEmptyGuid>>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AnArrayPropertyOf_RootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingAnArrayOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AnArrayPropertyOf_RootObject_NextToAnArrayOfInts()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingAnArrayOfNonEmptyGuidsAndAnArrayOfIntsInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsAndAnArrayOfInts>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AListPropertyOf_RootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingAListOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AnIEnumerablePropertyOf_RootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingAnIenumerableOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_ObjectContainedByRootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingPositionalRecordContainingNonEmptyGuidInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid>()
							.GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_ObjectContainedByObjects_InsideRootArray()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostArrayOfPositionalRecordsContainingPositionalRecordContainingNonEmptyGuidInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid[]>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_ObjectContainedByObjects_InsideRootList()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostListOfPositionalRecordsContainingPositionalRecordContainingNonEmptyGuidInsideBody);
						var payload = Some.Valid<List<ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid>>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_APropertyOf_ObjectContainedByObjects_InsideRootIEnumerable()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostIenumerableOfPositionalRecordsContainingPositionalRecordContainingNonEmptyGuidInsideBody);
						var payload = Some.Valid<HashSet<ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid>>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AnArrayPropertyOf_ObjectContainedByRootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingPositionalRecordContainingAnArrayOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuids>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AListPropertyOf_ObjectContainedByRootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingPositionalRecordContainingAListOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuids>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task As_AnIEnumerablePropertyOf_ObjectContainedByRootObject()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingPositionalRecordContainingAListOfNonEmptyGuidsInsideBody);
						var payload = Some.Valid<ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuids>().GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
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
		var validGuid = Some.Valid<Guid, NonEmptyGuid>();
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostGuidInsideBody);
		var payload = validGuid.GetStringRepresentation();

		// Act
		var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
			controllerMethod,
			payload.ToJsonContent());

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		Assert.That(await httpResponse.ToResponseObject<Guid>(), Is.EqualTo(validGuid));
	}
}
