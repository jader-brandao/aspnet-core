using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Infra.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TemplateArquitetura.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<WeatherForecast> Post([FromBody] WeatherRequest request)
        {
            var rng = new Random();

            var weather = new WeatherForecast()
            {
                Date = DateTime.Now,
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };

            DomainService();

            return Ok(weather);
        }

        private void DomainService()
        {
            throw new ValidationDomainException("Ocorreu sim :)!!!");
        }

        public class WeatherRequest
        {
            [Required]
            public int Id { get; set; }

            [Required]
            public string Description { get; set; }
        }
    }
}
