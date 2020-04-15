using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Worksheet.Parser
{
    public abstract class WorksheetMap<T>
    {
        private readonly List<Field<T>> fields = new List<Field<T>>();

        protected Field<T> Map(Expression<Func<T, object>> property)
        {
            var campo = new Field<T>(property);
            fields.Add(campo);
            return campo;
        }

        public List<Field<T>> GetFields() => new List<Field<T>>(fields);
    }
}
