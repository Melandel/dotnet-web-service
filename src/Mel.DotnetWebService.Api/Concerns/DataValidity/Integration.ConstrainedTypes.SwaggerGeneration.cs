using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Mel.DotnetWebService.Api.Concerns.DataValidity;

static partial class Integration
{
	public partial class ConstrainedTypes
	{
		public static class SwaggerGeneration
		{
			public static class OpenApiSchemaBuilder
			{
				// 👇 https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/58c5095bb17aab1a158dceb263a138ff087524f2/src/Swashbuckle.AspNetCore.SwaggerGen/SchemaGenerator/JsonSerializerDataContractResolver.cs#L246
				static readonly Dictionary<Type, Func<OpenApiSchema>> PrimitiveTypeOpenApiSchemaBuildersByDeclaringType = new()
				{
					[typeof(bool)]           = () => new OpenApiSchema { Type = OpenApiDataType.Boolean },
					[typeof(byte)]           = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
					[typeof(sbyte)]          = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
					[typeof(short)]          = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
					[typeof(ushort)]         = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
					[typeof(int)]            = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
					[typeof(uint)]           = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int32 },
					[typeof(long)]           = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int64 },
					[typeof(ulong)]          = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int64 },
					[typeof(float)]          = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Float },
					[typeof(double)]         = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Double },
					[typeof(decimal)]        = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Double },
					[typeof(byte[])]         = () => new OpenApiSchema { Type = OpenApiDataType.String, Format  = OpenApiDataFormat.Byte },
					[typeof(string)]         = () => new OpenApiSchema { Type = OpenApiDataType.String },
					[typeof(char)]           = () => new OpenApiSchema { Type = OpenApiDataType.String },
					[typeof(DateTime)]       = () => new OpenApiSchema { Type = OpenApiDataType.String, Format  = OpenApiDataFormat.DateTime },
					[typeof(DateTimeOffset)] = () => new OpenApiSchema { Type = OpenApiDataType.String, Format  = OpenApiDataFormat.DateTime },
					[typeof(TimeSpan)]       = () => new OpenApiSchema { Type = OpenApiDataType.String, Format  = OpenApiDataFormat.DateSpan },
					[typeof(Guid)]           = () => new OpenApiSchema { Type = OpenApiDataType.String, Format  = OpenApiDataFormat.Uuid },
					[typeof(Uri)]            = () => new OpenApiSchema { Type = OpenApiDataType.String, Format  = OpenApiDataFormat.Uri },
					[typeof(Version)]        = () => new OpenApiSchema { Type = OpenApiDataType.String },
					[typeof(DateOnly)]       = () => new OpenApiSchema { Type = OpenApiDataType.String, Format  = OpenApiDataFormat.Date },
					[typeof(TimeOnly)]       = () => new OpenApiSchema { Type = OpenApiDataType.String, Format  = OpenApiDataFormat.Time },
					[typeof(Int128)]         = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int128 },
					[typeof(UInt128)]        = () => new OpenApiSchema { Type = OpenApiDataType.Integer, Format = OpenApiDataFormat.Int128 },
				};
				public static class OpenApiDataType
				{
					public const string Boolean = "boolean";
					public const string Integer = "integer";
					public const string Number = "number";
					public const string String = "string";
					public const string Array = "array";
					public const string Object = "object";
				}
				public static class OpenApiDataFormat
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
				public static OpenApiSchema BuildFrom(Type primitiveType)
				=> primitiveType switch
				{
					var t when PrimitiveTypeOpenApiSchemaBuildersByDeclaringType.TryGetValue(t, out var schemaBuilder) => schemaBuilder.Invoke(),
					_ => throw new InvalidOperationException()
				};
			}

			public static class OpenApiPrimitiveBuilder
			{
				static readonly Dictionary<Type, Func<object, IOpenApiPrimitive>> OpenApiPrimitiveBuilderByDeclaringType = new()
				{
					[typeof(bool)]           = obj => new OpenApiBoolean((bool)obj),
					[typeof(byte)]           = obj => new OpenApiByte((byte)obj),
					[typeof(sbyte)]          = obj => new OpenApiByte((byte)obj),
					[typeof(short)]          = obj => new OpenApiInteger((short)obj),
					[typeof(ushort)]         = obj => new OpenApiInteger((ushort)obj),
					[typeof(int)]            = obj => new OpenApiInteger((int)obj),
					[typeof(uint)]           = obj => new OpenApiInteger((int)obj),
					[typeof(long)]           = obj => new OpenApiLong((long)obj),
					[typeof(ulong)]          = obj => new OpenApiLong((long)obj),
					[typeof(float)]          = obj => new OpenApiFloat((float)obj),
					[typeof(double)]         = obj => new OpenApiDouble((double)obj),
					[typeof(decimal)]        = obj => new OpenApiDouble((double)obj),
					[typeof(byte[])]         = obj => new OpenApiString((string)obj),
					[typeof(string)]         = obj => new OpenApiString((string)obj),
					[typeof(char)]           = obj => new OpenApiString((string)obj),
					[typeof(DateTime)]       = obj => new OpenApiDateTime((DateTime)obj),
					[typeof(DateTimeOffset)] = obj => new OpenApiDateTime((DateTimeOffset)obj),
					[typeof(TimeSpan)]       = obj => new OpenApiString((string)obj),
					[typeof(Guid)]           = obj => new OpenApiString(obj.ToString()),
					[typeof(Uri)]            = obj => new OpenApiString((string)obj),
					[typeof(Version)]        = obj => new OpenApiString((string)obj),
					[typeof(DateOnly)]       = obj => new OpenApiString((string)obj),
					[typeof(TimeOnly)]       = obj => new OpenApiString((string)obj),
					[typeof(Int128)]         = obj => new OpenApiLong((long)obj),
					[typeof(UInt128)]        = obj => new OpenApiLong((long)obj),
				};
				public static IOpenApiPrimitive BuildFrom(object value)
				=> value.GetType() switch
				{
					var t when OpenApiPrimitiveBuilderByDeclaringType.ContainsKey(t) => OpenApiPrimitiveBuilderByDeclaringType[t].Invoke(value),
					_ => throw new InvalidOperationException()
				};
			}
		}
	}
}
