using System.Diagnostics;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

[DebuggerDisplay($"{{{nameof(Value)}}}")]
[DebuggerTypeProxy(typeof(ICollectionDebugView<>))]
public abstract class Constrained<TValue>
{
	protected TValue Value;
	protected Constrained(TValue value)
	{
		Value = value;
	}

	public static implicit operator TValue(Constrained<TValue> constrainedValue) => constrainedValue.Value;
	public override string? ToString() => GetType().GetConstrainedTypeRootType() switch
	{
		var t when t.ImplementsGenericInterface(typeof(IEnumerable<>), out _) => Value.GetStringRepresentation(),
		var t when t.ImplementsGenericInterface(typeof(IDictionary<,>), out _) => Value.GetStringRepresentation(),
		_ => Value?.ToString()
	};
}

internal sealed class ICollectionDebugView<TValue>
{
	readonly object _debugFriendlyObject;

	public ICollectionDebugView(Constrained<TValue> obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(nameof(obj));
		}

		if (typeof(TValue).ImplementsGenericInterface(typeof(ICollection<>), out var argumentTypes))
		{
			var elementType = argumentTypes.First();
			foreach (var converter in obj.GetType().GetUserDefinedConversions(browseParentTypes: true))
			{
				try
				{
					dynamic enumerable = converter.Invoke(null, new[] { obj })!;
					var array = Array.CreateInstance(elementType, enumerable.Count);
					enumerable.CopyTo(array);
					_debugFriendlyObject = array;
					break;
				}
				catch
				{
				}
			}

			if (_debugFriendlyObject == null)
			{
				throw new NotImplementedException($"{typeof(ICollectionDebugView<>).GetName()} could not build an array from {obj.GetType().GetName()} using an available user defined conversion.");
			}
		}
		else
		{
			_debugFriendlyObject = obj;
		}

	}

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	public object? DebugFriendlyObject => _debugFriendlyObject;
}
