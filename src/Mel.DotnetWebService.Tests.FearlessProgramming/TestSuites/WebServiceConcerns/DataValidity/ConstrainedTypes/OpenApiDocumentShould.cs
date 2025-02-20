using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ControllerTestDoubles;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.DataValidity.ConstrainedTypes;

class OpenApiDocumentShould : TestSuiteUsingTestServer
{
	static OpenApiSchema GetSchemaReferencedBy(OpenApiSchema schemaConstainingReference)
	=> TestServer.OpenApiDocument.Components.Schemas[schemaConstainingReference.Reference.Id];

	static string GetExampleAsString(OpenApiSchema schemaContainingExample)
	=> (schemaContainingExample.Example as OpenApiString).Value;

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
						public void In_HttpRequest()
						{
							var bodySchema = RouteDocumentation.RequestBody.Content.First().Value.Schema;
							var nonEmptyGuidSchema = GetSchemaReferencedBy(bodySchema);
							Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
						}

						[Test]
						public void In_HttpResponse()
						{
							var responseSchema = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
							var nonEmptyGuidSchema = GetSchemaReferencedBy(responseSchema);
							Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
						}
					}
					public class As_APropertyOf_RootObject
					{
						OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
							nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingNonEmptyGuidInsideBody),
							HttpMethod.Post);
						readonly string NameOfTheClassContainingConstrainedTypeProperty = nameof(ClassArchetype.PositionalRecordContainingANonEmptyGuid);
						readonly string ConstrainedTypePropertyName                     = nameof(ClassArchetype.PositionalRecordContainingANonEmptyGuid.SecondProperty).WithFirstCharacterInLowerCase();

						[Test]
						public void In_HttpRequest()
						{
							var schemaReferencingBodyType = RouteDocumentation.RequestBody.Content.First().Value.Schema;
							Assert.That(schemaReferencingBodyType.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

							var bodySchema = GetSchemaReferencedBy(schemaReferencingBodyType);
							var schemaReferencingNonEmptyGuid = bodySchema.Properties[ConstrainedTypePropertyName];
							var nonEmptyGuidSchema = GetSchemaReferencedBy(schemaReferencingNonEmptyGuid);
							Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
						}

						[Test]
						public void In_HttpResponse()
						{
							var schemaReferencingResponseType = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
							Assert.That(schemaReferencingResponseType.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

							var responseSchema = GetSchemaReferencedBy(schemaReferencingResponseType);
							var schemaReferencingNonEmptyGuid = responseSchema.Properties[ConstrainedTypePropertyName];
							var nonEmptyGuidSchema = GetSchemaReferencedBy(schemaReferencingNonEmptyGuid);
							Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
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
				public void In_HttpRequest()
				{
					var routeParameterSchema = RouteDocumentation.Parameters.First().Schema;
					var nonEmptyGuidSchema = GetSchemaReferencedBy(routeParameterSchema);
					Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				}

				[Test]
				public void In_HttpResponse()
				{
					var schemaReferencingResponseType = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
					var nonEmptyGuidSchema = GetSchemaReferencedBy(schemaReferencingResponseType);
					Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				}
			}
			public class When_HttpVerb_IsPost
			{
				OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
					nameof(StubbedEndpointsSpecificallyCreatedForTests.PostNonEmptyGuidInsideRoute),
					HttpMethod.Post);

				[Test]
				public void In_HttpRequest()
				{
					var routeParameterSchema = RouteDocumentation.Parameters.First().Schema;
					var nonEmptyGuidSchema = GetSchemaReferencedBy(routeParameterSchema);
					Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				}

				[Test]
				public void In_HttpResponse()
				{
					var responseSchema = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
					var nonEmptyGuidSchema = GetSchemaReferencedBy(responseSchema);
					Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				}
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
				public void In_HttpRequest()
				{
					var routeParameterSchema = RouteDocumentation.Parameters.First().Schema;
					var nonEmptyGuidSchema = GetSchemaReferencedBy(routeParameterSchema);
					Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				}

				[Test]
				public void In_HttpResponse()
				{
					var schemaReferencingNonEmptyGuid = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
					var nonEmptyGuidSchema = GetSchemaReferencedBy(schemaReferencingNonEmptyGuid);
					Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				}
			}

			public class When_HttpVerb_IsPost
			{
				OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
					nameof(StubbedEndpointsSpecificallyCreatedForTests.PostNonEmptyGuidInsideQuery),
					HttpMethod.Post);

				[Test]
				public void In_HttpRequest()
				{
					var routeParameterSchema = RouteDocumentation.Parameters.First().Schema;
					var nonEmptyGuidSchema = GetSchemaReferencedBy(routeParameterSchema);
					Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				}

				[Test]
				public void In_HttpResponse()
				{
					var schemaReferencingNonEmptyGuid = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
					var nonEmptyGuidSchema = GetSchemaReferencedBy(schemaReferencingNonEmptyGuid);
					Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				}
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

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}

							[Test]
							public void In_HttpResponse()
							{
								var collectionSchema = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
								Assert.That(collectionSchema.Type, Is.EqualTo("array"));

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
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

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}

							[Test]
							public void In_HttpResponse()
							{
								var collectionSchema = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
								Assert.That(collectionSchema.Type, Is.EqualTo("array"));

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
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

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}

							[Test]
							public void In_HttpResponse()
							{
								var collectionSchema = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
								Assert.That(collectionSchema.Type, Is.EqualTo("array"));

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
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
							readonly string NameOfTheClassContainingConstrainedTypeProperty = nameof(ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids);
							string ConstrainedTypePropertyName => nameof(ClassArchetype.PositionalRecordContainingAnArrayOfNonEmptyGuids.SecondProperty).WithFirstCharacterInLowerCase();

							[Test]
							public void In_HttpRequest()
							{
								var schemaReferencingContainingRecordType = RouteDocumentation.RequestBody.Content.First().Value.Schema;
								Assert.That(schemaReferencingContainingRecordType.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = GetSchemaReferencedBy(schemaReferencingContainingRecordType);
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That(collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionOfConstrainedTypesSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}

							[Test]
							public void In_HttpResponse()
							{
								var schemaReferencingContainingRecordType = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
								Assert.That(schemaReferencingContainingRecordType.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = GetSchemaReferencedBy(schemaReferencingContainingRecordType);
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionOfConstrainedTypesSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
						}

						public class With_List_Type
						{
							OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
								nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingAListOfNonEmptyGuidsInsideBody),
								HttpMethod.Post);
							readonly string NameOfTheClassContainingConstrainedTypeProperty = nameof(ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids);
							string ConstrainedTypePropertyName => nameof(ClassArchetype.PositionalRecordContainingAListOfNonEmptyGuids.SecondProperty).WithFirstCharacterInLowerCase();

							[Test]
							public void In_HttpRequest()
							{
								var schemaReferencingContainingRecordType = RouteDocumentation.RequestBody.Content.First().Value.Schema;
								Assert.That(schemaReferencingContainingRecordType.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = GetSchemaReferencedBy(schemaReferencingContainingRecordType);
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionOfConstrainedTypesSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}

							[Test]
							public void In_HttpResponse()
							{
								var schemaReferencingContainingRecordType = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
								Assert.That(schemaReferencingContainingRecordType.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = GetSchemaReferencedBy(schemaReferencingContainingRecordType);
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionOfConstrainedTypesSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
						}

						public class With_IEnumerable_Type
						{
							OpenApiOperation RouteDocumentation => TestServer.GetRouteDocumentation<StubbedEndpointsSpecificallyCreatedForTests>(
								nameof(StubbedEndpointsSpecificallyCreatedForTests.PostPositionalRecordContainingAnIenumerableOfNonEmptyGuidsInsideBody),
								HttpMethod.Post);
							readonly string NameOfTheClassContainingConstrainedTypeProperty = nameof(ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids);
							string ConstrainedTypePropertyName => nameof(ClassArchetype.PositionalRecordContainingAnIEnumerableOfNonEmptyGuids.SecondProperty).WithFirstCharacterInLowerCase();

							[Test]
							public void In_HttpRequest()
							{
								var schemaReferencingContainingRecordType = RouteDocumentation.RequestBody.Content.First().Value.Schema;
								Assert.That(schemaReferencingContainingRecordType.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = GetSchemaReferencedBy(schemaReferencingContainingRecordType);
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionOfConstrainedTypesSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}

							[Test]
							public void In_HttpResponse()
							{
								var schemaReferencingContainingRecordType = RouteDocumentation.Responses.First().Value.Content.First().Value.Schema;
								Assert.That(schemaReferencingContainingRecordType.Reference.Id, Is.EqualTo(NameOfTheClassContainingConstrainedTypeProperty));

								var containingRecordSchema = GetSchemaReferencedBy(schemaReferencingContainingRecordType);
								var collectionOfConstrainedTypesSchema = containingRecordSchema.Properties[ConstrainedTypePropertyName];
								Assert.That( collectionOfConstrainedTypesSchema.Type, Is.EqualTo("array"));

								var nonEmptyGuidSchema = GetSchemaReferencedBy(collectionOfConstrainedTypesSchema.Items);
								Assert.That(GetExampleAsString(nonEmptyGuidSchema), Is.EqualTo("00000000-0000-0000-0000-000000000001"));
							}
						}
					}
				}
			}
		}
	}
}

