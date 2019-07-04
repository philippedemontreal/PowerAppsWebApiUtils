using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PowerAppsWebApiUtils.Linq
{
    public class Query<T> : IOrderedQueryable<T>
    {
         private readonly QueryProvider _provider;

        private readonly Expression _expression;

        public Query(QueryProvider provider) 
        {
            if (provider == null) 
                throw new ArgumentNullException(nameof(provider));

            _provider = provider;
            _expression = Expression.Constant(this);
        }

        public Query(QueryProvider provider, Expression expression) {

            if (provider == null) 
                throw new ArgumentNullException(nameof(provider));

            if (expression == null) 
                throw new ArgumentNullException(nameof(expression));


            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
                throw new ArgumentOutOfRangeException(nameof(expression));


            _provider = provider;
            _expression = expression ?? Expression.Constant(this);
        }

        public Type ElementType 
            => typeof(T); 

        public Expression Expression 
            => _expression;

        public IQueryProvider Provider 
            => _provider;


        public IEnumerator<T> GetEnumerator() 
            => ((IEnumerable<T>)Provider.Execute(_expression)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ((IEnumerable)Provider.Execute(_expression)).GetEnumerator();

        // public override string ToString() 
        //     => Provider.GetQueryText(_expression);
    }
}