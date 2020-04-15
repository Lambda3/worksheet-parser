using System.Collections.Generic;
using System.Linq;

namespace Worksheet.Parser
{
    public class ValidationResult
    {
        public List<Error> Errors { get; private set; }

        public ValidationResult() => Errors = new List<Error>();

        public ValidationResult(Error error) : this() => AddError(error);

        public void AddError(Error error) => Errors.Add(error);
        public void AddError(List<Error> errors) => Errors.AddRange(errors);
        public void AddResult(ValidationResult validation) => AddError(validation.Errors);

        public bool IsSuccess => !Errors.Any();
    }

    public class ValidationResult<T> : ValidationResult
    {
        public List<T> Itens { get; private set; }

        public ValidationResult() => Itens = new List<T>();

        public void AddItem(T item) => Itens.Add(item);
        public void AddItem(List<T> itens) => Itens.AddRange(itens);

        public void AddResult(ValidationResult<T> validation)
        {
            if (validation.IsSuccess)
                AddItem(validation.Itens);
            else
                AddError(validation.Errors);
        }
    }
}
