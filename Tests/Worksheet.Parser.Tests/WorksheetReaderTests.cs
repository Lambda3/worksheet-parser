using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace Worksheet.Parser.Tests
{
    public class WorksheetReaderTests
    {
        private int rows;
        private int columns;
        private FakeWorksheet fakeWorksheet;

        [SetUp]
        public void SetUp()
        {
            rows = 2;
            columns = 3;
            fakeWorksheet = new FakeWorksheet(rows, columns);
        }

        [Test]
        public void ShouldGetStartRow() => fakeWorksheet.StartRow.Should().Be(1);

        [Test]
        public void ShouldGetStartColumn() => fakeWorksheet.StartColumn.Should().Be(1);

        [Test]
        public void ShouldCountRows() => fakeWorksheet.CountRows().Should().Be(rows);

        [Test]
        public void ShouldCountColumns() => fakeWorksheet.CountColumns().Should().Be(columns);

        [TestCase(1, 1, "0")]
        [TestCase(1, 2, "1")]
        [TestCase(1, 3, "2")]
        [TestCase(2, 1, "Value0")]
        [TestCase(2, 2, "Value1")]
        [TestCase(2, 3, "Value2")]
        public void ShouldGetCellValue(int row, int column, string value) => fakeWorksheet.GetCellValue(row, column).Should().Be(value);

        [Test]
        public void ShouldReturnEmptyHeaderIfWorksheetIsEmpty() => new FakeWorksheet(0, 3).GetHeader().Should().BeEmpty();

        [TestCase(1, 1)]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(2, 2)]
        public void ShouldGetHeaderWithVariantStartsIndex(int startRow, int startColumn)
            => new FakeWorksheet(rows, columns, startRow, startColumn).GetHeader().Should().BeEquivalentTo(new List<string> { "0", "1", "2" });

        public class FakeWorksheet : WorksheetReader
        {
            private object[,] rows;
            private readonly int startRow;
            private readonly int startColumn;

            public FakeWorksheet(int rowsCount, int columnsCount, int startRow = 1, int startColumn = 1)
            {
                rows = new object[rowsCount, columnsCount];
                this.startRow = startRow;
                this.startColumn = startColumn;

                if (rowsCount < 1)
                    return;

                for (var column = 0; column < columnsCount; column++)
                    rows[0, column] = $"{column}";

                for (var row = 1; row < rowsCount; row++)
                {
                    for (var column = 0; column < columnsCount; column++)
                        rows[row, column] = $"Value{column}";
                }

            }

            public override int StartRow => startRow;

            public override int StartColumn => startColumn;

            public override void AddError(Error error) { }

            public override int CountColumns() => rows.GetLength(1);

            public override int CountRows() => rows.GetLength(0);

            public override string? GetCellValue(int row, int column) => rows[row - StartRow, column - StartColumn]?.ToString();

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

            public override void Dispose() { }
        }
    }
}
