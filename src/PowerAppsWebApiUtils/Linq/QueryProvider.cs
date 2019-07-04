using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PowerAppsWebApiUtils.Linq
{
    public abstract class QueryProvider: IQueryProvider
    {
        protected QueryProvider() 
        {
        }
               
        public IQueryable<T> CreateQuery<T>(Expression expression)
            =>new Query<T>(this, expression);

        public IQueryable CreateQuery(Expression expression) 
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try 
            {
                return (IQueryable)Activator.CreateInstance(typeof(Query<>).MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException tie) 
            {
                throw tie.InnerException;
            }
        }

        T IQueryProvider.Execute<T>(Expression expression) 
            => Execute<T>(expression);

        object IQueryProvider.Execute(Expression expression) 
            => Execute(expression);

        public abstract string GetQueryText(Expression expression);

        public abstract object Execute(Expression expression);
        public abstract T Execute<T>(Expression expression);

    }
}