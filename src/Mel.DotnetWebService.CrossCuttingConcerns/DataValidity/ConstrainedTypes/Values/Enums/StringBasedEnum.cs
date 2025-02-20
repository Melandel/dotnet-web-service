using System.Reflection;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public abstract class StringBasedEnum<TConcrete> : ConstrainedValue<string>
	where TConcrete : StringBasedEnum<TConcrete>
{
	protected StringBasedEnum(string value) : base(value)
	{
	}

	public static IReadOnlyCollection<TConcrete> AllPossibleValues => _allPossibleValues.Value;
	static Lazy<IReadOnlyCollection<TConcrete>> _allPossibleValues = new(() =>
		typeof(TConcrete)
		.GetMembers(BindingFlags.Static | BindingFlags.Public)
		.OfType<FieldInfo>()
		.Where(f => typeof(TConcrete).IsAssignableFrom(f.FieldType))
		.Select(f => (TConcrete)f.GetValue(null)!)
		.ToArray());

	public static TConcrete? ThatMatches(string? name)
	=> _allPossibleValues.Value.FirstOrDefault(enumValue => name.Matches(enumValue));

	public static TConcrete ThatMatches(string? environmentName, TConcrete defaultValue)
	=> ThatMatches(environmentName) ?? defaultValue;
}
