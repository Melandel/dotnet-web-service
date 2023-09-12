using Mel.DotnetWebService.Api.Controllers;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Collections;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Guids;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes.ClassArchetype;

namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestEnvironments.TestDoubles;

static class ControllerTestDoubles
{

	[ApiVersion("3")]
	[ApiVersion("2", Deprecated = true)]
	[ApiVersion("1", Deprecated = true)]
	public class StubbedEndpointsSpecificallyCreatedForTests : ApiController
	{
		readonly ILogger<StubbedEndpointsSpecificallyCreatedForTests> _logger;
		public StubbedEndpointsSpecificallyCreatedForTests(ILogger<StubbedEndpointsSpecificallyCreatedForTests> logger)
		{
			_logger = logger;
			}

		public enum CityName { TechnicalDefaultEnumValue = 0, Toulouse = 1, Bordeaux = 2, Paris = 3 };
		public record RequestWithCityNameProperty(CityName CityName);
		public record ResponseWithCityNameProperty(CityName CityName, string helloCity);
		string HelloCity(CityName cityName) => DefinedOnlyInApiV1().Replace("world", cityName.ToString());

		[MapToApiVersion("1")]
		[HttpGet, Route("{cityName}")]
		public ResponseWithCityNameProperty WithEnumInOutputFieldAndInRoute(CityName cityName)
		=> cityName switch
		{
			CityName.TechnicalDefaultEnumValue => throw TestDataIntegrityException.GeneratedBy(GetType(), nameof(WithEnumInOutputFieldAndInRoute), $"Route parameter {nameof(CityName)} is {CityName.TechnicalDefaultEnumValue}"),
			_ => new(cityName, HelloCity(cityName))
		};

		[MapToApiVersion("1")]
		[HttpGet]
		public ResponseWithCityNameProperty WithEnumInOutputFieldAndInQueryString([FromQuery]CityName cityName)
		=> cityName switch
		{
			CityName.TechnicalDefaultEnumValue => throw TestDataIntegrityException.GeneratedBy(GetType(), nameof(WithEnumInOutputFieldAndInQueryString), $"Query string {nameof(CityName)} is {CityName.TechnicalDefaultEnumValue}"),
			_ => new(cityName, HelloCity(cityName))
		};

		[MapToApiVersion("1")]
		[HttpPost]
		public ResponseWithCityNameProperty WithEnumInOutputFieldAndInRequestBody(RequestWithCityNameProperty payload)
		=> payload switch
		{
			{ CityName: CityName.TechnicalDefaultEnumValue } => throw TestDataIntegrityException.GeneratedBy(GetType(), nameof(WithEnumInOutputFieldAndInRequestBody), $"Payload contains {nameof(CityName)} {CityName.TechnicalDefaultEnumValue}"),
			_ => new(payload.CityName, HelloCity(payload.CityName))
		};

		[MapToApiVersion("3")]
		[HttpGet]
		public JsonResult GetHttpProblemTypeFromAnotherController() => new JsonResult(HttpProblemTypeProvider.GetDeveloperMistake());

		[HttpGet]
		public IActionResult CallSomeService([FromServices] ISomeService someService)
		{
			var input = new ISomeService.Input();
			var output = someService.Process(input);
			return Ok(output);
		}

		[HttpGet]
		public int DivisionByZero()
		{
			int zero = 0;
			return 1/zero;
		}

		[HttpGet]
		public string DefinedInAllApiVersions() => "Hello, word!";

		[MapToApiVersion("2")]
		[HttpGet]
		public string DefinedOnlyInApiV2() => "Hello world!";

		[MapToApiVersion("2")]
		[MapToApiVersion("1")]
		[HttpGet]
		public string DefinedOnlyInApiV1AndApiV2() => "Hello world";

		[MapToApiVersion("1")]
		[HttpGet]
		public string DefinedOnlyInApiV1() => "hello world";

		public record RequestContainingGuidProperty(Guid Guid);

		[HttpPost]
		public Guid GuidPassThrough(RequestContainingGuidProperty request) => request.Guid;

		[HttpGet]
		public Guid GuidPassThrough(Guid guid) => guid;

		[HttpGet]
		public Guid NonEmptyGuidPassThrough(NonEmptyGuid nonEmptyGuid) => nonEmptyGuid;

