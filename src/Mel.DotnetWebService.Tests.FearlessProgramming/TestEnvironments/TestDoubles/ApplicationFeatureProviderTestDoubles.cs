using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

internal static class ApplicationFeatureProviderTestDouble
{
	public static IApplicationFeatureProvider<ControllerFeature> That_Adds_Controller<TController>()
		where TController : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		var testDouble = Substitute.For<IApplicationFeatureProvider<ControllerFeature>>();
		testDouble
			.When(_ => _.PopulateFeature(Arg.Any<IEnumerable<ApplicationPart>>(), Arg.Any<ControllerFeature>()))
			.Do(x => x.Arg<ControllerFeature>().Controllers.Add(typeof(TController).GetTypeInfo()));

		return testDouble;
	}
}
