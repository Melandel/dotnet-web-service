using System.Text.Json;
using System.Text.Encodings.Web;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Exceptions;

class ObjectConstructionException : Exception
{
	ObjectConstructionException(string message, Exception? innerException = null) : base(message, innerException)
	{
	}

	public static ObjectConstructionException WhenConstructing<TObjectUnderConstruction>(
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

	public static ObjectConstructionException WhenConstructing<TObjectUnderConstruction>(Exception innerException)
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
	=> JsonSerializer.Serialize(invalidValue, new JsonSerializerOptions(JsonSerializerDefaults.Web) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
}
