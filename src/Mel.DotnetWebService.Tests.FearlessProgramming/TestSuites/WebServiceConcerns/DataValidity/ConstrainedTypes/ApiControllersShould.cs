using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Guids;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes.ClassArchetype;
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
						var payload = NonEmptyHashSet.Storing(Guid.Parse("f0cae17c-0219-4a8f-9966-6b34601df708"), Guid.Parse("d45ebe7d-b983-432b-87b6-b7ccf7d514e9")).GetStringRepresentation();

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
						var payload = NonEmptyGuid.FromString("3a6f9351-711b-4244-9124-9fec321a9c3c").GetStringRepresentation();

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
						var payload = new[]
						{
							NonEmptyGuid.FromString("ecc1b56d-b71c-47ed-89e5-8181d741638e"),
							NonEmptyGuid.FromString("23f7a299-109b-4962-ac08-5b038186981d")
						}.GetStringRepresentation();

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
						var payload = new List<NonEmptyGuid>
						{
							NonEmptyGuid.FromString("eb44460e-8ddd-436a-b845-99543ba9efad"),
							NonEmptyGuid.FromString("eede98f8-0b98-40ee-a86b-67b4a37a2359")
						}.GetStringRepresentation();

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
						var payload = new HashSet<NonEmptyGuid>
						{
							NonEmptyGuid.FromString("11345d6b-4048-43c4-ac9e-b963c59d2159"),
							NonEmptyGuid.FromString("b04128bd-101e-46ab-a7c1-bcadac6f9aa1")
						}.GetStringRepresentation();

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
						var payload = EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter.From(
						new[] {
							NonEmptyGuid.FromString("dc3bd957-b3ac-411b-bc6d-e6606b684aea"),
							NonEmptyGuid.FromString("84443d55-f7a5-43bb-8ae3-7914f23bd804")
						}).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootEncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostEncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameterInsideBody);
						var payload = EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter.From(
						new[] {
							NonEmptyGuid.FromString("278b6e64-3610-4c4e-b396-e7d9b6b75ae6"),
							NonEmptyGuid.FromString("1c9fc2bc-3d9f-4b5a-a204-7c8c609e79ac")
						}).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootEncapsulationOfAnIenumerableOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostEncapsulationOfAnIenumerableOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameterInsideBody);
						var payload = EncapsulationOfAnIEnumerableOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter.From(
						new[] {
							NonEmptyGuid.FromString("0805fe6d-e0bc-4d5c-88cf-edc7a3b02998"),
							NonEmptyGuid.FromString("0007c032-bc7f-45de-b110-08b7468375d8")
						}).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}

					[Test]
					public async Task InsideARootEncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostEncapsulationOfAHashsetOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameterInsideBody);
						var payload = EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter.From(
						new[] {
							NonEmptyGuid.FromString("a17669ca-c484-4dbd-b9d8-1f288ed40d93"),
							NonEmptyGuid.FromString("e07727f6-264f-4c17-828c-7cf743c2b82a")
						}).GetStringRepresentation();

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
						var payload = EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter.From(
						new[] {
							NonEmptyGuid.FromString("d527d99b-5343-4fed-8138-dceabb128293"),
							NonEmptyGuid.FromString("23333b57-1e05-419f-bf74-a9faa7a315c9")
						}).GetStringRepresentation();

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
						var payload = EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter.From(
						new Guid[] {
							Guid.Parse("ee3a2e5d-bc16-482b-a1bf-94855a99e970"),
							Guid.Parse("f91b853f-f86f-4798-9aac-14ceafa7715b")
						}).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}
					[Test]
					public async Task InsideARootEncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnArrayParameter()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostEncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnArrayParameterInsideBody);
						var payload = new EncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnArrayParameter(
						new[] {
							NonEmptyGuid.FromString("6e234afd-b9c7-4c71-a190-4051f8af13eb"),
							NonEmptyGuid.FromString("ecd1fb80-983b-41c9-adc0-cea30574e94b")
						}).GetStringRepresentation();

						// Act
						var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
							controllerMethod,
							payload.ToJsonContent());

						// Assert
						Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
						Assert.That(await httpResponse.GetContentAsString(), Is.EqualTo(payload));
					}


					[Test]
					public async Task InsideARootEncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnIenumerableParameter()
					{
						// Arrange
						var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostEncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnIenumerableParameterInsideBody);
						var payload = new EncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnIEnumerableParameter(
						new[] {
							NonEmptyGuid.FromString("ff93b29d-2679-478a-bff0-082894a87ec6"),
							NonEmptyGuid.FromString("b492e50d-6333-4850-899c-81ef2b106a62")
						}).GetStringRepresentation();

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
						var payload = new HashSet<NonEmptyGuid>
						{
							NonEmptyGuid.FromString("eb44460e-8ddd-436a-b845-99543ba9efad"),
							NonEmptyGuid.FromString("eede98f8-0b98-40ee-a86b-67b4a37a2359")
						}.GetStringRepresentation();

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
						var payload = new Dictionary<string, NonEmptyGuid>
						{
							{ StringArchetype.Foo, NonEmptyGuid.FromString("cb023949-05d8-41f5-957a-5628fc9fa83d") },
							{ StringArchetype.Bar, NonEmptyGuid.FromString("3e00b96a-8e00-462d-a611-639634ded4c6") },
						}.GetStringRepresentation();

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
						var payload = new[]
						{
							new[]
							{
								NonEmptyGuid.FromString("d4d10a51-8f95-455c-86b3-d34f1bcb1800"),
								NonEmptyGuid.FromString("63a83cda-b5c8-4c05-9772-90e199cdfe84")
							},
							new[]
							{
								NonEmptyGuid.FromString("90f68541-484f-4f21-9c2d-d577eead9a35"),
								NonEmptyGuid.FromString("494ec89b-1f20-49dd-9248-cb4186281715")
							}
						}.GetStringRepresentation();

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
						var payload = new ClassArchetype.PositionalRecordContainingANonEmptyGuid(
							StringArchetype.Foo,
							NonEmptyGuid.FromString("1184cb10-0428-4d35-b6e0-ebe9413bfcbb")).GetStringRepresentation();

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
							StringArchetype.Foo,
							NonEmptyGuid.FromString("ffabd9c8-dc4f-482f-9954-d6cd06816cc3"));
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
						var payload = new ClassArchetype.GetSetStyleClassContainingANonEmptyGuid
						{
							FirstProperty = StringArchetype.Foo,
							SecondProperty = NonEmptyGuid.FromString("6fb00d4b-4688-437b-a45e-0febeedceed6")
						}.GetStringRepresentation();

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
						var payload = new[]
						{
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("e1d9d98e-6752-43dd-af10-d66dbf9b5037")),
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("a3ea4100-32b5-41e3-9a76-f67e8b350e02"))
						}.GetStringRepresentation();

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
						var payload = new List<ClassArchetype.PositionalRecordContainingANonEmptyGuid>
						{
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("924c9966-2d88-4840-959d-ddb2ecf8af59")),
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("aa708279-389a-46de-92fe-14d3522bcbe0"))
						}.GetStringRepresentation();

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
						var payload = new HashSet<ClassArchetype.PositionalRecordContainingANonEmptyGuid>
						{
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Foo, NonEmptyGuid.FromString("ca727cfa-43c0-40de-961f-de244b82c5d0")),
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(StringArchetype.Bar, NonEmptyGuid.FromString("7851c746-f86e-4cc1-a290-6b2eabea35c8"))
						}.GetStringRepresentation();

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
						var payload = new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids(
							StringArchetype.Foo,
							new[]
							{
								NonEmptyGuid.FromString("49c27a56-8e4a-46fc-b60a-d74b4bc6692e"),
								NonEmptyGuid.FromString("d8ad6467-eb49-4d3c-bfac-3a6f4aeb11b4"),
							}
						).GetStringRepresentation();

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
						var payload = new ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuidsAndAnArrayOfInts(
							StringArchetype.Foo,
							new[]
							{
								NonEmptyGuid.FromString("d3b75988-820c-4726-8db1-028305432428"),
								NonEmptyGuid.FromString("78fde2ba-cb10-4706-8053-cd1a78cb310a"),
							},
							Enumerable.Range(1,3).ToArray()
						).GetStringRepresentation();

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
						var payload = new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(
							StringArchetype.Foo,
							new List<NonEmptyGuid>
							{
								NonEmptyGuid.FromString("49c27a56-8e4a-46fc-b60a-d74b4bc6692e"),
								NonEmptyGuid.FromString("d8ad6467-eb49-4d3c-bfac-3a6f4aeb11b4"),
							}).GetStringRepresentation();

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
						var payload = new ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids(
							StringArchetype.Foo,
							new HashSet<NonEmptyGuid>
							{
								NonEmptyGuid.FromString("767ee8ed-069c-4f1f-a4f6-23f13940f0ca"),
								NonEmptyGuid.FromString("05f97c30-a54f-4e33-963a-27dce2902960"),
							}).GetStringRepresentation();

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
						var payload = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid(
							StringArchetype.Bar,
							new ClassArchetype.PositionalRecordContainingANonEmptyGuid(
								StringArchetype.Foo,
								NonEmptyGuid.FromString("b5fcd95a-7ace-417f-8214-2590fdbd315e")))
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
						var payload = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuids(
							StringArchetype.Foo,
							new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(
								StringArchetype.Bar,
								new List<NonEmptyGuid>
								{
									NonEmptyGuid.FromString("cb894f78-bffd-4c97-9e98-a17d96f0bed7"),
									NonEmptyGuid.FromString("27d88795-db6c-4d35-ac7d-9e2977a15102")
								})).GetStringRepresentation();

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
						var payload = new ClassArchetype.PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuids(
							StringArchetype.Foo,
							new ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids(
								StringArchetype.Bar,
								new List<NonEmptyGuid>
								{
									NonEmptyGuid.FromString("402f1135-521d-4017-bd7b-23559fa38375"),
									NonEmptyGuid.FromString("0b876528-8064-4cce-bbd9-ef66ebd6c29a")
								})).GetStringRepresentation();

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
		var validGuid = "bfa352fd-9e00-49a1-acb2-74f923aa578a";
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.PostGuidInsideBody);
		var payload = $"\"{validGuid}\"";

		// Act
		var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
			controllerMethod,
			payload.ToJsonContent());

		// Assert
		Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		Assert.That(await httpResponse.ToResponseObject<Guid>(), Is.EqualTo(Guid.Parse(validGuid)));
	}
}
