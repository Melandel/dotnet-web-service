using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

static class ClassArchetype
{
	public record SimplePositionalRecord(
		string FirstProperty,
		NonEmptyGuid SecondProperty);

	public class NonEmptyGuids
	{
		readonly NonEmptyGuid[] _encapsulated;
		NonEmptyGuids(NonEmptyGuid[] encapsulated) => _encapsulated = encapsulated;
		public static NonEmptyGuids FromStrings(params string[] strings)
		{
			try
			{
				return new(strings.Select(str => NonEmptyGuid.FromGuid(Guid.Parse(str))).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichWithInformationAbout<NonEmptyGuids>(strings);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuids>(developerMistake, strings);
			}
		}
	}
	public class ValueObjectFeaturingNonEmptyGuidCollects
	{
		readonly NonEmptyGuids _nonEmptyGuids;
		readonly NonEmptyGuids[] _nonEmptyGuidss;
		ValueObjectFeaturingNonEmptyGuidCollects(NonEmptyGuids nonEmptyGuids, NonEmptyGuids[] nonEmptyGuidss)
		{
			_nonEmptyGuids = nonEmptyGuids;
			_nonEmptyGuidss = nonEmptyGuidss;
		}
		public static ValueObjectFeaturingNonEmptyGuidCollects From(string[] nonEmptyGuids, string[][] multipleSetsOfNonEmptyGuids)
		{
			try
			{
				return new(
					NonEmptyGuids.FromStrings(nonEmptyGuids),
					multipleSetsOfNonEmptyGuids.Select(array => NonEmptyGuids.FromStrings(array)).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichWithInformationAbout<ValueObjectFeaturingNonEmptyGuidCollects>(nonEmptyGuids, multipleSetsOfNonEmptyGuids);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<ValueObjectFeaturingNonEmptyGuidCollects>(developerMistake, nonEmptyGuids, multipleSetsOfNonEmptyGuids);
			}
		}
	}
	public class NonEmptyGuidsWithPublicProperty
	{
		readonly NonEmptyGuid[] _encapsulated;
		public int PublicProperty => _encapsulated.Length;
		NonEmptyGuidsWithPublicProperty(NonEmptyGuid[] encapsulated)
		{
			_encapsulated = encapsulated;
		}

		public static NonEmptyGuidsWithPublicProperty FromStrings(params string[] strings)
		{
			try
			{
				return new(strings.Select(guid => NonEmptyGuid.FromGuid(Guid.Parse(guid))).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichWithInformationAbout<NonEmptyGuidsWithPublicProperty>(strings);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuidsWithPublicProperty>(developerMistake, strings);
			}
		}
	}
}
