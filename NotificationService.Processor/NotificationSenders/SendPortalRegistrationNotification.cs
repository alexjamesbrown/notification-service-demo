using NotificationService.Contract;
using System;
using System.Threading.Tasks;

namespace NotificationService.Processor.NotificationSenders
{
    [NotificationType("portal-registration")]
    public class SendPortalRegistrationNotification : INotificationSender
    {
        public Task GenerateAndSend(NotificationToSend notificationToSend)
        {
            //here we'd go and get the customer
            //get other bits and pieces (from meta data or whatever)
            //generate the dictionary to send to sendgrid
            Console.WriteLine($"Sending portal registration notification to customer id {notificationToSend.CustomerId}");
            return Task.FromResult(0);

        }
    }
}
