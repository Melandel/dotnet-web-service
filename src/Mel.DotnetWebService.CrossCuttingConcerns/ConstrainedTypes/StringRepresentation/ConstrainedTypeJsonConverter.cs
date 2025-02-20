using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

public class ConstrainedTypeJsonConverter : JsonConverter<object>
{
	readonly static JsonConverter<object> DefaultConverter = (JsonConverter<object>) JsonSerializerOptions.Default.GetConverter(typeof(object));
	record ObjectGeneratedForSerializationPurposes(object? Value);
	record KeyValuePairGeneratedForSerializationPurposes(object? Key, object? Value);
	static readonly ConcurrentDictionary<Type, bool> _couldConvert = new ConcurrentDictionary<Type, bool>();
	public override bool CanConvert(Type typeToConvert)
	{
		Console.WriteLine($"[{typeToConvert.FullName}]");
		if (typeToConvert == typeof(ObjectGeneratedForSerializationPurposes))
		{
			return true;
		}
		if (_couldConvert.ContainsKey(typeToConvert))
		{
			var successfulRetrieval = false;
			var couldConvert = false;
			while (!successfulRetrieval)
			{
				successfulRetrieval = _couldConvert.TryGetValue(typeToConvert, out couldConvert);
			}
			return couldConvert;
		}
		if (typeToConvert.IsArray && _couldConvert.TryGetValue(typeToConvert.GetElementType()!, out bool couldConvertElementTypeOfTheArray))
		{
			_couldConvert.TryAdd(typeToConvert, couldConvertElementTypeOfTheArray);
		}
		if (typeToConvert.IsGenericType && typeToConvert.GetGenericArguments().Any(_couldConvert.ContainsKey))
		{
			foreach (var argType in typeToConvert.GetGenericArguments())
			{
				if (_couldConvert.TryGetValue(argType, out bool couldConvertGenericArgumentTypeAsNonGenericType))
				{

					var successfulAdd = false;
					while (!successfulAdd)
					{
						successfulAdd = _couldConvert.TryAdd(typeToConvert, couldConvertGenericArgumentTypeAsNonGenericType);
					}
					Console.WriteLine(couldConvertGenericArgumentTypeAsNonGenericType ? $"I can convert {typeToConvert.FullName} (0)" : $"I cannot convert {typeToConvert.FullName}");
					return couldConvertGenericArgumentTypeAsNonGenericType;
				}
			}
		}
		Console.WriteLine($"a");
		Console.WriteLine($"Can I convert {typeToConvert.FullName}?");
		if (IsDictionaryGeneratedForSerializationPurposes(typeToConvert))
		{
			_couldConvert.TryAdd(typeToConvert, true);
			Console.WriteLine($"I can convert {typeToConvert.FullName} (1)");
			return true;
		}

		if (IsCollectionOfObjectTypeGeneratedForSerializationPurposes(typeToConvert))
		{

			_couldConvert.TryAdd(typeToConvert, true);
			Console.WriteLine($"I can convert {typeToConvert.FullName} (2)");
			return true;
		}

		if (IsObjectTypeGeneratedForSerializationPurposes(typeToConvert))
		{
			
			_couldConvert.TryAdd(typeToConvert, true);
			Console.WriteLine($"I can convert {typeToConvert.FullName} (3)");
			return true;
		}

		var v = typeToConvert switch
		{
			var t when t.IsOrInvolvesAConstrainedType() => true,
			var t when t.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) => true,
			var t when t.IsAGenericDictionaryType(argType => argType.IsOrInvolvesAConstrainedType()) => true,
			_ => false
		};

		_couldConvert.TryAdd(typeToConvert, v);
		Console.WriteLine(v ? $"I can convert {typeToConvert.FullName} (4)" : $"I cannot convert {typeToConvert.FullName}");

