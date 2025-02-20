using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

public class ConstrainedTypeJsonConverter : JsonConverter<object>
{
	readonly static JsonConverter<object> DefaultConverter = (JsonConverter<object>) JsonSerializerOptions.Default.GetConverter(typeof(object));
	record ObjectGeneratedForSerializationPurposes(object? Value);
	record KeyValuePairGeneratedForSerializationPurposes(object? Key, object? Value);
	static readonly ConcurrentDictionary<Type, bool> _couldConvert = new ConcurrentDictionary<Type, bool>();
	public static bool IsAbleToConvert(Type typeToConvert)
	{
		if (typeToConvert == typeof(ObjectGeneratedForSerializationPurposes))
		{
			return true;
		}
		if (IsDictionaryGeneratedForSerializationPurposes(typeToConvert))
		{
			return true;
		}

		if (IsCollectionOfObjectTypeGeneratedForSerializationPurposes(typeToConvert))
		{
			return true;
		}

		if (IsObjectTypeGeneratedForSerializationPurposes(typeToConvert))
		{
			return true;
		}

		var v = typeToConvert switch
		{
			var t when t.IsOrInvolvesAConstrainedType() => true,
			var t when t.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) => true,
			var t when t.IsAGenericDictionaryType(argType => argType.IsOrInvolvesAConstrainedType()) => true,
			_ => false
		};

