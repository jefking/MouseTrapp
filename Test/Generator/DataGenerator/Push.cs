using King.Service;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DataGenerator
{
    class Push : InitializeTask
    {
        public override void Run()
        {
            Trace.Write("starting");

            var r = new Random();
            var items = this.Get();
            foreach (var td in items)
            {
                var m = new EventModel
                {
                    TrapId = Guid.Parse(td.TrapId),
                    Location = td.Location,
                    Building = "33",
                };
           
                var current = new DateTime(2016, 1, 1);
                var switched = false;
                while (current < DateTime.UtcNow)
                {
                    m.Time = current;
                    m.Type = (byte)(switched ? 2 : 1);

                    var msg = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(m)));

                    //Send data to IoT Hub
                    var client = DeviceClient.CreateFromConnectionString("", TransportType.Amqp);
                    client.SendEventAsync(msg).Wait();

                    switched = !switched;

                    var samplePeriod = r.Next(45, 2880);
                    current = current.AddMinutes(samplePeriod);
                }
            }

            Trace.Write("Completed");

        }

        private class TestData
        {
            public string TrapId;
            public string Location;
        }
        private IEnumerable<TestData> Get()
        {
            yield return new TestData { TrapId = "4807BE67-4BC2-482D-8B08-DDF49F488417", Location = "1" };
            yield return new TestData { TrapId = "B369FF26-F400-4E24-B132-AC8FCD1D0006", Location = "2" };
            yield return new TestData { TrapId = "{1B44AA62-4D65-444E-9487-EA91F66143BE}", Location = "3" };
            yield return new TestData { TrapId = "{05C159BD-359F-44B7-984C-97B87CA2DD3E}", Location = "4" };
            yield return new TestData { TrapId = "{710160FB-5700-4299-8B84-69025EF570C9}", Location = "5" };
            yield return new TestData { TrapId = "{2BC21EFF-8565-48BB-AB62-92D12331D4F5}", Location = "6" };
            yield return new TestData { TrapId = "{50C19048-2076-4B85-97AA-3F4A5C29DC22}", Location = "7" };
            yield return new TestData { TrapId = "{964FFD3D-AD12-47A8-B116-062F753DA0F4}", Location = "8" };
            yield return new TestData { TrapId = "{C33F229C-F763-4D89-9C80-5C35D6D0DAE2}", Location = "9" };
            yield return new TestData { TrapId = "{C764ECA9-8E09-4D33-876F-7C01C424E5E4}", Location = "10" };
        }
    }
}