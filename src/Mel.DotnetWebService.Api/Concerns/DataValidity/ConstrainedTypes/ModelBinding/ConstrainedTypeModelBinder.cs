using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Encodings.Web;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity.ConstrainedTypes.ModelBinding;

public class ConstrainedTypeModelBinder : IModelBinder
{
	static readonly Type[] TypesThatGetDeserializedFromString = new[]
	{
		typeof(string),
		typeof(char),
		typeof(DateTime),
		typeof(DateTimeOffset),
		typeof(TimeSpan),
		typeof(Guid),
		typeof(Uri),
		typeof(Version),
		typeof(DateOnly),
		typeof(TimeOnly),
	};

	public async Task BindModelAsync(ModelBindingContext bindingContext)
	{
		try
		{
			var targetType = bindingContext.ModelType;
			var valueAsJson = await GetValueAsJsonFrom(bindingContext);

			var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.General) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
			serializerOptions.Converters.Add(new ConstrainedTypeJsonConverter());
			var deserialized = JsonSerializer.Deserialize(valueAsJson, targetType, serializerOptions);

			bindingContext.Result = ModelBindingResult.Success(deserialized);
		}
		catch (ObjectConstructionException objectConstructionException)
		{
			var error = string.Join(" | ", objectConstructionException.MessageLayers);

			bindingContext.ModelState.AddModelError(bindingContext.FieldName, error);
			bindingContext.Result = ModelBindingResult.Failed();
		}
	}

	async Task<string> GetValueAsJsonFrom(ModelBindingContext bindingContext)
	{
		if (bindingContext.BindingSource == BindingSource.Body)
		{
			var body = "";
			using (var sr = new StreamReader(bindingContext.HttpContext.Request.Body))
			{
				body = await sr.ReadToEndAsync();
			}

			return body;
		}
		else
		{
			var modelName = bindingContext.ModelName;
			var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
			var value = valueProviderResult.FirstValue ?? "";

			return bindingContext.ModelType switch
			{
				var t when t.IsAConstrainedType(out var rootType) => GetValueAsJsonFrom(value, rootType!),
				var t => GetValueAsJsonFrom(value, t)
			};
		}
	}

	string GetValueAsJsonFrom(string str, Type targetTypeRootType)
	=> TypesThatGetDeserializedFromString.Contains(targetTypeRootType)
		? $"\"{str}\""
		: str;
}
