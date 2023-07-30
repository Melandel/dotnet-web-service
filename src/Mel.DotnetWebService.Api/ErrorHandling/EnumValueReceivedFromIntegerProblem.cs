namespace Mel.DotnetWebService.Api.ErrorHandling;

class EnumValueReceivedFromIntegerProblem : Problem
{
	public int IntegerValue { get; }
	public string ParameterName { get; }
	public string[] AcceptedEnumValues { get; }
	public const string IntegerValueDebuggingInformationKey = "integer-value";
	public const string EnumNameDebuggingInformationKey = "enum-name";
	public const string AcceptedEnumValuesDebuggingInformationKey = "enum-accepted";
	protected override IDictionary<string, object?> DebuggingInformation
	=> new Dictionary<string, object?>
	{
		{ IntegerValueDebuggingInformationKey, IntegerValue },
		{ EnumNameDebuggingInformationKey, ParameterName },
		{ AcceptedEnumValuesDebuggingInformationKey, AcceptedEnumValues },
	};

	EnumValueReceivedFromIntegerProblem(ProblemType type, ProblemOccurrence occurrence, int integerValue, string parameterName, string[] acceptedEnumValues)
		: base(type, occurrence)
	{
		IntegerValue = integerValue;
		ParameterName = parameterName;
		AcceptedEnumValues = acceptedEnumValues;
	}

	public static EnumValueReceivedFromIntegerProblem From(int integer, string parameterName, IEnumerable<string> acceptedEnumValues)
		=> new(
			ProblemType.EnumValueReceivedFromInteger,
			ProblemOccurrence.FromExplanationThatShouldHelpTheClientCorrectTheProblem($"An enum value was passed as integer instead of being one of: {string.Join(',', acceptedEnumValues)}"),
			integer,
			parameterName,
			acceptedEnumValues.ToArray());
}
