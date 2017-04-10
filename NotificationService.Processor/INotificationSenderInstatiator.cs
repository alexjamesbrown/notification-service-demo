using NotificationService.Processor.NotificationSenders;

namespace NotificationService.Processor
{
    public interface INotificationSenderInstatiator
    {
        INotificationSender GetNotificationSender(string notificationType);
    }
}