using ClosedXML.Excel;
using System.IO;

namespace Worksheet.Parser.ClosedXML
{
    public class ClosedXMLWorksheetParser<T> where T : class, new()
    {
        private readonly WorksheetParser<T> parser;

        public ClosedXMLWorksheetParser(WorksheetParser<T> parser) => this.parser = parser;

        public ValidationResult<T> Parse(IXLWorksheet worksheet)
            => parser.Parse(new ClosedXMLReader(worksheet));

        public ValidationResult<T> Parse(string path, string worksheetName)
        {
            using var workbook = new XLWorkbook(path);
            return Parse(workbook.Worksheet(worksheetName));
        }
        public ValidationResult<T> Parse(Stream stream, string worksheetName)
        {
            using var workbook = new XLWorkbook(stream);
            return Parse(workbook.Worksheet(worksheetName));
        }
    }
}
