namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.DynamicTypeGeneration;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
class IgnoresAccessChecksToAttribute : Attribute
{
	public IgnoresAccessChecksToAttribute(string assemblyName)
	{
		AssemblyName = assemblyName;
	}

	public string AssemblyName { get; }
}
