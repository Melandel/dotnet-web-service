namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection;

public static class SourceCodeScalability
{
	public static Lazy<IReadOnlyCollection<T>> CreateArrayContainingAllPublicStaticFieldsValuesIn<T>()
	=> new Lazy<IReadOnlyCollection<T>>(SourceCode.GetAllPublicStaticFieldsValuesOfType<T>);
}
