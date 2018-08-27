using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Lib.Utility
{
    public class LambdaHelper<T>
    {
        public static PropertyInfo GetProperty<TValue>(Expression<Func<T, TValue>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (PropertyInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static string GetPropertyName<TValue>(Expression<Func<T, TValue>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)body).Member.Name;
                default:
                    throw new InvalidOperationException();
            }
        }

        public static string GetPropertyName<TValue>(Expression<Func<T, TValue>> selector, bool includeClassInName)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (includeClassInName ? ((MemberExpression)body).Member.DeclaringType.Name : "") + ((MemberExpression)body).Member.Name;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
