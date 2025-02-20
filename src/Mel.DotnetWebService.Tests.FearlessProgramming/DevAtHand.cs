using System.Text.Encodings.Web;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using NUnit.Framework.Internal;

namespace Mel.DotnetWebService.Tests.FearlessProgramming;

class DevAtHand
{
	[Test]
	public void a()
	{
		var method = typeof(NonEmptyGuid).GetMethods().First(m => m.Name == nameof(IConstrainedValue<Guid, NonEmptyGuid>.ApplyConstraintsTo));
		var lambdaExpression = LambdaExpressionBuilder.CreateWithParameterTypes([typeof(Guid)])
			.AddCallTo(method)
			.BuildAsDynamic();
		var invokable = lambdaExpression.Compile();
		object v = invokable.Invoke(Guid.Parse("753f661f-cdb0-4667-b086-002266b0b93e"));
	}
	[Test]
	public void b()
	{
		var method = typeof(NonEmptyGuid).GetUserDefinedConversions(browseParentTypes: true).First(m => m.ReturnParameter.ParameterType == typeof(Guid));
		var lambdaExpression = LambdaExpressionBuilder.CreateWithParameterTypes([typeof(NonEmptyGuid)])
			.AddCallTo(method)
			.BuildAsDynamic();
		dynamic invokable = lambdaExpression.Compile();
		object v = invokable.Invoke(NonEmptyGuid.ApplyConstraintsTo(Guid.Parse("95e43576-cf1c-4bbd-8773-26703c3968e2")));
	}
	[Test]
	public void c()
	{
		var method = typeof(NonEmptyGuid).GetMethods().First(m => m.Name == nameof(IConstrainedValue<Guid, NonEmptyGuid>.ApplyConstraintsTo));
		var method2 = typeof(NonEmptyGuid).GetUserDefinedConversions(browseParentTypes: true).First(m => m.ReturnParameter.ParameterType == typeof(Guid));
		var lambdaExpression = LambdaExpressionBuilder.CreateWithParameterTypes([typeof(Guid)])
			.AddCallTo(method)
			.AddCallTo(method2)
			.Build();
		dynamic invokable = lambdaExpression.Compile();
		object v = invokable.Invoke(Guid.Parse("62226c9c-cf3f-4142-9c59-267f5cf6ea18"));
	}
	[Test]
	public void d()
	{
		var method = typeof(NonEmptyGuid).GetMethods().First(m => m.Name == nameof(IConstrainedValue<Guid, NonEmptyGuid>.ApplyConstraintsTo));
		var method2 = typeof(NonEmptyGuid).GetUserDefinedConversions(browseParentTypes: true).First(m => m.ReturnParameter.ParameterType == typeof(Guid));
		var lambdaExpression = LambdaExpressionBuilder.CreateWithParameterTypes([typeof(Guid)])
			.AddCallTo(NonEmptyArray.ApplyConstraintsTo([method, method2]))
			.Build();
		dynamic invokable = lambdaExpression.Compile();
		object v = invokable.Invoke(Guid.Parse("62226c9c-cf3f-4142-9c59-267f5cf6ea18"));
	}

