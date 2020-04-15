using ClosedXML.Excel;
using System.IO;

namespace Worksheet.Parser.ClosedXML
{
    public class ClosedXmlReader : WorksheetReader
    {
        private readonly IXLWorksheet worksheet;
        private readonly XLWorkbook workbook;

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
            worksheet.Range(StartRow, StartColumn, CountRows(), StartColumn).InsertColumnsBefore(StartColumn);
            SetCellValue(StartRow, StartColumn, header);
            worksheet.Column(StartColumn).SetAutoFilter();
            worksheet.Range(StartRow, StartColumn, CountRows(), CountColumns()).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Columns().AdjustToContents();
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
