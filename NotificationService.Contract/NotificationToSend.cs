using System.Collections.Generic;

namespace NotificationService.Contract
{
    public class NotificationToSend
    {

        public string NotificationType { get; set; }

        public string PlatformCode { get; set; }

        public int CustomerId { get; set; }

        public Dictionary<string, object> Metadata { get; set; }
    }
}
