using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Worksheet.Parser
{
    public class Field<T>
    {
        private readonly List<Validation> validations;
        public string Name { get; private set; }
        public bool Required { get; private set; }
        public bool ShouldBeIgnored { get; private set; }
        public Action<T, object> Converter { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }

        public Field(Expression<Func<T, object>> property)
        {
            validations = new List<Validation>();
            PropertyInfo = PropertyInfo.Create(property);
        }

        public Field<T> ToFieldName(string nome)
        {
            Name = nome;
            return this;
        }

        public Field<T> ToRequiredField(string nome)
        {
            Required = true;
            Name = nome;
            return this;
        }

        public Field<T> WithCustomConverter(Action<T, object> converter)
        {
            Converter = converter;
            return this;
        }

        public Field<T> WithValidation(Validation validation)
        {
            validations.Add(validation);
            return this;
        }

        public Field<T> Ignored()
        {
            ShouldBeIgnored = true;
            return this;
        }

        public List<Validation> GetValidations() => new List<Validation>(validations);

        public bool HasCustomConverter() => Converter != null;
    }
}
