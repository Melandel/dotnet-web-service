namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ExceptionBasedFeedback;

class ObjectConstructionFailure
{
	public Type TypeOfTheObjectUnderConstruction { get; }
	public string Name => _messages.First();
	public IReadOnlyCollection<string> Details
	=> _messages.Any()
		? _messages.Skip(1).ToArray()
		: Array.Empty<string>();

	readonly Stack<string> _messages;

	ObjectConstructionFailure(
		Type typeOfTheObjectUnderConstruction,
		Stack<string> messages)
	{
		TypeOfTheObjectUnderConstruction = typeOfTheObjectUnderConstruction;
		_messages = messages;
	}

	public static ObjectConstructionFailure FromConstructing(
		Type typeOfTheObjectUnderConstruction,
		string failureMessage)
	{
		var messages = new Stack<string>();
		messages.Push(failureMessage);

		return new(
			typeOfTheObjectUnderConstruction,
			messages);
	}

	public void AddDetails(string failureMessage)
	=> _messages.Push(failureMessage);

	public string LastDetailAdded
	=> _messages.FirstOrDefault() ?? "";
}
