namespace Worksheet.Parser
{
    public class MessageErrors
    {
        public virtual string NullHeaderErrorMessage() => "Header can´t be null";
        public virtual string MissingItemHeader(string header) => $"Missing field {header} on header";
        public virtual string InvalidItemHeader(string header) => $"Not expected field {header} on header";
        public virtual string InvalidParseValue(string header)
           => $"Invalid value to field {header}";
        public virtual string InvalidItemValue(string header, object value)
             => $"Error to set value to field {header}: {value}";
        public virtual string EmptyRequiredField(string header) => $"Field {header} can´t not be null";

        public virtual string TextFirstColumnWithErrors => "error (s)";
        public virtual string HeaderFirstColumnWithErrors => "Error validations";
    }
}
