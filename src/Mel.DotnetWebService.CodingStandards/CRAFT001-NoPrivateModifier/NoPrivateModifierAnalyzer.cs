namespace Mel.DotnetWebService.CodingStandards.CRAFT001.NoPrivateModifier;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class NoPrivateModifierAnalyzer : DiagnosticAnalyzer
{
	public const string DiagnosticId = "CRAFT001";

	static readonly DiagnosticDescriptor Rule = new(
		id: DiagnosticId,
		title: "Avoid private modifier",
		messageFormat: "The 'private' modifier is not allowed on class members",
		category: "Design",
		defaultSeverity: DiagnosticSeverity.Warning,
		isEnabledByDefault: true,
		description: "We want to incentivize responsibility by making the use of `public` a decision as visible, and intentional, as possible.");

	public override ImmutableArray<DiagnosticDescriptor>
		SupportedDiagnostics => [Rule];

	public override void Initialize(
		AnalysisContext context)
	{
		context.ConfigureGeneratedCodeAnalysis(
			GeneratedCodeAnalysisFlags.None);

		context.EnableConcurrentExecution();

		context.RegisterSyntaxNodeAction(
			AnalyzeMember,
			SyntaxKind.ClassDeclaration);
	}

	static void AnalyzeMember(
		SyntaxNodeAnalysisContext context)
	{
		var classDeclaration =
			(ClassDeclarationSyntax)context.Node;

		foreach (var member in classDeclaration.Members)
		{
			SyntaxToken privateToken = default;

			switch (member)
			{
				case FieldDeclarationSyntax field:
					privateToken = FindPrivate(field.Modifiers);
					break;

				case MethodDeclarationSyntax method:
					privateToken = FindPrivate(method.Modifiers);
					break;

				case PropertyDeclarationSyntax property:
					privateToken = FindPrivate(property.Modifiers);
					break;

				case EventDeclarationSyntax eventDeclaration:
					privateToken = FindPrivate(eventDeclaration.Modifiers);
					break;

				case EventFieldDeclarationSyntax eventField:
					privateToken = FindPrivate(eventField.Modifiers);
					break;

				case ConstructorDeclarationSyntax constructor:
					privateToken = FindPrivate(constructor.Modifiers);
					break;

				case DestructorDeclarationSyntax destructor:
					privateToken = FindPrivate(destructor.Modifiers);
					break;

				case IndexerDeclarationSyntax indexer:
					privateToken = FindPrivate(indexer.Modifiers);
					break;

				case OperatorDeclarationSyntax op:
					privateToken = FindPrivate(op.Modifiers);
					break;

				case ConversionOperatorDeclarationSyntax conversion:
					privateToken = FindPrivate(conversion.Modifiers);
					break;
			}

			if (privateToken != default)
			{
				context.ReportDiagnostic(
					Diagnostic.Create(
						Rule,
						privateToken.GetLocation()));
			}
		}
	}

	static SyntaxToken FindPrivate(
		SyntaxTokenList modifiers)
	{
		foreach (var modifier in modifiers)
		{
			if (modifier.IsKind(SyntaxKind.PrivateKeyword))
				return modifier;
		}

		return default;
	}
}


