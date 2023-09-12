using Mel.DotnetWebService.CrossCuttingConcerns.EnumTypesHandling;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

public static class KeyValuePairArchetype
{
	public static readonly KeyValuePair<int, string> ZeroValuedEnumItem_Named_TechnicalDefaultEnumValue
	= KeyValuePair.Create(0, EnumTypesNamingConvention.TechnicalDefaultEnumValue);
}
