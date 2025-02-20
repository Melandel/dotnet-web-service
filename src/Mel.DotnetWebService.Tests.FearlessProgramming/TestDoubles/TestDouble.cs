using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDataGeneration.DynamicallyGeneratedTypes;
using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDoubleGeneration.DummyGeneration;
using Mel.DotnetWebService.Tests.FearlessProgramming.Concerns.TestDoubleGeneration.ExceptionThrowingStubGeneration;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestDoubles;

public abstract record TestDouble<TInterface, TSelf> where TSelf : TestDouble<TInterface, TSelf>, TInterface, new()
{
	TestDoubleCategory Category { get; set; } = TestDoubleCategory.TechnicalDefaultEnumValue;

	public static TInterface Dummy => DummyGenerator.Generate<TInterface>();
	public static TInterface StubThatThrows
	=> ExceptionThrowingStubGenerator.BuildStubThatThrowsOnEachCall<TInterface>(
		ExceptionUnknownAtCompileTimeGenerator.GenerateWithMessage(
			string.Format(
				"{0}.{1} has thrown. Shocker!",
				typeof(TSelf).GetName(),
				nameof(StubThatThrows))));

	public static TSelf Stub => new() { Category  = TestDoubleCategory.Stub };
	protected bool IsStub            => Category is TestDoubleCategory.Stub;

	public static TSelf Fake   => new() { Category  = TestDoubleCategory.Fake };
	protected bool IsFake              => Category is TestDoubleCategory.Fake;

	public static TSelf Spy    => new() { Category  = TestDoubleCategory.Spy };
	protected bool IsSpy               => Category is TestDoubleCategory.Spy;

	public static TSelf Mock   => new() { Category  = TestDoubleCategory.Mock };
	protected bool IsMock              => Category is TestDoubleCategory.Mock;

	public static TSelf Configure => new() { Category  = TestDoubleCategory.Unconventional };
	protected bool IsUnconventional       => Category is TestDoubleCategory.Unconventional;
}
