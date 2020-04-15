using ClosedXML.Excel;
using System.IO;

namespace Worksheet.Parser.ClosedXML
{
    public static class XLWorkbookExtensions
    {
        public static MemoryStream GetStream(this XLWorkbook workbook)
        {
            var memoryStrem = new MemoryStream();
            workbook.SaveAs(memoryStrem);
            return memoryStrem;
        }
    }
}