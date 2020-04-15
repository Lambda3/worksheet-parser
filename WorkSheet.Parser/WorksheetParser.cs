using System.Linq;

namespace Worksheet.Parser
{
    public class WorksheetParser<T> where T : class, new()
    {
        private readonly ValueSetter valueSetter;
        private readonly WorksheetMap<T> worksheetMap;
        private readonly MessageErrors messageErrors;

        public WorksheetParser(ValueSetter valueSetter,
            WorksheetMap<T> worksheetMap,
            MessageErrors messageErrors)
            => (this.valueSetter, this.worksheetMap, this.messageErrors)
            = (valueSetter, worksheetMap, messageErrors);

        public virtual ValidationResult<T> Parse(WorksheetReader worksheet)
        {
            var validationResult = new ValidationResult<T>();

            var fields = worksheetMap.GetFields().ToDictionary(x => x.Name);
            var headerResult = worksheet.GetHeaderWithValidation(worksheetMap, messageErrors);
            validationResult.AddResult(headerResult);

            if (!validationResult.IsSuccess)
                return validationResult;

            var header = headerResult.Itens;
            for (var row = worksheet.StartRowItens; row < worksheet.CountRows() + worksheet.StartRow; row++)
            {
                var item = new T();
                var validationRow = new ValidationResult<T>();
                for (var column = worksheet.StartColumn; column < worksheet.CountColumns() + worksheet.StartColumn; column++)
                {
                    var headerName = header[column - worksheet.StartColumn];
                    if (headerName == messageErrors.HeaderFirstColumnWithErrors)
                        continue;
                    var field = fields[headerName];

                    if (field.ShouldBeIgnored)
                        continue;

                    var value = worksheet.GetCellValue(row, column);
                    var validationColumn = valueSetter.SetValue(field, item, value, row, column, messageErrors);
                    validationRow.AddResult(validationColumn);
                }
                validationRow.AddItem(item);
                validationResult.AddResult(validationRow);
            }

            return validationResult;
        }
    }
}
