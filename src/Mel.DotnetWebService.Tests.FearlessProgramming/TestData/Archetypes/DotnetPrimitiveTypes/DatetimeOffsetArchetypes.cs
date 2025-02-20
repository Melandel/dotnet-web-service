namespace Mel.DotnetWebService.Tests.FearlessProgramming.TestData.Archetypes.DotnetPrimitiveTypes;

public static class DateTimeOffsetArchetype
{
	public static readonly DateTimeOffset SomeValue = new DateTimeOffset(year: 2010, month: 2, day: 1, hour: 6, minute: 5, second: 4, offset: TimeSpan.FromHours(1));
	public static readonly DateTimeOffset SomeOtherValue = new DateTimeOffset( 2025,        3,      2,       9,         8,         7,         TimeSpan.FromHours(-1));
	public static readonly DateTimeOffset YetAnotherValue = new DateTimeOffset(2175,        4,      3,      12,        11,         10,        TimeSpan.FromHours(4));
}
