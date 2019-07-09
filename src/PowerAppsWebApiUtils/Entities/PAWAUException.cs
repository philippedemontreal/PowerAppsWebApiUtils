using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using Microsoft.Dynamics.CRM;

namespace PowerAppsWebApiUtils.Entities
{
        [System.Serializable]   
        public class PAWAUException : System.Exception
        {
            public HttpStatusCode StatusCode { get; private set; }
            public PAWAUException() { }
            public PAWAUException(HttpStatusCode statuscode, string message) : base(message) 
                => StatusCode = statuscode;

            public PAWAUException(string message) : base(message) { }
            public PAWAUException(string message, System.Exception inner) : base(message, inner) { }
            protected PAWAUException(
                System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

}


