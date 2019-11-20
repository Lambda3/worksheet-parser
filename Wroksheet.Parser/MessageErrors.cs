namespace Worksheet.Parser
{
    public class MessageErrors
    {
        public virtual string NullHeaderErrorMessage() => "Header can´t be null";
        public virtual string MissingItemHeader(string header) => $"Missing field {header} on header";
        public virtual string InvalidItemHeader(string header) => $"Not expected field {header} on header";
        public virtual string InvalidParseValue(string header, int row, object value)
           => $"Invalid value to field {header} at row {row}: {value}";
        public virtual string InvalidItemValue(string header, int row, object value)
             => $"Error to set value to field {header} at row {row}: {value}";
        public virtual string EmptyRequiredField(string header, int row) => $"Field {header} can´t not be null at row {row}";
    }
}
