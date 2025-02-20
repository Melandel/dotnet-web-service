using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes;
using Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.Guids;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

static class ClassArchetype
{
	public record SimplePositionalRecord(
		string FirstProperty,
		NonEmptyGuid SecondProperty);

	public class NonEmptyGuidStartingWithTheCharacter1 : Constrained<NonEmptyGuid>
	{
		NonEmptyGuidStartingWithTheCharacter1(NonEmptyGuid encapsulated) : base(encapsulated)
		{
			if (!Value.ToString().StartsWith('1'))
			{
				throw ObjectConstructionException.WhenConstructingAMemberFor<NonEmptyGuidStartingWithTheCharacter1>(nameof(Value), Value, "@member must start with the character '1'");
			}
		}

		public static NonEmptyGuidStartingWithTheCharacter1 FromString(string guidString)
		{
			try { return new(NonEmptyGuid.FromString(guidString)); }
			catch (ObjectConstructionException objectConstructionException) {       objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuidStartingWithTheCharacter1>(guidString); throw; }
			catch (Exception defect)                                        { throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuidStartingWithTheCharacter1>(defect, guidString); }
		}
	}

	public class NonEmptyGuids : Constrained<NonEmptyGuid[]>
	{
		NonEmptyGuids(NonEmptyGuid[] value) : base(value)
		{ }
		public static NonEmptyGuids FromStrings(params string[] strings)
		{
			try
			{
				return new(strings.Select(str => NonEmptyGuid.FromGuid(Guid.Parse(str))).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuids>(strings);
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
				objectConstructionException.EnrichConstructionFailureContextWith<ValueObjectFeaturingNonEmptyGuidCollects>(nonEmptyGuids, multipleSetsOfNonEmptyGuids);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<ValueObjectFeaturingNonEmptyGuidCollects>(developerMistake, nonEmptyGuids, multipleSetsOfNonEmptyGuids);
			}
		}
	}
	public class NonEmptyGuidsWithPublicProperty : Constrained<NonEmptyGuid[]>
	{
		public int PublicProperty => Value.Length;
		NonEmptyGuidsWithPublicProperty(NonEmptyGuid[] value) : base(value)
		{
		}

		public static NonEmptyGuidsWithPublicProperty FromStrings(params string[] strings)
		{
			try
			{
				return new(strings.Select(guid => NonEmptyGuid.FromGuid(Guid.Parse(guid))).ToArray());
			}
			catch (ObjectConstructionException objectConstructionException)
			{
				objectConstructionException.EnrichConstructionFailureContextWith<NonEmptyGuidsWithPublicProperty>(strings);
				throw;
			}
			catch (Exception developerMistake)
			{
				throw ObjectConstructionException.WhenConstructingAnInstanceOf<NonEmptyGuidsWithPublicProperty>(developerMistake, strings);
			}
		}
	}
}
