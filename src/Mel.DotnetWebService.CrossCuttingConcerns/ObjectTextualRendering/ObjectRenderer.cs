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
		var v when v.HasInstancePropertiesWithPublicGetter() && TrySerialize(invalidValue, out var serialized) => serialized,
		var v when v.HasUserDefinedConversions(out var converters) && TryRenderConverted(invalidValue, converters, out var renderedConversion) => renderedConversion,
		var v when v.HasConstructorWhoseParametersAllHaveAMatchingPropertyOrField(out var propertiesToRender, out var fieldsToRender)
			&& TryRenderMembers(invalidValue, propertiesToRender, fieldsToRender, out var renderedFromConstructorParams) => renderedFromConstructorParams,
		_ => invalidValue.ToString() ?? "null"
	};

	static bool TrySerialize(object invalidValue, out string serialized)
	{
		try
		{
			serialized = JsonSerializer.Serialize(
				invalidValue,
				new JsonSerializerOptions(JsonSerializerDefaults.Web)
				{
					Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
				});
			return true;
		}
		catch
		{
			serialized = "";
			return false;
		}
	}

	static bool TryRenderConverted(object invalidValue, MethodInfo[] converters, out string converted)
	{
		foreach (var converter in converters)
		{
			try
			{
				var convertedValue = converters.First().Invoke(null, new[] { invalidValue });
				converted = Render(convertedValue);
				return true;
			}
			catch
			{
			}
		}

		converted = "";
		return false;
	}

	static bool TryRenderMembers(object invalidValue, PropertyInfo[] propertiesToRender, FieldInfo[] fieldsToRender, out string rendered)
	{
		try
		{
			rendered = RenderMembers(invalidValue, propertiesToRender, fieldsToRender);
			return true;
		}
		catch (Exception)
		{
			rendered = "";
			return false;
		}
	}

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