		return v;
	}

	private bool IsObjectTypeGeneratedForSerializationPurposes(Type t)
	{
		return t == typeof(ObjectGeneratedForSerializationPurposes);
	}

	private bool IsCollectionOfObjectTypeGeneratedForSerializationPurposes(Type t)
	{
		return t.IsReadOnlyCollectionWithoutBeingString() && (t.IsAGenericCollectionType(argType => argType == typeof(ObjectGeneratedForSerializationPurposes)) || t.IsAGenericDictionaryType(argType => argType == typeof(ObjectGeneratedForSerializationPurposes)));
	}

	private bool IsDictionaryGeneratedForSerializationPurposes(Type t)
	{
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) && t.GetGenericArguments().First()== typeof(string) && t.GetGenericArguments().Last() == typeof(ObjectGeneratedForSerializationPurposes);
	}

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
		if (type.FullName.Contains("PositionalRecordContainingADictionaryWithPositionalRecordsContainingAListOfNonEmptyGuidKeys"))
		{
			int i = 42;
		}

		if (IsObjectTypeGeneratedForSerializationPurposes(type))
		{
			var nestedValue = (value as ObjectGeneratedForSerializationPurposes)!.Value;
			var serialized = JsonSerializer.Serialize(nestedValue, options);
			writer.WriteRawValue(serialized);
			return;
		}

		if (IsCollectionOfObjectTypeGeneratedForSerializationPurposes(type))
		{
			WriteCollectionInvolvingContrainedType(writer, value, options);
			return;
		}

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

		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
		{
			WriteKeyValuePair(writer, value, type, options);
		}

		WriteDataMatchingConstructorParametersAndPublicProperties(writer, value, options, type);
	}

	private void WriteKeyValuePair(Utf8JsonWriter writer, object value, Type type, JsonSerializerOptions options)
	{
		var key = type.GetProperty("Key").GetValue(value);
		writer.WritePropertyName(JsonSerializer.Serialize(key).Trim('"'));

		var valueToSerialize = type.GetProperty("Value").GetValue(value);
		var serializedValue = JsonSerializer.Serialize(valueToSerialize, options);
		writer.WriteRawValue(serializedValue);
	}

	static bool IsContrainedTypesInvolvingDictionary(Type type)
	=> type.GetInterfaces().Any(itf => itf == typeof(IDictionary));

	static bool IsContrainedTypesCollection(Type type)
	=> type.GetInterfaces().Any(itf => itf.IsGenericType && itf.GetGenericTypeDefinition() == typeof(IEnumerable<>));

	static bool IsCollectionConstrainedType(Type type)
	=> type.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>), genericArgType => genericArgType.Implements(typeof(IEnumerable<>)));

	static bool IsAConstrainedType(Type type)
	=> type.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>));

	void WriteDataMatchingConstructorParametersAndPublicProperties(Utf8JsonWriter writer, object value, JsonSerializerOptions options, Type type)
	{
		//Console.WriteLine($"[{type.FullName}] Writing...");
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
			DefaultConverter.Write(writer, value, options);
		}
		//Console.WriteLine($"[{type.FullName}] Written!");

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
		foreach (var item in (IEnumerable)value)
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
			//Console.WriteLine($"{{{item.Key}, {item.Value}}}");
			var key = item.Key switch
			{
				ObjectGeneratedForSerializationPurposes k => k.Value,
				var k => k
			};
			
			var serializedKey = JsonSerializer.Serialize(key, options).Trim('"');
			writer.WritePropertyName(serializedKey);

			var valueToSerialize = item.Value switch
			{
				ObjectGeneratedForSerializationPurposes v => v.Value,
				var v => v
			};
			var serializedValue = JsonSerializer.Serialize(valueToSerialize, options);
			writer.WriteRawValue(serializedValue);
			//
		}
		writer.WriteEndObject();
	}

	Dictionary<string, ObjectGeneratedForSerializationPurposes?> BuildDictionary(object value, FieldInfo[] fields, PropertyInfo[] properties)
	{
		var dictionary = new Dictionary<string, ObjectGeneratedForSerializationPurposes?>();

		foreach (var field in fields)
		{
			var fieldName = field.Name;
			var fieldValue = field.GetValue(value);
			if (fieldValue == null)
			{
				dictionary.Add(fieldName, new ObjectGeneratedForSerializationPurposes(null));
				continue;
			}

			try { dictionary.Add( fieldName, BuildObjectGeneratedForSerializationPurposes(fieldValue)); }
			catch (Exception e)	{ dictionary.Add(fieldName, new ObjectGeneratedForSerializationPurposes(e.GetType().FullName)); }
		}

		foreach (var property in properties)
		{
			var propertyName = property.Name;
			var propertyValue = property.GetValue(value);
			if (propertyValue == null)
			{
				dictionary.Add(propertyName, new ObjectGeneratedForSerializationPurposes(null));
				continue;
			}

			try { dictionary.Add(propertyName, BuildObjectGeneratedForSerializationPurposes(propertyValue)); }
			catch (Exception e) { dictionary.Add(propertyName, new ObjectGeneratedForSerializationPurposes(e.GetType().FullName)); }
		}

		return dictionary;
	}

	private static ObjectGeneratedForSerializationPurposes BuildObjectGeneratedForSerializationPurposes(object propertyValue)
	{
		return new ObjectGeneratedForSerializationPurposes(
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

	public override void WriteAsPropertyName(
		Utf8JsonWriter writer,
		object obj,
		JsonSerializerOptions options) =>
			writer.WritePropertyName(JsonSerializer.Serialize(obj, options).Trim('"'));
}
