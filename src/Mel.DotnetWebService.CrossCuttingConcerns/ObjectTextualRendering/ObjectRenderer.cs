using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ObjectTextualRendering;

static class ObjectRenderer
{
	public static string Render(object? invalidValue)
	=> invalidValue switch
	{
		null => "null",
		var v when v.HasInstancePropertiesWithPublicGetter() => JsonSerializer.Serialize(invalidValue, new JsonSerializerOptions(JsonSerializerDefaults.Web) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }),
		var v when v.HasUserDefinedConversions(out var converters) => Render(converters.First().Invoke(null, new[] { invalidValue })!),
		var v when v.HasConstructorWhoseParametersAllHaveAMatchingPropertyOrField(out var propertiesToRender, out var fieldsToRender) => RenderMembers(invalidValue, propertiesToRender, fieldsToRender),
		_ => invalidValue.ToString() ?? "null"
	};

	static string RenderMembers(object invalidValue, PropertyInfo[] propertiesToRender, FieldInfo[] fieldsToRender)
	=> (propertiesToRender, fieldsToRender) switch
	{
		([var encapsulatedProperty], _) => Render(encapsulatedProperty.GetValue(invalidValue)),
		(_, [var encapsulatedField]) => Render(encapsulatedField.GetValue(invalidValue)),
		_ => RenderComplexObject(invalidValue, propertiesToRender, fieldsToRender)
	};

	static string RenderComplexObject(object invalidValue, PropertyInfo[] propertiesToRender, FieldInfo[] fieldsToRender)
	{
		var renderedFields = fieldsToRender.Select(field => $"\"{field.Name}\": {Render(field.GetValue(invalidValue))}");
		var renderedProperties = propertiesToRender.Select(property => $"\"{property.Name}\": {Render(property.GetValue(invalidValue))}");
		var renderedMembers = renderedFields.Concat(renderedProperties);

		return $"{{ {string.Join(", ", renderedMembers)} }}";
	}
}
