using System.Runtime.CompilerServices;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ErrorHandling;

public class ObjectConstructionException : Exception
{
	readonly ObjectConstructionFailureLayers _objectConstructionLayers;
	public Type TypeThatCouldNotBeConstructed => _objectConstructionLayers.First().TypeOfTheObjectUnderConstruction;

	public override string Message
	{
		get
		{
			var sb = new System.Text.StringBuilder();
			foreach (var layer in _objectConstructionLayers)
			{
				sb.AppendLine(layer.Name);
				foreach (var details in layer.Details)
				{
					sb.AppendLine(details);
				}
			}
			return sb.ToString().TrimEnd();
		}
	}

	ObjectConstructionException(Type typeOfTheObjectUnderConstruction, string message, Exception? innerException = null) : base(message, innerException)
	{
		_objectConstructionLayers = ObjectConstructionFailureLayers.InitializeWith(typeOfTheObjectUnderConstruction, message);
	}

	public static ObjectConstructionException WhenConstructingAMemberFor<TObjectUnderConstruction>(string memberUnderConstruction, object? invalidValue, string? ruleThatInvalidatesTheValue = null, Exception? innerException = null) => CreateFromSourceFailure<TObjectUnderConstruction>(memberUnderConstruction, invalidValue)	;
	static ObjectConstructionException CreateFromSourceFailure<TObjectUnderConstruction>(
		string memberUnderConstruction,
		object? invalidValue,
		string? ruleThatInvalidatesTheValue = null,
		Exception? innerException = null)
	{
		var message = BuildMessageAboutSpecificMemberConstructionFailure(
			typeof(TObjectUnderConstruction).Name,
			typeof(TObjectUnderConstruction).Namespace!,
			memberUnderConstruction,
			invalidValue,
			ruleThatInvalidatesTheValue);

		return new(typeof(TObjectUnderConstruction), message, innerException);
	}

	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(string ruleThatInvalidatesTheInstanceConstruction, [CallerMemberName] string callerMethodName = "")                                                                                                                                                         => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: Array.Empty<object?>(), callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(string ruleThatInvalidatesTheInstanceConstruction, object? constructionMethodParameter, [CallerMemberName] string callerMethodName = "")                                                                                                                    => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: new[] { constructionMethodParameter }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(string ruleThatInvalidatesTheInstanceConstruction, object? constructionMethodParameter1, object? constructionMethodParameter2, [CallerMemberName] string callerMethodName = "")                                                                             => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2 }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(string ruleThatInvalidatesTheInstanceConstruction, object? constructionMethodParameter1, object? constructionMethodParameter2, object? constructionMethodParameter3, [CallerMemberName] string callerMethodName = "")                                       => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2, constructionMethodParameter3 }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(string ruleThatInvalidatesTheInstanceConstruction, object? constructionMethodParameter1, object? constructionMethodParameter2, object? constructionMethodParameter3, object? constructionMethodParameter4, [CallerMemberName] string callerMethodName = "") => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2, constructionMethodParameter3, constructionMethodParameter4 }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(string ruleThatInvalidatesTheInstanceConstruction, IEnumerable<object?> constructionMethodParameters, [CallerMemberName] string callerMethodName = "")                                                                                                      => CreateFromContextHigherThanSourceFailure<TObjectUnderConstruction>(innerException: null, ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, [CallerMemberName] string callerMethodName = "")                                                                                                                                                         => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, constructionMethodParameters: Array.Empty<object?>(), callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, object? constructionMethodParameter, [CallerMemberName] string callerMethodName = "")                                                                                                                    => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, constructionMethodParameters: new[] { constructionMethodParameter }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, object? constructionMethodParameter1, object? constructionMethodParameter2, [CallerMemberName] string callerMethodName = "")                                                                             => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2 }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, object? constructionMethodParameter1, object? constructionMethodParameter2, object? constructionMethodParameter3, [CallerMemberName] string callerMethodName = "")                                       => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2, constructionMethodParameter3 }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, object? constructionMethodParameter1, object? constructionMethodParameter2, object? constructionMethodParameter3, object? constructionMethodParameter4, [CallerMemberName] string callerMethodName = "") => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2, constructionMethodParameter3, constructionMethodParameter4 }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, IEnumerable<object?> constructionMethodParameters, [CallerMemberName] string callerMethodName = "")                                                                                                      => CreateFromContextHigherThanSourceFailure<TObjectUnderConstruction>(innerException, ruleThatInvalidatesTheInstanceConstruction: null, constructionMethodParameters, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, string ruleThatInvalidatesTheInstanceConstruction, [CallerMemberName] string callerMethodName = "")                                                                                                                                                         => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: Array.Empty<object?>(), callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, string ruleThatInvalidatesTheInstanceConstruction, object? constructionMethodParameter, [CallerMemberName] string callerMethodName = "")                                                                                                                    => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: new[] { constructionMethodParameter }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, string ruleThatInvalidatesTheInstanceConstruction, object? constructionMethodParameter1, object? constructionMethodParameter2, [CallerMemberName] string callerMethodName = "")                                                                             => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2 }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, string ruleThatInvalidatesTheInstanceConstruction, object? constructionMethodParameter1, object? constructionMethodParameter2, object? constructionMethodParameter3, [CallerMemberName] string callerMethodName = "")                                       => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2, constructionMethodParameter3 }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, string ruleThatInvalidatesTheInstanceConstruction, object? constructionMethodParameter1, object? constructionMethodParameter2, object? constructionMethodParameter3, object? constructionMethodParameter4, [CallerMemberName] string callerMethodName = "") => WhenConstructingAnInstanceOf<TObjectUnderConstruction>(innerException, ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2, constructionMethodParameter3, constructionMethodParameter4 }, callerMethodName);
	public static ObjectConstructionException WhenConstructingAnInstanceOf<TObjectUnderConstruction>(Exception innerException, string ruleThatInvalidatesTheInstanceConstruction, IEnumerable<object?> constructionMethodParameters, [CallerMemberName] string callerMethodName = "")                                                                                                      => CreateFromContextHigherThanSourceFailure<TObjectUnderConstruction>(innerException, ruleThatInvalidatesTheInstanceConstruction, constructionMethodParameters, callerMethodName);
	static ObjectConstructionException CreateFromContextHigherThanSourceFailure<TObjectUnderConstruction>(
		Exception? innerException,
		string? ruleThatInvalidatesTheInstanceConstruction,
		IEnumerable<object?> constructionMethodParameters,
		string callerMethodName)
	{
		var message = BuildMessageAboutInstanceConstructionFailure(
			typeof(TObjectUnderConstruction).Name,
			typeof(TObjectUnderConstruction).Namespace!,
			innerException,
			ruleThatInvalidatesTheInstanceConstruction,
			callerMethodName,
			constructionMethodParameters.ToArray());

		return new(typeof(TObjectUnderConstruction), message, innerException);
	}

	public ObjectConstructionException EnrichWithInformationAbout<TObjectUnderConstruction>([CallerMemberName] string callerMethodName = "")                                                                                                                                                         => EnrichWithInformationAbout<TObjectUnderConstruction>(constructionMethodParameters: Array.Empty<object?>(), callerMethodName);
	public ObjectConstructionException EnrichWithInformationAbout<TObjectUnderConstruction>(object? constructionMethodParameter, [CallerMemberName] string callerMethodName = "")                                                                                                                    => EnrichWithInformationAbout<TObjectUnderConstruction>(constructionMethodParameters: new[] { constructionMethodParameter }, callerMethodName);
	public ObjectConstructionException EnrichWithInformationAbout<TObjectUnderConstruction>(object? constructionMethodParameter1, object? constructionMethodParameter2, [CallerMemberName] string callerMethodName = "")                                                                             => EnrichWithInformationAbout<TObjectUnderConstruction>(constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2 }, callerMethodName);
	public ObjectConstructionException EnrichWithInformationAbout<TObjectUnderConstruction>(object? constructionMethodParameter1, object? constructionMethodParameter2, object? constructionMethodParameter3, [CallerMemberName] string callerMethodName = "")                                       => EnrichWithInformationAbout<TObjectUnderConstruction>(constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2, constructionMethodParameter3 }, callerMethodName);
	public ObjectConstructionException EnrichWithInformationAbout<TObjectUnderConstruction>(object? constructionMethodParameter1, object? constructionMethodParameter2, object? constructionMethodParameter3, object? constructionMethodParameter4, [CallerMemberName] string callerMethodName = "") => EnrichWithInformationAbout<TObjectUnderConstruction>(constructionMethodParameters: new[] { constructionMethodParameter1, constructionMethodParameter2, constructionMethodParameter3, constructionMethodParameter4 }, callerMethodName);
	public ObjectConstructionException EnrichWithInformationAbout<TObjectUnderConstruction>(IEnumerable<object?> constructionMethodParameters, [CallerMemberName] string callerMethodName = "")                                                                                                      => AddInformationAboutContextHigherThanSourceFailure<TObjectUnderConstruction>(constructionMethodParameters, callerMethodName);
	ObjectConstructionException AddInformationAboutContextHigherThanSourceFailure<TObjectUnderConstruction>(
		IEnumerable<object?> constructionMethodParameters,
		string callerMethodName)
	{
		var message = BuildMessageAboutInstanceConstructionFailure(
			typeof(TObjectUnderConstruction).Name,
			typeof(TObjectUnderConstruction).Namespace!,
			innerException: null,
			ruleThatInvalidatesTheInstanceConstruction: null,
			callerMethodName,
			constructionMethodParameters.ToArray());

		_objectConstructionLayers.Push(typeof(TObjectUnderConstruction), message);

		return this;
	}

	static string BuildMessageAboutInstanceConstructionFailure(
		string objectUnderConstruction,
		string objectUnderConstructionNamespace,
		Exception? innerException,
		string? ruleThatInvalidatesTheInstanceConstruction,
		string callerMethodName,
		object?[] constructionMethodParameters)
	{
		var sb = new System.Text.StringBuilder();

		sb.Append($"{objectUnderConstruction}: construction failed ");

		if (innerException != null)
		{
			sb.Append($"due to {innerException.GetType().Name} ");
		}

		if (!string.IsNullOrEmpty(ruleThatInvalidatesTheInstanceConstruction))
		{
			sb.Append($"({ruleThatInvalidatesTheInstanceConstruction}) ");
		}

		sb.Append($"via {BuildCallerMethodSignatureApproximation(objectUnderConstruction, callerMethodName, constructionMethodParameters)} ");

		if (constructionMethodParameters.Length == 1)
		{
			sb.Append($"with the following parameter: {constructionMethodParameters.First().Serialize()} ");
		}
		else if (constructionMethodParameters.Length > 1)
		{
			sb.Append($"with the following parameters: [{string.Join(", ", constructionMethodParameters.Select(p => p.Serialize()))}] ");
		}

		if (!string.IsNullOrEmpty(objectUnderConstructionNamespace))
		{
			sb.Append($"[{objectUnderConstructionNamespace}]");
		}

		return sb.ToString();
	}

	static string BuildMessageAboutSpecificMemberConstructionFailure(
		string objectUnderConstruction,
		string objectUnderConstructionNamespace,
		string memberUnderConstruction,
		object? invalidValue,
		string? ruleThatInvalidatesTheValue,
		string callerMethodName = "")
	{
		var sb = new System.Text.StringBuilder();

		sb.Append($"{objectUnderConstruction}: ");

		if (!string.IsNullOrEmpty(callerMethodName))
		{
			sb.Append($"when called from {BuildCallerMethodSignatureApproximation(objectUnderConstruction, callerMethodName)}, ");
		}

		sb.Append($"{memberUnderConstruction} cannot accept value {invalidValue.Serialize()} ");

		if (!string.IsNullOrEmpty(ruleThatInvalidatesTheValue))
		{
			sb.Append($"- {Format(ruleThatInvalidatesTheValue, objectUnderConstruction, memberUnderConstruction)} ");
		}

		if (!string.IsNullOrEmpty(objectUnderConstructionNamespace))
		{
			sb.Append($"[{objectUnderConstructionNamespace}]");
		}

		return sb.ToString();
	}

	static string? Format(string? rule, string objectUnderConstruction, string memberUnderConstruction)
	=> rule switch
	{
		null => null,
		_ => rule
			.Replace("{type}", objectUnderConstruction)
			.Replace("{member}", memberUnderConstruction)
	};

	static string BuildCallerMethodSignatureApproximation(
		string objectUnderConstruction,
		string callerMethodName,
		params object?[] constructionMethodParameters)
	=> $"{objectUnderConstruction}.{callerMethodName}({string.Join(", ", constructionMethodParameters.Select(p => p?.GetType().Name ?? "_"))})";
}
