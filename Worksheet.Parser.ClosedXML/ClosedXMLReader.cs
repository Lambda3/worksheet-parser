using ClosedXML.Excel;

namespace Worksheet.Parser.ClosedXML
{
    public class ClosedXMLReader : WorksheetReader
    {
        private readonly IXLWorksheet worksheet;

        public ClosedXMLReader(IXLWorksheet worksheet) => this.worksheet = worksheet;

        public override int StartRow => 1;

        public override int StartColumn => 1;

        public override int CountColumns() => worksheet.LastColumnUsed().ColumnNumber();

        public override int CountRows() => worksheet.LastRowUsed().RowNumber();

        public override string? GetCellValue(int row, int column)
            => worksheet.Cell(row, column).GetString();
    }
}
