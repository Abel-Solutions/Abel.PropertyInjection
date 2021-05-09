using System;
using System.Collections.Generic;
using System.Linq;
using Abel.PropertyInjection.TestServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Abel.PropertyInjection.TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHelloWorld _helloWorld;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHelloWorld helloWorld, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _helloWorld = helloWorld;
            var lol = serviceProvider.GetService(typeof(IHelloWorld));
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _helloWorld.Hello();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
