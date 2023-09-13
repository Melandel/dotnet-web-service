using System.Text;

namespace Mel.DotnetWebService.Api.Concerns.Routing.AttributeRouteTokenReplacement;

class KebabCaseConverter
{
	public string? Convert(string? str)
	{
		if (String.IsNullOrEmpty(str))
		{
			return str;
		}

		StringBuilder builder = new StringBuilder();
		for (var i = 0; i < str.Length; i++)
		{
			if (char.IsLower(str[i])) // if current char is already lowercase
			{
				builder.Append(str[i]);
			}
			else if (i == 0) // if current char is the first char
			{
				builder.Append(char.ToLower(str[i]));
			}
			else if (char.IsDigit(str[i]) && !char.IsDigit(str[i - 1])) // if current char is a number and the previous is not
			{
				if (char.IsLower(str[i - 1]))
				{
					builder.Append('-');
					builder.Append(str[i]);
				}
				else
				{
					builder.Append(str[i]);
				}
			}
			else if (char.IsDigit(str[i])) // if current char is a number and previous is
			{
				builder.Append(str[i]);
			}
			else if (char.IsLower(str[i - 1])) // if current char is upper and previous char is lower
			{
				builder.Append('-');
				builder.Append(char.ToLower(str[i]));
			}
			else if (i + 1 == str.Length || char.IsUpper(str[i + 1])) // if current char is upper and next char doesn't exist or is upper
			{
				builder.Append(char.ToLower(str[i]));
			}
			else // if current char is upper and next char is lower
			{
				builder.Append('-');
				builder.Append(char.ToLower(str[i]));
			}
		}
		return builder.ToString();
	}
}
