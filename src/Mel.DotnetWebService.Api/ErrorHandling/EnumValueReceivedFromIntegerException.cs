namespace Mel.DotnetWebService.Api.ErrorHandling;

class EnumValueReceivedFromIntegerException : Exception
{
	public int IntegerValue { get; }
	public string ParameterName { get; }
	public string[] AcceptedEnumValues { get; }

	EnumValueReceivedFromIntegerException(int integerValue, string parameterName, string[] acceptedEnumValues)
	{
		IntegerValue = integerValue;
		ParameterName = parameterName;
		AcceptedEnumValues = acceptedEnumValues;
	}

	public static EnumValueReceivedFromIntegerException From(int integerValue, string parameterName, IEnumerable<string> acceptedEnumValues)
		=> new EnumValueReceivedFromIntegerException(integerValue, parameterName, acceptedEnumValues.ToArray());
}
