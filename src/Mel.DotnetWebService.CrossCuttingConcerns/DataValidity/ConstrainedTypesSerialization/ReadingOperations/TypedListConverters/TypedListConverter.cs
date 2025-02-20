namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.Serialization.ReadingOperations.TypedListConverters;

abstract class TypedListConverter
{
	public abstract object Convert(dynamic typedList, Type typedListElementType, Type targetType);

	static readonly Dictionary<Type, TypedListConverter> converterByTargetType = new Dictionary<Type, TypedListConverter>();
	public static TypedListConverter InstanceSuitedFor(Type targetType)
	{
		if (!converterByTargetType.ContainsKey(targetType))
		{
			converterByTargetType.Add(
				targetType,
				targetType switch
				{
					{ IsArray: true } => TypedListToArrayConverter.Instance,
					{ IsInterface: true, Namespace: var ns } when ns != null && ns.StartsWith("System") => TypedListToInterfaceInSystemNamespaceConverter.Instance,
					{ IsInterface: true } => NotImplementedTypedListConverter.Instance,
					{ IsGenericType: true } => targetType.GetGenericTypeDefinition() switch
					{
						var t when t == typeof(List<>) => TypedListToListConverter.Instance,
						var t when t == typeof(HashSet<>) => TypedListToHashSetConverter.Instance,
						_ => NotImplementedTypedListConverter.Instance
					},
					_ => TypedListToClassWithMatchingSingleInstanciatorParameterConverter.Instance
				});
		}

		return converterByTargetType[targetType];
	}
}
