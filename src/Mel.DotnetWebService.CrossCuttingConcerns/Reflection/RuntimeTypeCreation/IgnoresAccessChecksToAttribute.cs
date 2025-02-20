namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeCreation;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
class IgnoresAccessChecksToAttribute : Attribute
{
	public IgnoresAccessChecksToAttribute(string assemblyName)
	{
		AssemblyName = assemblyName;
	}

	public string AssemblyName { get; }
}
