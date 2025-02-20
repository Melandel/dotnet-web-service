using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeCompilation.Instanciation;

public abstract class ParentObjectInstanciationExpressionBuilder : InstanciationExpressionBuilder
{
	static readonly Dictionary<Type, ParentObjectInstanciationExpressionBuilder> ParentObjectInstanciationExpressionBuildersByType = [];
	public static ParentObjectInstanciationExpressionBuilder InstanceSuitedFor(Type type)
	{
		if (!ParentObjectInstanciationExpressionBuildersByType.TryGetValue(type, out var InstanciationExpressionBuilder))
		{
			InstanciationExpressionBuilder = type switch
				{
					var t when ConstrainedTypeInfos.Include(t)                                                                                    => ConstrainedTypeInstanciationExpressionBuilder.Instance,
					var t when t.IsGenericType && ConstrainedTypeInfos.Include(t.GetGenericTypeDefinition())                                      => ConstrainedTypeInstanciationExpressionBuilder.Instance,
					var t when t.IsAFirstClassCollection(out var constructorParameterType, out var instanciationOperationParameterTypeCandidates) => FirstClassCollectionParentObjectInstanciationExpressionBuilder.Instance(type, constructorParameterType, instanciationOperationParameterTypeCandidates),
					var t when t.IsDeclaredAsAPositionalRecord(out var propertyBasedConstructor)                                                  => PositionalRecordParentObjectInstanciationExpressionBuilder.Instance(type, propertyBasedConstructor!),
					var t when t.IsClassWithPublicConstructorHavingParameters(out var constructorsWithParameters)                                 => ClassWithPublicConstructorHavingParametersInstanciationExpressionBuilder.Instance(type, constructorsWithParameters),
					var t when t.IsClassWithPublicParameterlessConstructorAndSetters(out var settableProperties)                                  => ClassWithPublicParameterlessConstructorAndSettersInstanciationExpressionBuilder.Instance(type, settableProperties),
					var t when t.IsClassWithPublicStaticFactoryMethod(out var staticFactoryMethods)                                               => ClassWithPublicStaticFactoryMethodInstanciationExpressionBuilder.Instance(type, staticFactoryMethods),
					var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughProperty(out var singletonAccessProperty)                 => ClassWithSinglePublicAccessToSingletonInstanceThroughPropertyInstanciationExpressionBuilder.Instance(type, singletonAccessProperty!),
					var t when t.IsClassWithSinglePublicAccessToSingletonInstanceThroughField(out var singletonAccessProperty)                    => ClassWithSinglePublicAccessToSingletonInstanceThroughFieldInstanciationExpressionBuilder.Instance(type, singletonAccessProperty!),
					var t when t.IsClassWithPublicInstancesExposedAsStaticReadonlyFields(out var staticReadonlyInstanceAccessFields)              => ClassWithPublicInstancesExposedAsFieldsInstanciationExpressionBuilder.Instance(type, staticReadonlyInstanceAccessFields),
					var t when t.IsClassWithPublicInstancesExposedAsProperties(out var staticInstanceAccessProperties)                            => ClassWithPublicInstancesExposedAsPropertiesInstanciationExpressionBuilder.Instance(type, staticInstanceAccessProperties),
					var t when t.IsClassWithPublicInstancesExposedAsStaticFields(out var staticInstanceAccessFields)                              => ClassWithPublicInstancesExposedAsFieldsInstanciationExpressionBuilder.Instance(type, staticInstanceAccessFields),
					_ => throw new InvalidOperationException($"{nameof(ParentObjectInstanciationExpressionBuilder)}: Cannot resolve an {nameof(InstanciationExpressionBuilder)} for type {type.GetName()}.")
				};
			ParentObjectInstanciationExpressionBuildersByType.Add(type, InstanciationExpressionBuilder);
		}
		return InstanciationExpressionBuilder;
	}
}