		[HttpPost]
		public Guid PostGuidInsideBody([FromBody] Guid guid) => guid;
		[HttpPost]
		public Guid[] PostGuidsInsideBody([FromBody] Guid[] guids) => guids;
		[HttpPost]
		public NonEmptyGuid PostNonEmptyGuidInsideBody([FromBody] NonEmptyGuid nonEmptyGuid) => nonEmptyGuid;
		[HttpPost]
		public NonEmptyGuid PostNonEmptyGuidInsideQuery([FromQuery] NonEmptyGuid nonEmptyGuid) => nonEmptyGuid;
		[HttpPost]
		public NonEmptyGuid PostNonEmptyGuidInsideRoute([FromRoute] NonEmptyGuid nonEmptyGuid) => nonEmptyGuid;
		[HttpPost]
		public NonEmptyGuid[] PostArrayOfNonEmptyGuidsInsideBody([FromBody] NonEmptyGuid[] arrayOfNonEmptyGuids) => arrayOfNonEmptyGuids;
		[HttpPost]
		public List<NonEmptyGuid> PostListOfNonEmptyGuidsInsideBody([FromBody] List<NonEmptyGuid> listOfNonEmptyGuids) => listOfNonEmptyGuids;
		[HttpPost]
		public HashSet<NonEmptyGuid> PostHashsetOfNonEmptyGuidsInsideBody([FromBody] HashSet<NonEmptyGuid> hashSetOfNonEmptyGuids) => hashSetOfNonEmptyGuids;
		[HttpPost]
		public EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter PostEncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIenumerableParameterInsideBody([FromBody] EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter encapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIenumerableParameter) => encapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnIenumerableParameter;
		[HttpPost]
		public EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter PostEncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameterInsideBody([FromBody] EncapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter encapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter) => encapsulationOfAnArrayOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter;
		[HttpPost]
		public EncapsulationOfAnIEnumerableOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter PostEncapsulationOfAnIenumerableOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameterInsideBody([FromBody] EncapsulationOfAnIEnumerableOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter encapsulationOfAnIEnumerableOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter) => encapsulationOfAnIEnumerableOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter;
		[HttpPost]
		public EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter PostEncapsulationOfAHashsetOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameterInsideBody([FromBody] EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter encapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter) => encapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnArrayParameter;
		[HttpPost]
		public EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter PostEncapsulationOfAHashsetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIenumerableParameterInsideBody([FromBody] EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter encapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter) => encapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableParameter;
		[HttpPost]
		public EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter PostEncapsulationOfAHashsetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIenumerableOfGuidsParameterInsideBody([FromBody] EncapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter encapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter) => encapsulationOfAHashSetOfNonEmptyGuidsExposingAFactoryMethodTakingAnIEnumerableOfGuidsParameter;
		[HttpPost]
		public EncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnArrayParameter PostEncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnArrayParameterInsideBody([FromBody] EncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnArrayParameter encapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnArrayParameter) => encapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnArrayParameter;
		[HttpPost]
		public EncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnIEnumerableParameter PostEncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnIenumerableParameterInsideBody([FromBody] EncapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnIEnumerableParameter encapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnIEnumerableParameter) => encapsulationOfAnArrayOfNonEmptyGuidsExposingAConstructorTakingAnIEnumerableParameter;
		[HttpPost]
		public IEnumerable<NonEmptyGuid> PostIenumerableOfNonEmptyGuidsInsideBody([FromBody] IEnumerable<NonEmptyGuid> ienumerableOfNonEmptyGuids) => ienumerableOfNonEmptyGuids;
		[HttpPost]
		public Dictionary<string, NonEmptyGuid> PostDictionaryWithStringKeysAndNonEmptyGuidValuesInsideBody([FromBody] Dictionary<string, NonEmptyGuid> dictionaryWithStringKeysAndNonEmptyGuidValues) => dictionaryWithStringKeysAndNonEmptyGuidValues;
		[HttpPost]
		public NonEmptyGuid[][] PostArrayOfArraysOfNonEmptyGuidsInsideBody([FromBody] NonEmptyGuid[][] arrayOfArraysOfNonEmptyGuids) => arrayOfArraysOfNonEmptyGuids;
		[HttpGet]
		public NonEmptyGuid GetNonEmptyGuidInsideQuery([FromQuery] NonEmptyGuid nonEmptyGuid) => nonEmptyGuid;

		[HttpGet("{nonEmptyGuid}")]
		public NonEmptyGuid GetNonEmptyGuidInsideRoute([FromRoute] NonEmptyGuid nonEmptyGuid) => nonEmptyGuid;

