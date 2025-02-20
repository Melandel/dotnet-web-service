using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class DefaultReadingOperation : ConstrainedTypeConverterReadingOperation
{
	public static readonly DefaultReadingOperation Instance = new();
	DefaultReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options)
	=> ((JsonConverter<object>)JsonSerializerOptions.Default.GetConverter(typeof(object))).Read(ref reader, targetType, options);

	object ReadSystemType(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
	=> type switch
	{
		var t when t == typeof(string) => ReadSystemType<string>(ref reader, type, options),
		var t when t == typeof(Guid) => ReadSystemType<Guid>(ref reader, type, options),
		_ => throw new NotImplementedException($"Type {type.Name} not supported by {nameof(ReadSystemType)}")
	};

	T ReadSystemType<T>(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
	{
		var res = ((JsonConverter<T>)JsonSerializerOptions.Default.GetConverter(type)).Read(ref reader, type, options)!;
		reader.Read();
		return res;
	}
}
