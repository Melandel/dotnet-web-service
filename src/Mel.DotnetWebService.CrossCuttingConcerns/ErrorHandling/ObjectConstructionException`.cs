namespace Mel.DotnetWebService.CrossCuttingConcerns.ErrorHandling;

public class ObjectConstructionException<TObjectUnderConstruction> : Exception
{
	internal ObjectConstructionException(string message, Exception? innerException = null) : base(message, innerException)
	{
	}
}