		[HttpPost]
		public PositionalRecordContainingANonEmptyGuid PostPositionalRecordContainingNonEmptyGuidInsideBody([FromBody] PositionalRecordContainingANonEmptyGuid recordContainingNonEmptyGuid) => recordContainingNonEmptyGuid;
		[HttpPost]
		public GetSetStyleClassContainingANonEmptyGuid PostGetSetStyleClassContainingNonEmptyGuidInsideBody([FromBody] GetSetStyleClassContainingANonEmptyGuid getSetStyleClassContainingNonEmptyGuid) => getSetStyleClassContainingNonEmptyGuid;
		[HttpPost]
		public PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid PostPositionalRecordContainingPositionalRecordContainingNonEmptyGuidInsideBody([FromBody] PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid recordContainingRecordContainingNonEmptyGuid) => recordContainingRecordContainingNonEmptyGuid;
		[HttpPost]
		public PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid[] PostArrayOfPositionalRecordsContainingPositionalRecordContainingNonEmptyGuidInsideBody([FromBody] PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid[] arrayOfRecordsContainingRecordContainingNonEmptyGuid) => arrayOfRecordsContainingRecordContainingNonEmptyGuid;
		[HttpPost]
		public List<PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid> PostListOfPositionalRecordsContainingPositionalRecordContainingNonEmptyGuidInsideBody([FromBody] List<PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid> listOfRecordsContainingRecordContainingNonEmptyGuid) => listOfRecordsContainingRecordContainingNonEmptyGuid;
		[HttpPost]
		public IEnumerable<PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid> PostIenumerableOfPositionalRecordsContainingPositionalRecordContainingNonEmptyGuidInsideBody([FromBody] IEnumerable<PositionalRecordContainingAPositionalRecordContainingANonEmptyGuid> ienumerableOfRecordsContainingRecordContainingNonEmptyGuid) => ienumerableOfRecordsContainingRecordContainingNonEmptyGuid;
		[HttpPost]
		public PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuids PostPositionalRecordContainingPositionalRecordContainingAnArrayOfNonEmptyGuidsInsideBody([FromBody] PositionalRecordContainingAPositionalRecordContainingAnArrayOfNonEmptyGuids recordContainingRecordContainingAnArrayOfNonEmptyGuids) => recordContainingRecordContainingAnArrayOfNonEmptyGuids;
		[HttpPost]
		public PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuids PostPositionalRecordContainingPositionalRecordContainingAListOfNonEmptyGuidsInsideBody([FromBody] PositionalRecordContainingAPositionalRecordContainingAListOfNonEmptyGuids recordContainingRecordContainingAListOfNonEmptyGuids) => recordContainingRecordContainingAListOfNonEmptyGuids;
		[HttpPost]
		public PositionalRecordContainingAnArrayOfNonEmptyGuids PostPositionalRecordContainingAnArrayOfNonEmptyGuidsInsideBody([FromBody] PositionalRecordContainingAnArrayOfNonEmptyGuids recordContainingAnArrayOfNonEmptyGuids) => recordContainingAnArrayOfNonEmptyGuids;
		[HttpPost]
		public PositionalRecordContainingAnArrayOfNonEmptyGuidsAndAnArrayOfInts PostPositionalRecordContainingAnArrayOfNonEmptyGuidsAndAnArrayOfIntsInsideBody([FromBody] PositionalRecordContainingAnArrayOfNonEmptyGuidsAndAnArrayOfInts recordContainingAnArrayOfNonEmptyGuidsAndAnArrayOfInts) => recordContainingAnArrayOfNonEmptyGuidsAndAnArrayOfInts;
		[HttpPost]
		public PositionalRecordContainingAListOfNonEmptyGuids PostPositionalRecordContainingAListOfNonEmptyGuidsInsideBody([FromBody] PositionalRecordContainingAListOfNonEmptyGuids recordContainingAListOfNonEmptyGuids) => recordContainingAListOfNonEmptyGuids;
		[HttpPost]
		public PositionalRecordContainingAnIEnumerableOfNonEmptyGuids PostPositionalRecordContainingAnIenumerableOfNonEmptyGuidsInsideBody([FromBody] PositionalRecordContainingAnIEnumerableOfNonEmptyGuids recordContainingAnIEnumerableOfNonEmptyGuids) => recordContainingAnIEnumerableOfNonEmptyGuids;
		[HttpPost]
		public IEnumerable<PositionalRecordContainingANonEmptyGuid> PostIenumerableOfPositionalRecordsContainingANonEmptyGuidInsideBody([FromBody] IEnumerable<PositionalRecordContainingANonEmptyGuid> ienumerableOfRecordsNonEmptyGuid) => ienumerableOfRecordsNonEmptyGuid;

		[HttpPost]
		public PositionalRecordContainingANonEmptyGuid[] PostArrayOfPositionalRecordsContainingNonEmptyGuidInsideBody([FromBody] PositionalRecordContainingANonEmptyGuid[] recordsContainingNonEmptyGuid) => recordsContainingNonEmptyGuid;
		[HttpPost]
		public PositionalRecordContainingANonEmptyGuid[] PostArrayOfPositionalRecordsContainingANonEmptyGuidInsideBody([FromBody] PositionalRecordContainingANonEmptyGuid[] recordsContainingNonEmptyGuid) => recordsContainingNonEmptyGuid;
		[HttpPost]
		public List<PositionalRecordContainingANonEmptyGuid> PostListOfPositionalRecordsContainingANonEmptyGuidInsideBody([FromBody] List<PositionalRecordContainingANonEmptyGuid> recordsContainingNonEmptyGuid) => recordsContainingNonEmptyGuid;

		[HttpPost]
		public PositionalRecordContainingAGuid[] PostPositionalRecordsContainingGuidInsideBody([FromBody] PositionalRecordContainingAGuid[] recordsContainingGuid) => recordsContainingGuid;

		[HttpPost]
		public NonEmptyHashSet<Guid> PostNonEmptyHashsetOfGuidsInsideBody([FromBody] NonEmptyHashSet<Guid> nonEmptyHashSetOfGuids) => nonEmptyHashSetOfGuids;
	}
}
