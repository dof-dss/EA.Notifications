using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using EA.Notifications.AWS.Lambda.ApiGatewayHandlers;
using EA.Notifications.AWS.Lambda.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EA.Notifications.AWS.Lambda
{
    public class NotificationApp
    {
        private readonly IPushManager _pushManager;

        public NotificationApp(IPushManager pushManager)
        {
            _pushManager = pushManager;
        }

        public async Task<APIGatewayProxyResponse> Run(APIGatewayProxyRequest request)
        {
            switch (request.HttpMethod)
            {
                case "OPTIONS":
                    return ApiGatewayResponseBuilder.Build( HttpStatusCode.OK, string.Empty);
                case "POST":
                    return await new PostApiGatewayHandler(request, _pushManager).Execute();
                default:
                    throw new NotImplementedException($"Http {request.HttpMethod} not implemented ");
            }
        }
    }
}