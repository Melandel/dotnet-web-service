using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.ErrorHandling.ConstrainedTypes;

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
						OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/non-empty-guid-inside-body"].Operations[OperationType.Post];

						[Test]
						public void In_HttpRequest() => Assert.That((RouteDocumentation.RequestBody.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
						[Test]
						public void In_HttpResponse() => Assert.That((RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
					}
					public class As_APropertyOf_RootObject
					{
						OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-non-empty-guid-inside-body"].Operations[OperationType.Post];
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
				OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/non-empty-guid-inside-route/{nonEmptyGuid}"].Operations[OperationType.Get];

				[Test]
				public void In_HttpRequest() => Assert.That((RouteDocumentation.Parameters.First().Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
				[Test]
				public void In_HttpResponse() => Assert.That((RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
			}
			public class When_HttpVerb_IsPost
			{
				OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/non-empty-guid-inside-route"].Operations[OperationType.Post];

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
				OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/non-empty-guid-inside-query"].Operations[OperationType.Get];

				[Test]
				public void In_HttpRequest() => Assert.That((RouteDocumentation.Parameters.First().Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));

				[Test]
				public void In_HttpResponse()
				=> Assert.That((RouteDocumentation.Responses.First().Value.Content.First().Value.Schema.Example as OpenApiString).Value, Is.EqualTo("00000000-0000-0000-0000-000000000001"));
			}

			public class When_HttpVerb_IsPost
			{
				OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/non-empty-guid-inside-query"].Operations[OperationType.Post];

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
							OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/array-of-non-empty-guids-inside-body"].Operations[OperationType.Post];

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
							OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/list-of-non-empty-guids-inside-body"].Operations[OperationType.Post];

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
							OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/ienumerable-of-non-empty-guids-inside-body"].Operations[OperationType.Post];

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
							OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-an-array-of-non-empty-guids-inside-body"].Operations[OperationType.Post];
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
							OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-a-list-of-non-empty-guids-inside-body"].Operations[OperationType.Post];
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
							OpenApiOperation RouteDocumentation => TestServer.OpenApiDocument.Paths["/api/v1/stubbed-endpoints-specifically-created-for-tests/positional-record-containing-an-ienumerable-of-non-empty-guids-inside-body"].Operations[OperationType.Post];
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
