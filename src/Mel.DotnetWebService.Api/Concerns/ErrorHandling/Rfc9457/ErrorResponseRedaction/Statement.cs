using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Rfc9457.ErrorResponseRedaction;

sealed class Statement : ConstrainedValue<string>, IConstrainedValue<string, Statement>
{
	Statement(string value) : base(value)
	{
		Value = value switch
		{
			null => "",
			[var firstCharacter, ..] when char.IsLetter(firstCharacter) && !char.IsUpper(firstCharacter) => throw ObjectConstructionException.WhenConstructingAMemberFor<Statement>(nameof(Value), value, $"@member must start with an upper-case letter."),
			[.., var lastCharacter] when char.IsLetterOrDigit(lastCharacter) || char.IsWhiteSpace(lastCharacter) => throw ObjectConstructionException.WhenConstructingAMemberFor<Statement>(nameof(Value), value, $"@member must end with a dot or a special character."),
			_ => value
		};
	}

	public static ExampleValues<string> Examples
	=> ExampleValues.ValidAndInvalid(
		validValues: new[]
		{
			"As aspiring Software Craftspeople we are raising the bar of professional software development by practicing it and helping others learn the craft.",
			"We are uncovering better ways of developing software by doing it and helping others do it.",
			"The quick brown fox jumps over the lazy dog.",
			""
		},
		errorMessagesByInvalidValue: new Dictionary<string, string>
		{
			{
				"as aspiring Software Craftspeople we are raising the bar of professional software development by practicing it and helping others learn the craft.",
				"Value must start with an upper-case letter."
			},
			{
				"We are uncovering better ways of developing software by doing it and helping others do it",
				"Value must end with a dot or a special character."
			},
		});

	public static Statement ApplyConstraintsTo(string scalarValue)
	=> new(scalarValue);

	public static Statement operator +(Statement first, Statement? second)
	=> (first, second) switch
	{
		(_, null) => first,
		_ => new Statement($"{first} {second}")
	};
}
