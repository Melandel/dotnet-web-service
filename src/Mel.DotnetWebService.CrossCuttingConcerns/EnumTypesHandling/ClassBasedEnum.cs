using System.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.EnumTypesHandling;

public abstract class ClassBasedEnum<TConcrete>
	where TConcrete : ClassBasedEnum<TConcrete>
{
	readonly string _name;
	protected ClassBasedEnum(string name)
	{
		_name = name;
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

	public static implicit operator string(ClassBasedEnum<TConcrete> enumValue) => enumValue._name;
}
