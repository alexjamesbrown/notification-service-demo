using System;
using System.Threading.Tasks;
using NotificationService.Contract;

namespace NotificationService.Processor.NotificationSenders
{
    [NotificationType("outbid")]
    public class SendOutbidNotification : INotificationSender
    {
        public Task GenerateAndSend(NotificationToSend notificationToSend)
        {
            //here we'd go and get the customer
            //get other bits and pieces (from meta data or whatever)
            //generate the dictionary to send to sendgrid
            Console.WriteLine($"Sending outbid notification to customer id {notificationToSend.CustomerId}");
            return Task.FromResult(0);
        }
    }
}
