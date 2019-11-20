namespace Worksheet.Parser
{
    public abstract class Validation
    {
        public abstract ValidationResult IsValid<T>(T source, object value);
    }
}
