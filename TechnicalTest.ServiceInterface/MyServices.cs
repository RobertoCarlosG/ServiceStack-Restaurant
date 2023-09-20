using System;
using ServiceStack;
using TechnicalTest.ServiceModel;

namespace TechnicalTest.ServiceInterface
{
    public class MyServices : Service
    {
        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }
    }
}
