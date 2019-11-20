namespace Worksheet.Parser.Sample
{
    public class RangeValidation : Validation
    {
        private const string Error = "Invalid Range";

        public override ValidationResult IsValid<T>(T source, object value)
        {
            var number = (decimal)value;
            return number > 100 && number < 1000000 ? new ValidationResult() : new ValidationResult(Error);
        }
    }
}
