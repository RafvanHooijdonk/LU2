using System.ComponentModel.DataAnnotations;

namespace LU2Raf.Models;

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [Required]
    public string? Summary { get; set; }
}
