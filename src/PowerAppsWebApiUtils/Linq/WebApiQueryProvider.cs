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
    
        private string Translate(Expression expression, Type elementType = null)
        {
            expression = Evaluator.PartialEval(expression);
            return new WebApiQueryTranslator().Translate(expression, elementType);
        }
            
        public override T Execute<T>(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression != null && methodCallExpression.Method.Name == "FirstOrDefault")
            {
                methodCallExpression = methodCallExpression.Arguments[0] as MethodCallExpression;
            }
            var elementType = GetElementTypeFromExpression(methodCallExpression) ?? TypeSystem.GetElementType(expression.Type);          

            var webapi = _serviceProvider.GetService(typeof(GenericRepository<>).MakeGenericType(elementType));
                
            var genericRepositoryType = webapi.GetType();    
            var command = Translate(methodCallExpression ?? expression, elementType);

            try
            {
                
                var methodCall = genericRepositoryType.GetMethod("RetrieveMultiple").Invoke(webapi, new object[]{ command } );
                var result = ((IEnumerable<object>)methodCall.GetType().GetProperty("Result").GetGetMethod().Invoke(methodCall, null)).FirstOrDefault();

                if (result == null || methodCallExpression == null)
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

        private static Type GetElementTypeFromExpression(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            Type elementType = null;
            if (methodCallExpression != null && methodCallExpression.Arguments.Count > 0)
            {
                var expr = methodCallExpression.Arguments[0];
                do
                {
                    elementType = TypeSystem.GetElementType(expr.Type);
                    if (typeof(crmbaseentity).IsAssignableFrom(elementType))
                        break; 
                    if (!(methodCallExpression is MethodCallExpression) || ((MethodCallExpression)expr).Arguments.Count == 0)
                        break;
                    expr =  ((MethodCallExpression)expr).Arguments[0];
                } while (true);
            }

            return elementType;
        }
        public override object Execute(Expression expression)
        {
            var methodCallExpression = expression as MethodCallExpression;
            var elementType = GetElementTypeFromExpression(expression) ?? TypeSystem.GetElementType(expression.Type);          

            if (!typeof(crmbaseentity).IsAssignableFrom(elementType))
                throw new NotSupportedException($"Type '{elementType.FullName}' is not supported with GenericRepository<>.");

            var webapi = _serviceProvider.GetService(typeof(GenericRepository<>).MakeGenericType(elementType));

            var genericRepositoryType = webapi.GetType();
            var command = Translate(expression, elementType);

            try
            {
                var methodCall = genericRepositoryType.GetMethod("RetrieveMultiple").Invoke(webapi, new object[]{ command } );
                var result = methodCall.GetType().GetProperty("Result").GetGetMethod().Invoke(methodCall, null);

                if (methodCallExpression == null)
                    return result;
                 
                if (methodCallExpression.Method.Name == "OrderBy" || methodCallExpression.Method.Name == "OrderByDescending")
                {
                    var mc = methodCallExpression;
                    while (mc != null)
                    {
                        if (mc.Arguments.Count == 0)
                            break;
                        mc = mc.Arguments[0] as MethodCallExpression;
                            if (mc?.Method.Name == "Select")
                            {
                                methodCallExpression = mc;
                                break;
                            }
                    }
                }

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