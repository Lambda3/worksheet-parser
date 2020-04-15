using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Worksheet.Parser.Api.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorksheetController : ControllerBase
    {
        private const string worksheetName = "SampleSheet";
        private const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly WorksheetParser<WeatherForecast> parser;

        public WorksheetController(WorksheetParser<WeatherForecast> parser) => this.parser = parser;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sample.xlsx");
            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            using var stream = new MemoryStream(bytes);
            var result = parser.Parse(stream, worksheetName);

            if (result.IsSuccess)
                return Ok(result.Itens);

            using var streamWithErrors = parser.WriteErrorsWithSummary(stream, worksheetName, result.Errors);
            return File(streamWithErrors.ToArray(), contentType, "planilha.xlsx");
        }
    }
}
