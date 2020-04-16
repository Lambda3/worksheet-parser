using ClosedXML.Excel;
using System.IO;

namespace Worksheet.Parser.ClosedXML
{
    public class ClosedXmlReader : WorksheetReader
    {
        private IXLWorksheet worksheet;
        private XLWorkbook workbook;

        public ClosedXmlReader(XLWorkbook workbook, string workSheetName)
        {
            this.workbook = workbook;
            worksheet = workbook.Worksheet(workSheetName);
        }

        public override int StartRow => 1;

        public override int StartColumn => 1;

        public override int CountColumns() => worksheet.LastColumnUsed().ColumnNumber();

        public override int CountRows() => worksheet.LastRowUsed().RowNumber();

        public override string? GetCellValue(int row, int column)
            => worksheet.Cell(row, column).GetString();
        public override void SetCellValue(int row, int column, object value)
            => worksheet.Cell(row, column).SetValue(value);

        public override void AddError(Error error)
        {
            var cell = worksheet.Cell(error.Row, error.Column);
            var message = $"- {error.Message}";
            if (cell.HasComment)
                cell.Comment.AddNewLine().AddText(message);
            else
            {
                cell.Comment.AddText(message);
                FormatCellWithError(cell);
            }
        }

        public override void InsertNewColumnBeforeFirst(string header)
        {
            var newWorkbook = new XLWorkbook();
            var newWorksheet = newWorkbook.Worksheets.Add(worksheet.Name);

            newWorksheet.Cell(StartRow, StartColumn).Value = header;
            for (var row = StartRow; row <= CountRows(); row++)
                for (var column = StartColumn; column <= CountColumns(); column++)
                    newWorksheet.Cell(row, column + 1).Value = worksheet.Cell(row, column);

            for (var column = StartColumn; column <= CountColumns(); column++)
                newWorksheet.Column(column + 1).Width = worksheet.Column(column).Width;

            worksheet.Delete();
            workbook.Dispose();
            worksheet = newWorksheet;
            workbook = newWorkbook;

            newWorksheet.Column(StartColumn).SetAutoFilter();
            newWorksheet.Column(StartColumn).Width = header.Length + 2;
            newWorksheet.Range(StartRow, StartColumn, CountRows(), CountColumns()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        }

        private void FormatCellWithError(IXLCell cell)
        {
            cell.Style.Fill.SetBackgroundColor(XLColor.FromArgb(255, 193, 193));
            var borderColor = XLColor.Red;
            var borderType = XLBorderStyleValues.Thin;
            cell.Style.Border.LeftBorder = borderType;
            cell.Style.Border.LeftBorderColor = borderColor;
            cell.Style.Border.RightBorder = borderType;
            cell.Style.Border.RightBorderColor = borderColor;
            cell.Style.Border.TopBorder = borderType;
            cell.Style.Border.TopBorderColor = borderColor;
            cell.Style.Border.BottomBorder = borderType;
            cell.Style.Border.BottomBorderColor = borderColor;
        }

        public override void Dispose() => workbook.Dispose();

        public override MemoryStream Write() => workbook.GetStream();
    }
}
