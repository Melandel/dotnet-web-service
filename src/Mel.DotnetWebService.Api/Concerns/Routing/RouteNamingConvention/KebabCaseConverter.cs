using System.Text;

namespace Mel.DotnetWebService.Api.Concerns.Routing.RouteNamingConvention;

class KebabCaseConverter
{
	public string? Convert(string? str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return str;
		}

		var builder = new StringBuilder();
		for (var i = 0; i < str.Length; i++)
		{
			builder.Append(
				str[i] switch
				{
					var c when char.IsLower(c) => c,
					var c when i == 0 => char.ToLower(c),
					var c => str[i-1] switch
					{
						var p when char.IsDigit(c) && !char.IsDigit(p) && char.IsLower(p) => $"-{c}",
						var p when char.IsDigit(c) && !char.IsDigit(p) => c,
						var p when char.IsDigit(c) && char.IsDigit(p) => c,
						var p when char.IsLower(p) => $"-{char.ToLower(c)}",
						var p when i+1 == str.Length => char.ToLower(c),
						var p => str[i+1] switch
						{
							var n when char.IsUpper(n) => char.ToLower(c),
						 _ => $"-{char.ToLower(c)}"
						}
					},
				});
		}
		return builder.ToString();
	}
}
