using System.ComponentModel;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

public class ConstrainedTypeDescriptor : CustomTypeDescriptor
{
	readonly Type _concreteType;

	public ConstrainedTypeDescriptor(ICustomTypeDescriptor parent, Type concreteType) : base(parent)
	{
		_concreteType = concreteType;
	}

	public override TypeConverter GetConverter()
	=> new ConstrainedTypeConverter(_concreteType);
}
