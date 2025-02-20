using System.Text;
using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

public static class Utf8JsonReaderExtensionMethods
{
	static readonly Lazy<Type[]> Utf8JsonReaderNativelySupportedTypes = new Lazy<Type[]>(() => typeof(Utf8JsonReader).GetMethods().Where(m => m.Name.StartsWith("Get")).Select(m => m.ReturnType).Distinct().Where(t => t != typeof(Type) && !t.IsArray).ToArray());
	public static Type[] NativelySupportedTypes => Utf8JsonReaderNativelySupportedTypes.Value;
	public static Utf8JsonReader GetUtf8JsonReaderForPropertyName(this Utf8JsonReader reader, Type? targetType = null)
	{
		if (reader.TokenType != JsonTokenType.PropertyName)
		{
			throw new InvalidOperationException($"{nameof(Utf8JsonReader)}.{nameof(GetUtf8JsonReaderForPropertyName)} expected a {JsonTokenType.PropertyName} json token, but encountered a {reader.TokenType} json token instead");
		}

		var expectedType = targetType ?? typeof(string);
		var value = reader.GetString()!;
		var jsonifiedValue = IsSerializedIntoString(expectedType) ? $"\"{value}\"" : $"{value}";
		var utf8 = Encoding.UTF8.GetBytes(jsonifiedValue);
		return new Utf8JsonReader(utf8);
	}

	public static bool NativelySupports(this Utf8JsonReader reader, Type type)
	=> Utf8JsonReaderNativelySupportedTypes.Value.Contains(type);

	static bool IsSerializedIntoString(Type type)
	{
		type = Nullable.GetUnderlyingType(type) ?? type;

		if (type == typeof(string) ||
			type == typeof(char) ||
			type == typeof(DateTime) ||
			type == typeof(DateTimeOffset) ||
			type == typeof(TimeSpan) ||
			type == typeof(Guid) ||
			type == typeof(Uri) ||
			type == typeof(Version))
		{
			return true;
		}

		return type.IsEnum;
	}
}
