namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Exceptions;

class ObjectConstructionException<TObjectUnderConstruction> : Exception
{
	public ObjectConstructionException(string message, Exception? innerException = null) : base(message, innerException)
	{
	}
}
