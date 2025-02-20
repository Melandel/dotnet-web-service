using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

static class JsonSerializerOptionsExtensionMethods
{
	public static JsonSerializerOptions Without<TConverter>(this JsonSerializerOptions options)
	{
		var converterToRemove = options.Converters.FirstOrDefault(converter => converter.GetType().IsAssignableTo(typeof(TConverter)));
		if (converterToRemove == null)
		{
			return options;
		}

		var opt = new JsonSerializerOptions(options);
		opt.Converters.Remove(converterToRemove);
		return opt;
	}
}
