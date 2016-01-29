#region Using

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace C4rm4x.WebApi.Validation.Core
{
    internal class PropertyChain
    {
        private readonly List<string> _memberNames = new List<string>();

        public PropertyChain(IEnumerable<string> memberNames)
        {
            _memberNames.AddRange(memberNames);
        }

        public static PropertyChain FromExpression(LambdaExpression expression)
        {
            var memberNames = new Stack<string>();

            var getMemberExpression = new Func<Expression, MemberExpression>(toUnwrap =>
            {
                if (toUnwrap is UnaryExpression)
                    return ((UnaryExpression)toUnwrap).Operand as MemberExpression;

                return toUnwrap as MemberExpression;
            });

            var memberExpExpression = getMemberExpression(expression.Body);

            while (memberExpExpression != null)
            {
                memberNames.Push(memberExpExpression.Member.Name);
                memberExpExpression = getMemberExpression(memberExpExpression.Expression);
            }

            return new PropertyChain(memberNames);
        }

        public override string ToString()
        {
            return string.Join(".", _memberNames.ToArray());
        }
    }
}
