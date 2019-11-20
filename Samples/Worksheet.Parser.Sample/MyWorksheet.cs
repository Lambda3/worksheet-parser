using System;

namespace Worksheet.Parser.Sample
{
    public class MyWorksheet : WorksheetReader
    {
        private readonly object[,] rows;

        public MyWorksheet(int rowsCount = 11)
        {
            var columnsCount = 8;
            rows = new object[rowsCount + 1, columnsCount];
            rows[0, 0] = "NU_ID";
            rows[0, 1] = "TX_NAME";
            rows[0, 2] = "DT_CREATION";
            rows[0, 3] = "DT_FINISH";
            rows[0, 4] = "NU_REGISTER";
            rows[0, 5] = "BO_ENABLE";
            rows[0, 6] = "DE_VALUE";
            rows[0, 7] = "DB_BONUS";

            for (var row = 1; row <= rowsCount; row++)
            {
                rows[row, 0] = $"{row}";
                rows[row, 1] = $"Nome{row}";
                rows[row, 2] = $"{new DateTime(2019, 12, 30).AddDays(row)}";
                rows[row, 3] = (row % 2) == 0 ? $"{new DateTime(2020, 10, 30).AddDays(row)}" : null;
                rows[row, 4] = $"{row * 10}";
                rows[row, 5] = (row % 2) == 0 ? "true" : "false";
                rows[row, 6] = $"{120.43 + row}";
                rows[row, 7] = (row % 2) != 0 ? $"{10.3 + row}" : null;

            }
        }

        public override int StartRow => 0;

        public override int StartColumn => 0;

        public override int CountColumns() => rows.GetLength(1);

        public override int CountRows() => rows.GetLength(0);

        public override string? GetCellValue(int row, int column) => rows[row, column]?.ToString();
    }
}
