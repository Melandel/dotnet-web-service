using System.Reflection;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ErrorHandling;

public static class ObjectConstructionException
{
	public static ObjectConstructionException<TObjectUnderConstruction> WhenConstructing<TObjectUnderConstruction>(
		string memberUnderConstruction,
		object? invalidValue,
		string? ruleThatInvalidatesTheValue = null,
		Exception? innerException = null)
	=> new(
		ruleThatInvalidatesTheValue switch
		{
			null => $"{typeof(TObjectUnderConstruction).Name}: {memberUnderConstruction} cannot accept value {Render(invalidValue)} [{typeof(TObjectUnderConstruction).Namespace}]",
			_ =>    $"{typeof(TObjectUnderConstruction).Name}: {memberUnderConstruction} cannot accept value {Render(invalidValue)} - {Format(ruleThatInvalidatesTheValue, typeof(TObjectUnderConstruction).Name, memberUnderConstruction)} [{typeof(TObjectUnderConstruction).Namespace}]"
		},
		innerException);

	public static ObjectConstructionException<TObjectUnderConstruction> WhenConstructing<TObjectUnderConstruction>(Exception innerException)
	=> new(
		$"{typeof(TObjectUnderConstruction).Name}: construction failed due to a {innerException.GetType().Name} somewhere [{typeof(TObjectUnderConstruction).Namespace}]",
		innerException);

	static string? Format(string? rule, string objectUnderConstruction, string memberUnderConstruction)
	=> rule switch
	{
		null => null,
		_ => rule
			.Replace("{type}", objectUnderConstruction)
			.Replace("{member}", memberUnderConstruction)
	};

	static string Render(object? invalidValue)
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
