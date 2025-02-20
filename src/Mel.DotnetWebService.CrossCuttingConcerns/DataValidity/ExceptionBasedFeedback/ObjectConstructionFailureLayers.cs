using System.Collections;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ExceptionBasedFeedback;

record ObjectConstructionFailureLayers : IEnumerable<ObjectConstructionFailure>
{
	ObjectConstructionFailure _latestFailure;
	readonly Stack<ObjectConstructionFailure> _failuresExceptLatest;
	ObjectConstructionFailureLayers(
		ObjectConstructionFailure latestfailure,
		Stack<ObjectConstructionFailure> failuresExceptLatest)
	{
		_latestFailure = latestfailure;
		_failuresExceptLatest = failuresExceptLatest;
	}

	public static ObjectConstructionFailureLayers InitializeWith(
		Type typeOfTheObjectUnderConstruction,
		string failureMessage)
	{
		try
		{
			var failure = ObjectConstructionFailure.FromConstructing(
				typeOfTheObjectUnderConstruction,
				failureMessage);

			return new(
				failure,
				new Stack<ObjectConstructionFailure>());
		}
		catch (ObjectConstructionException objectConstructionException)
		{
			objectConstructionException.EnrichConstructionFailureContextWith<ObjectConstructionFailureLayers>(new object[] { typeOfTheObjectUnderConstruction, failureMessage });
			throw;
		}
		catch (Exception developerMistake)
		{
			throw ObjectConstructionException.WhenConstructingAnInstanceOf<ObjectConstructionFailureLayers>(developerMistake, new object[] { typeOfTheObjectUnderConstruction, failureMessage });
		}
	}

	public void Push(Type typeOfTheObjectUnderConstruction, string failureMessage)
	{
		if (typeOfTheObjectUnderConstruction == _latestFailure.TypeOfTheObjectUnderConstruction && failureMessage == _latestFailure.LastDetailAdded)
		{
			return;
		}

		if (typeOfTheObjectUnderConstruction == _latestFailure.TypeOfTheObjectUnderConstruction)
		{
			_latestFailure.AddDetails(failureMessage);
			return;
		}

		_failuresExceptLatest.Push(_latestFailure);
		_latestFailure = ObjectConstructionFailure.FromConstructing(typeOfTheObjectUnderConstruction, failureMessage);
	}

	public IEnumerator<ObjectConstructionFailure> GetEnumerator()
	{
		var enumerable = new List<ObjectConstructionFailure>();
		enumerable.Add(_latestFailure);
		enumerable.AddRange(_failuresExceptLatest);
		return ((IEnumerable<ObjectConstructionFailure>)enumerable).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		var enumerable = new List<ObjectConstructionFailure>();
		enumerable.Add(_latestFailure);
		enumerable.AddRange(_failuresExceptLatest);
		return ((IEnumerable)enumerable).GetEnumerator();
	}
}
