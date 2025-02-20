using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.WritingOperations;

class DataMatchingConstructorParametersAndPublicPropertiesWritingOperation : ConstrainedTypeConverterWritingOperation
{
	public static readonly DataMatchingConstructorParametersAndPublicPropertiesWritingOperation Instance = new();
	DataMatchingConstructorParametersAndPublicPropertiesWritingOperation()
	{
	}

	public override void Execute(ref Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		var type = value.GetType();
		if (type.DefinesConstructorWhoseParametersAllHaveAMatchingField(out var fields, out var properties, out _))
		{
			if (fields is [var field] && !properties.Any())
			{
				var serialized = JsonSerializer.Serialize(field.GetValue(value), options);
				writer.WriteRawValue(serialized);
			}
			else
			{
				var objectToSerialize = BuildDictionary(value, fields, properties);
				var serialized = JsonSerializer.Serialize(objectToSerialize, options);
				writer.WriteRawValue(serialized);
			}
		}
		else
		{
			((JsonConverter<object>)JsonSerializerOptions.Default.GetConverter(typeof(object))).Write(writer, value, options);
		}
	}

	Dictionary<string, ObjectTypeSpecificallyGeneratedForSerializationPurposes?> BuildDictionary(object value, FieldInfo[] fields, PropertyInfo[] properties)
	{
		var dictionary = new Dictionary<string, ObjectTypeSpecificallyGeneratedForSerializationPurposes?>();

		foreach (var field in fields)
		{
			var fieldName = field.Name;
			var fieldValue = field.GetValue(value);
			if (fieldValue == null)
			{
				dictionary.Add(fieldName, new ObjectTypeSpecificallyGeneratedForSerializationPurposes(null));
				continue;
			}

			try { dictionary.Add(fieldName, BuildObjectGeneratedForSerializationPurposes(fieldValue)); }
			catch (Exception e) { dictionary.Add(fieldName, new ObjectTypeSpecificallyGeneratedForSerializationPurposes(e.GetType().FullName)); }
		}

		foreach (var property in properties)
		{
			var propertyName = property.Name;
			var propertyValue = property.GetValue(value);
			if (propertyValue == null)
			{
				dictionary.Add(propertyName, new ObjectTypeSpecificallyGeneratedForSerializationPurposes(null));
				continue;
			}

			try { dictionary.Add(propertyName, BuildObjectGeneratedForSerializationPurposes(propertyValue)); }
			catch (Exception e) { dictionary.Add(propertyName, new ObjectTypeSpecificallyGeneratedForSerializationPurposes(e.GetType().FullName)); }
		}

		return dictionary;
	}

	static ObjectTypeSpecificallyGeneratedForSerializationPurposes BuildObjectGeneratedForSerializationPurposes(object propertyValue)
	{
		return new ObjectTypeSpecificallyGeneratedForSerializationPurposes(
			propertyValue switch
			{
				var v when v.IsADictionary() => BuildGenericDictionary((IDictionary)propertyValue),
				var v when v.IsAnIEnumerableWithoutBeingString() => ((IEnumerable)propertyValue).Cast<object>().ToArray(),
				var v => v
			});
	}

	static Dictionary<object, object?> BuildGenericDictionary(IDictionary dictionary)
	{
		var d = new Dictionary<object, object?>();
		foreach (DictionaryEntry entry in dictionary)
		{
			d.Add(entry.Key, entry.Value);
		}
		return d;
	}
}
