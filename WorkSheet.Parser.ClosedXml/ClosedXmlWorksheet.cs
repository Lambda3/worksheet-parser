using System.IO;
using ClosedXML.Excel;

namespace Worksheet.Parser.ClosedXML
{
    public class ClosedXmlWorksheet<T> : WorksheetDefault<T> where T : class, new()
    {
        public ClosedXmlWorksheet(WorksheetParser<T> parser, MessageErrors messageErrors) : base(parser, messageErrors) { }

        protected override WorksheetReader GetReader(string path, string worksheetName) => GetReader(new XLWorkbook(path), worksheetName);

        protected override WorksheetReader GetReader(Stream stream, string worksheetName) => GetReader(new XLWorkbook(stream), worksheetName);

        protected override MemoryStream Write(WorksheetReader reader) => reader.Write();

        private WorksheetReader GetReader(XLWorkbook worksheet, string name)
                => new ClosedXmlReader(worksheet, name);
    }
}
