namespace Mel.DotnetWebService.CodingStandards.CRAFT001.NoPrivateModifier;

class NoPrivateModifierCodeFixProviderShould
{
	[Test]
	public async Task RemovePrivateModifierFromMemberDeclaration()
	{
		var test = new CSharpCodeFixTest<NoPrivateModifierAnalyzer, NoPrivateModifierCodeFixProvider, DefaultVerifier>
		{
			TestCode = """
				class SomeClass
				{
					private readonly int _intField;
					public string StringProperty { get; private set; }
				}
				""",
			FixedCode = """
				class SomeClass
				{
					readonly int _intField;
					public string StringProperty { get; private set; }
				}
				"""
		};
		test.ExpectedDiagnostics.Add(
			DiagnosticResult.CompilerWarning("CRAFT001").WithSpan(3, 2, 3, 9));

		await test.RunAsync();
	}
}
