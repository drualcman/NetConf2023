using BlazorPagination.Shared;
using Database.ADO;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlazorPagination.Server.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    class vib
    {
        public int vibno { get; set; }
        public string company { get; set; }
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get(DataBaseWithADO database)
    {
        Console.WriteLine($"Connection String: '{database.ConnectionString}'");

        DataTable dataTable = database.GetDataTable<vib>(nameof(vib.vibno), 50, 5);
        DataSet ds = database.GetDataSet<vib>(nameof(vib.vibno), 50, 5);
        DataView dv = database.GetDataView<vib>(nameof(vib.vibno), 50, 5);
        List<vib> list = database.List<vib>(nameof(vib.vibno), 50, 5);
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
