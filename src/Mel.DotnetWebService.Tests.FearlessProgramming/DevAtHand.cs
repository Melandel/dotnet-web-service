using System.Collections;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Serialization;
using Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeExecution;
using Mel.DotnetWebService.Tests.FearlessProgramming.TestData.AutoValueProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework.Internal;

namespace Mel.DotnetWebService.Tests.FearlessProgramming;

public class DevAtHand
{

	[Test]
	public void trolololola()
	{
		var v = Some.Valid<NotImplementedException>();
	}

	[Test]
	public void trololololo()
	{
		var v = DependencyTestDouble.StubThatThrowsNotImplementedException;
		Assert.That(() => v.Read(0), Throws.Exception.InstanceOf<NotImplementedException>());
		Assert.That(() => v.Write(Some.Valid<StoredObject>()), Throws.Exception.InstanceOf<NotImplementedException>());

		var w = DependencyTestDouble.StubThatThrowsObjectConstructionException;
		Assert.That(() => w.Read(0), Throws.Exception.InstanceOf<ObjectConstructionException>());
		Assert.That(() => w.Write(Some.Valid<StoredObject>()), Throws.Exception.InstanceOf<ObjectConstructionException>());
	}

	[Test]
	public void ConfigurationTests()
	{
		// Arrange
		var str = Some.Valid<string>();
		var nonEmptyGuid = Some.Valid<NonEmptyGuid>();
		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string>
			{
				["Foo:FirstProperty"] = str,
				["Foo:SecondProperty"] = nonEmptyGuid.ToString()
			})
		.Build();

		var optionsFactory = new OptionsFactory<ClassArchetype.GetSetStyleClassContainingANonEmptyGuid>(
			[],
			[]);

		var options = optionsFactory.Create(Options.DefaultName);
		var configSection = configuration.GetSection("Foo");

		// Act
		configSection.Bind(options);

