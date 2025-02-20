using System.Text;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ExceptionBasedFeedback;

namespace Mel.DotnetWebService.CrossCuttingConcerns.ErrorHandling;

public partial class ObjectConstructionException : Exception
{
	readonly ObjectConstructionFailureLayers _objectConstructionLayers;
	public Type TypeThatCouldNotBeConstructed => _objectConstructionLayers.First().TypeOfTheObjectUnderConstruction;

	public override string Message
	{
		get
		{
			var sb = new StringBuilder();
			foreach (var layer in MessageLayers)
			{
				sb.AppendLine(layer);
			}
			return sb.ToString().TrimEnd();
		}
	}

	public IReadOnlyCollection<string> MessageLayers
	{
		get
		{
			var layers = new List<string>();
			foreach (var layer in _objectConstructionLayers)
			{
				layers.Add(layer.Name);
				foreach (var details in layer.Details)
				{
					layers.Add(details);
				}
			}
			return layers;
		}
	}

	ObjectConstructionException(Type typeOfTheObjectUnderConstruction, string message, Exception? innerException = null) : base(message, innerException)
	{
		_objectConstructionLayers = ObjectConstructionFailureLayers.InitializeWith(typeOfTheObjectUnderConstruction, message);
	}

	static ObjectConstructionException CreateFromViolatedConstraintOnSpecificMember<TObjectUnderConstruction>(
		string memberUnderConstruction,
		object? invalidValue,
		Exception? innerException,
		string? ruleThatInvalidatesTheValue,
		IEnumerable<object?> constructionMethodParameters,
		IEnumerable<Type> methodGenericTypeParameters,
		string callerMethodName)
	{
		return new(
			typeof(TObjectUnderConstruction),
			BuildMessageAboutSpecificMemberConstructionFailure(
				typeof(TObjectUnderConstruction),
				externalClassThatImplementsTheInstantiation: null,
				memberUnderConstruction,
				invalidValue,
				innerException,
				ruleThatInvalidatesTheValue,
				constructionMethodParameters,
				methodGenericTypeParameters,
				callerMethodName),
			innerException);
	}
	static ObjectConstructionException CreateFromMethodOrObjectThatIsParentToViolatedConstraintOnSpecificMember<TObjectUnderConstruction>(
		Exception? innerException,
		string? ruleThatInvalidatesTheInstanceConstruction,
		IEnumerable<object?> constructionMethodParameters,
		IEnumerable<Type> methodGenericTypeParameters,
		string callerMethodName)
	{
		return new(
			typeof(TObjectUnderConstruction),
			BuildMessageAboutInstanceConstructionFailure(
				typeof(TObjectUnderConstruction),
				externalClassThatImplementsTheInstantiation: null,
				innerException,
				ruleThatInvalidatesTheInstanceConstruction,
				callerMethodName,
				constructionMethodParameters.ToArray(),
				methodGenericTypeParameters.ToArray()),
			innerException);
	}

	static ObjectConstructionException CreateFromMemberConfigurationFailureInAnotherClass<TObjectUnderConstruction, TOtherClass>(
		string memberUnderConstruction,
		object? invalidValue,
		Exception? innerException,
		string? ruleThatInvalidatesTheValue,
		IEnumerable<object?> constructionMethodParameters,
		IEnumerable<Type> methodGenericTypeParameters,
		string callerMethodName)
	{
		return new(
			typeof(TObjectUnderConstruction),
			BuildMessageAboutSpecificMemberConstructionFailure(
				typeof(TObjectUnderConstruction),
				typeof(TOtherClass),
				memberUnderConstruction,
				invalidValue,
				innerException,
				ruleThatInvalidatesTheValue,
				constructionMethodParameters,
				methodGenericTypeParameters,
				callerMethodName),
			innerException);
	}

	static ObjectConstructionException CreateFromInstantiationFailureInAnotherClass<TObjectUnderConstruction, TOtherClass>(
		Exception? innerException,
		string? ruleThatInvalidatesTheInstanceConstruction,
		IEnumerable<object?> constructionMethodParameters,
		IEnumerable<Type> methodGenericTypeParameters,
		string callerMethodName)
	{
		return new(
			typeof(TObjectUnderConstruction),
			BuildMessageAboutInstanceConstructionFailure(
				typeof(TObjectUnderConstruction),
				typeof(TOtherClass),
				innerException,
				ruleThatInvalidatesTheInstanceConstruction,
				callerMethodName,
				constructionMethodParameters.ToArray(),
				methodGenericTypeParameters.ToArray()),
			innerException);
	}

