using Mel.DotnetWebService.CrossCuttingConcerns.Reflection;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Configurability;

public class DeploymentEnvironment
{
	public const string ConfigurationSettingKey = "environment";

	readonly string _name;
	DeploymentEnvironment(string name)
	{
		_name = name;
	}
	public static readonly DeploymentEnvironment Development = new(nameof(Development));
	public static readonly DeploymentEnvironment Staging = new(nameof(Staging));
	public static readonly DeploymentEnvironment Production = new(nameof(Production));

	public static IReadOnlyCollection<DeploymentEnvironment> AllPossibleValues => _allPossibleValues.Value;
	static Lazy<IReadOnlyCollection<DeploymentEnvironment>> _allPossibleValues = SourceCodeScalability.CreateArrayContainingAllPublicStaticFieldsValuesIn<DeploymentEnvironment>();

	public static DeploymentEnvironment? ThatMatches(string? environmentName)
	=> _allPossibleValues.Value.FirstOrDefault(env => environmentName.Matches(env));

	public static DeploymentEnvironment ThatMatches(string? environmentName, DeploymentEnvironment defaultValue)
	=> ThatMatches(environmentName) ?? defaultValue;

	public string? ApplySuffixToConfigurationFilePath(string? filePath)
	=> filePath switch
	{
		null => null,
		var file when this == Production => file,
		var file => $"{Path.GetFileNameWithoutExtension(file)}.{_name}{Path.GetExtension(file)}"
	};

	public static implicit operator string(DeploymentEnvironment status) => status._name;
}
