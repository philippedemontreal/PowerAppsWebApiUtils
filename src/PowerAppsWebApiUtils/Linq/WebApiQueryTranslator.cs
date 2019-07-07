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
                        var p1 = new ColumnProjector().ProjectColumns(m.Arguments[1], _row);
                        _sbSelectClause.Append($"$select={p1.Columns}");
                        Visit(m.Arguments[0]);
                        _projection = p1;

                        return m;
                    case "FirstOrDefault":
                        _sbSelectClause.Append($"$top=1");
                        var select = m.Arguments.Where(p => p.NodeType == ExpressionType.Call && ((MethodCallExpression)p).Method.Name == "Select").FirstOrDefault();
                        if (select != null)
                        {
                            var p2 = new ColumnProjector().ProjectColumns(select, _row);
                            
                            if (p2.Columns.Length > 0)
                                _sbSelectClause.Append($"&$select={p2.Columns}");
                            _projection = p2;
                        }

                        Visit(m.Arguments[0]);
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
                        if (c.Value.GetType() == typeof(Guid))
                        {
                            _sbFilterClause.Append($"'{c.Value}'");
                        }
                        else if (c.Value.GetType() == typeof(NavigationProperty))
                        {
                            _sbFilterClause.Append($"'{((NavigationProperty)c.Value).Id}'");
                        }
                        else
                            throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));
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
                // Case of Id (overriden attribute), propertyInfo is from the base class crmbaseentity so we do not get the DataMemberAttribute. 
                //Lets find the DataMemberAttribute in the overriding class
                if (attr == null) 
                {
                    var property = m.Expression.Type.GetProperty(m.Member.Name);
                    attr = property.GetCustomAttribute<DataMemberAttribute>();
                }

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