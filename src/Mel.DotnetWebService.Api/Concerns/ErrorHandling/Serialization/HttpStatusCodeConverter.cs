using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Serialization;
public class HttpStatusCodeConverter : JsonConverter<HttpStatusCode>
{
	public override HttpStatusCode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	=> throw new NotImplementedException("Unnecessary because Read is never supposed to be used.");

	public override void Write(Utf8JsonWriter writer, HttpStatusCode httpStatusCodeValue, JsonSerializerOptions options)
	=> writer.WriteNumberValue((int)httpStatusCodeValue);
}
