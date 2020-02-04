using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EA.Notifications.AWS.Lambda.ApiGatewayHandlers
{
    public static class ApiGatewayResponseBuilder
    {
        private static JsonSerializerSettings JsonSettings { get; set; }
        private static Dictionary<string, string> Headers { get; set; }

        static ApiGatewayResponseBuilder()
        {
            Headers = new Dictionary<string, string>
            {
                {"Access-Control-Allow-Origin", "*"},
                {"Access-Control-Allow-Headers", "*"},
                {"Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,OPTIONS"}
            };
            JsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }

        public static APIGatewayProxyResponse Build(HttpStatusCode statusCode, object responseContent)
        {
            return new APIGatewayProxyResponse
            {
                Headers = Headers,
                StatusCode = (int)statusCode,
                Body = responseContent != null ? responseContent.ToString() : string.Empty
            };
        }
    }
}
