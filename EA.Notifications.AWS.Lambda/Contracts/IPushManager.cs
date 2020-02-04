using System.Threading.Tasks;
using EA.Notifications.AWS.Lambda.Models;

namespace EA.Notifications.AWS.Lambda.Contracts
{
    public interface IPushManager
    {
        Task<PushSubscriptionTable> PushSubscribe(PushSubscriptionModel pushSubscriptionModel);
        Task<string> SendPush(string payload);
    }
}