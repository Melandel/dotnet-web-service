using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.FrameworkExtension.Serialization;

static class JsonConverter
{
	public class That_Does_Not_Ignore_ProblemDetails_Extensions_Property : Newtonsoft.Json.JsonConverter
	{
		public override bool CanConvert(Type objectType)
		=> objectType == typeof(ProblemDetails);

		public override object ReadJson(
			JsonReader reader,
			Type objectType,
			object existingValue,
			JsonSerializer serializer)
		{
			var problemDetailsWithCorrectlyDeserializedExtensionsProperty = serializer.Deserialize<ProblemDetailsWithExtensionsPropertyAsJsonExtensionData>(reader);
			if (problemDetailsWithCorrectlyDeserializedExtensionsProperty == null)
			{
				return null;
			}

			var extensions = problemDetailsWithCorrectlyDeserializedExtensionsProperty.Extensions;
			foreach (var key in extensions.Keys)
			{
				var extensionValue = extensions[key];
				if (extensionValue is not JArray)
				{
					continue;
				}
				var array = (JArray) extensionValue;
				extensions[key] = array switch
				{
					_ when array.All(_ => _.Type is JTokenType.Boolean) => array.ToObject<bool[]>(),
					_ when array.All(_ => _.Type is JTokenType.Bytes) => array.ToObject<byte[]>(),
					_ when array.All(_ => _.Type is JTokenType.Date) => array.ToObject<DateTime[]>(),
					_ when array.All(_ => _.Type is JTokenType.Float) => array.ToObject<float[]>(),
					_ when array.All(_ => _.Type is JTokenType.Guid) => array.ToObject<Guid[]>(),
					_ when array.All(_ => _.Type is JTokenType.Integer) => array.ToObject<int[]>(),
					_ when array.All(_ => _.Type is JTokenType.String) => array.ToObject<string[]>(),
					_ when array.All(_ => _.Type is JTokenType.Uri) => array.ToObject<Uri[]>(),
					_ => array.ToObject<object[]>()
				};
			}

			var problemDetails = (ProblemDetails)existingValue ?? new ProblemDetails();
			problemDetailsWithCorrectlyDeserializedExtensionsProperty.CopyTo(problemDetails);

			return problemDetails;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}

			var problemDetails = (ProblemDetails)value;
			var problemDetailsWithCorrectlySerializedExtensionsProperty = new ProblemDetailsWithExtensionsPropertyAsJsonExtensionData(problemDetails);

			serializer.Serialize(writer, problemDetailsWithCorrectlySerializedExtensionsProperty);
		}

		class ProblemDetailsWithExtensionsPropertyAsJsonExtensionData
		{
			public string Type { get; set; }
			public string Title { get; set; }
			public int Status { get; set; }
			public string Detail { get; set; }
			public string Instance { get; set; }

			[JsonExtensionData]
			public IDictionary<string, object> Extensions { get; } = new Dictionary<string, object>(StringComparer.Ordinal);

			public ProblemDetailsWithExtensionsPropertyAsJsonExtensionData() { }
			public ProblemDetailsWithExtensionsPropertyAsJsonExtensionData(ProblemDetails problemDetails)
			{
				Detail = problemDetails.Detail;
				Instance = problemDetails.Instance;
				Status = problemDetails.Status.GetValueOrDefault();
				Title = problemDetails.Title;
				Type = problemDetails.Type;

				foreach (var kvp in problemDetails.Extensions)
				{
					Extensions[kvp.Key] = kvp.Value;
				}
			}

			public void CopyTo(ProblemDetails problemDetails)
			{
				problemDetails.Type = Type;
				problemDetails.Title = Title;
				problemDetails.Status = Status;
				problemDetails.Instance = Instance;
				problemDetails.Detail = Detail;

				foreach (var kvp in Extensions)
				{
					problemDetails.Extensions[kvp.Key] = kvp.Value;
				}
			}
		}
	}
}
