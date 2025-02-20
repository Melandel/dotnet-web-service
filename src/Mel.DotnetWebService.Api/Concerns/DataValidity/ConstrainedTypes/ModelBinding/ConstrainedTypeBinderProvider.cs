using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.ModelBinding;

public class ConstrainedTypeBinderProvider : IModelBinderProvider
{
	public IModelBinder GetBinder(ModelBinderProviderContext context)
	{
		if (context == null)
		{
			throw new ArgumentNullException(nameof(context));
		}

		if (ConstrainedTypeJsonConverter.IsAbleToConvert(context.Metadata.ModelType))
		{
			return new ConstrainedTypeModelBinder();
		}

		return null!;
	}
}
