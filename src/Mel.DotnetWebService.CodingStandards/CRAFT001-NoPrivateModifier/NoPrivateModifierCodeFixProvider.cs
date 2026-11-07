namespace Mel.DotnetWebService.CodingStandards.CRAFT001.NoPrivateModifier;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NoPrivateModifierCodeFixProvider))]
[Shared]
public sealed class NoPrivateModifierCodeFixProvider : CodeFixProvider
{
	public override ImmutableArray<string> FixableDiagnosticIds => [NoPrivateModifierAnalyzer.DiagnosticId];
	public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

	public override async Task RegisterCodeFixesAsync(CodeFixContext context)
	{
		var root = await context.Document
			.GetSyntaxRootAsync(context.CancellationToken)
			.ConfigureAwait(false);

		if (root is null)
		{
			return;
		}

		var diagnostic = context.Diagnostics[0];
		var token = root.FindToken(diagnostic.Location.SourceSpan.Start);
		var member = token.Parent?.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();
		if (member is null)
		{
			return;
		}

		context.RegisterCodeFix(
			CodeAction.Create(
				"Remove 'private' modifier",
				ct => RemovePrivateModifierAsync(context.Document, member, token, ct),
				"RemovePrivateModifier"),
			diagnostic);
	}

	static async Task<Document> RemovePrivateModifierAsync(Document document, MemberDeclarationSyntax member, SyntaxToken privateToken, CancellationToken cancellationToken)
	{
		var modifiers = GetModifiers(member);

		if (modifiers.Count == 0)
		{
			return document;
		}

		var index = modifiers.IndexOf(privateToken);
		var updatedModifiers = modifiers.RemoveAt(index);
		if (index < updatedModifiers.Count)
		{
			var trivia = privateToken.LeadingTrivia;

			updatedModifiers = updatedModifiers.Replace(
				updatedModifiers[index],
				updatedModifiers[index].WithLeadingTrivia(trivia));
		}

		var updatedMember = WithModifiers(member, updatedModifiers);
		if (updatedMember is null)
		{
			return document;
		}

		var root = await document
			.GetSyntaxRootAsync(cancellationToken)
			.ConfigureAwait(false);

		if (root is null)
		{
			return document;
		}

		var newRoot = root.ReplaceNode(member, updatedMember);

		return document.WithSyntaxRoot(newRoot);
	}

	static SyntaxTokenList GetModifiers(MemberDeclarationSyntax member)
	=> member switch
	{
		BaseFieldDeclarationSyntax field => field.Modifiers,
		BaseMethodDeclarationSyntax method => method.Modifiers,
		BasePropertyDeclarationSyntax property => property.Modifiers,
		_ => default
	};

	static MemberDeclarationSyntax? WithModifiers(MemberDeclarationSyntax member, SyntaxTokenList modifiers)
	=> member switch
	{
		FieldDeclarationSyntax field => field.WithModifiers(modifiers),
		EventFieldDeclarationSyntax eventField => eventField.WithModifiers(modifiers),
		MethodDeclarationSyntax method => method.WithModifiers(modifiers),
		ConstructorDeclarationSyntax constructor => constructor.WithModifiers(modifiers),
		DestructorDeclarationSyntax destructor => destructor.WithModifiers(modifiers),
		PropertyDeclarationSyntax property => property.WithModifiers(modifiers),
		IndexerDeclarationSyntax indexer => indexer.WithModifiers(modifiers),
		EventDeclarationSyntax eventDeclaration => eventDeclaration.WithModifiers(modifiers),
		OperatorDeclarationSyntax op => op.WithModifiers(modifiers),
		ConversionOperatorDeclarationSyntax conversion => conversion.WithModifiers(modifiers),
		_ => null
	};
}

