using System.Text.Json;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ConstrainedTypes.StringRepresentation.ReadingOperations;

class ClassWithConstructorAndSettersReadingOperation : TypeInvolvingConstrainedTypeReadingOperation
{
	public static readonly ClassWithConstructorAndSettersReadingOperation Instance = new();
	ClassWithConstructorAndSettersReadingOperation()
	{
	}

	public override object? Execute(ref Utf8JsonReader reader, Type targetType, JsonSerializerOptions options, JsonSerializerOptions preComputedOptionsWithoutConstrainedTypeConverter)
	{
		throw new NotImplementedException();
	}
}
