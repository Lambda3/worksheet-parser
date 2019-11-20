using System.Collections.Generic;
using System.Linq;

namespace Worksheet.Parser
{
    public abstract class WorksheetReader
    {
        public MessageErrors MessageErrors { get; private set; }

        public WorksheetReader(MessageErrors messageErrors) => this.MessageErrors = messageErrors;

        public WorksheetReader() : this(new MessageErrors()) { }

        public abstract int StartRow { get; }
        public virtual int StartRowItens => StartRow + 1;
        public abstract int StartColumn { get; }
        public abstract int CountRows();
        public abstract int CountColumns();
        public abstract string? GetCellValue(int row, int column);

        public virtual List<string> GetHeader()
        {
            var headers = new List<string>();
            for (var column = StartColumn; CountRows() > 1 && column < CountColumns() + StartColumn; column++)
                headers.Add(GetCellValue(StartRow, column).ToString());
            return headers;
        }

        public virtual ValidationResult<string> GetHeaderWithValidation<T>(WorksheetMap<T> worksheetMap, 
            MessageErrors messageErrors)
        {
            var expectedHeader = worksheetMap.GetFields().Select(s => s.Name).ToList();
            var header = GetHeader();

            var validationResult = new ValidationResult<string>();

            if (header == null)
                validationResult.AddError(MessageErrors.NullHeaderErrorMessage());

            foreach (var headerItem in header)
                if (!expectedHeader.Contains(headerItem))
                    validationResult.AddError(MessageErrors.InvalidItemHeader(headerItem));
                else
                    validationResult.AddItem(headerItem);

            foreach (var expectedHeaderItem in expectedHeader)
                if (!header.Contains(expectedHeaderItem))
                    validationResult.AddError(MessageErrors.MissingItemHeader(expectedHeaderItem));

            return validationResult;
        }
    }
}
