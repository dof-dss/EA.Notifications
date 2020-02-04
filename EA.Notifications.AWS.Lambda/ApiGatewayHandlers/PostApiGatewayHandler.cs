using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using EA.Notifications.AWS.Lambda.Contracts;
using EA.Notifications.AWS.Lambda.Models;
using Newtonsoft.Json;

namespace EA.Notifications.AWS.Lambda.ApiGatewayHandlers
{
    public class PostApiGatewayHandler: ApiGatewayHandler
    {
        public PostApiGatewayHandler(APIGatewayProxyRequest request, IPushManager pushManager) : base(request, pushManager)
        {
            GatewayFunctionMapper.Add("/push/subscribe", SubscribePush);
            GatewayFunctionMapper.Add("/push", Push);
        }

        private async Task<APIGatewayProxyResponse> Push()
        {
            return ApiGatewayResponseBuilder.Build(HttpStatusCode.Created,
                await PushManager.SendPush(Request.Body));
        }

        private async Task<APIGatewayProxyResponse> SubscribePush()
        {
            var pushSubscriptionModel = JsonConvert.DeserializeObject<PushSubscriptionModel>(Request.Body);
            return ApiGatewayResponseBuilder.Build(HttpStatusCode.Created,
                await PushManager.PushSubscribe(pushSubscriptionModel));
        }
    }
}
