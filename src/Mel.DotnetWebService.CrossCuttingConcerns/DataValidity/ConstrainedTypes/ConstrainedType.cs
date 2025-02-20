using System.ComponentModel;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

[TypeDescriptionProvider(typeof(ConstrainedTypeDescriptionProvider))]
public abstract class ConstrainedType
{
}
