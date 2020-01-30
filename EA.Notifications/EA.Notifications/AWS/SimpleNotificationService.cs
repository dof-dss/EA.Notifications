using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace EA.Notifications.AWS
{
    public class SimpleNotificationService
    {

        public enum Platform
        {
            Android,
            IOS,
            WindowsPhone
        }

        private static AWSCredentials _credentials;

        private static AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(AppConstants.IdentityPoolId, AppConstants.CognitoRegion);
                return _credentials;
            }
        }

        private static IAmazonSimpleNotificationService _snsClient;

        private static IAmazonSimpleNotificationService SnsClient
        {
            get
            {
                if (_snsClient == null)
                    _snsClient = new AmazonSimpleNotificationServiceClient(Credentials, AppConstants.SnsRegion);
                return _snsClient;
            }
        }

        public static async Task RegisterDevice(Platform platform, string registrationId)
        {
            var arn = string.Empty;
            string _endpointArn = string.Empty;
            switch (platform)
            {
                case Platform.Android:
                    arn = AppConstants.AndroidPlatformApplicationArn;
                    break;
                case Platform.IOS:
                    arn = AppConstants.iOSPlatformApplicationArn;
                    break;
            }

            var response = await SnsClient.CreatePlatformEndpointAsync(new CreatePlatformEndpointRequest
                {
                    Token = registrationId,
                    PlatformApplicationArn = arn
                }
            );

            _endpointArn = response.EndpointArn;

        }

    }
}
