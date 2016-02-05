using King.Service;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;

namespace DataGenerator
{
    class Push : RecurringTask
    {
        string connString = "HostName=mousetrapp.azure-devices.net;DeviceId=MouseTrappIOT;SharedAccessKey=W3ayO1IDKvJ0VW56OCTRih9FnNfINpUwtlfKDRbVqlM=;GatewayHostName=ssl://MouseTrappIOT:8883";
        public Push() : base(30) { }
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
            var client = DeviceClient.CreateFromConnectionString(connString, TransportType.Amqp);
            client.SendEventAsync(msg).Wait();
        }
    }
}