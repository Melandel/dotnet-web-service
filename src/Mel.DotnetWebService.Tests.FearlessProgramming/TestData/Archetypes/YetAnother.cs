using System.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes;

static class YetAnother
{
	static readonly int SaltLevel = 2;

	public static T           Value<T>(int salt = 0) => ExampleValueGenerator.GenerateExampleOf<T>(SaltLevel+salt);

	public static object      Value             (Type type, int salt = 0) => ExampleValueGenerator.GenerateExampleOf(type, SaltLevel+salt);
	public static dynamic     ValueAsDynamic    (Type type, int salt = 0) => ExampleValueGenerator.GenerateExampleOf(type, SaltLevel+salt);
	public static IEnumerable ValueAsIEnumerable(Type type, int salt = 0) => (IEnumerable) ExampleValueGenerator.GenerateExampleOf(type, SaltLevel+salt);
	public static ICollection ValueAsICollection(Type type, int salt = 0) => (ICollection) ExampleValueGenerator.GenerateExampleOf(type, SaltLevel+salt);
	public static IList       ValueAsIList      (Type type, int salt = 0) => (IList)       ExampleValueGenerator.GenerateExampleOf(type, SaltLevel+salt);

	public static T[]             ArrayOf<T>       (int salt = 0) => ExampleValueGenerator.GenerateExampleOf<T[]>(salt: SaltLevel+salt);
	public static List<T>         ListOf<T>        (int salt = 0) => ExampleValueGenerator.GenerateExampleOf<List<T>>(salt: SaltLevel+salt);
	public static Dictionary<T,U> DictionaryOf<T,U>(int salt = 0) => ExampleValueGenerator.GenerateExampleOf<Dictionary<T,U>>(salt: SaltLevel+salt);

	public static IList       CollectionOf(Type itemType,                int salt = 0) => (IList)       ExampleValueGenerator.GenerateExampleOf(typeof(List<>).MakeGenericType(itemType), SaltLevel+salt);
	public static IDictionary DictionaryOf(Type keyType, Type valueType, int salt = 0) => (IDictionary) ExampleValueGenerator.GenerateExampleOf(typeof(Dictionary<,>).MakeGenericType(keyType, valueType), salt: SaltLevel+salt);
}
