namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation;

static class TypeKind
{
	public const string ThatDoesNotInvolveAnyConstrainedType = "no constrained type involved";

	public const string ConstrainedScalarValue = nameof(ConstrainedScalarValue);

	public static class DataStructure
	{
		public static class ThatContainsEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems
		{
			public const string AconstrainedScalarValue = $"{nameof(DataStructure)}_{nameof(ThatContainsEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
		}
	}

	public static class CollectionOf
	{
		public const string ConstrainedScalarValues = $"{nameof(CollectionOf)}_{nameof(ConstrainedScalarValues)}";
		public static class DataStructures
		{
			public static class ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems
			{
				public const string AconstrainedScalarValue = $"{nameof(CollectionOf)}_{nameof(DataStructures)}_{nameof(ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
			}
		}

		public static class CollectionsOf
		{
			public const string ConstrainedScalarValues = $"{nameof(CollectionOf)}_{nameof(CollectionsOf)}_{nameof(ConstrainedScalarValues)}";
			public static class DataStructures
			{
				public static class ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems
				{
					public const string AconstrainedScalarValue = $"{nameof(CollectionOf)}_{nameof(CollectionsOf)}_{nameof(DataStructures)}_{nameof(ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
				}
			}
			public static class Collections_Of
			{
				public const string ConstrainedScalarValues = $"{nameof(CollectionOf)}_{nameof(CollectionsOf)}_{nameof(Collections_Of)}_{nameof(ConstrainedScalarValues)}";
				public static class DataStructures
				{
					public static class ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems
					{
						public const string AconstrainedScalarValue = $"{nameof(CollectionOf)}_{nameof(CollectionsOf)}_{nameof(Collections_Of)}_{nameof(DataStructures)}_{nameof(ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
					}
				}
			}
			public static class Dictionaries
			{
				public static class ThatInvolveEitherAsKeyOrValue
				{
					public const string ConstrainedScalarValues = $"{nameof(CollectionOf)}_{nameof(CollectionsOf)}_{nameof(Dictionaries)}_{nameof(ThatInvolveEitherAsKeyOrValue)}_{nameof(ConstrainedScalarValues)}";
					public static class DataStructures
					{
						public static class ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems
						{
							public const string AconstrainedScalarValue = $"{nameof(CollectionOf)}_{nameof(CollectionsOf)}_{nameof(Dictionaries)}_{nameof(DataStructures)}_{nameof(ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
						}
					}
				}
			}
		}
		public static class Dictionaries
		{
			public static class ThatInvolveEitherAsKeyOrValue
			{
				public const string ConstrainedScalarValues = $"{nameof(CollectionOf)}_{nameof(Dictionaries)}_{nameof(ThatInvolveEitherAsKeyOrValue)}_{nameof(ConstrainedScalarValues)}";
				public static class ADataStructure
				{
					public static class ThatContainsEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems
					{
						public const string AconstrainedScalarValue = $"{nameof(CollectionOf)}_{nameof(Dictionaries)}_{nameof(ThatInvolveEitherAsKeyOrValue)}_{nameof(ADataStructure)}_{nameof(ThatContainsEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
					}
				}
				public static class ACollectionOf
				{
					public const string ConstrainedScalarValues = $"{nameof(CollectionOf)}_{nameof(Dictionaries)}_{nameof(ThatInvolveEitherAsKeyOrValue)}_{nameof(ACollectionOf)}_{nameof(ConstrainedScalarValues)}";
					public static class DataStructures
					{
						public static class ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems
						{
							public const string AconstrainedScalarValue = $"{nameof(CollectionOf)}_{nameof(Dictionaries)}_{nameof(ThatInvolveEitherAsKeyOrValue)}_{nameof(ACollectionOf)}_{nameof(DataStructures)}_{nameof(ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
						}
					}
				}
			}
		}
	}
	public static class Dictionary
	{
		public static class ThatInvolvesEitherAsKeyOrValue
		{
			public const string AConstrainedScalarValue = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(AConstrainedScalarValue)}";
			public static class ACollectionOf
			{
				public const string ConstrainedScalarValues = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ACollectionOf)}_{nameof(ConstrainedScalarValues)}";
				public static class DataStructures
				{
					public static class ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems
					{
						public const string AconstrainedScalarValue = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ACollectionOf)}_{nameof(DataStructures)}_{nameof(ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
					}
				}
				public static class CollectionsOf
				{
					public const string ConstrainedScalarValues = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ACollectionOf)}_{nameof(CollectionsOf)}_{nameof(ConstrainedScalarValues)}";
					public static class DataStructures
					{
						public static class ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems
						{
							public const string AconstrainedScalarValue = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ACollectionOf)}_{nameof(CollectionsOf)}_{nameof(DataStructures)}_{nameof(ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
						}
					}
				}
				public static class Dictionaries
				{
					public static class ThatInvolveEitherAsKeyOrValue
					{
						public static class ADataStructure
						{
							public static class ThatContainsEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems
							{
								public const string AconstrainedScalarValue = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ACollectionOf)}_{nameof(Dictionaries)}_{nameof(ThatInvolveEitherAsKeyOrValue)}_{nameof(ADataStructure)}_{nameof(AconstrainedScalarValue)}";
							}
						}
					}
				}
			}
			public static class ADictionary
			{
				public static class ThatInvolvesEitherAsKeyOrValue
				{
					public const string AConstrainedScalarValue = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ADictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(AConstrainedScalarValue)}";
					public static class ADataStructure
					{
						public static class ThatContainsEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems
						{
							public const string AconstrainedScalarValue = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ADictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ADataStructure)}_{nameof(ThatContainsEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
						}
					}
					public static class ACollectionOf
					{
						public const string ConstrainedScalarValues = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ADictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ACollectionOf)}_{nameof(ConstrainedScalarValues)}";
						public static class DataStructures
						{
							public static class ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems
							{
								public const string AconstrainedScalarValue = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ADictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ACollectionOf)}_{nameof(DataStructures)}_{nameof(ThatContainEitherDirectlyOrInTheirSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
							}
						}
					}
					public static class A_Dictionary
					{
						public static class ThatInvolvesEitherAsKeyOrValue
						{
							public const string ConstrainedScalarValues = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ADictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(A_Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ConstrainedScalarValues)}";
							public static class ADataStructure
							{
								public static class ThatContainsEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems
								{
									public const string AconstrainedScalarValue = $"{nameof(Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ADictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(A_Dictionary)}_{nameof(ThatInvolvesEitherAsKeyOrValue)}_{nameof(ADataStructure)}_{nameof(ThatContainsEitherDirectlyOrInItsSubStructuresIncludingCollectionAndDictionaryItems)}_{nameof(AconstrainedScalarValue)}";
								}
							}
						}
					}
				}
			}
		}
	}
}
