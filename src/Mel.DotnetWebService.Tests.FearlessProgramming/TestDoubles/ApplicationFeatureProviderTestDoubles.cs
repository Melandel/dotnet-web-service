using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestDoubles;

class ApplicationFeatureProviderThatAddsController<TController> : IApplicationFeatureProvider<ControllerFeature>
{
	public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
	=> feature.Controllers.Add(typeof(TController).GetTypeInfo());
}
