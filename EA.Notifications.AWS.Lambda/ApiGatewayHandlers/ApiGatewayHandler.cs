using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using EA.Notifications.AWS.Lambda.Contracts;
using EA.Notifications.AWS.Lambda.Push;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EA.Notifications.AWS.Lambda.ApiGatewayHandlers
{
    public class ApiGatewayHandler
    {
        protected APIGatewayProxyRequest Request { get; set; }
        protected IDictionary<string, Func<Task<APIGatewayProxyResponse>>> GatewayFunctionMapper;
        protected IPushManager PushManager { get; }

        public virtual async Task<APIGatewayProxyResponse> Execute()
        {
            return GatewayFunctionMapper.ContainsKey(Request.Resource) ?
                await GatewayFunctionMapper[Request.Resource]()
                : throw new NotImplementedException($"Http {Request.Resource} not implemented ");
        }

        protected ApiGatewayHandler(APIGatewayProxyRequest request, IPushManager pushManager)
        {
            Request = request;
            GatewayFunctionMapper = new Dictionary<string, Func<Task<APIGatewayProxyResponse>>>();
            PushManager = pushManager;
        }

    }
}
