using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using EA.Notifications.AWS.Lambda.Contracts;
using EA.Notifications.AWS.Lambda.Push;
using Microsoft.Extensions.DependencyInjection;
using WebPush;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace EA.Notifications.AWS.Lambda
{
    public class NotificationFunction
    {
        private ServiceCollection _serviceCollection;
        private IServiceProvider _serviceProvider;

        public NotificationFunction() => ConfigureServices();

        public async Task<APIGatewayProxyResponse> NotificationHandler(APIGatewayProxyRequest apiGatewayProxyRequest, ILambdaContext context)
            => await _serviceProvider.GetService<NotificationApp>().Run(apiGatewayProxyRequest);

        private void ConfigureServices()
        {
            var accessKey = Environment.GetEnvironmentVariable("accessKey");
            var secretKey = Environment.GetEnvironmentVariable("secretKey");

            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddTransient<NotificationApp>();
            _serviceCollection.AddScoped<IDynamoDBContext, DynamoDBContext>
            (provider =>
            {
                var amazonDynamoDbClient = new AmazonDynamoDBClient(accessKey, secretKey, RegionEndpoint.EUWest2);
                return new DynamoDBContext(amazonDynamoDbClient);
            });
            _serviceCollection.AddScoped<WebPushClient, WebPushClient>(provider => CreateWebPushClient());
            _serviceCollection.AddScoped<IPushManager, PushManager>();

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }

        private WebPushClient CreateWebPushClient()
        {
            var vapidPublicKey = Environment.GetEnvironmentVariable("vapidPublicKey");
            var vapidPrivateKey = Environment.GetEnvironmentVariable("vapidPrivateKey");
            var vapidDetails = new VapidDetails("mailto:example@example.com", vapidPublicKey, vapidPrivateKey);

            var webPushClient = new WebPushClient();
            webPushClient.SetVapidDetails(vapidDetails);

            return webPushClient;
        }

        ~NotificationFunction()
        {
            var disposable = _serviceProvider as IDisposable;
            disposable?.Dispose();
        }
    }
}
