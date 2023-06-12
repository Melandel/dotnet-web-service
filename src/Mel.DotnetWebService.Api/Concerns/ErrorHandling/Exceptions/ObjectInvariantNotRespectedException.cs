using System.Text.Json;

namespace Mel.DotnetWebService.Api.Concerns.ErrorHandling.Exceptions;


class ObjectInvariantNotRespectedException : Exception
{
	ObjectInvariantNotRespectedException(string message, Exception? innerException = null) : base(message, innerException)
	{
	}

	public static ObjectInvariantNotRespectedException WhenConstructingObject(Type objectUnderConstruction, string memberUnderConstruction, object memberValueNotRespectingInvariant, string? invariant, Exception? innerException = null)
	{
		var message = invariant switch
		{
			null => $"{objectUnderConstruction.Name}: {memberUnderConstruction} cannot accept value {JsonSerializer.Serialize(memberUnderConstruction)}",
			_ => $"{objectUnderConstruction.Name}: {memberUnderConstruction} cannot accept value {JsonSerializer.Serialize(memberUnderConstruction)} - {invariant}"
		};
		return new(message, innerException);
	}
}
