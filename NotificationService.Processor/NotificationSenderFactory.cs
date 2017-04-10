using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Processor.NotificationSenders;

namespace NotificationService.Processor
{
    public class NotificationSenderFactory : INotificationSenderFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<string, Type> typeMappings;

        public NotificationSenderFactory(IServiceCollection serviceCollection)
        {

            typeMappings = new Dictionary<string, Type>();

            var type = typeof(INotificationSender);
            var types = type.GetTypeInfo()
                .Assembly.GetTypes()
                .Where(t => type.IsAssignableFrom(t))
                .Where(t => t.GetTypeInfo().IsClass);

            foreach (var t in types)
            {
                var notificationType = t.GetTypeInfo().GetCustomAttribute<NotificationType>();

                if (notificationType != null)
                {
                    typeMappings.Add(notificationType.Name, t);
                    serviceCollection.AddTransient(t);
                }
            }

            serviceProvider = serviceCollection.BuildServiceProvider();

        }

        public INotificationSender GetNotificationSender(string notificationType)
        {
            if (!typeMappings.ContainsKey(notificationType))
                return null;

            var type = typeMappings[notificationType];
            return serviceProvider.GetService(type) as INotificationSender;
        }
    }
}
