using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ControllerTestDoubles;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.DataValidity.ConstrainedTypes;

class OpenApiDocumentShould : TestSuiteUsingTestServer
{
	public class Document_ConstrainedTypes_Examples_As_Their_RootType
	{
		public class Inside_Body
		{
			public class When_HttpVerb_IsPost
			{
				public class When_Signature_Contains_ConstrainedType
				{
					public class As_RootObject
					{
						OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
							nameof(StubbedEndpointsSpecificallyCreatedForTests.PostNonEmptyGuidInsideBody),
							HttpMethod.Post);

						[Test]
						public void In_HttpRequest() => Assert.That((RouteDocumentation.RequestBody.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
						[Test]
						public void In_HttpResponse() => Assert.That((RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
					}
					public class As_APropertyOf_RootObject
					{
						OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
							nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingNonEmptyGuidInsideBody),
							HttpMethod.Post);
						string NameOfTheClassContainingConstrainedTypeProperty = nameof(ClassArchetype.PositionalRecordContainingANonEmptyGuid);
						string ConstrainedTypePropertyName => nameof(ClassArchetype.PositionalRecordContainingANonEmptyGuid.SecondProperty).WithFirstCharacterInLowerCase();

						[Test]
						public void In_HttpRequest()
						{
							Assert.That(RouteDocumentation.RequestBody.Content.First().Value.Schema.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));
							Assert.That((TestServer.OpenApiDocument.Components.Schemas[NameOfTheClassContainingConstrainedTypeProperty].Properties[ConstrainedTypePropertyName].Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
						}
						[Test]
						public void In_HttpResponse()
						{
							Assert.That(RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));
							Assert.That((TestServer.OpenApiDocument.Components.Schemas[NameOfTheClassContainingConstrainedTypeProperty].Properties[ConstrainedTypePropertyName].Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
						}
					}
				}
			}
		}

		public class Inside_Route
		{
			public class When_HttpVerb_IsGet
			{
				OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
					nameof(StubbedEndpointsSpecificallyCreatedForTests.GetNonEmptyGuidInsideRoute),
					"/{nonEmptyGuid}",
					HttpMethod.Get);

