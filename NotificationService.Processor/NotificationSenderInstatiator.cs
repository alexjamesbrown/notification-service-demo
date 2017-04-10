using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Processor.NotificationSenders;

namespace NotificationService.Processor
{
    public class NotificationSenderInstatiator : INotificationSenderInstatiator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _typeMappings;

        public NotificationSenderInstatiator(IServiceCollection serviceCollection)
        {
            _typeMappings = new Dictionary<string, Type>();

            //scan this assembly for types taht implement INotificationSender
            //we could include other assemblies here if we wanted
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
                    _typeMappings.Add(notificationType.Name, t);
                    serviceCollection.AddTransient(t);
                }
            }

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public INotificationSender GetNotificationSender(string notificationType)
        {
            if (!_typeMappings.ContainsKey(notificationType))
                return null;

            var type = _typeMappings[notificationType];

            //instantiate it using the ioc container
            //so that any dependencies are also resolved
            //would need more error handling around here
            return _serviceProvider.GetService(type) as INotificationSender;
        }
    }
}
