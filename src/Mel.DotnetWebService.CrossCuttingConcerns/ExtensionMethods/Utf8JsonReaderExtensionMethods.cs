using System.Reflection.PortableExecutable;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

static class Utf8JsonReaderExtensionMethods
{
	public static bool NativelySupports(this Utf8JsonReader reader, Type type)
	=> type switch
	{
			var t when t == typeof(string) => true,
			var t when t == typeof(bool) => true,
			var t when t == typeof(byte) => true,
			var t when t == typeof(byte[]) => true,
			var t when t == typeof(DateTime) => true,
			var t when t == typeof(DateTimeOffset) => true,
			var t when t == typeof(decimal) => true,
			var t when t == typeof(double) => true,
			var t when t == typeof(Guid) => true,
			var t when t == typeof(short) => true,
			var t when t == typeof(int) => true,
			var t when t == typeof(long) => true	,
			var t when t == typeof(sbyte) => true,
			var t when t == typeof(float) => true,
			var t when t == typeof(ushort) => true,
			var t when t == typeof(uint) => true,
			var t when t == typeof(ulong) => true,
			_ => false
		};
	public static object? GetNativelySupportedValue(this Utf8JsonReader reader, Type expectedType)
	=> expectedType switch
	{
		var t when t == typeof(string) => reader.GetString(),
		var t when t == typeof(bool) => reader.GetBoolean(),
		var t when t == typeof(byte) => reader.GetByte(),
		var t when t == typeof(byte[]) => reader.GetBytesFromBase64(),
		var t when t == typeof(DateTime) => reader.GetDateTime(),
		var t when t == typeof(DateTimeOffset) => reader.GetDateTimeOffset(),
		var t when t == typeof(decimal) => reader.GetDecimal(),
		var t when t == typeof(double) => reader.GetDouble(),
		var t when t == typeof(Guid) => reader.GetGuid(),
		var t when t == typeof(short) => reader.GetInt16(),
		var t when t == typeof(int) => reader.GetInt32(),
		var t when t == typeof(long) => reader.GetInt64(),
		var t when t == typeof(sbyte) => reader.GetSByte(),
		var t when t == typeof(float) => reader.GetSingle(),
		var t when t == typeof(ushort) => reader.GetUInt16(),
		var t when t == typeof(uint) => reader.GetUInt32(),
		var t when t == typeof(ulong) => reader.GetUInt64(),
		_ => throw new NotImplementedException()
	};
}
