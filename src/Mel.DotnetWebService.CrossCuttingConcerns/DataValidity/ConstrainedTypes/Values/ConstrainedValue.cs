using System.Diagnostics;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Runtime;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

[DebuggerDisplay($"{{{nameof(Value)}}}")]
[DebuggerTypeProxy(typeof(ICollectionDebugView<>))]
public abstract class ConstrainedValue<TValue>
	: ConstrainedType,
		IComparable,
		IComparable<TValue?>,
		IComparable<ConstrainedValue<TValue>?>,
		IEquatable<TValue?>,
		IEquatable<ConstrainedValue<TValue>?>
	where TValue:
		notnull,
		IComparable,
		IComparable<TValue?>,
		IEquatable<TValue?>
{
	protected TValue Value;
	protected ConstrainedValue(TValue value)
	{
		Value = value;
	}

	public static implicit operator TValue(ConstrainedValue<TValue> constrainedValue) => constrainedValue.Value;
	public override string? ToString() => ConstrainedTypeInfos.GetRootType(GetType()) switch
	{
		var t when t.ImplementsGenericInterface(typeof(IEnumerable<>), out _) => Value.GetStringRepresentation(),
		var t when t.ImplementsGenericInterface(typeof(IDictionary<,>), out _) => Value.GetStringRepresentation(),
		_ => Value?.ToString()
	};

	public override bool Equals(object? obj) => Value.Equals(obj);
	public bool Equals(TValue? other) => Value.Equals(other);
	public bool Equals(ConstrainedValue<TValue>? other) => other is null ? false : Value.Equals(other.Value);

	public int CompareTo(object? obj) => Value.CompareTo(obj);
	public int CompareTo(TValue? other) => Value.CompareTo(other);
	public int CompareTo(ConstrainedValue<TValue>? other) => other is null ? 1 : Value.CompareTo(other.Value);

	public override int GetHashCode() => Value.GetHashCode();

	public static bool operator ==(ConstrainedValue<TValue>? a, ConstrainedValue<TValue>? b)
	=> (a, b) switch
	{
		(null, not null) => false,
		(not null, null) => false,
		(null, null) => true,
		({ Value: var va }, { Value: var vb })_ => va.Equals(vb)
	};
	public static bool operator !=(ConstrainedValue<TValue>? a, ConstrainedValue<TValue>? b) => !(a != b);
}

internal sealed class ICollectionDebugView<TValue>
	where TValue :
		notnull,
		IComparable,
		IComparable<TValue?>,
		IEquatable<TValue?>
{
	readonly object _debugFriendlyObject;

	public ICollectionDebugView(ConstrainedValue<TValue> obj)
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
