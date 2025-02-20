using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.Serialization;

class MethodInfoJsonConverter: JsonConverter<MethodInfo>
{
	public override MethodInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}

	public override void Write(Utf8JsonWriter writer, MethodInfo value, JsonSerializerOptions options)
	=> writer.WriteRawValue($"\"{value.DeclaringType}.{value.Name}({string.Join(", ", value.GetParameters().Select(p => $"{p.ParameterType.GetName()} {p.Name}"))})\"");


}
