using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Globalization.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.Serialization;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

static class JsonSerializerOptionsArchetype
{
	public static readonly JsonSerializerOptions ThatInvolvesConstrainedTypesHandling;
	static JsonSerializerOptionsArchetype()
	{
		ThatInvolvesConstrainedTypesHandling = new JsonSerializerOptions()
		{
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};
		ThatInvolvesConstrainedTypesHandling.Converters.Add(new KeyValuePairsWithComplexKeyTypeJsonConverter());
		ThatInvolvesConstrainedTypesHandling.Converters.Add(new ConstrainedTypeJsonConverter());
		ThatInvolvesConstrainedTypesHandling.Converters.Add(new JsonStringEnumConverter());
		ThatInvolvesConstrainedTypesHandling.Converters.Add(new TypeJsonConverter());
		ThatInvolvesConstrainedTypesHandling.Converters.Add(new MethodInfoJsonConverter());
		ThatInvolvesConstrainedTypesHandling.Converters.Add(new CultureInfoJsonConverter());
		ThatInvolvesConstrainedTypesHandling.Converters.Add(new RegionInfoJsonConverter());
	}
}
