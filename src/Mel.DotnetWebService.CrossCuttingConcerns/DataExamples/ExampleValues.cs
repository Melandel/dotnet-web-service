using System.Globalization;
using Mel.DotnetWebService.CrossCuttingConcerns.DataValidity.ConstrainedTypes;

namespace Mel.DotnetWebService.CrossCuttingConcerns.DataExamples;

public static class ExampleValues
{
	public static ExampleValues<T> ValidAndInvalid<T>(IEnumerable<T> validValues, IEnumerable<ConstraintViolationExample<T>> constraintViolationExamples)
	=> ExampleValues<T>.ValidAndInvalid(validValues, constraintViolationExamples);

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<byte>    ForByteType    = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements<byte>  ( default, 1,         24,   60,                             255);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<decimal> ForDecimalType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements        ( default, 1m, 2.5m, -24m,  60m, 79228162514264337593543950335m, -79228162514264337593543950335m);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<double>  ForDoubleType  = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements        ( default, 1d, 2.5d, -24d,  60d,        1.7976931348623157E+308,        -1.7976931348623157E+308);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<float>   ForFloatType   = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements        ( default, 1f, 2.5f, -24f,  60f,                 3.4028235E+38F,                 -3.4028235E+38F);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<int>     ForIntType     = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements        ( default, 1,        -24,   60,                      2147483647,                     -2147483648);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<long>    ForLongType    = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements        ( default, 1L,       -24L,  60L,            9223372036854775807,            -9223372036854775808);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<nint>    ForNIntType    = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements<nint>  ( default, 1,        -24,   60);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<nuint>   ForNUIntType   = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements<nuint> ( default, 1,         24,   60);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<sbyte>   ForSByteType   = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements<sbyte> ( default, 1,        -24,   60,                             127,                            -128);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<short>   ForShortType   = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements<short> ( default, 1,        -24,   60,                           32767,                          -32768);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<uint>    ForUIntType    = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements        ( default, 1U,        24U,  60U,                     4294967295);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<ulong>   ForULongType   = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements        ( default, 1UL,       24UL, 60UL,          18446744073709551615);
	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<ushort>  ForUShortType  = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements<ushort>( default, 1,         24,   60,                           65535);

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<bool>    ForBoolType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements(
		default,
		true);

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<Guid>    ForGuidType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements(
		default,
		Guid.Parse("00000000-0000-0000-0000-000000000001"),
		Guid.Parse("00000000-0000-0000-0000-000000000002"),
		Guid.Parse("00000000-0000-0000-0000-000000000003"));

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<string>  ForStringType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements(
		"",
		"foo",
		"bar",
		"foobar",
		"baz",
		"qux",
		"quux",
		"Foo",
		"123",
		" ",
		"-",
		"\"",
		"é",
		"İ",
		"	foo ",
		" f O o",
		" f 0 o;");

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<DateTime> ForDateTimeType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements(
		default,
		new DateTime(year: 2000, month: 2, day: 1, hour: 21, minute: 20, second: 19),
		new DateTime(      2026,        3,      2,       18,         17,         16),
		new DateTime(      2100,        4,      3,       15,         14,         13));

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<DateTimeOffset> ForDateTimeOffsetType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements(
		default,
		new DateTimeOffset(year: 2010, month: 2, day: 1, hour: 6, minute: 5, second: 4, offset: TimeSpan.FromHours(1)),
		new DateTimeOffset(      2025,        3,      2,       9,         8,         7,         TimeSpan.FromHours(-1)),
		new DateTimeOffset(      2175,        4,      3,      12,        11,         10,        TimeSpan.FromHours(4)));

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<CultureInfo> ForCultureInfoType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements(
		new CultureInfo("en-US"),
		new CultureInfo("fr-FR"),
		new CultureInfo("ja-JP"));

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<RegionInfo> ForRegionInfoType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements(
		new RegionInfo("US"),
		new RegionInfo("FR"),
		new RegionInfo("JP"));

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<Version> ForVersionType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements(
		new Version(1, 0),
		new Version(2, 5, 3),
		new Version(3, 1, 4, 2));

	public static readonly ArrayOfUniqueValuesWithAtLeast2Items<Uri> ForUriType = ArrayOfUniqueValuesWithAtLeast2Items.CreateFromElements(
		new Uri("https://manifesto.softwarecraftsmanship.org/"),
		new Uri("https://example.com/products?id=10"),
		new Uri("https://www.google.com/search?q=csharp&source=web"),
		new Uri("mailto:user@example.com?subject=Hello&body=Welcome"),
		new Uri("tel:+33123456789?extension=123&country=FR"),
		new Uri("ftp://ftp.example.com/files?type=download&format=zip"));
}
