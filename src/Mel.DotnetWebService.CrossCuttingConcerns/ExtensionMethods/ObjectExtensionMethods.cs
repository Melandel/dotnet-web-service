using System.Data;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Globalization.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

public static class ObjectExtensionMethods
{
	static readonly JsonSerializerOptions SerializerOptions;
	static readonly JsonSerializerOptions FallbackSerializerOptions;
	static ObjectExtensionMethods()
	{
		SerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.General) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
		SerializerOptions.Converters.Add(new KeyValuePairsWithComplexKeyTypeJsonConverter());
		SerializerOptions.Converters.Add(new JsonStringEnumConverter());
		SerializerOptions.Converters.Add(new ConstrainedTypeJsonConverter());
		SerializerOptions.Converters.Add(new TypeJsonConverter());
		SerializerOptions.Converters.Add(new MethodInfoJsonConverter());
		SerializerOptions.Converters.Add(new CultureInfoJsonConverter());
		SerializerOptions.Converters.Add(new RegionInfoJsonConverter());

		FallbackSerializerOptions = SerializerOptions.Without<ConstrainedTypeJsonConverter>();
	}

	public static string GetStringRepresentation(this object? obj, bool indent = false)
	{
		if (obj == null)
		{
			return "null";
		}

		var options = indent
		? new JsonSerializerOptions(SerializerOptions) { WriteIndented = true }
		: SerializerOptions;
		try
		{
			return JsonSerializer.Serialize(obj, options);
		}
		catch (Exception ex) {
			throw new Exception($"Could not {nameof(GetStringRepresentation)}({obj.GetType()} {JsonSerializer.Serialize(obj, FallbackSerializerOptions)})", ex);
		}
	}

	public static bool HasUserDefinedConversions(this object obj, out MethodInfo[] converters)
	{
		converters = obj.GetType().GetUserDefinedConversions(browseParentTypes: true);
		return converters.Any();
	}

	public static string RenderObjectState(this object obj)
	{
		var type = obj.GetType();
		if (!type.IsOrInvolvesAConstrainedType())
		{
			return JsonSerializer.Serialize(obj, FallbackSerializerOptions);
		}

		var dict = BuildDictionaryContainingFieldsAndProperties(obj);
		if (!dict.Any())
		{
			return "";
		}

		try
		{
			return JsonSerializer.Serialize(dict, SerializerOptions);
		}
		catch
		{
			return JsonSerializer.Serialize(dict, FallbackSerializerOptions);
		}
	}

	static Dictionary<string, object> BuildDictionaryContainingFieldsAndProperties(object obj)
	{
		var type = obj.GetType();
		var fields = type
					.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
					.Where(f => f.FieldType != typeof(Type))
					.Where(f => !f.Name.Contains("k__BackingField"))
					.ToArray(); ;
		var properties = type
			.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
			.Where(prop => prop.PropertyType != typeof(Type))
			.Where(prop => !prop.Name.Contains("k__BackingField")).ToArray();
		var dict = new Dictionary<string, object>();
		foreach (var field in fields)
		{
			try
			{
				var fieldValue = field.GetValue(obj);
				if (fieldValue != null)
				{
					dict.Add(field.Name, fieldValue);
				}
			}
			catch
			{
			}
		}

		foreach (var prop in properties)
		{
			try
			{
				var propertyValue = prop.GetValue(obj);
				if (propertyValue != null)
				{
					dict.Add(prop.Name, propertyValue);
				}
			}
			catch
			{
			}
		}

		return dict;
	}
}

