using System.Collections.Generic;
using System.IO;

namespace Worksheet.Parser.Tests.Fakers
{
    public class MyWorksheet : WorksheetReader
    {
        private object[,] rows;

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

        public override void AddError(Error error) { }

        public override int CountColumns() => rows.GetLength(1);

        public override int CountRows() => rows.GetLength(0);

        public override void Dispose() { }

        public override string? GetCellValue(int row, int column) => rows[row, column]?.ToString();

        public override void InsertNewColumnBeforeFirst(string header)
        {
            var newColumn = new object[CountRows() + 1, CountColumns() + 1];
            for (var i = 0; i < CountRows(); i++)
            {
                for (var j = 0; j < CountColumns(); j++)
                {
                    newColumn[i + 1, j + 1] = rows[i, j];
                }
                newColumn[i, StartColumn] = header;
            }
            rows = newColumn;
        }

        public override void SetCellValue(int row, int column, object value) => rows[row, column] = value;

        public override MemoryStream Write() => throw new System.NotImplementedException();
    }
}
