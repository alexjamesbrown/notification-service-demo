using System;
using System.Text;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NotificationService.Contract;

namespace NotificationService.Processor
{
    internal class Program
    {
        private static NotificationSenderFactory _notificationSenderFactory;

        private const string ServiceBusConnectionString = "Endpoint=sb://notificationsvc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=NDQRlMHoRDR2CRp4Bds/N9Tyx532k8+4fiWbsvfwn+U=";
        private const string QueueName = "notifications";

        private static async Task ReceiveMessages()
        {
            var queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            try
            {
                // Register a OnMessage callback
                queueClient.RegisterMessageHandler(async (message, token) =>
                    {
                        var notificationToSend = JsonConvert.DeserializeObject<NotificationToSend>(Encoding.UTF8.GetString(message.Body));

                        var notificationSender = _notificationSenderFactory
                            .GetNotificationSender(notificationToSend.NotificationType);

                        if (notificationSender != null)
                        {
                            await notificationSender.GenerateAndSend(notificationToSend);
                        }

                        // Complete the message so that it is not received again.
                        // This can be done only if the queueClient is opened in ReceiveMode.PeekLock mode.
                        await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                    },
                    new RegisterHandlerOptions { MaxConcurrentCalls = 1, AutoComplete = false });
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }

            Console.WriteLine("Press any key to stop receiving messages.");
            Console.ReadKey();

            // Close the client after the ReceiveMessages method has exited.
            await queueClient.CloseAsync();
        }

        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            //wire up services here

            _notificationSenderFactory = new NotificationSenderFactory(serviceCollection);

            ReceiveMessages()
                .GetAwaiter()
                .GetResult();
        }
    }
}