		return v;
	}
	public override bool CanConvert(Type typeToConvert)
	{
		var typeToConvertName = typeToConvert.IsGenericType
			? $"{typeToConvert.GetGenericTypeDefinition()}<{string.Join(",", typeToConvert.GetGenericArguments().Select(t => t.Name))}>"
			: typeToConvert.Name;
		Console.WriteLine($"[{typeToConvertName}]");
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
					Console.WriteLine(couldConvertGenericArgumentTypeAsNonGenericType ? $"I can convert {typeToConvertName} (0)" : $"I cannot convert {typeToConvertName}");
					return couldConvertGenericArgumentTypeAsNonGenericType;
				}
			}
		}
		Console.WriteLine($"a");
		Console.WriteLine($"Can I convert {typeToConvertName}?");
		if (IsDictionaryGeneratedForSerializationPurposes(typeToConvert))
		{
			_couldConvert.TryAdd(typeToConvert, true);
			Console.WriteLine($"I can convert {typeToConvertName} (1)");
			return true;
		}

		if (IsCollectionOfObjectTypeGeneratedForSerializationPurposes(typeToConvert))
		{

			_couldConvert.TryAdd(typeToConvert, true);
			Console.WriteLine($"I can convert {typeToConvertName} (2)");
			return true;
		}

		if (IsObjectTypeGeneratedForSerializationPurposes(typeToConvert))
		{

			_couldConvert.TryAdd(typeToConvert, true);
			Console.WriteLine($"I can convert {typeToConvertName} (3)");
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
		Console.WriteLine(v ? $"I can convert {typeToConvertName} (4)" : $"I cannot convert {typeToConvertName}");

		return v;
	}

	static bool IsObjectTypeGeneratedForSerializationPurposes(Type t)
	{
		return t == typeof(ObjectGeneratedForSerializationPurposes);
	}

	static bool IsCollectionOfObjectTypeGeneratedForSerializationPurposes(Type t)
	{
		return t.IsReadOnlyCollectionWithoutBeingString() && (t.IsAGenericCollectionType(argType => argType == typeof(ObjectGeneratedForSerializationPurposes)) || t.IsAGenericDictionaryType(argType => argType == typeof(ObjectGeneratedForSerializationPurposes)));
	}

	static bool IsDictionaryGeneratedForSerializationPurposes(Type t)
	{
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) && t.GetGenericArguments().First()== typeof(string) && t.GetGenericArguments().Last() == typeof(ObjectGeneratedForSerializationPurposes);
	}

	public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return typeToConvert switch
		{
			var t when t.IsAConstrainedType(out var rootType) => DeserializeConstrainedType(ref reader, t, rootType!, options),
			var t when !t.IsEnumerableWithoutBeingString() && t.HasFieldOrPropertyWithConstrainedType(browseRecursively: true) => DeserializeComplexObject(ref reader, typeToConvert, options),
			var t when t.IsEnumerableWithoutBeingString() && t.ImplementsGenericIEnumerableWithAnArgumentTypeThatVerifies(arg => arg.IsAConstrainedType()) => DeserializeCollectionOfConstrainedTypes(ref reader, t, options),
			var t when t.IsEnumerableWithoutBeingString() && t.ImplementsGenericIEnumerableWithAnArgumentTypeThatVerifies(arg => arg.IsOrInvolvesAConstrainedType()) => DeserializeCollectionOfObjectsContainingConstrainedTypes(ref reader, t, options),
			_ => DefaultConverter.Read(ref reader, typeToConvert, options)
		};
	}

	object? DeserializeCollectionOfConstrainedTypes(ref Utf8JsonReader reader, Type t, JsonSerializerOptions options)
	{
		var elementType = t switch
		{
			{ IsArray: true } => t.GetElementType(),
			{ IsGenericType: true } => t.GetGenericArguments().First(),
			_ => throw new NotImplementedException()
		};
		var typedList = typeof(List<>).MakeGenericType(elementType);
		dynamic collection = Activator.CreateInstance(typedList);
		reader.Read(); // [
		while (reader.TokenType != JsonTokenType.EndArray)
		{
			var str = reader.GetString();
			var obj = elementType.DeserializeFromJson(str)!;
			typedList.InvokeMember(
				nameof(IList.Add),
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null,
				collection,
				new object[] { obj });
			reader.Read();
		}
		reader.Read(); // ]

		if (t.IsArray)
		{
			var array = typedList.InvokeMember(
				nameof(List<object>.ToArray),
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null,
				collection,
				Array.Empty<object>());
			return array;
		}

		if (t.IsGenericType)
		{
			if (t.GetGenericTypeDefinition() == typeof(List<>))
			{
				return collection;
			}
		}
		return collection;
	}

	object? DeserializeCollectionOfObjectsContainingConstrainedTypes(ref Utf8JsonReader reader, Type t, JsonSerializerOptions options)
	{
		var elementType = t switch
		{
			{ IsArray: true } => t.GetElementType(),
			{ IsGenericType: true } => t.GetGenericArguments().First(),
			_ => throw new NotImplementedException()
		};
		var typedList = typeof(List<>).MakeGenericType(elementType);
		dynamic collection = Activator.CreateInstance(typedList);
		var collectionWithAddMethod = (IList)collection;
		reader.Read(); // [
		while (reader.TokenType != JsonTokenType.EndArray)
		{
			var obj = DeserializeComplexObject(ref reader, elementType, options);
			typedList.InvokeMember(
				nameof(IList.Add),
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null,
				collection,
				new object[] { obj });
			reader.Read();
		}
		reader.Read(); // ]

		if (t.IsArray)
		{
			var array = typedList.InvokeMember(
				nameof(List<object>.ToArray),
				BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
				null,
				collection,
				Array.Empty<object>());
			return array;
		}

		if (t.IsGenericType)
		{
			if (t.GetGenericTypeDefinition() == typeof(List<>))
			{
				return collection;
			}
		}
		return collection;
	}

	private dynamic? DeserializeComplexObject(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
	{
		if (type.DefinesInstanceConstructorsAndHasNoSetterProperty(out var constructors))
		{
			List<object> constructorParameters = new List<object>();
			ConstructorInfo constructorToUse = constructors.First();
			reader.Read();

			foreach(var param in constructorToUse.GetParameters())
			{
				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					reader.Read();
				}

				if (param.ParameterType == typeof(NonEmptyGuid))
				{
					var typedP = (Guid)ReadSystemType(ref reader, typeof(Guid), options);
					constructorParameters.Add(NonEmptyGuid.FromGuid(typedP));
				}
				else if (param.ParameterType == typeof(string))
				{
					var p = (string)ReadSystemType(ref reader, typeof(string), options);
					constructorParameters.Add(p);
				}
				else if (param.ParameterType.IsAGenericCollectionType(argType => argType.IsAConstrainedType()))
				{
					var typedP = DeserializeCollectionOfConstrainedTypes(ref reader, param.ParameterType, options);
					constructorParameters.Add(typedP);
				}
				else
				{
					var p = JsonSerializer.Deserialize(ref reader, param.ParameterType, options);
					constructorParameters.Add(p);
				}
			}

			reader.Read();
			return constructorToUse?.Invoke(constructorParameters.ToArray());
		}
		else
		{
			return DefaultConverter.Read(ref reader, type, options);
		}
	}
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

	object? DeserializeConstrainedType(ref Utf8JsonReader reader, Type constrainedType, Type rootType, JsonSerializerOptions options)
	{
		if (rootType == typeof(Guid))
		{
			var v = reader.GetString()!;
			var w = Guid.Parse(v);
			return NonEmptyGuid.FromGuid(w);
		}
		return rootType switch
		{
			var t when t == typeof(Guid) => Guid.Parse(reader.GetString()!),
			var t when t == typeof(string) => reader.GetString()!,
			_ => throw new NotImplementedException($"type {rootType} not supported")
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
	=> type.ExtendsGenericClassWithGenericArgument(typeof(Constrained<>), genericArgType => genericArgType.ImplementsInterface(typeof(IEnumerable<>)));

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
