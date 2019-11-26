using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApolloDemo.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptionsSnapshot<EventBus> _eventBusSettings;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptionsSnapshot<EventBus> eventBusSettings) {
            _logger = logger;
            _eventBusSettings = eventBusSettings;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get() {
            var rng = new Random();
            var value = _eventBusSettings.Value.Message;
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
