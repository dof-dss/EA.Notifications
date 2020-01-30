using Amazon;

namespace EA.Notifications
{
    public static class AppConstants
    {
        /// <summary>
        /// Notification channels are used on Android devices starting with "Oreo"
        /// </summary>
        public static string NotificationChannelName { get; set; } = "Enterprise Architecture Channel";

        /// <summary>
        /// This is the name of your Azure Notification Hub, found in your Azure portal.
        /// </summary>
        public static string NotificationHubName { get; set; } = "EANotifications";

        /// <summary>
        /// This is the "DefaultListenSharedAccessSignature" connection string, which is
        /// found in your Azure Notification Hub portal under "Access Policies".
        /// 
        /// You should always use the ListenShared connection string. Do not use the
        /// FullShared connection string in a client application.
        /// </summary>
        public static string ListenConnectionString { get; set; } = "Endpoint=sb://eaarchitecture.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=097fzCudYqZ1PjKHLzmZQJW3SpduLohXIQHIgWH6C9k=";

        /// <summary>
        /// Tag used in log messages to easily filter the device log
        /// during development.
        /// </summary>
        public static string DebugTag { get; set; } = "EA.Notifications";

        /// <summary>
        /// The tags the device will subscribe to. These could be subjects like
        /// news, sports, and weather. Or they can be tags that identify a user
        /// across devices.
        /// </summary>
        public static string[] SubscriptionTags { get; set; } = { "default" };

        /// <summary>
        /// This is the template json that Android devices will use. Templates
        /// are defined by the device and can include multiple parameters.
        /// </summary>
        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\"}}";

        /// <summary>
        /// This is the template json that Apple devices will use. Templates
        /// are defined by the device and can include multiple parameters.
        /// </summary>
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";

        //identity pool id for cognito credentials
        public const string IdentityPoolId = "eu-west-1:afba7271-9f8e-4fb1-b33e-794ceffe7731";

        //sns android platform arn
        public const string AndroidPlatformApplicationArn = "arn:aws:sns:eu-west-1:111094818217:app/GCM/EANotifications";

        //sns ios platform arn
        public const string iOSPlatformApplicationArn = "";

        //project id for android gcm
        public const string GoogleConsoleProjectId = "1:795373614767:android:8fd526c23cfd9c46ab465a";

        public static RegionEndpoint CognitoRegion = RegionEndpoint.EUWest1;

        public static RegionEndpoint SnsRegion = RegionEndpoint.EUWest1;
    }
}
