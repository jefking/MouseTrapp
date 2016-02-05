using King.Service;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;

namespace DataGenerator
{
    class Push : RecurringTask
    {
        public override void Run()
        {
            var r = new Random();
            var m = new EventModel
            {
                TrapId = Guid.NewGuid(),
                Building = "Building",
                Location = "Location",
                Type = (byte)r.Next(0,2),
                Time = DateTime.UtcNow,
            };

            var msg = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(m)));

            //Send data to IoT Hub
            var client = DeviceClient.CreateFromConnectionString("", TransportType.Http1);
            client.SendEventAsync(msg);
        }
    }
}