		// Assert
		var optionsWrapper = Options.Create(options);
		using (Assert.EnterMultipleScope())
		{
			Assert.That(optionsWrapper, Is.Not.Null);
			Assert.That(optionsWrapper.Value, Is.Not.Null);
			Assert.That(optionsWrapper.Value.FirstProperty, Is.EqualTo(str));
			Assert.That(optionsWrapper.Value.SecondProperty, Is.EqualTo(nonEmptyGuid));
		}
	}

	[Test]
	public void t()
	{
		var v = Some.Valid<IOptions<string>>();
		var vv = v.Value;
		var a = Some.Valid<IOptionsSnapshot<ClassArchetype.GetSetStyleClassContainingANonEmptyGuid>>();
		var aa = a.Value;
		var w = 0;
	}
	[Test]
	public void aaaaa()
	{
		var v = Some.Valid<Lazy<int>>();
		var w = v.Value;

	}
	[Test]
	public void bbbbbbb()
	{
		var v = Some.Valid<Func<int>>();
		var a = Some.Valid<Action<int>>();
		var b = Some.Valid<Action>();
		var w = v.Invoke();

	}

	[Test]
	public void arzeoijzeraq()
	{
		var v = DependencyTestDouble.Dummy;
		var w = Substitute.For<IDependency>();
		var x = GenericDependencyTestDouble<int>.Dummy;
		var v1 = v.ReturnStrictlyPositiveInt();
		var w1 = w.ReturnStrictlyPositiveInt();
	}

	public interface IGenericDependency<T>
	{
		int Write(T storedObject);
		T Read(int index);
		void DoNothing(int index);
		Lazy<T> ReturnLazyT();
	}
	public record GenericDependencyTestDouble<T> : TestDouble<IGenericDependency<T>, GenericDependencyTestDouble<T>>, IGenericDependency<T>
	{
		public void DoNothing(int index)
		{
			throw new NotImplementedException();
		}

		public T Read(int index)
		{
			throw new NotImplementedException();
		}

		public Lazy<T> ReturnLazyT()
		{
			throw new NotImplementedException();
		}

		public int Write(T storedObject)
		{
			throw new NotImplementedException();
		}
	}

	public interface IDependency
	{
		int Write(StoredObject storedObject);
		StoredObject Read(int index);
		void  DoNothing(int index);
		StrictlyPositiveInt ReturnStrictlyPositiveInt();
	}
	public class StoredObject
	{
		public int Id { get; set; }
		public string Value { get; set; }
	};
	public record DependencyTestDouble : TestDouble<IDependency, DependencyTestDouble>, IDependency
	{
		StubbedMethod[] _stubbedMethods = null;
		public StubbedMethod StubbedMethod
		{
			get => _stubbedMethods.FirstOrDefault();
			init => _stubbedMethods = [value];
		}
		public StubbedMethod[] StubbedMethods
		{
			get => _stubbedMethods;
			init => _stubbedMethods = value;
		}
		public ICollection FakedImplementationWorkingAround { get; init; } = null;
		public ICollection SpiedActivityReportedInside { get; init; } = null;
		public IDependency MockedImplementation { get; init; } = null;

		public int Write(StoredObject storedObject)
		=> TestDoubleCategory switch
			{
				TestDoubleCategory.Stub => (int)StubbedMethods.First().ReturnValue,
				TestDoubleCategory.Fake => FakeWrite(storedObject),
				_ => throw new NotImplementedException()
			};

		int FakeWrite(StoredObject storedObject)
		{
			((Dictionary<int, StoredObject>)FakedImplementationWorkingAround).TryAdd(0, new());
			return 0;
		}

		public StoredObject Read(int index)
		=> TestDoubleCategory switch
		{
			TestDoubleCategory.Stub => StubbedRead(index),
			TestDoubleCategory.Fake => FakedRead(index),
			_ => throw new NotImplementedException()
		};
		StoredObject StubbedRead(int index)
		=> (StoredObject)StubbedMethods.First().ReturnValue;

		StoredObject FakedRead(int index)
		{
			return ((Dictionary<int, StoredObject>)FakedImplementationWorkingAround).First().Value;
		}

		public void DoNothing(int index)
		{
			throw new NotImplementedException();
		}

		StrictlyPositiveInt IDependency.ReturnStrictlyPositiveInt()
		{
			throw new NotImplementedException();
		}
	}
	class Service
	{
		IDependency _d1;
		IDependency _d2;
		IDependency _d3;
		IDependency _d4;
		IDependency _d5;

		public Service(IDependency d1, IDependency d2, IDependency d3, IDependency d4, IDependency d5)
		{
			_d1 = d1;
			_d2 = d2;
			_d3 = d3;
			_d4 = d4;
			_d5 = d5;
		}

		public string ExecuteService() => "foo";
	}


	[Test]
	public void SomeTest()
	{
		// Arrange
		var dictionary = new Dictionary<int, StoredObject>();
		var methodCalls = new Dictionary<MethodInfo, object>();

		var service = new Service(
			DependencyTestDouble.Dummy,
			DependencyTestDouble.Stub with { StubbedMethods = [Implement.MethodFor(nameof(IDependency.Write)).With(() => "")] },
			DependencyTestDouble.Fake with { FakedImplementationWorkingAround = dictionary },
			DependencyTestDouble.Spy  with { SpiedActivityReportedInside = methodCalls },
			DependencyTestDouble.Mock with { MockedImplementation = new DependencyTestDouble() });

		// Act
		var output = service.ExecuteService();

		// Assert
		Assert.That(output, Is.EqualTo("foo"));
	}

	public class StubbedMethod
	{
		public object ReturnValue;
		public Func<object> Implementation;
	}


	class StubBuildingImplementation<TInterface>
	{

	}
	static class Implement
	{
		public static T MethodFor<T>() where T:new()
		{
			return new T();
		}
		public static StubbedMethodBuilder MethodFor(string methodName)
		{
			return new();
		}
	}
	record StubbedMethodBuilder
	{
		public StubbedMethod With<T>(Func<T> implementation) => new();
		public StubbedMethod With(Action implementation) => new();

		public static implicit operator StubbedMethod(StubbedMethodBuilder builder) => new();
	}
	static class Stub
	{
		public static string Method(string methodName)
			=> methodName;
		public static Func<TResult> With<TResult>(Func<TResult> implementation)
		=> implementation;
		public static Func<T1, TResult> With<T1, TResult>(Func<T1, TResult> implementation)
		=> implementation;
	}
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
