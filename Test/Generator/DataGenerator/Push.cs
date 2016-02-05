using King.Service;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator
{
    class Push : RecurringTask
    {
        public override void Run()
        {
            var m = new SampleModel
            {
                Id = Guid.NewGuid(),
                Activated = false,
                Time = DateTime.UtcNow,
            };

            var msg = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(m)));

            //Send data to IoT Hub
            var client = DeviceClient.CreateFromConnectionString("", TransportType.Http1);
            client.SendEventAsync(msg);
        }
    }
}
