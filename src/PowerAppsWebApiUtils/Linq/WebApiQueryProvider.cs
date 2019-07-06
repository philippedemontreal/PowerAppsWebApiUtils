using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using PowerAppsWebApiUtils.Repositories;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Linq
{
    public class WebApiQueryProvider: QueryProvider
    {

        private readonly AuthenticationMessageHandler _authenticationMessageHandler;
   
        public WebApiQueryProvider(AuthenticationMessageHandler authenticationMessageHandler)
        {
            _authenticationMessageHandler = authenticationMessageHandler;            
        }
    
        private string Translate(Expression expression)
        {
            expression = Evaluator.PartialEval(expression);
            return new WebApiQueryTranslator().Translate(expression);
        }
            
        public override T Execute<T>(Expression expression)
        {
            var webapi = new GenericRepository<T>(_authenticationMessageHandler);
            var command = Translate(expression);
            return webapi.Retrieve(command).GetAwaiter().GetResult();
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

            var webapi = Activator.CreateInstance(
                typeof(GenericRepository<>).MakeGenericType(elementType),            
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new object[] { _authenticationMessageHandler },
                null);

            var genericRepositoryType = webapi.GetType();
            var command = Translate(expression);

            try
            {
                var methodCall = genericRepositoryType.GetMethod("RetrieveMultiple").Invoke(webapi, new object[]{ command } );
                var result = methodCall.GetType().GetProperty("Result").GetGetMethod().Invoke(methodCall, null);
                
                if (methodCallExpression?.Method?.Name == "Select")
                {
                    var operand = (methodCallExpression.Arguments[1] as UnaryExpression).Operand as LambdaExpression;
                    if (operand?.ReturnType != elementType)
                    {
                        if (operand.ReturnType.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).FirstOrDefault() != null &&
                            operand.ReturnType.FullName.Contains("AnonymousType"))
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