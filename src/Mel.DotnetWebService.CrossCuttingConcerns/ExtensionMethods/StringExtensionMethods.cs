using System.Text;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ExtensionMethods;

public static class StringExtensionMethods
{
	public static bool Matches(this string? left, string? right, bool caseSensitive = false)
	=> caseSensitive
		? string.Equals(left, right)
		: string.Equals(left, right, StringComparison.InvariantCultureIgnoreCase);

	public static string AsValidJsonValue(this string? str, Type deserializationType)
	=> str switch
	{
		null => "",
		_ when deserializationType.IsSerializedIntoString() => $"\"{str}\"",
		_ => str
	};

	public static string WithFirstCharacterInLowerCase(this string str)
	=> str switch
	{
		"" => "",
		[var c] => $"{char.ToLower(c)}",
		_ => $"{char.ToLower(str[0])}{str.Substring(1)}"
	};

	public static string IndentWith(this string str, string indent)
	{
		if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(indent))
		{
			return str;
		}

		var roughEstimateOfAggregatedIndentLengthes = indent.Length * 16;
		var builder = new StringBuilder(str.Length + roughEstimateOfAggregatedIndentLengthes);
		bool hasContent = false;

		foreach (char c in str)
		{
			if (!hasContent && c != '\r' && c != '\n')
			{
				builder.Append(indent);
				hasContent = true;
			}

			builder.Append(c);

			if (c == '\n')
			{
				hasContent = false;
			}
		}

		return builder.ToString();
	}
}
