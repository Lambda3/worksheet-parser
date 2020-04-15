namespace Worksheet.Parser.Api.Sample
{
    public class WeatherMap: WorksheetMap<WeatherForecast>
    {
        public WeatherMap()
        {
            Map(x => x.Date).ToRequiredField("Date");
            Map(x => x.Summary).ToFieldName("Summary");
            Map(x => x.TemperatureC).ToFieldName("TemperatureC");
        }
    }
}
