using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using EA.Notifications.AWS.Lambda.Contracts;
using EA.Notifications.AWS.Lambda.Models;
using Newtonsoft.Json;
using WebPush;

namespace EA.Notifications.AWS.Lambda.Push
{
    public class PushManager: IPushManager
    {
        public IEnumerable<ScanCondition> Conditions { get; set; } = new List<ScanCondition>();

        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly WebPushClient _webPushClient;

        public PushManager(IDynamoDBContext dynamoDbContext, WebPushClient webPushClient)
        {
            _dynamoDbContext = dynamoDbContext;
            _webPushClient = webPushClient;
        }

        public async Task<PushSubscriptionTable> PushSubscribe(PushSubscriptionModel pushSubscriptionModel)
        {
            var item = new PushSubscriptionTable
            {
                Id = Guid.NewGuid().ToString(),
                PushSubscription = JsonConvert.SerializeObject(pushSubscriptionModel)
            };
            await _dynamoDbContext.SaveAsync(item);
            return item;
        }

        public async Task<string> SendPush(string payload)
        {
            var allSubscriptions =
                await _dynamoDbContext.ScanAsync<PushSubscriptionTable>(Conditions).GetRemainingAsync();

            foreach (var subscription in allSubscriptions)
                await SendNotification(subscription, payload);

            return payload;
        }

        private async Task SendNotification(PushSubscriptionTable subscription, string payload)
        {
            try
            {
                var pushNotificationModel =
                    JsonConvert.DeserializeObject<PushSubscriptionModel>(subscription.PushSubscription);
                var pushSubscription = new PushSubscription(pushNotificationModel.Endpoint, pushNotificationModel.Keys.P256dh, pushNotificationModel.Keys.Auth);
                
                await _webPushClient.SendNotificationAsync(pushSubscription, payload);
            }
            catch (WebPushException ex)
            {
                //TODO Handle exceptions with more elegance
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                    case (HttpStatusCode)429:
                    case HttpStatusCode.RequestEntityTooLarge:
                        throw;
                    case HttpStatusCode.NotFound:
                    case HttpStatusCode.Gone:
                        // Swallow exception
                        break;
                    default:
                        throw;
                }
            }
        }
    }
}
