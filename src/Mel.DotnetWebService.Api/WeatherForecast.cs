namespace Mel.DotnetWebService.Api;

public class WeatherForecast
{
	public DateOnly Date { get; set; }

	public int TemperatureC { get; set; }

	public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

	public string? Summary { get; set; }

	public City City { get; set; }
}

public enum City
{
	TechnicalDefaultEnumValue = 0,
	Toulouse = 1,
	Bordeaux = 2,
	Paris = 3
};

public record PayloadWithCityProperty(City City);
