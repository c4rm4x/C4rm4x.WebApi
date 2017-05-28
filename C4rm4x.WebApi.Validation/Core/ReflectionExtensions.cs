#region Using

using C4rm4x.Tools.Utilities;
using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace C4rm4x.WebApi.Validation.Core
{
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// Gets a MemberInfo from a member expression.
        /// </summary>
        public static MemberInfo GetMember<T, TProperty>(
            this Expression<Func<T, TProperty>> expression)
        {
            var memberExp = RemoveUnary(expression.Body);

            if (memberExp.IsNull())
                return null;

            return memberExp.Member;
        }

        private static MemberExpression RemoveUnary(Expression toUnwrap)
        {
            if (toUnwrap is UnaryExpression)
                return ((UnaryExpression)toUnwrap).Operand as MemberExpression;

            return toUnwrap as MemberExpression;
        }
    }
}
