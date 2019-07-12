using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Dynamics.CRM;
using PowerAppsWebApiUtils.Repositories;


namespace PowerAppsWebApiUtils.Linq
{
    public class WebApiQueryProvider: QueryProvider
    {

        private readonly IServiceProvider _serviceProvider;
   
        public WebApiQueryProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;     
        }
    
        private string Translate(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            return new WebApiQueryTranslator().Translate(expression);
        }
            
        public override T Execute<T>(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression != null && methodCallExpression.Method.Name == "FirstOrDefault")
            {
                methodCallExpression = methodCallExpression.Arguments[0] as MethodCallExpression;
            }
            
            Type elementType = null;
            if (methodCallExpression != null)
            {
                var mc = methodCallExpression;
                do 
                {
                    if (mc == null || methodCallExpression.Arguments.Count == 0)
                        break;
                    elementType = TypeSystem.GetElementType(mc.Arguments[0].Type);
                    if (typeof(crmbaseentity).IsAssignableFrom(elementType))
                        break;
                    mc = mc.Arguments[0] as MethodCallExpression;

                } while (true);
                //methodCallExpression = mc;
            }
            else    
                elementType = TypeSystem.GetElementType(expression.Type);

            var webapi = _serviceProvider.GetService(typeof(GenericRepository<>).MakeGenericType(elementType));
                
            var genericRepositoryType = webapi.GetType();    
            var command = Translate(methodCallExpression ?? expression);

            try
            {
                
                var methodCall = genericRepositoryType.GetMethod("RetrieveMultiple").Invoke(webapi, new object[]{ command } );
                var result = ((IEnumerable<object>)methodCall.GetType().GetProperty("Result").GetGetMethod().Invoke(methodCall, null)).FirstOrDefault();

                if (methodCallExpression == null)
                    return (T)result;
                
                if (methodCallExpression.Method.Name == "Select")
                {
                    var operand = (methodCallExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression;
                    if (operand?.ReturnType != elementType)
                    {
                        var fn = operand.Compile();
                        result = fn.DynamicInvoke(result);
                    }
                }

                return (T)result;               
            }
            finally
            {
                genericRepositoryType.GetMethod("Dispose").Invoke(webapi, null);                
            }
        }

        public override string GetQueryText(Expression expression)
            => Translate(expression);

        public override object Execute(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            Type elementType = null;
            if (methodCallExpression != null && methodCallExpression.Arguments.Count > 0)
                elementType = TypeSystem.GetElementType(methodCallExpression.Arguments[0].Type);
            else    
                elementType = TypeSystem.GetElementType(expression.Type);

            var webapi = _serviceProvider.GetService(typeof(GenericRepository<>).MakeGenericType(elementType));

            var genericRepositoryType = webapi.GetType();
            var command = Translate(expression);

            try
            {
                var methodCall = genericRepositoryType.GetMethod("RetrieveMultiple").Invoke(webapi, new object[]{ command } );
                var result = methodCall.GetType().GetProperty("Result").GetGetMethod().Invoke(methodCall, null);

                if (methodCallExpression == null)
                    return result;
                
                if (methodCallExpression.Method.Name == "Select")
                {
                    var operand = (methodCallExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression;
                    if (operand?.ReturnType != elementType)
                    {
                        var resultWithLambdaInvoked = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(operand.ReturnType));
                        var fn = operand.Compile();
                        foreach (var item in (IEnumerable)result)
                        {
                            resultWithLambdaInvoked.Add(fn.DynamicInvoke(item));
                        }
                        result = resultWithLambdaInvoked;
                    }
                }

                return result;               
            }
            finally
            {
                genericRepositoryType.GetMethod("Dispose").Invoke(webapi, null);                
            }
        }        
    }
}