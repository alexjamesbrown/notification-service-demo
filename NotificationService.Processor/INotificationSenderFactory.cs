using NotificationService.Processor.NotificationSenders;

namespace NotificationService.Processor
{
    public interface INotificationSenderFactory
    {
        INotificationSender GetNotificationSender(string notificationType);
    }
}