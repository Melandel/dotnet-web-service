using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mel.DotnetWebService.Api.ErrorHandling;
using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.Controllers;

[ApiVersion("3")]
[ApiVersion("2", Deprecated = true)]
[ApiVersion("1", Deprecated = true)]
public class HttpProblemTypesController : ApiController
{
	readonly ControllerBase _controllerThatKnowsTheActionContext;
	Uri BasApiUri => new Uri($"{_controllerThatKnowsTheActionContext.Request.Scheme}://{_controllerThatKnowsTheActionContext.Request.Host.Value}/");

	public HttpProblemTypesController(ControllerBase? controllerThatInstantiatedThisClass = null)
	{
		_controllerThatKnowsTheActionContext = controllerThatInstantiatedThisClass switch
		{
			null => this,
			var instantiator => instantiator
		};
	}
	Uri BuildFullRouteToProblemType([CallerMemberName] string memberName = "")
	{
		var problemTypeActionName = memberName;
		var problemTypeControllerName = GetType().Name.Replace("Controller", "");
		var relativeRouteToProblemType = _controllerThatKnowsTheActionContext.Url.Action(problemTypeActionName, problemTypeControllerName);

		var fullRouteToProblemType = new Uri(BasApiUri, relativeRouteToProblemType);
		return fullRouteToProblemType;
	}

	[HttpGet]
	public IReadOnlyCollection<HttpProblemType> GetAllHttpProblemTypes()
	{
		var currentMethodName = MethodBase.GetCurrentMethod()!.Name;
		var methods = GetType()
			.GetMethods(BindingFlags.Instance | BindingFlags.Public)
			.Where(methodInfo => methodInfo.ReturnType == typeof(HttpProblemType));

		var httpProblemTypes = methods
			.Select(methodInfo => methodInfo.Invoke(obj: this, parameters:	 null))
			.Cast<HttpProblemType>()
			.ToArray();

		return httpProblemTypes;
	}

	[HttpGet("developer-mistake")]
	public HttpProblemType GetDeveloperMistake()
	=> HttpProblemType.CreateWithDefiningSpecification(
		BuildFullRouteToProblemType(),
		HttpStatusCode.InternalServerError,
		title: "An unexpected error took place.",
		definingSpecification: "An error, flaw or fault in the design, development, or operation of computer software caused the application to produce an incorrect or unexpected result, or to behave in unintended ways.");
}
