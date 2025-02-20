namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

public class EnumValueGenerator : ExampleValueGenerator
{
	public static readonly EnumValueGenerator Instance = new();

	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		var enumValueCandidates = Enum.GetValues(type)
			.Cast<object>()
			.Where(e => (int)e != 0)
			.OrderBy(e => (int)e)
			.ThenBy(e => e.ToString())
			.ToArray();

			return enumValueCandidates[salt % enumValueCandidates.Length];
	}
}
