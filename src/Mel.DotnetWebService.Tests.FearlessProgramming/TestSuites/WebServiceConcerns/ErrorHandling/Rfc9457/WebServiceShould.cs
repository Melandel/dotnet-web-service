using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction.HttpProblemTypeExtensionMember;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ControllerTestDoubles;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestSuites.WebServiceConcerns.ErrorHandling.Rfc9457;

class WebServiceShould : TestSuiteUsingTestServer
{
	public class Implementent_Rfc9457
	{
		public const string TestThatSpecificallyRequiresServiceNotInjected = $"{nameof(WebServiceShould)}.{nameof(WebServiceShould.Implementent_Rfc9457)}.{nameof(WebServiceShould.Implementent_Rfc9457.On_Invalid_DependencyInjectionConfiguration)}";
		public static readonly Type TypeOfServiceThatShouldNotBeInjected = typeof(SomeServiceTestDouble.Dummy_That_Is_NOT_Added_To_DependencyInjection);
		[Test]
		public void On_Invalid_DependencyInjectionConfiguration()
		{
			// Arrange
			var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.CallSomeService);

			// Act & Assert
			Assert.That(async() =>
			{
				var httpResponse = await TestServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod);
			// 👇 An exception is expected in the current test instead of a ProblemDetails.
			//    Justification: one can observe that if InMemoryTestServer throws at this point
			//      then the non-test server would actually return a ProblemDetail looking like the following:
			//      {
			//        "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
			//        "title": "System.InvalidOperationException",
			//        "status": 500,
			//        "detail": "No service for type [...] has been registered.",
			//         ...
			//      }
			}, Throws.Exception
				.AssignableTo<InvalidOperationException>()
				.With.Message.EqualTo("No service for type 'Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles.ISomeService' has been registered."));
		}

		[Test]
		public async Task On_Division_By_Zero()
		{
			// Arrange
			var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.DivisionByZero);

			// Act
			var httpResponse = await TestServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod);

			// Assert
			Assert.Multiple(() =>
			{
				ProblemDetails problemDetails = null;
				Assert.That(
					async () => problemDetails = await httpResponse.ToResponseObject<ProblemDetails>(),
					Throws.Nothing);

				var developerMistake = TestServer.GetHttpProblemTypeByName("developer-mistake");
				Assert.That(problemDetails.Type, Is.EqualTo(developerMistake.Uri.ToString()));
				Assert.That(problemDetails.Title, Is.EqualTo(developerMistake.Title));
				Assert.That(problemDetails.Status, Is.EqualTo((int)developerMistake.RecommendedHttpStatusCode));
				Assert.That(problemDetails.Detail, Is.Not.Null.And.Not.Empty);
			});
		}

		[Test]
		public async Task On_Invalid_Guid_Deserialization_From_Body()
		{
			// Arrange
			var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.GuidPassThrough);
			var invalidGuid = "I-am-an-invalid-guid";
			var payload = $@"{{ ""Guid"": ""{invalidGuid}"" }}";

			// Act
			var httpResponse = await TestServer.HttpClient.PostAsync<StubbedEndpointsSpecificallyCreatedForTests>(
				controllerMethod,
				payload.ToJsonContent());

			// Assert
			Assert.Multiple(() =>
			{
				ProblemDetails problemDetails = null;
				Assert.That(
					async () => problemDetails = await httpResponse.ToResponseObject<ProblemDetails>(),
					Throws.Nothing);
				Assert.That(problemDetails.Type, Is.EqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.1"));
				Assert.That(problemDetails.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
				Assert.That(problemDetails.Extensions, Does.ContainKey("errors"));
				Assert.That(problemDetails.Extensions, Does.ContainKey("traceId"));
			});
		}

		[Test]
		public async Task On_Invalid_Guid_Deserialization_From_QueryString()
		{
			// Arrange
			var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.GuidPassThrough);
			var invalidGuid = "I-am-an-invalid-guid";

			// Act
			var httpResponse = await TestServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod, $"?guid={invalidGuid}");

			// Assert
			Assert.Multiple(() =>
			{
				ProblemDetails problemDetails = null;
				Assert.That(
					async () => problemDetails = await httpResponse.ToResponseObject<ProblemDetails>(),
					Throws.Nothing);
				Assert.That(problemDetails.Type, Is.EqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.1"));
				Assert.That(problemDetails.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
				Assert.That(problemDetails.Extensions, Does.ContainKey("errors"));
				Assert.That(problemDetails.Extensions, Does.ContainKey("traceId"));
			});
		}

		[Test]
		public void On_ConstrainedType_Failed_Deserialization()
		{
			// Arrange
			var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.NonEmptyGuidPassThrough);

			// Act & Assert
			Assert.That(async() =>
			{
				var httpResponse = await TestServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod, "?nonEmptyGuid=00000000-0000-0000-0000-000000000000\"");
			}, Throws.Exception);
		}
	}

	[Test]
	public async Task Not_Leak_Debugging_Information_When_DeploymentEnvironment_Is_Production()
	{
		// Arrange
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.DivisionByZero);
		var testServer = TestServer;

		// Act & Assert
		var httpResponse = await TestServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod);

		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.Multiple(() =>
		{
			ProblemDetails problemDetails = null;
			Assert.That(
					() => problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent),
					Throws.Nothing);

			Assert.That(problemDetails, Is.Not.Null);
			Assert.That(responseContent, Does.Not.Contain(StackTrace.ToString()).And.Not.Contain(nameof(StubbedEndpointsSpecificallyCreatedForTests.DivisionByZero)));
			Assert.That(responseContent, Does.Contain(ExceptionsTypes.ToString()).And.Contain(nameof(DivideByZeroException)));
			Assert.That(responseContent, Does.Contain(ExceptionsAggregatedMessages.ToString()));
			Assert.That(responseContent, Does.Contain(ExceptionsAggregatedData.ToString()));
		});
	}

	[TestCaseSource(typeof(DeploymentEnvironments), nameof(DeploymentEnvironments.All_DeploymentEnvironments_Except_Production))]
	public async Task Return_Debugging_Information_When_DeploymentEnvironment_Is_Not_Production(DeploymentEnvironment deploymentEnvironment)
	{
		// Arrange
		var controllerMethod = nameof(StubbedEndpointsSpecificallyCreatedForTests.DivisionByZero);
		var testServer = InMemoryTestServer.Create(deploymentEnvironment);

		// Act & Assert
		var httpResponse = await testServer.HttpClient.GetAsync<StubbedEndpointsSpecificallyCreatedForTests>(controllerMethod);

		var responseContent = await httpResponse.GetContentAsString();

		// Assert
		Assert.Multiple(() =>
		{
			ProblemDetails problemDetails = null;
			Assert.That(
					() => problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(responseContent),
					Throws.Nothing);

			Assert.That(problemDetails, Is.Not.Null);
			Assert.That(responseContent, Does.Contain(StackTrace.ToString()).And.Contain(nameof(StubbedEndpointsSpecificallyCreatedForTests.DivisionByZero)));
			Assert.That(responseContent, Does.Contain(ExceptionsTypes.ToString()).And.Contain(nameof(DivideByZeroException)));
			Assert.That(responseContent, Does.Contain(ExceptionsAggregatedMessages.ToString()));
			Assert.That(responseContent, Does.Contain(ExceptionsAggregatedData.ToString()));
		});
	}
}