	static ObjectConstructionException CreateFromDataFetchingFailure<TObjectToConstruct>(
		Exception? innerException,
		string? ruleThatInvalidatesFetchedData,
		IEnumerable<object?> dataFetchingParameters,
		IEnumerable<Type> methodGenericTypeParameters,
		string callerFilePath,
		string callerMethodName)
	{
		return new(
			typeof(TObjectToConstruct),
			BuildMessageAboutFailedDataFetching(
				typeof(TObjectToConstruct),
				innerException,
				ruleThatInvalidatesFetchedData,
				dataFetchingParameters.ToArray(),
				methodGenericTypeParameters.ToArray(),
				callerClassName: Path.GetFileNameWithoutExtension(callerFilePath),
				callerMethodName),
			innerException);
	}

	ObjectConstructionException AddInformationToObjectConstructionFailureContext<TObjectUnderConstruction>(
		IEnumerable<object?> constructionMethodParameters,
		IEnumerable<Type> methodGenericTypeParameters,
		string callerMethodName)
	{
		_objectConstructionLayers.Push(
			typeof(TObjectUnderConstruction),
			BuildMessageAboutInstanceConstructionFailure(
				typeof(TObjectUnderConstruction),
				externalClassThatImplementsTheInstantiation: null,
				innerException: null,
				ruleThatInvalidatesTheInstanceConstruction: null,
				callerMethodName,
				constructionMethodParameters.ToArray(),
				methodGenericTypeParameters.ToArray()));
		return this;
	}

	ObjectConstructionException AddInformationToDataFetchingFailureContext<TObjectToConstruct>(
		IEnumerable<object?> dataFetchingParameters,
		IEnumerable<Type> methodGenericTypeParameters,
		string callerFilePath,
		string callerMethodName)
	{
		_objectConstructionLayers.Push(
			typeof(TObjectToConstruct),
			BuildMessageAboutFailedDataFetching(
				typeof(TObjectToConstruct),
				innerException: null,
				ruleThatInvalidatesFetchedData: null,
				dataFetchingParameters.ToArray(),
				methodGenericTypeParameters.ToArray(),
				callerClassName: Path.GetFileNameWithoutExtension(callerFilePath),
				callerMethodName));
		return this;
	}

	static string BuildMessageAboutInstanceConstructionFailure(
		Type objectUnderConstructionType,
		Type? externalClassThatImplementsTheInstantiation,
		Exception? innerException,
		string? ruleThatInvalidatesTheInstanceConstruction,
		string callerMethodName,
		IReadOnlyCollection<object?> constructionMethodParameters,
		IEnumerable<Type> methodGenericTypeParameters)
	{
		var sb = new StringBuilder();

		sb.Append($"{objectUnderConstructionType.GetName()}: construction failed ");

		if (innerException != null)
		{
			sb.Append($"due to {innerException.GetType().GetName()} ");
		}

		if (!string.IsNullOrEmpty(ruleThatInvalidatesTheInstanceConstruction))
		{
			sb.Append($"({ruleThatInvalidatesTheInstanceConstruction}) ");
		}

		sb.Append($"via ");
		if (externalClassThatImplementsTheInstantiation != null)
		{
			sb.Append(externalClassThatImplementsTheInstantiation.GetName());
			if (!String.IsNullOrEmpty(externalClassThatImplementsTheInstantiation.Namespace))
			{
				sb.Append($"[{externalClassThatImplementsTheInstantiation.Namespace}]");
			}
			sb.Append('.');
		}
		sb.Append($"{BuildCallerMethodSignatureApproximation(objectUnderConstructionType, callerMethodName, constructionMethodParameters, methodGenericTypeParameters)} ");

		if (constructionMethodParameters.Count == 1)
		{
			sb.Append($"with the following parameter: {constructionMethodParameters.First().GetStringRepresentation()} ");
		}
		else if (constructionMethodParameters.Count > 1)
		{
			sb.Append($"with the following parameters: [{string.Join(", ", constructionMethodParameters.Select(p => p.GetStringRepresentation()))}] ");
		}

		if (!string.IsNullOrEmpty(objectUnderConstructionType.Namespace))
		{
			sb.Append($"[{objectUnderConstructionType.Namespace}]");
		}

		return sb.ToString();
	}

