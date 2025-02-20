namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

class Statement
{
	readonly string _encapsulated;
	Statement(string? str)
	{
		_encapsulated = str switch
		{
			null => "",
			[var firstCharacter, ..] when !char.IsUpper(firstCharacter) => throw ObjectConstructionException.WhenConstructingAMemberFor<Statement>(nameof(_encapsulated), str, $"A {nameof(Statement)} starts with an upper-case letter."),
			[.., var lastCharacter] when lastCharacter is not '.' => throw ObjectConstructionException.WhenConstructingAMemberFor<Statement>(nameof(_encapsulated), str, $"A {nameof(Statement)} ends with a dot."),
			_ => str
		};
	}

	public static Statement FromString(string str)
	=> new(str);

	public static implicit operator string(Statement statement)
	=> statement._encapsulated;

	public override string ToString()
	=> _encapsulated;

	public static Statement operator +(Statement first, Statement? second)
	=> (first, second) switch
	{
		(_, null) => first,
		_ => new Statement($"{first} {second}")
	};
}
