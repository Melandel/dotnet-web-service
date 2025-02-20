using System.Text.Json;
using System.Text.Encodings.Web;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ErrorHandling;

public class ObjectConstructionException : Exception
{
	readonly List<string> _messages;
	ObjectConstructionException(string message, Exception? innerException = null) : base(message, innerException)
	{
		_messages = new List<string>() { message };
	}

	public static ObjectConstructionException WhenConstructingAMemberFor<TObjectUnderConstruction>(
		string memberUnderConstruction,
		object? invalidValue,
		string? ruleThatInvalidatesTheValue = null,
		Exception? innerException = null)
	{
		var errorDuringSpecificMemberConstruction = BuildMemberConstructionMessage(
			typeof(TObjectUnderConstruction).Name,
			typeof(TObjectUnderConstruction).Namespace!,
			memberUnderConstruction,
			invalidValue);

		return new(errorDuringSpecificMemberConstruction, innerException);
	}

	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(
		Exception? innerException = null,
		object? constructionMethodParameter = null,
		[CallerMemberName] string callerMethodName = "")
	{
		string errorMessage = BuildInstanceConstructionErrorMessage(
			typeof(TObjectUnderConstruction).Name,
			typeof(TObjectUnderConstruction).Namespace!,
			innerException,
			callerMethodName,
			constructionMethodParameter == null
				? Array.Empty<object?>()
				: new[] { constructionMethodParameter });

		return new(errorMessage, innerException);
	}

	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(
		Exception? innerException = null,
		IEnumerable<object?>? constructionMethodParameters = null,
		[CallerMemberName] string callerMethodName = "")
	{
		string errorMessage = BuildInstanceConstructionErrorMessage(
			typeof(TObjectUnderConstruction).Name,
			typeof(TObjectUnderConstruction).Namespace!,
			innerException,
			callerMethodName,
			constructionMethodParameters?.ToArray() ?? Array.Empty<object?>());

		return new(errorMessage, innerException);
	}

	public ObjectConstructionException EnrichWithInformationAbout<TObjectUnderConstruction>(
		string memberUnderConstruction,
		object? invalidValue,
		string? ruleThatInvalidatesTheValue = null,
		Exception? innerException = null,
		[CallerMemberName] string callerMethodName = "",
		params object?[] constructionMethodParameters)
	{
		_messages.Add(
			BuildMemberConstructionMessage(
				typeof(TObjectUnderConstruction).Name,
				typeof(TObjectUnderConstruction).Namespace!,
				memberUnderConstruction,
				invalidValue,
				ruleThatInvalidatesTheValue: null,
				callerMethodName,
				constructionMethodParameters));

		return this;
	}

	public ObjectConstructionException EnrichWithInformationAbout<TObjectUnderConstruction>(
		IEnumerable<object?> constructionMethodParameters,
		Exception? innerException = null,
		[CallerMemberName] string callerMethodName = "")
	{
		_messages.Add(
			BuildInstanceConstructionErrorMessage(
				typeof(TObjectUnderConstruction).Name,
				typeof(TObjectUnderConstruction).Namespace!,
				innerException,
				callerMethodName,
				constructionMethodParameters.ToArray()));

		return this;
	}

	public ObjectConstructionException EnrichWithInformationAbout<TObjectUnderConstruction>(
		object? constructionMethodParameter,
		Exception? innerException = null,
		[CallerMemberName] string callerMethodName = "")
	{
		_messages.Add(
			BuildInstanceConstructionErrorMessage(
				typeof(TObjectUnderConstruction).Name,
				typeof(TObjectUnderConstruction).Namespace!,
				innerException,
				callerMethodName,
				new[] { constructionMethodParameter }));

		return this;
	}

	static string BuildInstanceConstructionErrorMessage(
		string objectUnderConstruction,
		string objectUnderConstructionNamespace,
		Exception? innerException,
		string callerMethodName,
		object?[] constructionMethodParameters)
	{
		var sb = new System.Text.StringBuilder();

		sb.Append($"{objectUnderConstruction}: construction failed ");

		if (innerException != null)
		{
			sb.Append($"due to {innerException.GetType().Name} ");
		}

		sb.Append($"calling {BuildCallerMethodPartialSignature(objectUnderConstruction, callerMethodName, constructionMethodParameters)} ");

		if (constructionMethodParameters.Length == 1)
		{
			sb.Append($"on {Render(constructionMethodParameters.First())}");
		}
		else if (constructionMethodParameters.Length > 1)
		{
			sb.Append($"with the following parameters: {string.Join(" ; ", constructionMethodParameters.Select(Render))}");
		}

		if (!string.IsNullOrEmpty(objectUnderConstructionNamespace))
		{
			sb.Append($"[{objectUnderConstructionNamespace}]");
		}

		return sb.ToString();
	}

	static string BuildMemberConstructionMessage(
		string objectUnderConstruction,
		string objectUnderConstructionNamespace,
		string memberUnderConstruction,
		object? invalidValue,
		string? ruleThatInvalidatesTheValue = null,
		string callerMethodName = "",
		params object?[] constructionMethodParameters)
	{
		var sb = new System.Text.StringBuilder();

		sb.Append($"{objectUnderConstruction}: ");

		if (!string.IsNullOrEmpty(callerMethodName))
		{
			sb.Append($"when called from {BuildCallerMethodPartialSignature(objectUnderConstruction, callerMethodName, constructionMethodParameters)}, ");
		}

		sb.Append($"{memberUnderConstruction} cannot accept value {Render(invalidValue)} ");

		if (!string.IsNullOrEmpty(ruleThatInvalidatesTheValue))
		{
			sb.Append($"- {Format(ruleThatInvalidatesTheValue, objectUnderConstruction, memberUnderConstruction)} ");
		}

		if (!string.IsNullOrEmpty(objectUnderConstructionNamespace))
		{
			sb.Append($"[{objectUnderConstructionNamespace}]");
		}

		return sb.ToString();
	}

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

	static string BuildCallerMethodPartialSignature(
		string objectUnderConstruction,
		string callerMethodName,
		params object?[] constructionMethodParameters)
	=> $"{objectUnderConstruction}.{callerMethodName}({(constructionMethodParameters.Any() ? "..." : "")})";
}