	static string BuildMessageAboutSpecificMemberConstructionFailure(
		Type objectUnderConstructionType,
		Type? externalClassThatImplementsTheInstantiation,
		string memberUnderConstruction,
		object? invalidValue,
		Exception? innerException,
		string? ruleThatInvalidatesTheValue,
		IEnumerable<object?> constructionMethodParameters,
		IEnumerable<Type> methodGenericTypeParameters,
		string callerMethodName)
	{
		var sb = new StringBuilder();

		sb.Append($"{objectUnderConstructionType.GetName()}: ");

		if (!callerMethodName.Contains(".ctor"))
		{
			sb.Append("when called from ");
			if (externalClassThatImplementsTheInstantiation != null)
			{
				sb.Append(externalClassThatImplementsTheInstantiation.GetName());
				if (!String.IsNullOrEmpty(externalClassThatImplementsTheInstantiation.Namespace))
				{
					sb.Append($"[{externalClassThatImplementsTheInstantiation.Namespace}]");
				}
				sb.Append('.');
			}
			sb.Append($"{BuildCallerMethodSignatureApproximation(objectUnderConstructionType, callerMethodName, constructionMethodParameters, methodGenericTypeParameters)}, ");
		}

		sb.Append($"{memberUnderConstruction} cannot accept value {invalidValue.GetStringRepresentation()}");

		if (innerException != null)
		{
			sb.Append($" due to {innerException.GetType().GetName()}");
		}

		if (!string.IsNullOrEmpty(ruleThatInvalidatesTheValue))
		{
			sb.Append($": {Format(ruleThatInvalidatesTheValue, objectUnderConstructionType, memberUnderConstruction)}");
		}

		if (!string.IsNullOrEmpty(objectUnderConstructionType.Namespace))
		{
			sb.Append($" [{objectUnderConstructionType.Namespace}]");
		}

		return sb.ToString();
	}

	static string BuildMessageAboutFailedDataFetching(
		Type objectToConstructType,
		Exception? innerException,
		string? ruleThatInvalidatesFetchedData,
		IReadOnlyCollection<object?> dataFetchingParameters,
		IReadOnlyCollection<Type> methodGenericTypeParameters,
		string callerClassName,
		string callerMethodName)
	{
		var sb = new StringBuilder();
		sb.Append($"{objectToConstructType.GetName()}: construction failed due to unobtained data ");

		if (!string.IsNullOrEmpty(ruleThatInvalidatesFetchedData))
		{
			sb.Append($"; {Format(ruleThatInvalidatesFetchedData, objectToConstructType)} ");
		}

		if (innerException != null)
		{
			sb.Append($"({innerException.GetType().GetName()} ");
		}

		sb.Append($"when calling ");
		sb.Append($"{BuildCallerMethodSignatureApproximation(callerClassName, callerMethodName, dataFetchingParameters, methodGenericTypeParameters)} ");
		if (dataFetchingParameters.Count == 1)
		{
			sb.Append($"with the following parameter: {dataFetchingParameters.First().GetStringRepresentation()}");
		}
		else if (dataFetchingParameters.Count > 1)
		{
			sb.Append($"with the following parameters: [{string.Join(", ", dataFetchingParameters.Select(p => p.GetStringRepresentation()))}]");
		}

		if (innerException != null)
		{
			sb.Append($")");
		}

		if (objectToConstructType.Namespace != null)
		{
			sb.Append($" [{objectToConstructType.Namespace}]");
		}
		return sb.ToString();
	}

	static string? Format(string? rule, Type objectUnderConstructionType, string memberUnderConstruction)
	=> rule switch
	{
		null => null,
		_ => rule
			.Replace("@type", objectUnderConstructionType.GetName())
			.Replace("@member", memberUnderConstruction)
	};

	static string? Format(string? rule, Type objectUnderConstructionType)
	=> rule switch
	{
		null => null,
		_ => rule
			.Replace("@type", objectUnderConstructionType.GetName())
	};

	static string BuildCallerMethodSignatureApproximation(
		Type objectUnderConstructionType,
		string callerMethodName,
		IEnumerable<object?> constructionMethodParameters,
		IEnumerable<Type> methodGenericTypeParameters)
	=> BuildCallerMethodSignatureApproximation(
		objectUnderConstructionType.GetName(),
		callerMethodName,
		constructionMethodParameters,
		methodGenericTypeParameters);

	static string BuildCallerMethodSignatureApproximation(
		string objectUnderConstructionTypeName,
		string callerMethodName,
		IEnumerable<object?> constructionMethodParameters,
		IEnumerable<Type> methodGenericTypeParameters)
	{
		var methodSignature = new StringBuilder();
		methodSignature.Append($"{objectUnderConstructionTypeName}.{callerMethodName}");
		if (methodGenericTypeParameters.Any())
		{
			methodSignature.Append($"<{string.Join(',', methodGenericTypeParameters.Select(p => p.GetName()))}>");
		}
		methodSignature.Append($"({string.Join(", ", constructionMethodParameters.Select(p => p?.GetType().GetName() ?? "_"))})");
		return methodSignature.ToString();
	}
}
