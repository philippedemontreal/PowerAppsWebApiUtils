using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using PowerAppsWebApiUtils.Entities;

namespace PowerAppsWebApiUtils.Linq
{
    public abstract class ProjectionRow 
    {
        public abstract object GetValue(int index);
    }

    internal class ColumnProjection {
        internal string Columns;
        internal Expression Selector;
    }
    
    internal class ColumnProjector : ExpressionVisitor 
    {
        private StringBuilder _sb;
        private int _iColumn;
        private ParameterExpression _row;
        private Type _elementType;
        private static MethodInfo miGetValue => 
            typeof(ProjectionRow).GetMethod("GetValue");
    
        internal ColumnProjector() 
        {
        }
    
        internal ColumnProjection ProjectColumns(Expression expression, ParameterExpression row, Type elementType = null) 
        {
            _sb = new StringBuilder();
            _row = row;
            _elementType = elementType;
            var selector = Visit(expression);
            return new ColumnProjection { Columns = _sb.ToString(), Selector = selector };
        }
    
        protected override Expression VisitMember(MemberExpression m) 
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter) 
            {
                if (_sb.Length > 0) 
                    _sb.Append(",");
                    
                var attr = (m.Member as PropertyInfo).GetCustomAttribute<DataMemberAttribute>();
                // Case of Id (overriden attribute), propertyInfo is from the base class crmbaseentity so we do not get the DataMemberAttribute. 
                //Lets find the DataMemberAttribute in the overriding class
                if (attr == null) 
                {
                    var property = (_elementType ?? m.Expression.Type).GetProperty(m.Member.Name);
                    attr = property.GetCustomAttribute<DataMemberAttribute>();
                }

                if (attr == null)
                    throw new NotSupportedException(string.Format("The member '{0}' has no attribute of type DataMember which is not supported", m.Member.Name));

                if ((m.Member as PropertyInfo).PropertyType == typeof(NavigationProperty))
                {
                        _sb.Append($"_{attr.Name}_value");
                }
                else
                {
                    _sb.Append(attr.Name);
                }

                
                return Expression.Convert(Expression.Call(_row, miGetValue, Expression.Constant(_iColumn++)), m.Type);
            }
            else 
            {
                return base.VisitMember(m);
            }
        }
    }
}