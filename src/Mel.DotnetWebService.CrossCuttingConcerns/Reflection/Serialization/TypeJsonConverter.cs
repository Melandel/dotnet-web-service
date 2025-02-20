using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.Serialization;

class TypeJsonConverter: JsonConverter<Type>
{
	public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}

	public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
	=> writer.WriteRawValue($"\"{value.GetName()}\"");
}
