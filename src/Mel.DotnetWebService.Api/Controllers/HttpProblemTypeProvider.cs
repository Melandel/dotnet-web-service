using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mel.DotnetWebService.Api.ErrorHandling;
using Mel.DotnetWebService.Api.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Mel.DotnetWebService.Api.Controllers;

[ApiVersion("3")]
[ApiVersion("2", Deprecated = true)]
[ApiVersion("1", Deprecated = true)]
public class HttpProblemTypeProvider : ApiController
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	ControllerContext ReceivingControllerContext => _httpContextAccessor.HttpContext!.RetrieveReceivingControllerContext()!;
	HttpContext ReceivingHttpContext => ReceivingControllerContext.HttpContext;
	Uri BasApiUri => new Uri($"{ReceivingHttpContext.Request.Scheme}://{ReceivingHttpContext.Request.Host.Value}/");
	IUrlHelper ReceivingControllerUrl
	=> ReceivingHttpContext.RequestServices
		.GetRequiredService<IUrlHelperFactory>()
		.GetUrlHelper(ReceivingControllerContext);

	public HttpProblemTypeProvider(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	Uri BuildFullRouteToProblemType([CallerMemberName] string memberName = "")
	{
		var problemTypeActionName = memberName;
		var problemTypeControllerName = GetType().Name.Replace("Controller", "");
		var relativeRouteToProblemType = ReceivingControllerUrl.Action(problemTypeActionName, problemTypeControllerName);

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
	=> HttpProblemType.Create(
		BuildFullRouteToProblemType(),
		HttpStatusCode.InternalServerError,
		title: "An unexpected error took place.",
		description: "An error, flaw or fault in the design, development, or operation of computer software caused the application to produce an incorrect or unexpected result, or to behave in unintended ways.",
		DebuggingInformationName.StackTrace);

	[HttpGet("enum-value-received-from-integer")]
	public HttpProblemType GetEnumValueReceivedFromInteger()
	=> HttpProblemType.Create(
		BuildFullRouteToProblemType(),
		HttpStatusCode.BadRequest,
		title: "An enum value was passed as integer instead of being passed as a string.",
		description: "https://stackoverflow.com/questions/49562774/what-is-the-best-way-to-prohibit-integer-value-for-enum-actions-parameter",
		DebuggingInformationName.IntegerValue,
		DebuggingInformationName.EnumParameterName,
		DebuggingInformationName.AcceptedEnumValues);
}
