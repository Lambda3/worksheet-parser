namespace Worksheet.Parser
{
    public class ValueSetter
    {
        private readonly Converter converter;

        public ValueSetter(Converter converter)
        {
            this.converter = converter;
        }

        public ValidationResult SetValue<T>(Field<T> field, T item, object value, int row, MessageErrors messageErrors)
        {
            var validationColumn = new ValidationResult();

            try
            {
                value = converter.Convert(field.PropertyInfo.Type, value);
            }
            catch
            {
                validationColumn.AddError(messageErrors.InvalidParseValue(field.Name, row, value));
            }

            if (!validationColumn.IsSuccess)
                return validationColumn;

            validationColumn.AddResult(IsValueValid(field, item, value, row, messageErrors));

            try
            {
                if (validationColumn.IsSuccess)
                    SetValue(field, item, field.PropertyInfo.Name, value);
            }
            catch
            {
                validationColumn.AddError(messageErrors.InvalidItemValue(field.Name, row, value));
            }

            return validationColumn;
        }

        public ValidationResult IsValueValid<T>(Field<T> field, T item, object value, int row, MessageErrors messageErrors)
        {
            var validationResult = new ValidationResult();

            if (!IsRequiredFieldFilled(field, value))
                validationResult.AddError(messageErrors.EmptyRequiredField(field.Name, row));

            foreach (var validation in field.GetValidations())
                validationResult.AddResult(validation.IsValid(item, value));

            return validationResult;
        }

        private bool IsRequiredFieldFilled<T>(Field<T> field, object value) => !field.Required || value != null;

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
