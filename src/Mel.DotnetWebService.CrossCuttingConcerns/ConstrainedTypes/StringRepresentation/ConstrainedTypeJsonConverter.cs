using System.Collections;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

public class ConstrainedTypeJsonConverter : JsonConverter<object>
{
	readonly static JsonConverter<object> DefaultConverter = (JsonConverter<object>) JsonSerializerOptions.Default.GetConverter(typeof(object));

	public override bool CanConvert(Type typeToConvert)
	=> typeToConvert switch
	{
		var t when t.IsOrInvolvesAConstrainedType() => true,
		var t when t.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) => true,
		_ => false
	};

	public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return typeToConvert switch
		{
			var t when t.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>), genericArgType => genericArgType.Implements(typeof(IEnumerable<>))) => t.DeserializeFromJson(reader.GetString()), // TODO
			var t when t.ImplementsGenericInterfaceWithGenericArgument(typeof(ICanBeDeserializedFromJson<>), t) => t.DeserializeFromJson(reader.GetString()),
			// TODO: enumerables
			_ => DefaultConverter.Read(ref reader, typeToConvert, options)
		};
	}

	public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		var type = value.GetType();

		if (IsContrainedTypesInvolvingDictionary(type)) // ie Dictionary<string, Constrained<int>>
		{
			WriteDictionaryInvolvingContrainedTypes(writer, value, options);
			return;
		}

		if (IsContrainedTypesCollection(type)) // ie List<Constrained<int>>
		{
			WriteCollectionInvolvingContrainedType(writer, value, options);
			return;
		}

		if (IsCollectionConstrainedType(type)) // ie Constrained<int[]>
		{
			WriteDataMatchingConstructorParametersAndPublicProperties(writer, value, options, type);
			return;
		}

		if (IsAConstrainedType(type)) // ie Constrained<int>
		{
			WriteConstrainedDataType(writer, value, options);
			return;
		}

		WriteDataMatchingConstructorParametersAndPublicProperties(writer, value, options, type);
	}

	static bool IsContrainedTypesInvolvingDictionary(Type type)
	=> type.GetInterfaces().Any(itf => itf == typeof(IDictionary));

	static bool IsContrainedTypesCollection(Type type)
	=> type.GetInterfaces().Any(itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IEnumerable<>));

	static bool IsCollectionConstrainedType(Type type)
	=> type.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>), genericArgType => genericArgType.Implements(typeof(IEnumerable<>)));

	static bool IsAConstrainedType(Type type)
	=> type.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>), genericArgType => genericArgType == type);

	void WriteCollectionConstrainedType(Utf8JsonWriter writer, object value, JsonSerializerOptions options, Type type)
	{
		if (type.DefinesConstructorWhoseParametersAllHaveAMatchingField(out var fields, out var properties, out var numberOfConstructorParameters))
		{
			if (numberOfConstructorParameters == 1)
			{
				WriteConstrainedDataType(writer, value, options);
			}
			else
			{
				var objectToSerialize = (fields, properties) switch
				{
					([var field], []) => field.GetValue(value),
						_ => BuildDictionary(value, fields, properties)
				};

				var serialized = JsonSerializer.Serialize(objectToSerialize, options);
				writer.WriteRawValue(serialized);
			}
		}
		else
		{
			DefaultConverter.Write(writer, value, options);
		}
	}
	void WriteDataMatchingConstructorParametersAndPublicProperties(Utf8JsonWriter writer, object value, JsonSerializerOptions options, Type type)
	{
		if (type.DefinesConstructorWhoseParametersAllHaveAMatchingField(out var fields, out var properties, out _))
		{
			var objectToSerialize = (fields, properties) switch
			{
				([var field], []) => field.GetValue(value),
					_ => BuildDictionary(value, fields, properties)
			};

				var serialized = JsonSerializer.Serialize(objectToSerialize, options);
				writer.WriteRawValue(serialized);
		}
		else
		{
			DefaultConverter.Write(writer, value, options);
		}
	}

	void WriteConstrainedDataType(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		foreach (var converter in value.GetType().GetUserDefinedConversions(browseParentTypes: true))
		{
			try
			{
				var converted = converter.Invoke(null, new[] { value });
				var serialized = JsonSerializer.Serialize(converted, options);
				writer.WriteRawValue(serialized);
				return;
			}
			catch
			{
			}
		}
	}

	void WriteCollectionInvolvingContrainedType(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		writer.WriteStartArray();
		foreach (var item in (object[])value)
		{
			var serialized = JsonSerializer.Serialize(item, options);
			writer.WriteRawValue(serialized);
		}
		writer.WriteEndArray();
	}

	void WriteDictionaryInvolvingContrainedTypes(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
		foreach (DictionaryEntry item in (IDictionary)value)
		{
			writer.WritePropertyName(JsonSerializer.Serialize(item.Key, options).Trim('"'));
			writer.WriteRawValue(JsonSerializer.Serialize(item.Value, options));
		}
		writer.WriteEndObject();
	}

	Dictionary<string, object?> BuildDictionary(object value, FieldInfo[] fields, PropertyInfo[] properties)
	{
		var dictionary = new Dictionary<string, object?>();

		foreach (var field in fields)
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

		foreach (var property in properties)
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
