using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;
using System.Text.Encodings.Web;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;

namespace Mel.DotnetWebService.Api;

[ApiVersion("1")]
public class FooController : ApiController
{
	[HttpPost]
	public Guid PostGuid([FromBody] Guid guid)
	=> guid;
	[HttpPost]
	public Guid PostGuids([FromBody] Guid[] guids)
	=> guids.Last();
	[HttpPost]
	public NonEmptyGuid PostNonEmptyGuid([FromBody] NonEmptyGuid nonEmptyGuid)
	=> nonEmptyGuid;
	[HttpPost]
	public NonEmptyGuid PostNonEmptyGuids([FromBody] NonEmptyGuid[] nonEmptyGuids)
	=> nonEmptyGuids.Last();
	[HttpGet]
	public NonEmptyGuid GetNonEmptyGuid([FromQuery] NonEmptyGuid nonEmptyGuid)
	=> nonEmptyGuid;

	[HttpGet("{nonEmptyGuid}")]
	public NonEmptyGuid GetNonEmptyGuidFromRoute([FromRoute] NonEmptyGuid nonEmptyGuid)
	=> nonEmptyGuid;

	[HttpPost]
	public NonEmptyGuid PostRecordContainingNonEmptyGuid([FromBody] RecordContainingNonEmptyGuid recordContainingNonEmptyGuid)
	=> recordContainingNonEmptyGuid.Value;

	[HttpPost]
	public Guid PostRecordContainingGuid([FromBody] RecordContainingGuid recordContainingGuid)
	=> recordContainingGuid.Value;

	[HttpPost]
	public NonEmptyGuid PostRecordsContainingNonEmptyGuid([FromBody] RecordContainingNonEmptyGuid[] recordsContainingNonEmptyGuid)
	=> recordsContainingNonEmptyGuid.Last().Value;

	[HttpPost]
	public Guid PostRecordsContainingGuid([FromBody] RecordContainingGuid[] recordsContainingGuid)
	=> recordsContainingGuid.Last().Value;
}

public record RecordContainingNonEmptyGuid(NonEmptyGuid Value);
public record RecordContainingGuid(Guid Value);
public class NonEmptyGuidBinderProvider : IModelBinderProvider
{
	public IModelBinder GetBinder(ModelBinderProviderContext context)
	{
		if (context == null)
		{
			throw new ArgumentNullException(nameof(context));
		}

		if (context.Metadata.ModelType == typeof(NonEmptyGuid))
		{
			return new NonEmptyGuidModelBinder();
		}

		return null;
	}
}

public class NonEmptyGuidModelBinder : IModelBinder
{
	public async Task BindModelAsync(ModelBindingContext bindingContext)
	{
		if (bindingContext.BindingSource == BindingSource.Body)
		{
			try
			{
				string body = "";
				using (var sr = new StreamReader(bindingContext.HttpContext.Request.Body))
				{
					body = await sr.ReadToEndAsync();
				}
				var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
				serializerOptions.Converters.Add(new ConstrainedTypeJsonConverter());
				bindingContext.Result = ModelBindingResult.Success(JsonSerializer.Deserialize<NonEmptyGuid>(body, serializerOptions));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				var error = string.Join(" | ", objectConstructionException.MessageLayers);

				bindingContext.ModelState.AddModelError(bindingContext.FieldName, error);
				bindingContext.Result = ModelBindingResult.Failed();
			}
		}
		else
		{
			var modelName = bindingContext.ModelName;
			var value = bindingContext.ValueProvider.GetValue(modelName);
			var result = value.FirstValue;

			try
			{
				bindingContext.Result = ModelBindingResult.Success(NonEmptyGuid.FromString(result));
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				var error = string.Join(" | ", objectConstructionException.MessageLayers);

				bindingContext.ModelState.AddModelError(modelName, error);
				bindingContext.Result = ModelBindingResult.Failed();
			}
		}
	}
}
