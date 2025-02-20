using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestDoubles;

record ApplicationFeatureProviderTestDouble<TController>
	: TestDouble<IApplicationFeatureProvider<ControllerFeature>, ApplicationFeatureProviderTestDouble<TController>>,
		IApplicationFeatureProvider<ControllerFeature>
{
	public string Behavior
	{
		get;
		init => field = IsStub ? value : throw new InvalidOperationException();
	}

	public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
	{
		if (Behavior == "add controller")
		{
			feature.Controllers.Add(typeof(TController).GetTypeInfo());
			return;
		}

		throw new NotImplementedException();
	}
}