	[Test]
	public void Baz()
	{
		var toSerialize = new Dictionary<List<NonEmptyGuid>, List<NonEmptyGuid>>
		{
			{ new List<NonEmptyGuid>{ NonEmptyGuid.ApplyConstraintsTo(Guid.Parse("00000000-0ecd-4883-ad6b-000000000000")) }, new List<NonEmptyGuid>{ NonEmptyGuid.ApplyConstraintsTo(Guid.Parse("11111111-0ecd-4883-ad6b-111111111111")) } },
			{ new List<NonEmptyGuid>{ NonEmptyGuid.ApplyConstraintsTo(Guid.Parse("22222222-0ecd-4883-ad6b-222222222222")) }, new List<NonEmptyGuid>{ NonEmptyGuid.ApplyConstraintsTo(Guid.Parse("33333333-0ecd-4883-ad6b-333333333333")) } },
		};

		var opts = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
		opts.Converters.Add(new KeyValuePairsWithComplexKeyTypeJsonConverter());
		opts.Converters.Add(new ConstrainedTypeJsonConverter());
		var serialized = JsonSerializer.Serialize(toSerialize, opts);
		Assert.Pass();
	}
	[Test]
	public void Bar()
	{
		var toSerialize = new Dictionary<List<Guid>, List<Guid>>
		{
			{ new List<Guid>{ Guid.Parse("00000000-0ecd-4883-ad6b-000000000000") }, new List<Guid>{ Guid.Parse("11111111-0ecd-4883-ad6b-111111111111") } },
			{ new List<Guid>{ Guid.Parse("22222222-0ecd-4883-ad6b-222222222222") }, new List<Guid>{ Guid.Parse("33333333-0ecd-4883-ad6b-333333333333") } },
		};

		var opts = new JsonSerializerOptions() { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
		opts.Converters.Add(new KeyValuePairsWithComplexKeyTypeJsonConverter());
		opts.Converters.Add(new ConstrainedTypeJsonConverter());
		var serialized = JsonSerializer.Serialize(toSerialize, opts);
		Assert.Pass();
	}

	// [Test]
	// public void Foo()
	// {
	// 	// I have a a NonEmptyGuids
	// 	// I want to dynamically turn it into a Guid[] using NonEmptyGuid's conversion

	// 	// Arrange
	// 	var nonEmptyGuids = ClassArchetype.NonEmptyGuids.FromStrings(
	// 		"252bacac-de85-4ac1-862e-370f1843c607",
	// 		"08902edb-4635-4b40-be79-b98743b78096");

	// 	// Act
	// 	var converted = ConvertToNativeTypeOnly(nonEmptyGuids);

	// 	// Assert
	// 	Assert.That(nonEmptyGuids.GetStringRepresentation(), Is.EqualTo("[\"252bacac-de85-4ac1-862e-370f1843c607\",\"08902edb-4635-4b40-be79-b98743b78096\"]"));
	// 	Assert.That(converted.GetStringRepresentation(), Is.EqualTo(nonEmptyGuids.GetStringRepresentation()));
	// }

	// public object ConvertToNativeTypeOnly(dynamic collectionInvolvingConstrainedType)
	// {
	// 	Type type = collectionInvolvingConstrainedType.GetType();
	// 	if (type != typeof(ClassArchetype.NonEmptyGuids))
	// 	{
	// 		throw new NotImplementedException();

	// 	}
	// 	ConstrainedTypeInfo constrainedTypeInfo = null!;
	// 	if (!ConstrainedTypeInfos.TryGet(type, out constrainedTypeInfo) || constrainedTypeInfo.Category != ConstrainedTypeCategory.NonGenericCollection)
	// 	{
	// 		throw new InvalidOperationException();
	// 	}

	// 	var rootType = constrainedTypeInfo.RootType;
	// 	var rootItemType = rootType.GetCollectionItemType();
	// 	ConstrainedTypeInfo constrainedItemTypeInfo = null!;
	// 	if (!ConstrainedTypeInfos.TryGet(rootItemType, out constrainedItemTypeInfo))
	// 	{
	// 		throw new InvalidOperationException();
	// 	}

	// 	return ConvertToNativeTypeOnlyInternal(collectionInvolvingConstrainedType, constrainedItemTypeInfo);
	// }

	// private static object ConvertToNativeTypeOnlyInternal(dynamic collectionInvolvingConstrainedType, ConstrainedTypeInfo constrainedItemTypeInfo)
	// {
	// 	// The problem:
	// 	// ClassArchetype.EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter
	// 	//   ConstrainedCollection<HashSet<NonEmptyGuid>>,
	// 	//   IConstrainedCollection<Guid, EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter>
	// 	// ApplyConstraintsTo(IEnumerable<Guid>)
	// 	// Examples : [ NonEmptyGuid[] ], should be [ Guid[] ] because the point is the test should be created from FactoryMethod input type

	// 	// For the Deserialization usecase
	// 	//   We have a List<Guid> and want to call ConstrainedTypeInfo.CreateFrom(IEnumerable<Guid>) We want a MethodInfo that converts

	// 	var rootItemRootType = constrainedItemTypeInfo.RootType;
	// 	var typedListType = typeof(List<>).MakeGenericType(rootItemRootType);
	// 	dynamic list = Activator.CreateInstance(typedListType);
	// 	foreach (var item in collectionInvolvingConstrainedType)
	// 	{
	// 		typedListType.InvokeMember(
	// 			nameof(System.Collections.IList.Add),
	// 			BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
	// 			null,
	// 			list,
	// 			new object[] { constrainedItemTypeInfo.InvokeImplicitConversionToRootType(item) });
	// 	}
	// 	return list;
	// }
}
