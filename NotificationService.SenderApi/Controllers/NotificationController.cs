using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NotificationService.Contract;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.SenderApi.Controllers
{
    [Route("api/notification")]
    public class NotificationController : Controller
    {
        private static QueueClient _queueClient;

        public NotificationController()
        {
            //this would come from config
            const string serviceBusConnectionString = "Endpoint=sb://notificationsvc.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=NDQRlMHoRDR2CRp4Bds/N9Tyx532k8+4fiWbsvfwn+U=";
            const string queueName = "notifications";

            //inject this
            _queueClient = new QueueClient(serviceBusConnectionString, queueName);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Waiting");
        }

        [HttpPost]
        public async Task Post([FromBody]NotificationToSend notificationToSend)
        {
            var json = JsonConvert.SerializeObject(notificationToSend,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            var message = new Message(Encoding.ASCII.GetBytes(json));

            //seems a tad slow? \500ms
            await _queueClient.SendAsync(message);
        }
    }
}
