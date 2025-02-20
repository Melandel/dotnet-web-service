using System.Text.Encodings.Web;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders;
using NUnit.Framework.Internal;

namespace Mel.DotnetWebService.Tests.FearlessProgramming;

class DevAtHand
{
	[Test]
	public void zzz()
	{
		var a =  Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<Dictionary<ClassArchetype.NonEmptyGuids, List<NonEmptyGuid>>>>();
		var b =  Regenerate.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<Dictionary<ClassArchetype.NonEmptyGuids, List<NonEmptyGuid>>>>(a.GenerativeSalts);

		var aStr = a.Value.GetStringRepresentation();
		var bStr = b.GetStringRepresentation();

		if (aStr != bStr)
		{
			Console.WriteLine(string.Join(Environment.NewLine, [$"Expected {aStr}", $"Got      {bStr}"]));
		}
		Assert.That(bStr, Is.EqualTo(aStr), a.FailingTestMessage);
	}
	[Test]
	public void zz()
	{
		var a = Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<NonEmptyGuid>>();
		var b = Regenerate.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<NonEmptyGuid>>(a.GenerativeSalts);

		var aStr = a.Value.GetStringRepresentation();
		var bStr = b.GetStringRepresentation();
		if (aStr != bStr)
		{
			Console.WriteLine(string.Join(Environment.NewLine, [ $"Expected {aStr}", $"Got      {bStr}"]));
		}
		Assert.That(aStr, Is.EqualTo(bStr), a.FailingTestMessage);
	}
	[Test]
	public void z()
	{
		var a = Randomized.Valid<int>(); Assert.That(a.GenerativeSalts, Is.Not.Null.And.Not.Empty);
		var b = Randomized.Valid<List<int>>(); Assert.That(b.GenerativeSalts, Is.Not.Null.And.Not.Empty);
		var c = Randomized.Valid<NonZeroInt>(); Assert.That(c.GenerativeSalts, Is.Not.Null.And.Not.Empty);
		var d = Randomized.Valid<List<NonZeroInt>>(); Assert.That(d.GenerativeSalts, Is.Not.Null.And.Not.Empty);
		var e = Randomized.Valid<NonEmptyList<NonZeroInt>>(); Assert.That(e.GenerativeSalts, Is.Not.Null.And.Not.Empty);

		var f = Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<int>>(); Assert.That(c.GenerativeSalts, Is.Not.Null.And.Not.Empty);
		var g = Randomized.Valid<ClassArchetype.PositionalRecordTypeContainingGenericValue<NonZeroInt>>(); Assert.That(c.GenerativeSalts, Is.Not.Null.And.Not.Empty);
	}

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
}
