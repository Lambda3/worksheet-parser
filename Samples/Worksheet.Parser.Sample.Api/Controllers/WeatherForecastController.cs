using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Worksheet.Parser.ClosedXML;

namespace Worksheet.Parser.Sample.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ClosedXMLWorksheetParser<WeatherForecast> parser;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            ClosedXMLWorksheetParser<WeatherForecast> parser
            )
        {
            _logger = logger;
            this.parser = parser;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample.xlsx");
            var worksheetName = "SampleSheet";

            return Ok(parser.Parse(path, worksheetName));
        }
    }
}
