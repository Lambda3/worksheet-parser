using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Worksheet.Parser
{
    public abstract class WorksheetReader : IDisposable
    {
        public abstract int StartRow { get; }
        public virtual int StartRowItens => StartRow + 1;
        public abstract int StartColumn { get; }
        public abstract int CountRows();
        public abstract int CountColumns();
        public abstract string? GetCellValue(int row, int column);
        public abstract void SetCellValue(int row, int column, object value);
        public abstract void AddError(Error error);
        public abstract void InsertNewColumnBeforeFirst(string header);

        public virtual List<string> GetHeader()
        {
            var headers = new List<string>();
            for (var column = StartColumn; CountRows() > 1 && column < CountColumns() + StartColumn; column++)
                headers.Add(GetCellValue(StartRow, column).ToString());
            return headers;
        }


        public void AddFirstColumnWithErrors(List<Error> errors, MessageErrors messageErrors)
        {
            if (!errors.Any())
                return;

            InsertNewColumnBeforeFirst(messageErrors.HeaderFirstColumnWithErrors);

            var summaryErrors = new Dictionary<int, int>();
            foreach (var error in errors)
            {
                AddError(new Error(error.Message, error.Row, StartColumn));
                if(summaryErrors.ContainsKey(error.Row))
                    summaryErrors[error.Row] ++;
                else
                    summaryErrors.Add(error.Row, 1);
            }

            foreach(var summary in summaryErrors)
                SetCellValue(summary.Key, StartColumn, $"{summary.Value} {messageErrors.TextFirstColumnWithErrors}");
        }

        public virtual ValidationResult<string> GetHeaderWithValidation<T>(WorksheetMap<T> worksheetMap,
            MessageErrors messageErrors)
        {
            var expectedHeader = worksheetMap.GetFields().Select(s => s.Name).ToList();
            var header = GetHeader();

            var validationResult = new ValidationResult<string>();

            if (header == null)
                validationResult.AddError(new Error(messageErrors.NullHeaderErrorMessage(), StartRow, StartColumn));

            foreach (var headerItem in header)
                if (headerItem != messageErrors.HeaderFirstColumnWithErrors && !expectedHeader.Contains(headerItem))
                    validationResult.AddError(new Error(messageErrors.InvalidItemHeader(headerItem), StartRow, header.IndexOf(headerItem)));
                else
                    validationResult.AddItem(headerItem);

            foreach (var expectedHeaderItem in expectedHeader)
                if (!header.Contains(expectedHeaderItem))
                    validationResult.AddError(new Error(messageErrors.MissingItemHeader(expectedHeaderItem), StartRow, StartColumn));

            return validationResult;
        }

        public abstract MemoryStream Write();

        public abstract void Dispose();
    }
}
