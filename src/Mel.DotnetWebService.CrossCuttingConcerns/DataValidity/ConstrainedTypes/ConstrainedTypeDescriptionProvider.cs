using System.ComponentModel;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public class ConstrainedTypeDescriptionProvider : TypeDescriptionProvider
{
	public ConstrainedTypeDescriptionProvider() : base(TypeDescriptor.GetProvider(typeof(ConstrainedType)))
	{
	}

	public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object? instance)
	{
		var descriptor = base.GetTypeDescriptor(objectType, instance)!;

		return new ConstrainedTypeDescriptor(descriptor, instance?.GetType() ?? objectType);
	}
}
