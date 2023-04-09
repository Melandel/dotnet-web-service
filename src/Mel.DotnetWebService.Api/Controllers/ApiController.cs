using Microsoft.AspNetCore.Mvc;

namespace Mel.DotnetWebService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
}
