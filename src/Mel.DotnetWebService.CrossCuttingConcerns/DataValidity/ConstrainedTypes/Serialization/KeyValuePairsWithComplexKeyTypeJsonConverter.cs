using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.ReadingOperations;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization.WritingOperations;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;

public class KeyValuePairsWithComplexKeyTypeJsonConverter : JsonConverter<object>
{
	public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return KeyValuePairsReadingOperation.Instance.Execute(ref reader, typeToConvert, options, options.Without<ConstrainedTypeJsonConverter>());
	}

	public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		KeyValuePairsWritingOperation.Instance.Execute(ref writer, value, options);
	}
	
	public override bool CanConvert(Type typeToConvert)
	{
		return typeToConvert.ImplementsGenericIEnumerableOfKeyPairValues(out var keyType, out _) && !keyType.IsANativeScalarType();
	}
}
