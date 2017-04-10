using System.Threading.Tasks;
using NotificationService.Contract;

namespace NotificationService.Processor.NotificationSenders
{
    public interface INotificationSender
    {
        Task GenerateAndSend(NotificationToSend notificationToSend);
    }
}
