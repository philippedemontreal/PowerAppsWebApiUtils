using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Dynamics.CRM;

namespace PowerAppsWebApiUtils.Linq
{
    public class WebApiQueryTranslator: ExpressionVisitor 
    {
        private StringBuilder _sb;
        public string Translate(Expression expression)
        {
            _sb = new StringBuilder();
            Visit(expression);
            return _sb.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where") 
            {
                Visit(m.Arguments[0]);
                _sb.Append("?$filter=");
                Visit(m.Arguments[1]);
                return m;
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitConstant(ConstantExpression c) 
        {
            if (c.Value is IQueryable) 
            {
                var genericArgumentType = c.Value.GetType().GetGenericArguments()[0];
                var baseentity = Activator.CreateInstance(genericArgumentType, false) as crmbaseentity;
                _sb.Append(baseentity.EntityCollectionName);
            }
            else if (c.Value == null) 
            {
                _sb.Append("null");
            }
            else 
            {
                switch (Type.GetTypeCode(c.Value.GetType())) {
                    // case TypeCode.Boolean:
                    //     _sb.Append(((bool)c.Value) ? 1 : 0);
                    //     break;
                    case TypeCode.String:
                        _sb.Append($"'{c.Value}'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                    default:
                        _sb.Append(c.Value);
                        break;
                }
            }

            return c;
        }

        protected override Expression VisitBinary(BinaryExpression b) 
        {
            Visit(b.Left);
            switch (b.NodeType) 
            {
                case ExpressionType.And:
                    _sb.Append(" and ");
                    break;

                case ExpressionType.Or:
                    _sb.Append(" or ");
                    break;

                case ExpressionType.Equal:
                    _sb.Append(" eq ");
                    break;

                case ExpressionType.NotEqual:
                    _sb.Append(" not ");
                    break;

                case ExpressionType.LessThan:
                    _sb.Append(" lt ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    _sb.Append(" le ");
                    break;

                case ExpressionType.GreaterThan:
                    _sb.Append(" gt ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    _sb.Append(" ge ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", b.NodeType));
            }

            Visit(b.Right);
            return b;
        }

            protected override Expression VisitMember(MemberExpression m) 
            {
                if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter) 
                {
                    var attr = m.Member.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault() as DataMemberAttribute;
                    if (attr == null)
                        throw new NotSupportedException(string.Format("The member '{0}' has no attribute of type DataMember which is not supported", m.Member.Name));

                    _sb.Append(attr.Name);
                    return m;
                }

                throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
            }
    }
}