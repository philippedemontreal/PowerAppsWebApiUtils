using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Dynamics.CRM;
using PowerAppsWebApiUtils.Repositories;
using PowerAppsWebApiUtils.Security;

namespace PowerAppsWebApiUtils.Linq
{
    public class WebApiContext: QueryProvider
    {

        private readonly AuthenticationMessageHandler _authenticationMessageHandler;
   
        public WebApiContext(AuthenticationMessageHandler authenticationMessageHandler)
        {
            _authenticationMessageHandler = authenticationMessageHandler;            
        }
    
        private string Translate(Expression expression)
            => new WebApiQueryTranslator().Translate(expression);

            
        public override T Execute<T>(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            var webapi = new GenericRepository<T>(_authenticationMessageHandler);
            var command = Translate(expression);
            return webapi.Retrieve(command).GetAwaiter().GetResult();
        }

        public override string GetQueryText(Expression expression)
            => Translate(expression);

        public override object Execute(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            var command = Translate(expression);

            var webapi = Activator.CreateInstance(
                typeof(GenericRepository<>).MakeGenericType(elementType),            
                BindingFlags.Instance | BindingFlags.Public,
                null,
                new object[] { _authenticationMessageHandler },
                null);

            var genericRepositoryType = webapi.GetType();

            try
            {
                var methodCall = genericRepositoryType.GetMethod("RetrieveMultiple").Invoke(webapi, new object[]{ command } );
                var result = methodCall.GetType().GetProperty("Result").GetGetMethod().Invoke(methodCall, null);
                return result;
                
            }
            finally
            {
                genericRepositoryType.GetMethod("Dispose").Invoke(webapi, null);                
            }
        }        
    }
}