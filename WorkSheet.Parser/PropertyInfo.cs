using System;
using System.Linq.Expressions;

namespace Worksheet.Parser
{
    public class PropertyInfo
    {
        public string Name { get; }
        public Type Type { get; }

        protected PropertyInfo(string name, Type type) => (Name, Type) = (name, type);

        public static PropertyInfo Create<T>(Expression<Func<T, object>> property)
        {
            var body = property.GetBody();
            var propertyName = body.Member.Name;
            var propertytype = Type.GetType(body.Type.FullName);
            return new PropertyInfo(propertyName, propertytype);
        }

    }
}
