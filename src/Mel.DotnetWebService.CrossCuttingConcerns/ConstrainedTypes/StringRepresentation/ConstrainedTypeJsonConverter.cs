using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.WritingOperations;
using Microsoft.Extensions.Options;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

public partial class ConstrainedTypeJsonConverter : JsonConverter<object>
{
	static readonly ConcurrentDictionary<Type, bool> _couldConvert = new ConcurrentDictionary<Type, bool>();
	public static bool IsAbleToConvert(Type typeToConvert)
	{
		if (typeToConvert.FullName.Contains("[][]"))
		{
			var w = 0;
		}
		if (typeToConvert == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes))
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
			var t when t.IsOrInvolvesAConstrainedType() => true, // Constrained<int> && RecordContainingAConstrainedInt
			var t when t.IsAGenericCollectionType(argType => argType.IsOrInvolvesAConstrainedType()) => true, // Constrained<int>[]
			var t when t.IsAGenericCollectionType(argType => argType.IsAGenericCollectionType(arg => arg.IsOrInvolvesAConstrainedType())) => true, // Constrained<int>[][]
			var t when t.IsAGenericDictionaryType(argType => argType.IsOrInvolvesAConstrainedType()) => true, // Dictionary<Constrained<int>, string>
			_ => false
		};

		return v;
	}
	public override bool CanConvert(Type typeToConvert)
	{
		if (typeToConvert.FullName.Contains("[][]"))
		{
			var w = 0;
		}
		var typeToConvertName = typeToConvert.IsGenericType
			? $"{typeToConvert.GetGenericTypeDefinition()}<{string.Join(",", typeToConvert.GetGenericArguments().Select(t => t.Name))}>"
			: typeToConvert.Name;
		Console.WriteLine($"[{typeToConvertName}]");
		if (typeToConvert == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes))
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

		if (IsAbleToConvert(typeToConvert))
		{
			_couldConvert.TryAdd(typeToConvert, true);
			Console.WriteLine($"I can convert {typeToConvertName} (4)");
			return true;
		}

		_couldConvert.TryAdd(typeToConvert, false);
		return false;
	}

	static bool IsObjectTypeGeneratedForSerializationPurposes(Type t)
	{
		return t == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes);
	}

	static bool IsCollectionOfObjectTypeGeneratedForSerializationPurposes(Type t)
	{
		return t.IsReadOnlyCollectionWithoutBeingString() && (t.IsAGenericCollectionType(argType => argType == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes)) || t.IsAGenericDictionaryType(argType => argType == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes)));
	}

	static bool IsDictionaryGeneratedForSerializationPurposes(Type t)
	{
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>) && t.GetGenericArguments().First() == typeof(string) && t.GetGenericArguments().Last() == typeof(ObjectTypeSpecificallyGeneratedForSerializationPurposes);
	}

	public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	=> ConstrainedTypeConverterReadingOperation.For(typeToConvert).Execute(ref reader, typeToConvert, options);

	public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
	=> ConstrainedTypeConverterWritingOperation.For(value).Execute(ref writer, value, options);
	
	public override void WriteAsPropertyName(
		Utf8JsonWriter writer,
		object obj,
		JsonSerializerOptions options) =>
			writer.WritePropertyName(JsonSerializer.Serialize(obj, options).Trim('"'));
}
