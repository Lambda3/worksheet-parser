using System.Collections.Generic;

namespace Worksheet.Parser.Tests.Shared
{
    public class MyWorksheet : WorksheetReader
    {
        private readonly object[,] rows;

        public MyWorksheet(List<FakeClass> rowsFake)
        {
            var columnsCount = 5;
            rows = new object[rowsFake.Count + 1, columnsCount];
            rows[0, 0] = "NU_ID";
            rows[0, 1] = "TX_NAME";
            rows[0, 2] = "DT_FINISH";
            rows[0, 3] = "BO_ENABLE";
            rows[0, 4] = "DB_BONUS";

            for (var row = 1; row <= rowsFake.Count; row++)
            {
                var rowFake = rowsFake[row - 1];
                rows[row, 0] = rowFake.Id.ToString();
                rows[row, 1] = rowFake.Name.ToString();
                rows[row, 2] = rowFake.FinishDate.ToString();
                rows[row, 3] = rowFake.Enable.ToString();
                rows[row, 4] = rowFake.Bonus.ToString();
            }
        }

        public override int StartRow => 0;

        public override int StartColumn => 0;

        public override int CountColumns() => rows.GetLength(1);

        public override int CountRows() => rows.GetLength(0);

        public override string? GetCellValue(int row, int column) => rows[row, column]?.ToString();
    }
}
