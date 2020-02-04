using Amazon.DynamoDBv2.DataModel;

namespace EA.Notifications.AWS.Lambda.Models
{
    [DynamoDBTable("pushSubscribers-beta")]
    public class PushSubscriptionTable
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string PushSubscription { get; set; }
    }
}