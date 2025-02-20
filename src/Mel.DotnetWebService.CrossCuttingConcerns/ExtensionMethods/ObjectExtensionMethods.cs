using System.Data;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

public static class ObjectExtensionMethods
{
	static readonly JsonSerializerOptions SerializerOptions;
	static ObjectExtensionMethods()
	{
		SerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.General) { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
		SerializerOptions.Converters.Add(new ConstrainedTypeJsonConverter());
	}

	public static string GetStringRepresentation(this object? obj)
	{
		try
		{
			return obj switch
			{
				null => "null",
				Type type => type.GetName(),
				MethodInfo methodInfo => $"{methodInfo.DeclaringType}.{methodInfo.Name}({string.Join(", ", methodInfo.GetParameters().Select(p => $"{p.ParameterType.GetName()} {p.Name}"))})",
				IEnumerable<Type> types => $"[{string.Join(",", types.Select(type => type.GetName()))}]",
				_ => JsonSerializer.Serialize(obj, SerializerOptions)
			};
		}
		catch (Exception ex)
		{
			throw new Exception($"Could not {nameof(GetStringRepresentation)}({obj!.GetType()} {JsonSerializer.Serialize(obj)})", ex);
		}
	}

	internal static bool IsFirstClassCollectionWithKnownCount(this object? obj, out int count)
	{
		if (obj == null)
		{
			count = 0;
			return false;
		}
		return obj.DefinesAFirstClassCollectionWithKnownCount(out count);
	}

	internal static bool DefinesAFirstClassCollectionWithKnownCount(this object obj, out int count)
	{
		var type = obj.GetType();
		if (!type.IsDefinedByOurOrganization())
		{
			count = 0;
			return false;
		}

		if (!type.DefinesConstructorWhoseParametersAllHaveAMatchingField(out var fields, out _, out _))
		{
			count = 0;
			return false;
		}

		var encapsulatedCollectionsFields = fields.Where(field => field.FieldType.IsEnumerableWithoutBeingString());
		if (encapsulatedCollectionsFields.Count() != 1)
		{
			count = 0;
			return false;
		}

		var encapsulatedCollectionField = encapsulatedCollectionsFields.First();
		try
		{
			var encapsulatedCollection = encapsulatedCollectionField.GetValue(obj);
			if (encapsulatedCollection == null)
			{
				count = 0;
				return false;
			}

			if (encapsulatedCollection.IsReadOnlyCollectionWithoutBeingString(out var asReadOnlyCollection))
			{
				count = asReadOnlyCollection.Count;
				return true;
			}

			if (encapsulatedCollection.DefinesAFirstClassCollectionWithKnownCount(out var size))
			{
				count = size;
				return true;
			}

			count = 0;
			return false;
		}
		catch
		{
			count = 0;
			return false;
		}
	}

	internal static bool IsEnumerableWithoutBeingString(this object obj, out IEnumerable<object> asEnumerable)
	{
		if (obj.GetType().IsEnumerableWithoutBeingString())
		{
			asEnumerable = ((System.Collections.IEnumerable)obj).Cast<object>();
			return true;
		}

		asEnumerable = Array.Empty<object>();
		return false;
	}

	internal static bool IsReadOnlyCollectionWithoutBeingString(this object? obj, out IReadOnlyCollection<object> asReadOnlyCollection)
	{
		if (obj == null)
		{
			asReadOnlyCollection = Array.Empty<object>();
			return false;
		}

		if (obj.GetType().IsReadOnlyCollectionWithoutBeingString())
		{
			asReadOnlyCollection = ((System.Collections.IEnumerable)obj).Cast<object>().ToArray();
			return true;
		}

		asReadOnlyCollection = Array.Empty<object>();
		return false;
	}

	public static bool HasUserDefinedConversions(this object obj, out MethodInfo[] converters)
	{
		converters = obj.GetType().GetUserDefinedConversions(browseParentTypes: true);
		return converters.Any();
	}

	internal static bool IsADictionary(this object obj)
	{
		return obj.GetType().IsAGenericDictionaryType(argType => true);
	}
	internal static bool IsAnIEnumerableWithoutBeingString(this object obj)
	{
		var type = obj.GetType();
		return type != typeof(string) && type.GetInterfaces().Contains(typeof(IEnumerable<>));
	}

	public static string RenderObjectState(this object obj)
	{
		var type = obj.GetType();
		if (type.Namespace!.StartsWith("System"))
		{
			return JsonSerializer.Serialize(obj);
		}

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

		if (!dict.Any())
		{
			return "";
		}

		return JsonSerializer.Serialize(dict, SerializerOptions);
	}
}

