using Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Mel.DotnetWebService.Api.ExtensionMethods;

public static class SwaggerGenOptionsExtensionMethods
{
	// 👇 https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/58c5095bb17aab1a158dceb263a138ff087524f2/src/Swashbuckle.AspNetCore.SwaggerGen/SchemaGenerator/JsonSerializerDataContractResolver.cs#L246
	static readonly Dictionary<Type, OpenApiSchema> PrimitiveTypesAndFormatsByDeclaringType = new()
	{
	[typeof(bool)] =           new OpenApiSchema { Type = "boolean" },
	[typeof(byte)] =           new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
	[typeof(sbyte)] =          new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
	[typeof(short)] =          new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
	[typeof(ushort)] =         new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
	[typeof(int)] =            new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
	[typeof(uint)] =           new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
	[typeof(long)] =           new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int64 },
	[typeof(ulong)] =          new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int64 },
	[typeof(float)] =          new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Float },
	[typeof(double)] =         new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Double },
	[typeof(decimal)] =        new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Double },
	[typeof(byte[])] =         new OpenApiSchema { Type = OpenApiDataType.String, Format = OpenApiDataFormat.Byte },
	[typeof(string)] =         new OpenApiSchema { Type = OpenApiDataType.String },
	[typeof(char)] =           new OpenApiSchema { Type = OpenApiDataType.String },
	[typeof(DateTime)] =       new OpenApiSchema { Type = OpenApiDataType.String, Format = OpenApiDataFormat.DateTime },
	[typeof(DateTimeOffset)] = new OpenApiSchema { Type = OpenApiDataType.String, Format = OpenApiDataFormat.DateTime },
	[typeof(TimeSpan)] =       new OpenApiSchema { Type = OpenApiDataType.String, Format = OpenApiDataFormat.DateSpan },
	[typeof(Guid)] =           new OpenApiSchema { Type = OpenApiDataType.String, Format = OpenApiDataFormat.Uuid, Example = new OpenApiString("00000000-0000-0000-0000-000000000001") },
	[typeof(Uri)] =            new OpenApiSchema { Type = OpenApiDataType.String, Format = OpenApiDataFormat.Uri },
	[typeof(Version)] =        new OpenApiSchema { Type = OpenApiDataType.String },
	[typeof(DateOnly)] =       new OpenApiSchema { Type = OpenApiDataType.String, Format = OpenApiDataFormat.Date },
	[typeof(TimeOnly)] =       new OpenApiSchema { Type = OpenApiDataType.String, Format = OpenApiDataFormat.Time },
	[typeof(Int128)] =         new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int128 },
	[typeof(UInt128)] =        new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int128 },
	};
	static class OpenApiDataType
	{
		public const string Boolean = "boolean";
		public const string Integer = "integer";
		public const string Number = "number";
		public const string String = "string";
		public const string Array = "array";
	}
	static class OpenApiDataFormat
	{
		public const string Int32 = "int32";
		public const string Int64 = "int64";
		public const string Float = "float";
		public const string Double = "double";
		public const string Byte = "byte";
		public const string DateTime = "date-time";
		public const string DateSpan = "date-span";
		public const string Uuid = "uuid";
		public const string Uri = "uri";
		public const string Date = "date";
		public const string Time = "time";
		public const string Int128 = "int128";
	}
	public static SwaggerGenOptions UseRootTypeSchemaForConstrainedTypes(this SwaggerGenOptions opt)
	{
		foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
		{
			foreach (Type loadedType in a.GetTypes())
			{
				if (loadedType.IsAConstrainedType(out var rootType))
				{
					opt.MapType(loadedType,() => PrimitiveTypesAndFormatsByDeclaringType[rootType!]);
					//foreach (var collectionType in GenerateCollectionTypesFromElementType(loadedType))
					//{
					//	opt.MapType(collectionType, () => new OpenApiSchema { Type = OpenApiDataType.Array, Items = PrimitiveTypesAndFormatsByDeclaringType[rootType!] });
					//}
				}
			}
		}

		return opt;
	}
	static IReadOnlyCollection<Type> GenerateCollectionTypesFromElementType(Type elementType)
	{
		return new[]
		{
			elementType.MakeArrayType(),
			typeof(List<>).MakeGenericType(elementType),
			typeof(IEnumerable<>).MakeGenericType(elementType),
			typeof(IReadOnlyCollection<>).MakeGenericType(elementType),
		};
	}
}
