using System;

namespace NotificationService.Processor
{
    public class NotificationType : Attribute
    {
        public NotificationType(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}