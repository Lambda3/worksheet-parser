using System;
using System.Linq.Expressions;

namespace Worksheet.Parser
{
    public static class ExpressionExtensions
    {
        public static MemberExpression GetBody<T>(this Expression<Func<T, object>> expression)
        {
            if (!(expression.Body is MemberExpression body))
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            return body;
        }
    }
}
