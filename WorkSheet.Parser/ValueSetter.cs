namespace Worksheet.Parser {
    public class ValueSetter
    {
        private readonly Converter converter;

        public ValueSetter(Converter converter)
        {
            this.converter = converter;
        }

        public ValidationResult SetValue<T>(Field<T> field, T item, object value, int row, int column, MessageErrors messageErrors)
        {
            var validationColumn = new ValidationResult();

            try
            {
                value = converter.Convert(field.PropertyInfo.Type, value);
            }
            catch
            {
                var error = new Error(messageErrors.InvalidParseValue(field.Name), row, column);
                validationColumn.AddError(error);
            }

            if (!validationColumn.IsSuccess)
                return validationColumn;

            validationColumn.AddResult(IsValueValid(field, item, value, row, column, messageErrors));

            try
            {
                if (validationColumn.IsSuccess)
                    SetValue(field, item, field.PropertyInfo.Name, value);
            }
            catch
            {
                var error = new Error(messageErrors.InvalidItemValue(field.Name, value), row, column);
                validationColumn.AddError(error);
            }

            return validationColumn;
        }

        public ValidationResult IsValueValid<T>(Field<T> field, T item, object value, int row, int column, MessageErrors messageErrors)
        {
            var validationResult = new ValidationResult();

            if (!IsRequiredFieldFilled(field, value))
                validationResult.AddError(new Error(messageErrors.EmptyRequiredField(field.Name), row, column));

            foreach (var validation in field.GetValidations())
                validationResult.AddResult(validation.IsValid(item, value, row, column));

            return validationResult;
        }

        private bool IsRequiredFieldFilled<T>(Field<T> field, object value) =>
            !field.Required || (value != null && !string.IsNullOrEmpty(value.ToString()));

        public virtual void SetValue<T>(Field<T> field, T item, string propertyName, object convertedValue)
        {
            if (field.HasCustomConverter())
                SetValue(field, item, convertedValue);
            else
                SetValue<T>(item, propertyName, convertedValue);
        }

        private void SetValue<T>(object item, string propertyName, object value) =>
               typeof(T).GetProperty(propertyName).SetValue(item, value, null);

        private void SetValue<T>(Field<T> field, T item, object value)
           => field.Converter(item, value);
    }
}
