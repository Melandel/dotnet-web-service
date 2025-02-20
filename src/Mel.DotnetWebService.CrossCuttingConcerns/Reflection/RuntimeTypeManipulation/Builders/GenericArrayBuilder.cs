using System.Collections;

namespace Mel.DotnetWebService.CrossCuttingConcerns.Reflection.RuntimeTypeManipulation;

class GenericArrayBuilder
{
	readonly Array _arrayUnderConstruction;
	int _currentIndex = 0;
	GenericArrayBuilder(Array array)
	{
		_arrayUnderConstruction = array;
	}

	public static GenericArrayBuilder ForACapacityOf(int capacity, Type itemType)
	=> new(Array.CreateInstance(itemType, capacity));

	public GenericArrayBuilder Add(object item)
	{
		_arrayUnderConstruction.SetValue(item, _currentIndex);
		_currentIndex++;
		return this;
	}

	public GenericArrayBuilder AddRange(IEnumerable collection)
	{
		var collectionType = collection.GetType();
		if (collectionType.ImplementsInterface(typeof(ICollection)))
		{
			var asICollection = (ICollection) collection;
			var lengthAfterOperation = _currentIndex + asICollection.Count;
			if (lengthAfterOperation > _arrayUnderConstruction.GetLength(0))
			{
				throw new InvalidOperationException($"{GetType().GetName()}.{nameof(AddRange)}: cannot add ${asICollection.Count}-items {collectionType.GetName()} without surpassing the configured capacity of {_arrayUnderConstruction.GetLength(0)}.");
			}
			asICollection.CopyTo(_arrayUnderConstruction, _currentIndex);
			_currentIndex = lengthAfterOperation;
			return this;
		}

		foreach (var item in collection)
		{
			Add(item);
		}
		return this;
	}

	public Array BuildAsArray()
	=> _arrayUnderConstruction;

	public IList BuildAsIList()
	=> _arrayUnderConstruction;

	public dynamic BuildAsDynamic()
	=> _arrayUnderConstruction;

	public dynamic[] BuildAsArrayOfDynamics()
	=> (dynamic[])_arrayUnderConstruction;
}
