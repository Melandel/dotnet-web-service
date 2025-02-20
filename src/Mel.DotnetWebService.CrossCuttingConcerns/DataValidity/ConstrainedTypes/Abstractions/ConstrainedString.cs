using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes.Abstractions;

public abstract class ConstrainedString
	: ConstrainedValue<string>,
		IEnumerable<char>,
		IEnumerable,
		ICloneable,
		IConvertible
{
	protected ConstrainedString(string value) : base(value)
	{
	}

	public object Clone() => Value.Clone();
	public IEnumerator<char> GetEnumerator() => ((IEnumerable<char>)Value).GetEnumerator();
	public TypeCode GetTypeCode() => Value.GetTypeCode();
	public bool     ToBoolean(IFormatProvider? provider)                   => ((IConvertible)Value).ToBoolean(provider);
	public byte     ToByte(IFormatProvider? provider)                      => ((IConvertible)Value).ToByte(provider);
	public char     ToChar(IFormatProvider? provider)                      => ((IConvertible)Value).ToChar(provider);
	public DateTime ToDateTime(IFormatProvider? provider)                  => ((IConvertible)Value).ToDateTime(provider);
	public decimal  ToDecimal(IFormatProvider? provider)                   => ((IConvertible)Value).ToDecimal(provider);
	public double   ToDouble(IFormatProvider? provider)                    => ((IConvertible)Value).ToDouble(provider);
	public short    ToInt16(IFormatProvider? provider)                     => ((IConvertible)Value).ToInt16(provider);
	public int      ToInt32(IFormatProvider? provider)                     => ((IConvertible)Value).ToInt32(provider);
	public long     ToInt64(IFormatProvider? provider)                     => ((IConvertible)Value).ToInt64(provider);
	public sbyte    ToSByte(IFormatProvider? provider)                     => ((IConvertible)Value).ToSByte(provider);
	public float    ToSingle(IFormatProvider? provider)                    => ((IConvertible)Value).ToSingle(provider);
	public string   ToString(IFormatProvider? provider)                    => Value.ToString(provider);
	public object   ToType(Type conversionType, IFormatProvider? provider) => ((IConvertible)Value).ToType(conversionType, provider);
	public ushort   ToUInt16(IFormatProvider? provider)                    => ((IConvertible)Value).ToUInt16(provider);
	public uint     ToUInt32(IFormatProvider? provider)                    => ((IConvertible)Value).ToUInt32(provider);
	public ulong    ToUInt64(IFormatProvider? provider)                    => ((IConvertible)Value).ToUInt64(provider);
	IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Value).GetEnumerator();

public char this[int index] => Value[index];
public int Length => Value.Length;

	public bool Contains(string value) => Value.Contains(value);
	public bool Contains(string value, StringComparison comparisonType) => Value.Contains(value, comparisonType);
	public bool Contains(char value) => Value.Contains(value);
	public bool Contains(char value, StringComparison comparisonType) => Value.Contains(value, comparisonType);
	public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count) => Value.CopyTo(sourceIndex, destination, destinationIndex, count);
	public void CopyTo(Span<char> destination) => Value.CopyTo(destination);
	public bool EndsWith(char value) => Value.EndsWith(value);
	public bool EndsWith(string value) => Value.EndsWith(value);
	public bool EndsWith(string value, bool ignoreCase, CultureInfo? culture)=> Value.EndsWith(value, ignoreCase, culture);
	public bool EndsWith(string value, StringComparison comparisonType) => Value.EndsWith(value, comparisonType);
	public StringRuneEnumerator EnumerateRunes() => Value.EnumerateRunes();
	public bool Equals([NotNullWhen(true)] string? value, StringComparison comparisonType) => Value.Equals(value, comparisonType);
	public int GetHashCode(StringComparison comparisonType) => Value.GetHashCode(comparisonType);
	public ref readonly char GetPinnableReference() => ref Value.GetPinnableReference();
	public int IndexOf(string value, int startIndex, StringComparison comparisonType) => Value.IndexOf(value, startIndex, comparisonType);
	public int IndexOf(char value, int startIndex, int count) => Value.IndexOf(value, startIndex, count);
	public int IndexOf(char value, int startIndex) => Value.IndexOf(value, startIndex);
	public int IndexOf(string value, StringComparison comparisonType) => Value.IndexOf(value, comparisonType);
	public int IndexOf(string value, int startIndex, int count, StringComparison comparisonType) => Value.IndexOf(value, startIndex, comparisonType);
	public int IndexOf(string value, int startIndex, int count) => Value.IndexOf(value, startIndex, count);
	public int IndexOf(string value, int startIndex) => Value.IndexOf(value, startIndex);
	public int IndexOf(string value) => Value.IndexOf(value);
	public int IndexOf(char value, StringComparison comparisonType) => Value.IndexOf(value, comparisonType);
	public int IndexOf(char value) => Value.IndexOf(value);
	public int IndexOfAny(char[] anyOf, int startIndex, int count) => Value.IndexOfAny(anyOf, startIndex, count);
	public int IndexOfAny(char[] anyOf, int startIndex) => Value.IndexOfAny(anyOf, startIndex);
	public int IndexOfAny(char[] anyOf) => Value.IndexOfAny(anyOf);
	public bool IsNormalized(NormalizationForm normalizationForm) => Value.IsNormalized(normalizationForm);
	public bool IsNormalized() => Value.IsNormalized();
	public int LastIndexOf(char value) => Value.LastIndexOf(value);
	public int LastIndexOf(char value, int startIndex) => Value.LastIndexOf(value, startIndex);
	public int LastIndexOf(char value, int startIndex, int count) => Value.LastIndexOf(value, startIndex, count);
	public int LastIndexOf(string value) => Value.LastIndexOf(value);
	public int LastIndexOf(string value, int startIndex) => Value.LastIndexOf(value, startIndex);
	public int LastIndexOf(string value, int startIndex, int count) => Value.LastIndexOf(value, startIndex, count);
	public int LastIndexOf(string value, int startIndex, int count, StringComparison comparisonType) => Value.LastIndexOf(value, startIndex, count, comparisonType);
	public int LastIndexOf(string value, int startIndex, StringComparison comparisonType) => Value.LastIndexOf(value, startIndex, comparisonType);
	public int LastIndexOf(string value, StringComparison comparisonType) => Value.LastIndexOf(value, comparisonType);
	public int LastIndexOfAny(char[] anyOf) => Value.LastIndexOfAny(anyOf);
	public int LastIndexOfAny(char[] anyOf, int startIndex, int count) => Value.LastIndexOfAny(anyOf, startIndex, count);
	public int LastIndexOfAny(char[] anyOf, int startIndex) => Value.LastIndexOfAny(anyOf, startIndex);
	public bool StartsWith(string value, StringComparison comparisonType) => Value.StartsWith(value, comparisonType);
	public bool StartsWith(string value, bool ignoreCase, CultureInfo? culture) => Value.StartsWith(value, ignoreCase, culture);
	public bool StartsWith(char value) => Value.StartsWith(value);
	public bool StartsWith(string value) => Value.StartsWith(value);
	public char[] ToCharArray(int startIndex, int length) => Value.ToCharArray(startIndex, length);
	public char[] ToCharArray() => Value.ToCharArray();
	public bool TryCopyTo(Span<char> destination) => Value.TryCopyTo(destination);
}
