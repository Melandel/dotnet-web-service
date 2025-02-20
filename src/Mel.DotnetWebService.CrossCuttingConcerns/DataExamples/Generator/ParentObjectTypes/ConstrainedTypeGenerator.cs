using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples.Generator;

class ConstrainedTypeGenerator : ParentObjectGenerator
{
	public static readonly ConstrainedTypeGenerator Instance = new();
	ConstrainedTypeGenerator()
	{}
	internal override object GenerateInstanceOf(Type type, int salt = 0)
	{
		if (!ConstrainedTypeInfos.TryGet(type, out var constrainedTypeInfo))
		{
			throw new InvalidOperationException($"{nameof(ConstrainedTypeGenerator)} could not {nameof(GenerateInstanceOf)}({type.GetName()}, {salt});");
		}

		var rootTypeExampleValue = constrainedTypeInfo.ValidValueExamples[salt % constrainedTypeInfo.ValidValueExamples.Count]!;
		return constrainedTypeInfo.InvokeStaticFactoryMethod(rootTypeExampleValue);

		//if (ConstrainedTypeInfos.TryGet(type.GetGenericTypeDefinition(), out var constrainedGenericTypeInfo))
		//{
		//	var rootTypeExampleValue = constrainedGenericTypeInfo.ValidValueExamples[salt % constrainedGenericTypeInfo.ValidValueExamples.Count]!;
		//	return constrainedTypeInfo.InvokeStaticFactoryMethod(rootTypeExampleValue);
		//}
	}
}
