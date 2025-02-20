namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingReflectiveCalls;

abstract class ExampleValueGeneratorUsingReflectiveCalls
{
	static readonly Dictionary<Type, Dictionary<int, object>> CachedExamplesBySaltByType = new();
	internal abstract object GenerateInstanceOf(Type type, int salt = 0);

	public static object GenerateExampleOf(Type type, int salt = 0)
	{
		if (CachedExamplesBySaltByType.TryGetValue(type, out var cachedExamplesBySalt))
		{
			if (cachedExamplesBySalt.TryGetValue(salt, out var cachedExample))
			{
				return cachedExample;
			}

			var example = GenerateExample(type, salt);
			cachedExamplesBySalt.Add(salt, example);
			return example;
		}
		else
		{
			var example = GenerateExample(type, salt);
			CachedExamplesBySaltByType.Add(type, new Dictionary<int, object>() { { salt, example } });
			return example;
		}
	}
	static object GenerateExample(Type type, int salt)
	{
		var typeCategory = TypeCategoryResolver.Resolve(type);
		var generator = ExampleValueGeneratorUsingReflectiveCallsResolver.Resolve(typeCategory, type);
		var example = generator.GenerateInstanceOf(type, salt);
		return example;
	}
}