				[Test]
				public void In_HttpRequest() => Assert.That((RouteDocumentation.Parameters.First().Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				[Test]
				public void In_HttpResponse() => Assert.That((RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
			}
			public class When_HttpVerb_IsPost
			{
				OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
					nameof(StubbedEndpointsSpecificallyCreatedForTests.PostNonEmptyGuidInsideRoute),
					HttpMethod.Post);

				[Test]
				public void In_HttpRequest() => Assert.That((RouteDocumentation.Parameters.First().Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				[Test]
				public void In_HttpResponse() => Assert.That((RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
			}
		}

		public class Inside_Query
		{
			public class When_HttpVerb_IsGet
			{
				OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
					nameof(StubbedEndpointsSpecificallyCreatedForTests.GetNonEmptyGuidInsideQuery),
					HttpMethod.Get);

				[Test]
				public void In_HttpRequest() => Assert.That((RouteDocumentation.Parameters.First().Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));

				[Test]
				public void In_HttpResponse()
				=> Assert.That((RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
			}

			public class When_HttpVerb_IsPost
			{
				OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
					nameof(StubbedEndpointsSpecificallyCreatedForTests.PostNonEmptyGuidInsideQuery),
					HttpMethod.Post);

				[Test]
				public void In_HttpRequest() => Assert.That((RouteDocumentation.Parameters.First().Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));

				[Test]
				public void In_HttpResponse()
				=> Assert.That((RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
			}
		}

	}

	public class Document_CollectionsOfConstrainedTypes_Examples_As_CollectionsOfTheirRootType
	{
		public class Inside_Body
		{
			public class When_HttpVerb_IsPost
			{
				public class When_Signature_Contains_CollectionOfConstrainedTypes
				{
					public class As_RootObject
					{
						public class With_Array_Type
						{
							OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
								nameof(StubbedEndpointsSpecificallyCreatedForTests.PostArrayOfNonEmptyGuidsInsideBody),
								HttpMethod.Post);

							[Test]
							public void In_HttpRequest()
							{
								var collectionSchema = RouteDocumentation.RequestBody.Content.First().Value.Schema;
								Assert.That(collectionSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
							[Test]
							public void In_HttpResponse()
							{
								var collectionSchema = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
								Assert.That(collectionSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
						}
						public class With_List_Type
						{
							OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
								nameof(StubbedEndpointsSpecificallyCreatedForTests.PostListOfNonEmptyGuidsInsideBody),
								HttpMethod.Post);

							[Test]
							public void In_HttpRequest()
							{
								var collectionSchema = RouteDocumentation.RequestBody.Content.First().Value.Schema;
								Assert.That(collectionSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
							[Test]
							public void In_HttpResponse()
							{
								var collectionSchema = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
								Assert.That(collectionSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
						}
						public class With_IEnumerable_Type
						{
							OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
								nameof(StubbedEndpointsSpecificallyCreatedForTests.PostIenumerableOfNonEmptyGuidsInsideBody),
								HttpMethod.Post);

							[Test]
							public void In_HttpRequest()
							{
								var collectionSchema = RouteDocumentation.RequestBody.Content.First().Value.Schema;
								Assert.That(collectionSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
							[Test]
							public void In_HttpResponse()
							{
								var collectionSchema = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
								Assert.That(collectionSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
						}
					}
					public class As_APropertyOf_RootObject
					{
						public class With_Array_Type
						{
							OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
								nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingAnArrayOfNonEmptyGuidsInsideBody),
								HttpMethod.Post);
							string NameOfTheClassContainingConstrainedTypeProperty = nameof(ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids);
							string ConstrainedTypePropertyName => nameof(ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids.SecondProperty).WithFirstCharacterInLowerCase();

							[Test]
							public void In_HttpRequest()
							{
								Assert.That(RouteDocumentation.RequestBody.Content.First().Value.Schema.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = TestServer.OpenApiDocument.Components.Schemas[NameOfTheClassContainingConstrainedTypeProperty];
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionOfConstrainedTypesSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
							[Test]
							public void In_HttpResponse()
							{
								Assert.That(RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = TestServer.OpenApiDocument.Components.Schemas[NameOfTheClassContainingConstrainedTypeProperty];
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionOfConstrainedTypesSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
						}

						public class With_List_Type
						{
							OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
								nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingAListOfNonEmptyGuidsInsideBody),
								HttpMethod.Post);
							string NameOfTheClassContainingConstrainedTypeProperty = nameof(ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids);
							string ConstrainedTypePropertyName => nameof(ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids.SecondProperty).WithFirstCharacterInLowerCase();

							[Test]
							public void In_HttpRequest()
							{
								Assert.That(RouteDocumentation.RequestBody.Content.First().Value.Schema.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = TestServer.OpenApiDocument.Components.Schemas[NameOfTheClassContainingConstrainedTypeProperty];
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionOfConstrainedTypesSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
							[Test]
							public void In_HttpResponse()
							{
								Assert.That(RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = TestServer.OpenApiDocument.Components.Schemas[NameOfTheClassContainingConstrainedTypeProperty];
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionOfConstrainedTypesSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
						}

						public class With_IEnumerable_Type
						{
							OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
								nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingAnIenumerableOfNonEmptyGuidsInsideBody),
								HttpMethod.Post);
							string NameOfTheClassContainingConstrainedTypeProperty = nameof(ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids);
							string ConstrainedTypePropertyName => nameof(ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids.SecondProperty).WithFirstCharacterInLowerCase();

							[Test]
							public void In_HttpRequest()
							{
								Assert.That(RouteDocumentation.RequestBody.Content.First().Value.Schema.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = TestServer.OpenApiDocument.Components.Schemas[NameOfTheClassContainingConstrainedTypeProperty];
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionOfConstrainedTypesSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
							[Test]
							public void In_HttpResponse()
							{
								Assert.That(RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = TestServer.OpenApiDocument.Components.Schemas[NameOfTheClassContainingConstrainedTypeProperty];
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));
								Assert.That((collectionOfConstrainedTypesSchema.Items.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
						}
					}
				}
			}
		}
	}
}
