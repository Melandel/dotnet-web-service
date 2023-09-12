using System.Reflection;
using System.Runtime.CompilerServices;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;
using Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.HttpProblemTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Mel.DotnetWebService.Api.Controllers;

[ApiVersion("1")]
[Route("api/v{version:apiVersion}/http-problem-types/[action]")]
public class HttpProblemTypeProvider : ApiController
{
	readonly IHttpContextAccessor _httpContextAccessor;
	readonly IWebHostEnvironment _hostEnv;
	ControllerContext ReceivingControllerContext => _httpContextAccessor.HttpContext!.RetrieveReceivingControllerContext()!;
	HttpContext ReceivingHttpContext => ReceivingControllerContext.HttpContext;
	Uri BasApiUri => new Uri($"{ReceivingHttpContext.Request.Scheme}://{ReceivingHttpContext.Request.Host.Value}/");
	IUrlHelper ReceivingControllerUrl
	=> ReceivingHttpContext.RequestServices
		.GetRequiredService<IUrlHelperFactory>()
		.GetUrlHelper(ReceivingControllerContext);

	public HttpProblemTypeProvider(IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostingEnvironment)
	{
		_httpContextAccessor = httpContextAccessor;
		_hostEnv = webHostingEnvironment;
	}

	Uri BuildFullRouteToProblemType([CallerMemberName] string memberName = "")
	{
		var problemTypeActionName = memberName;
		var problemTypeControllerName = GetType().Name.Replace("Controller", "");
		var relativeRouteToProblemType = ReceivingControllerUrl.Action(problemTypeActionName, problemTypeControllerName);

		var fullRouteToProblemType = new Uri(BasApiUri, relativeRouteToProblemType);
		return fullRouteToProblemType;
	}

	[HttpGet("~/api/v{version:apiVersion}/http-problem-types")]
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

	[HttpGet]
	public HttpProblemType GetDeveloperMistake()
	=> new DeveloperMistake(BuildFullRouteToProblemType(), _hostEnv);

	[HttpGet]
	public HttpProblemType GetEnumValueReceivedFromInteger()
	=> new EnumValueReceivedFromInteger(BuildFullRouteToProblemType());
}
