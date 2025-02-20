using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingExpressionTrees;
using Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator.UsingReflectiveCalls;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public abstract class ExampleValueGenerator
{
	public static T GenerateExampleOf<T>(int salt = 0)
	=> (T) GenerateExampleOf(typeof(T), salt);

	public static object GenerateExampleOf(Type type, int salt = 0, Using implementation = Using.ReflectiveCalls)
	=> implementation switch
	{
		Using.ReflectiveCalls => ExampleValueGeneratorUsingReflectiveCalls.GenerateExampleOf(type, salt),
		Using.ExpressionTrees => ExampleValueGeneratorUsingExpressionTrees.GenerateExampleOf(type, salt),
		_ => throw new NotImplementedException()
	};
}
