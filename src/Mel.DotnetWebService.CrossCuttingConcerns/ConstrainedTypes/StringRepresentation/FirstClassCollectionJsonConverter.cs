using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

public class FirstClassCollectionJsonConverter : JsonConverter<object>
{
	readonly static JsonConverter<object> DefaultConverter = (JsonConverter<object>)JsonSerializerOptions.Default.GetConverter(typeof(object));

	public override bool CanConvert(Type typeToConvert)
	=> typeToConvert.DefinesAFirstClassCollection();

	public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	=> DefaultConverter.Read(ref reader, typeToConvert, options);

	public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		value.GetType().DefinesConstructorWhoseParametersAllHaveAMatchingField(out var fields, out var properties);
		var v = JsonSerializer.Serialize(
				(fields, properties) switch
				{
					([var field], []) => field.GetValue(value),
					_ => BuildDictionary(value, fields, properties)
				},
				options);
		writer.WriteRawValue(v);
	}

	Dictionary<string, object?> BuildDictionary(object value, FieldInfo[] fields, PropertyInfo[] properties)
	{
		var dictionary = new Dictionary<string, object?>();

		foreach(var field in fields)
		{
			try
			{
				var fieldValue = field.GetValue(value);
				if (fieldValue.IsReadOnlyCollectionWithoutBeingString(out var asReadOnlyCollection))
				{
					dictionary.Add($"{field.Name}_count", asReadOnlyCollection.Count);
				}
				else if (fieldValue.IsFirstClassCollectionWithKnownCount(out var count))
				{
					dictionary.Add($"{field.Name}_count", count);
				}

				dictionary.Add(field.Name, fieldValue);
			}
			catch
			{
			}
		}

		foreach(var property in properties)
		{
			if (property.PropertyType.IsEnumerableWithoutBeingString())
			{
				try
				{
					dictionary.Add(
						property.Name,
						property.GetValue(value) switch
						{
							null => null,
							_ => "[..]"
						});
				}
				catch (Exception e)
				{
					dictionary.Add(property.Name, e.GetType().FullName);
				}
			}
			else
			{
				try
				{
					dictionary.Add(property.Name, property.GetValue(value));
				}
				catch (Exception e)
				{
					dictionary.Add(property.Name, e.GetType().FullName);
				}
			}
		}

		return dictionary;
	}
}
