namespace EA.Notifications.AWS.Lambda.Models
{
    public class PushSubscriptionModel
    {
        public string Endpoint { get; set; }
        public int? ExpirationTime { get; set; }
        public Keys Keys { get; set; }
    }

    public class Keys
    {
        public string P256dh { get; set; }
        public string Auth { get; set; }
    }
}