namespace Mel.DotnetWebService.CrossCuttingConcerns.EnumTypesHandling;

public class EnumValueReceivedFromIntegerException : Exception
{
	public int IntegerValue { get; }
	public string ParameterName { get; }
	public string[] SupportedEnumValues { get; }

	EnumValueReceivedFromIntegerException(int integerValue, string parameterName, string[] supportedEumValues)
	{
		IntegerValue = integerValue;
		ParameterName = parameterName;
		SupportedEnumValues = supportedEumValues;
	}

	public static EnumValueReceivedFromIntegerException From(int integerValue, string parameterName, IEnumerable<string> supportedEumValues)
	=> new EnumValueReceivedFromIntegerException(integerValue, parameterName, supportedEumValues.ToArray());
}
