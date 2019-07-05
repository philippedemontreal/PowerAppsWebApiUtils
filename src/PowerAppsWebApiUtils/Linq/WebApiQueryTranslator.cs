using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Dynamics.CRM;
using PowerAppsWebApiUtils.Entities;

namespace PowerAppsWebApiUtils.Linq
{
    public class WebApiQueryTranslator: ExpressionVisitor 
    {
        private StringBuilder _sbMainClause;
        private StringBuilder _sbFilterClause;
        private StringBuilder _sbSelectClause;

        private ParameterExpression _row;


        private ColumnProjection _projection;
        public string Translate(Expression expression)
        {
            _sbMainClause = new StringBuilder();
            _sbFilterClause = new StringBuilder();
            _sbSelectClause = new StringBuilder();
            _row = Expression.Parameter(typeof(ProjectionRow), "row");

            Visit(expression);

            return 
                _sbMainClause.ToString() +
               (_sbSelectClause.Length == 0 ? "" : "?" + _sbSelectClause.ToString()) + 
                (_sbFilterClause.Length == 0 ? "" : (_sbSelectClause.Length == 0 ? "?" : "&") + _sbFilterClause.ToString());
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable))
            {
                switch (m.Method.Name)
                {
                    case "Where":
                        Visit(m.Arguments[0]);

                        if (_sbFilterClause.Length == 0)
                            _sbFilterClause.Append("$filter=(");
                        else
                            _sbFilterClause.Append(" and (");

                        Visit(m.Arguments[1]);
                        _sbFilterClause.Append(")");
                        return m;

                    case "Select":
                        var projection = new ColumnProjector().ProjectColumns(m.Arguments[1], _row);
                        _sbSelectClause.Append($"$select={projection.Columns}");
                        Visit(m.Arguments[0]);
                        _projection = projection;

                        return m;
                }
            }

            throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));
        }

        protected override Expression VisitConstant(ConstantExpression c) 
        {
            if (c.Value is IQueryable) 
            {
                var genericArgumentType = c.Value.GetType().GetGenericArguments()[0];
                var baseentity = Activator.CreateInstance(genericArgumentType, false) as crmbaseentity;
                _sbMainClause.Append(baseentity.EntityCollectionName);
            }
            else if (c.Value == null) 
            {
                _sbFilterClause.Append("null");
            }
            else 
            {
                switch (Type.GetTypeCode(c.Value.GetType())) 
                {
                    case TypeCode.Int32:
                        _sbFilterClause.Append((int)c.Value);
                        break;                    
                    case TypeCode.String:
                        _sbFilterClause.Append($"'{c.Value}'");
                        break;
                    case TypeCode.Object:
                        if (c.Value.GetType() != typeof(NavigationProperty))
                            throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
                        _sbFilterClause.Append($"'{((NavigationProperty)c.Value).Id}'");
                        break;
                    default:
                        _sbFilterClause.Append(c.Value);
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
                case ExpressionType.AndAlso:
                    _sbFilterClause.Append(" and ");
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _sbFilterClause.Append(" or ");
                    break;

                case ExpressionType.Equal:
                    _sbFilterClause.Append(" eq ");
                    break;

                case ExpressionType.NotEqual:
                    _sbFilterClause.Append(" not ");
                    break;

                case ExpressionType.LessThan:
                    _sbFilterClause.Append(" lt ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    _sbFilterClause.Append(" le ");
                    break;

                case ExpressionType.GreaterThan:
                    _sbFilterClause.Append(" gt ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    _sbFilterClause.Append(" ge ");
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
                var attr = m.Member.GetCustomAttribute<DataMemberAttribute>();
                if (attr == null)
                    throw new NotSupportedException(string.Format("The member '{0}' has no attribute of type DataMember which is not supported", m.Member.Name));

                if ((m.Member as PropertyInfo).PropertyType == typeof(NavigationProperty))
                {
                        _sbFilterClause.Append($"_{attr.Name}_value");
                }
                else
                {
                    _sbFilterClause.Append(attr.Name);
                }
                return m;
            }

            throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));
        }
    }
}