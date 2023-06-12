using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public abstract class ApiController : ControllerBase
{
}
